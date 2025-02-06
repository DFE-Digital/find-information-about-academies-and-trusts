using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Contexts;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Mstr;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.PipelineAcademy;
using Microsoft.EntityFrameworkCore;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Repositories
{
    public class PipelineEstablishmentRepository : IPipelineEstablishmentRepository
    {
        private readonly IAcademiesDbContext academiesDbContext;

        /// <summary>
        /// List of valid pipeline statuses for the conversions side.
        /// </summary>
        private static readonly string[] ConversionStatuses =
        {
            PipelineStatuses.ApprovedForAO,
            PipelineStatuses.AwaitingModeration,
            PipelineStatuses.ConverterPreAO,
            PipelineStatuses.ConverterPreAOC,
            PipelineStatuses.Deferred,
            PipelineStatuses.DirectiveAcademyOrders
        };

        /// <summary>
        /// List of valid pipeline statuses for the transfers side.
        /// </summary>
        private static readonly string[] TransferStatuses =
        {
            PipelineStatuses.ConsideringAcademyTransfer,
            PipelineStatuses.InProcessOfAcademyTransfer
        };

        public PipelineEstablishmentRepository(IAcademiesDbContext academiesDbContext)
        {
            this.academiesDbContext = academiesDbContext;
        }

        /// <summary>
        /// Returns all Free School projects for the specified trust in the "FreeSchoolPipeline" stage.
        /// </summary>
        public async Task<PipelineEstablishment[]?> GetPipelineFreeSchoolProjectsAsync(string trustReferenceNumber)
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
                    m.RouteOfProject,
                    m.ActualDateOpened
                ))
                .ToArrayAsync();

            return freeSchoolProjects;
        }

        /// <summary>
        /// Returns all advisory Conversions (Pre or Post) for the specified trust.
        /// </summary>
        public async Task<PipelineEstablishment[]?> GetAdvisoryConversionEstablishmentsAsync(
            string trustReferenceNumber,
            AdvisoryType advisoryType)
        {
            // Base query
            var query = academiesDbContext.MstrAcademyConversions
                .Where(project => project.TrustID == trustReferenceNumber)
                .Where(project => ConversionStatuses.Contains(project.ProjectStatus));

            // Advisory filter
            query = advisoryType == AdvisoryType.PreAdvisory
                ? query.Where(project => project.InComplete == "No" && project.InPrepare == "Yes")
                : query.Where(project => project.InComplete == "Yes");

            var establishments = await query
                .Select(m => new PipelineEstablishment(
                    m.URN.ToString(),
                    m.ProjectName,
                    m.StatutoryLowestAge.HasValue && m.StatutoryHighestAge.HasValue
                        ? new AgeRange(m.StatutoryLowestAge.Value, m.StatutoryHighestAge.Value)
                        : null,
                    m.LocalAuthority,
                    "Conversion", // Could be a const, or from an enum
                    m.ExpectedOpeningDate
                ))
                .ToArrayAsync();

            return establishments;
        }

        /// <summary>
        /// Returns all advisory Transfers (Pre or Post) for the specified trust.
        /// </summary>
        public async Task<PipelineEstablishment[]?> GetAdvisoryTransferEstablishmentsAsync(
            string trustReferenceNumber,
            AdvisoryType advisoryType)
        {
            var query = academiesDbContext.MstrAcademyTransfers
                .Where(t => t.NewProvisionalTrustID == trustReferenceNumber)
                .Where(t => TransferStatuses.Contains(t.AcademyTransferStatus));

            // Advisory filter
            query = advisoryType == AdvisoryType.PreAdvisory
                ? query.Where(t => t.InComplete == "No" && t.InPrepare == "Yes")
                : query.Where(t => t.InComplete == "Yes");

            var establishments = await query
                .Select(t => new PipelineEstablishment(
                    t.AcademyURN.ToString(),
                    t.AcademyName,
                    t.StatutoryLowestAge.HasValue && t.StatutoryHighestAge.HasValue
                        ? new AgeRange(t.StatutoryLowestAge.Value, t.StatutoryHighestAge.Value)
                        : null,
                    t.LocalAuthority,
                    "Transfer", // Could be a const, or from an enum
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
            var preAdvisoryConversionsCount = await CountConversionsAsync(trustReferenceNumber, isPreAdvisory: true);
            var postAdvisoryConversionsCount = await CountConversionsAsync(trustReferenceNumber, isPreAdvisory: false);

            // Count pre- and post-advisory transfers
            var preAdvisoryTransfersCount = await CountTransfersAsync(trustReferenceNumber, isPreAdvisory: true);
            var postAdvisoryTransfersCount = await CountTransfersAsync(trustReferenceNumber, isPreAdvisory: false);

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
                PreAdvisoryCount: preAdvisoryCount,
                PostAdvisoryCount: postAdvisoryCount,
                FreeSchoolsCount: freeSchoolsCount
            );
        }

        private IQueryable<MstrAcademyConversions> ConversionsBaseQuery(string trustReferenceNumber)
        {
            return academiesDbContext.MstrAcademyConversions
                .Where(conv => conv.TrustID == trustReferenceNumber)
                .Where(conv => ConversionStatuses.Contains(conv.ProjectStatus));
        }

        private IQueryable<MstrAcademyTransfers> TransfersBaseQuery(string trustReferenceNumber)
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
            return query.Where(x => x.InComplete == "No" && x.InPrepare == "Yes");
        }

        /// <summary>
        /// Apply filter for Post-Advisory records: InComplete == "Yes"
        /// </summary>
        private static IQueryable<T> ApplyPostAdvisoryFilter<T>(IQueryable<T> query)
            where T : class, IInComplete
        {
            return query.Where(x => x.InComplete == "Yes");
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
}
