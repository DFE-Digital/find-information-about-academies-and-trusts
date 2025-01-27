using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Contexts;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.PipelineAcademy;
using Microsoft.EntityFrameworkCore;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Repositories;

public class PipelineEstablishmentRepository(IAcademiesDbContext academiesDbContext) : IPipelineEstablishmentRepository
{
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
            //DateSource = m.DateSource, TODO: Add these in for data source and updates
            //LastDataRefresh = m.LastDataRefresh,
            ))
            .ToArrayAsync();

        return freeSchoolProjects;
    }

    public async Task<PipelineEstablishment[]?> GetAdvisoryConversionEstablishmentsAsync(string trustReferenceNumber, AdvisoryType advisoryType)
    {
        // Base query
        var query = academiesDbContext.MstrAcademyConversions
            .Where(project => project.TrustID == trustReferenceNumber)
            .Where(project =>
                project.ProjectStatus == PipelineStatuses.ApprovedForAO ||
                project.ProjectStatus == PipelineStatuses.AwaitingModeration ||
                project.ProjectStatus == PipelineStatuses.ConverterPreAO ||
                project.ProjectStatus == PipelineStatuses.ConverterPreAOC ||
                project.ProjectStatus == PipelineStatuses.Deferred ||
                project.ProjectStatus == PipelineStatuses.DirectiveAcademyOrders);

        query = advisoryType == AdvisoryType.PostAdvisory
            ? query.Where(project => project.InComplete == "Yes")
            : query.Where(project => project.InPrepare == "No");

        var establishments = await query
            .Select(m => new PipelineEstablishment(
                m.URN.ToString(),
                m.ProjectName,
                m.StatutoryLowestAge.HasValue && m.StatutoryHighestAge.HasValue
                    ? new AgeRange(m.StatutoryLowestAge.Value, m.StatutoryHighestAge.Value)
                    : null,
                m.LocalAuthority,
                "Conversion", // TODO: Introduce const or enum
                m.ExpectedOpeningDate
            ))
            .ToArrayAsync();

        return establishments;
    }

    public async Task<PipelineEstablishment[]?> GetAdvisoryTransferEstablishmentsAsync(string trustReferenceNumber, AdvisoryType advisoryType)
    {
        var query = academiesDbContext.MstrAcademyTransfers
            .Where(t =>
                t.NewProvisionalTrustID == trustReferenceNumber
            )
            .Where(t =>
                t.AcademyTransferStatus == PipelineStatuses.ConsideringAcademyTransfer ||
                t.AcademyTransferStatus == PipelineStatuses.InProcessOfAcademyTransfer);

        query = advisoryType == AdvisoryType.PostAdvisory
            ? query.Where(project => project.InComplete == "Yes")
            : query.Where(project => project.InPrepare == "No");

        var establishments = await query
            .Select(t => new PipelineEstablishment(
                t.AcademyURN.ToString(),
                t.AcademyName,
                t.StatutoryLowestAge.HasValue && t.StatutoryHighestAge.HasValue
                    ? new AgeRange(t.StatutoryLowestAge.Value, t.StatutoryHighestAge.Value)
                    : null,
                t.LocalAuthority,
                "Transfer", // TODO: consts
                t.ExpectedTransferDate
            ))
            .ToArrayAsync();

        return establishments;
    }
}