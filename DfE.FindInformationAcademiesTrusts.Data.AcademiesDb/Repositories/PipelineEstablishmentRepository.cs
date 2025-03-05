using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Contexts;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Mstr;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.PipelineAcademy;
using Microsoft.EntityFrameworkCore;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Repositories;

public class PipelineEstablishmentRepository(IAcademiesDbContext academiesDbContext) : IPipelineEstablishmentRepository
{
    /// <summary>
    /// List of valid pipeline statuses for the conversions side.
    /// </summary>
    private static readonly string[] ConversionStatuses =
    [
        PipelineStatuses.ApprovedForAO,
        PipelineStatuses.AwaitingModeration,
        PipelineStatuses.ConverterPreAO,
        PipelineStatuses.ConverterPreAOC,
        PipelineStatuses.Deferred,
        PipelineStatuses.DirectiveAcademyOrders
    ];

    /// <summary>
    /// List of valid pipeline statuses for the transfers side.
    /// </summary>
    private static readonly string[] TransferStatuses =
    [
        PipelineStatuses.ConsideringAcademyTransfer,
        PipelineStatuses.InProcessOfAcademyTransfer
    ];

    /// <summary>
    /// Returns all Free School projects for the specified trust in the "FreeSchoolPipeline" stage.
    /// </summary>
    public async Task<PipelineEstablishment[]> GetPipelineFreeSchoolProjectsAsync(string trustReferenceNumber)
    {
        var freeSchoolProjects = await academiesDbContext.MstrFreeSchoolProjects
            .Where(trust => trust.TrustID == trustReferenceNumber)
            .Where(trust => trust.Stage == PipelineStatuses.FreeSchoolPipeline)
            .Select(m => new PipelineEstablishment(
                m.NewURN.ToString(),
                m.ProjectName,
                m.StatutoryLowestAge.HasValue && m.StatutoryHighestAge.HasValue
                    ? new AgeRange(m.StatutoryLowestAge.Value, m.StatutoryHighestAge.Value)
                    : null,
                m.LocalAuthority,
                m.ProjectApplicationType,
                m.ProvisionalOpeningDate
            ))
            .ToArrayAsync();

        return freeSchoolProjects;
    }

    /// <summary>
    /// Returns all advisory Conversions (Pre or Post) for the specified trust.
    /// </summary>
    public async Task<PipelineEstablishment[]> GetAdvisoryConversionEstablishmentsAsync(
        string trustReferenceNumber,
        AdvisoryType advisoryType)
    {
        // Base query
        var query = ConversionsBaseQuery(trustReferenceNumber);

        // Advisory filter
        query = advisoryType == AdvisoryType.PreAdvisory
            ? ApplyPreAdvisoryFilter(query)
            : ApplyPostAdvisoryFilter(query);

        var establishments = await query
            .Select(m => new PipelineEstablishment(
                m.URN.ToString(),
                m.ProjectName,
                m.StatutoryLowestAge.HasValue && m.StatutoryHighestAge.HasValue
                    ? new AgeRange(m.StatutoryLowestAge.Value, m.StatutoryHighestAge.Value)
                    : null,
                m.LocalAuthority,
                ProjectType.Conversion,
                m.ExpectedOpeningDate
            ))
            .ToArrayAsync();

        return establishments;
    }

    /// <summary>
    /// Returns all advisory Transfers (Pre or Post) for the specified trust.
    /// </summary>
    public async Task<PipelineEstablishment[]> GetAdvisoryTransferEstablishmentsAsync(
        string trustReferenceNumber,
        AdvisoryType advisoryType)
    {
        var query = academiesDbContext.MstrAcademyTransfers
            .Where(t => t.NewProvisionalTrustID == trustReferenceNumber)
            .Where(t => TransferStatuses.Contains(t.AcademyTransferStatus));

        // Advisory filter
        query = advisoryType == AdvisoryType.PreAdvisory
            ? ApplyPreAdvisoryFilter(query)
            : ApplyPostAdvisoryFilter(query);

        var establishments = await query
            .Select(t => new PipelineEstablishment(
                t.AcademyURN.ToString(),
                t.AcademyName,
                t.StatutoryLowestAge.HasValue && t.StatutoryHighestAge.HasValue
                    ? new AgeRange(t.StatutoryLowestAge.Value, t.StatutoryHighestAge.Value)
                    : null,
                t.LocalAuthority,
                ProjectType.Transfer,
                t.ExpectedTransferDate
            ))
            .ToArrayAsync();

        return establishments;
    }

    /// <summary>
    /// Returns counts for PreAdvisory, PostAdvisory, and FreeSchools across 
    /// conversions/transfers for the specified trust.
    /// </summary>
    public async Task<PipelineSummary> GetAcademiesPipelineSummaryAsync(string trustReferenceNumber)
    {
        // Count pre- and post-advisory conversions
        var preAdvisoryConversionsCount = await CountConversionsAsync(trustReferenceNumber, true);
        var postAdvisoryConversionsCount = await CountConversionsAsync(trustReferenceNumber, false);

        // Count pre- and post-advisory transfers
        var preAdvisoryTransfersCount = await CountTransfersAsync(trustReferenceNumber, true);
        var postAdvisoryTransfersCount = await CountTransfersAsync(trustReferenceNumber, false);

        // Free schools
        var freeSchoolsCount = await academiesDbContext.MstrFreeSchoolProjects
            .Where(fs => fs.TrustID == trustReferenceNumber)
            .Where(fs => fs.Stage == PipelineStatuses.FreeSchoolPipeline)
            .CountAsync();

        // Sum up totals
        var preAdvisoryCount = preAdvisoryConversionsCount + preAdvisoryTransfersCount;
        var postAdvisoryCount = postAdvisoryConversionsCount + postAdvisoryTransfersCount;

        // Return aggregated result
        return new PipelineSummary(
            preAdvisoryCount,
            postAdvisoryCount,
            freeSchoolsCount
        );
    }

    private IQueryable<MstrAcademyConversion> ConversionsBaseQuery(string trustReferenceNumber)
    {
        return academiesDbContext.MstrAcademyConversions
            .Where(conv => conv.TrustID == trustReferenceNumber)
            .Where(conv => ConversionStatuses.Contains(conv.ProjectStatus))
            .Where(conv => conv.DaoProgress != "dAO revoked");
    }

    private IQueryable<MstrAcademyTransfer> TransfersBaseQuery(string trustReferenceNumber)
    {
        return academiesDbContext.MstrAcademyTransfers
            .Where(t => t.NewProvisionalTrustID == trustReferenceNumber)
            .Where(t => TransferStatuses.Contains(t.AcademyTransferStatus));
    }

    /// <summary>
    /// Apply filter for Pre-Advisory records: InComplete == "No" && InPrepare == "Yes"
    /// </summary>
    private static IQueryable<T> ApplyPreAdvisoryFilter<T>(IQueryable<T> query)
        where T : class, IInComplete, IInPrepare
    {
        return query.Where(x => x.InComplete.HasValue && x.InPrepare.HasValue)
            .Where(x => !x.InComplete!.Value && x.InPrepare!.Value);
    }

    /// <summary>
    /// Apply filter for Post-Advisory records: InComplete == "Yes"
    /// </summary>
    private static IQueryable<T> ApplyPostAdvisoryFilter<T>(IQueryable<T> query)
        where T : class, IInComplete
    {
        return query.Where(x => x.InComplete.HasValue)
            .Where(x => x.InComplete!.Value);
    }

    /// <summary>
    /// Counts conversions for either PreAdvisory (true) or PostAdvisory (false).
    /// </summary>
    private async Task<int> CountConversionsAsync(string trustReferenceNumber, bool isPreAdvisory)
    {
        var query = ConversionsBaseQuery(trustReferenceNumber);
        query = isPreAdvisory
            ? ApplyPreAdvisoryFilter(query)
            : ApplyPostAdvisoryFilter(query);
        return await query.CountAsync();
    }

    /// <summary>
    /// Counts transfers for either PreAdvisory (true) or PostAdvisory (false).
    /// </summary>
    private async Task<int> CountTransfersAsync(string trustReferenceNumber, bool isPreAdvisory)
    {
        var query = TransfersBaseQuery(trustReferenceNumber);
        query = isPreAdvisory
            ? ApplyPreAdvisoryFilter(query)
            : ApplyPostAdvisoryFilter(query);
        return await query.CountAsync();
    }
}
