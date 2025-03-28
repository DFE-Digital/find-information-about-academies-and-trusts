using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Contexts;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Extensions;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.Academy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Repositories;

public partial class AcademyRepository(IAcademiesDbContext academiesDbContext, ILogger<AcademyRepository> logger)
    : IAcademyRepository
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

    public async Task<AcademyPupilNumbers[]> GetAcademiesInTrustPupilNumbersAsync(string uid)
    {
        return await academiesDbContext.GiasGroupLinks
            .Where(gl => gl.GroupUid == uid)
            .Join(academiesDbContext.GiasEstablishments,
                gl => gl.Urn!, e => e.Urn.ToString(),
                (_, e) =>
                    new AcademyPupilNumbers(e.Urn.ToString(),
                        e.EstablishmentName,
                        e.PhaseOfEducationName,
                        new AgeRange(e.StatutoryLowAge!, e.StatutoryHighAge!),
                        e.NumberOfPupils.ParseAsNullableInt(),
                        e.SchoolCapacity.ParseAsNullableInt()))
            .ToArrayAsync();
    }

    public async Task<AcademyFreeSchoolMeals[]> GetAcademiesInTrustFreeSchoolMealsAsync(string uid)
    {
        return await academiesDbContext.GiasGroupLinks
            .Where(gl => gl.GroupUid == uid)
            .Join(academiesDbContext.GiasEstablishments,
                gl => gl.Urn!, e => e.Urn.ToString(),
                (gl, e) =>
                    new AcademyFreeSchoolMeals(e.Urn.ToString(),
                        e.EstablishmentName,
                        e.PercentageFsm.ParseAsNullableDouble(),
                        int.Parse(e.LaCode!),
                        e.TypeOfEstablishmentName,
                        e.PhaseOfEducationName))
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

    public async Task<AcademyOverview[]> GetOverviewOfAcademiesInTrustAsync(string uid)
    {
        return await academiesDbContext.GiasGroupLinks
            .Where(gl => gl.GroupUid == uid)
            .Join(
                academiesDbContext.GiasEstablishments,
                gl => gl.Urn!,
                e => e.Urn.ToString(),
                (gl, e) =>
                    new AcademyOverview
                    (
                        e.Urn.ToString(),
                        e.LaName ?? string.Empty,
                        e.NumberOfPupils.ParseAsNullableInt(),
                        e.SchoolCapacity.ParseAsNullableInt()
                    ))
            .ToArrayAsync();
    }
}
