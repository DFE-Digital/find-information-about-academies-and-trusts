using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Contexts;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Mstr;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.PipelineEstablishments;
using Microsoft.EntityFrameworkCore;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Repositories;

public class PipelineEstablishmentRepository(IAcademiesDbContext academiesDbContext)
    : IPipelineEstablishmentRepository
{
    public async Task<MstrFreeSchoolProject[]> GetPipelineFreeSchoolProjects(string uid)
    {
        return await academiesDbContext.MstrFreeSchoolProjects
            .Where(gl => gl.TrustID!.Equals(uid))
            .ToArrayAsync();
    }
}