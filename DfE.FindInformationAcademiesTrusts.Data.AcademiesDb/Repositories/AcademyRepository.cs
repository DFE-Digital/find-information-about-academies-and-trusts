using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Contexts;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.Academy;
using Microsoft.EntityFrameworkCore;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Repositories;

public class AcademyRepository(IAcademiesDbContext academiesDbContext) : IAcademyRepository
{
    public async Task<AcademyDetails[]> GetAcademiesInTrustDetailsAsync(string uid)
    {
        return await academiesDbContext.GiasGroupLinks
            .Where(gl => gl.GroupUid == uid)
            .Join(academiesDbContext.GiasEstablishments,
                gl => gl.Urn!, e => e.Urn.ToString(),
                (gl, e) =>
                    new AcademyDetails(e.Urn.ToString(),
                        e.EstablishmentName,
                        e.TypeOfEstablishmentName,
                        e.LaName,
                        e.UrbanRuralName))
            .ToArrayAsync();
    }

    public async Task<int> GetNumberOfAcademiesInTrustAsync(string uid)
    {
        return await academiesDbContext.GiasGroupLinks.CountAsync(gl => gl.GroupUid == uid && gl.Urn != null);
    }

    public async Task<string?> GetSingleAcademyTrustAcademyUrnAsync(string uid)
    {
        return await academiesDbContext.GiasGroupLinks
            .Where(gl => gl.GroupUid == uid
                         && gl.GroupType == "Single-academy trust")
            .Select(gl => gl.Urn)
            .FirstOrDefaultAsync();
    }
}
