using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Contexts;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.PipelineEstablishments;
using Microsoft.EntityFrameworkCore;

public class PipelineEstablishmentRepository(IAcademiesDbContext academiesDbContext) : IPipelineEstablishmentRepository
{
    public async Task<FreeSchoolProject[]?> GetPipelineFreeSchoolProjects(string uid)
    {
        var freeSchoolProjects = await academiesDbContext.MstrFreeSchoolProjects
            .Where(trust => trust.TrustID == uid)
            .Where(trust => trust.ProjectStatus == FreeSchoolProjectStatuses.Pipeline)
            .Select(m => new FreeSchoolProject
            {
                SK = m.SK,
                ProjectID = m.ProjectID,
                ProjectName = m.ProjectName,
                ProjectApplicationType = m.ProjectApplicationType,
                LocalAuthority = m.LocalAuthority,
                Region = m.Region,
                SchoolPhase = m.SchoolPhase,
                SchoolType = m.SchoolType,
                ProjectStatus = m.ProjectStatus,
                Stage = m.Stage,
                RouteOfProject = m.RouteOfProject,
                StatutoryLowestAge = m.StatutoryLowestAge,
                StatutoryHighestAge = m.StatutoryHighestAge,
                NewURN = m.NewURN,
                EstablishmentName = m.EstablishmentName,
                ActualDateOpened = m.ActualDateOpened,
                TrustID = m.TrustID,
                TrustName = m.TrustName,
                TrustType = m.TrustType,
                CompaniesHouseNumber = m.CompaniesHouseNumber,
                DateSource = m.DateSource,
                LastDataRefresh = m.LastDataRefresh,
            })
            .ToArrayAsync();

        return freeSchoolProjects;
    }
}
