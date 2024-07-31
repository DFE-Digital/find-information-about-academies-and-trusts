using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Contexts;
using DfE.FindInformationAcademiesTrusts.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Repositories;

public class AcademyRepository(IAcademiesDbContext academiesDbContext) : IAcademyRepository
{
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
