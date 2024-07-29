using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Contexts;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Extensions;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Repositories;
using DfE.FindInformationAcademiesTrusts.Data.Dto;
using Microsoft.EntityFrameworkCore;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb;

public interface ITrustService
{
    Task<TrustSummaryDto?> GetTrustSummaryAsync(string uid);
    Task<TrustDetailsDto> GetTrustDetailsAsync(string uid);
}

public class TrustService(IAcademiesDbContext academiesDbContext, IAcademyRepository academyRepository)
    : ITrustService
{
    public async Task<TrustSummaryDto?> GetTrustSummaryAsync(string uid)
    {
        var details = await academiesDbContext.Groups
            .Where(g => g.GroupUid == uid)
            .Select(g => new { Name = g.GroupName ?? string.Empty, Type = g.GroupType ?? string.Empty })
            .SingleOrDefaultAsync();

        if (details is null) return null;

        var count = await academyRepository.GetNumberOfAcademiesInTrustAsync(uid);

        return new TrustSummaryDto(uid, details.Name, details.Type, count);
    }

    public async Task<TrustDetailsDto> GetTrustDetailsAsync(string uid)
    {
        var regionAndTerritory = await academiesDbContext.MstrTrusts
            .Where(m => m.GroupUid == uid)
            .Select(m => m.GORregion)
            .SingleOrDefaultAsync() ?? string.Empty;

        var singleAcademyUrn = await academyRepository.GetUrnForSingleAcademyTrustAsync(uid);

        var giasGroup = await academiesDbContext.Groups
            .Where(g => g.GroupUid == uid)
            .Select(giasGroup => new
            {
                giasGroup.GroupUid,
                giasGroup.GroupId,
                giasGroup.Ukprn,
                giasGroup.CompaniesHouseNumber,
                giasGroup.GroupType,
                giasGroup.GroupContactStreet,
                giasGroup.GroupContactLocality,
                giasGroup.GroupContactTown,
                giasGroup.GroupContactPostcode,
                giasGroup.IncorporatedOnOpenDate
            })
            .SingleAsync();

        var trustDetailsDto = new TrustDetailsDto(
            giasGroup.GroupUid!, //Searched by this field so it must be present
            giasGroup.GroupId,
            giasGroup.Ukprn,
            giasGroup.CompaniesHouseNumber,
            giasGroup.GroupType!, //Enforced by EF filter
            string.Join(", ", new[]
            {
                giasGroup.GroupContactStreet,
                giasGroup.GroupContactLocality,
                giasGroup.GroupContactTown,
                giasGroup.GroupContactPostcode
            }.Where(s => !string.IsNullOrWhiteSpace(s))),
            regionAndTerritory,
            singleAcademyUrn,
            giasGroup.IncorporatedOnOpenDate.ParseAsNullableDate()
        );

        return trustDetailsDto;
    }
}
