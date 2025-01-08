using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Contexts;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.PipelineEstablishments;
using Microsoft.EntityFrameworkCore;

public class PipelineEstablishmentRepository : IPipelineEstablishmentRepository
{
    private readonly IAcademiesDbContext academiesDbContext;

    public PipelineEstablishmentRepository(IAcademiesDbContext academiesDbContext)
    {
        this.academiesDbContext = academiesDbContext;
    }

    public async Task<FreeSchoolProject[]> GetPipelineFreeSchoolProjects(string uid)
    {
        var freeSchoolProjects = await academiesDbContext.MstrFreeSchoolProjects
            //.Where(m => m.TrustID == uid)
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
                DateSource = m.DateSource
            })
            .ToArrayAsync();

        return freeSchoolProjects;
    }
}
