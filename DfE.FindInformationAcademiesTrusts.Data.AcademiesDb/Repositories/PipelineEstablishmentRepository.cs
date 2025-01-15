using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Contexts;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.PipelineAcademy;
using Microsoft.EntityFrameworkCore;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Repositories;

public class PipelineEstablishmentRepository(IAcademiesDbContext academiesDbContext) : IPipelineEstablishmentRepository
{
    public async Task<PipelineEstablishment[]?> GetPipelineFreeSchoolProjects(string trustReferenceNumber)
    {
        var freeSchoolProjects = await academiesDbContext.MstrFreeSchoolProjects
            .Where(trust => trust.TrustID == trustReferenceNumber)
            .Where(trust => trust.Stage == FreeSchoolPipelineStatuses.Pipeline)
            .Select(m => new PipelineEstablishment(

                m.NewURN.ToString(),
                m.ProjectName,
                m.StatutoryLowestAge.HasValue && m.StatutoryHighestAge.HasValue
                ? new AgeRange(m.StatutoryLowestAge.Value, m.StatutoryHighestAge.Value)
                : null,
                m.LocalAuthority,
                m.RouteOfProject,
                m.ActualDateOpened
            //DateSource = m.DateSource,
            //LastDataRefresh = m.LastDataRefresh,
            ))
            .ToArrayAsync();

        return freeSchoolProjects;
    }
}