using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Contexts;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Extensions;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.Trust;
using Microsoft.EntityFrameworkCore;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Repositories;

public class TrustRepository(IAcademiesDbContext academiesDbContext) : ITrustRepository
{
    public async Task<TrustSummary?> GetTrustSummaryAsync(string uid)
    {
        var details = await academiesDbContext.Groups
            .Where(g => g.GroupUid == uid)
            .Select(g => new { Name = g.GroupName ?? string.Empty, Type = g.GroupType ?? string.Empty })
            .SingleOrDefaultAsync();

        return details is null ? null : new TrustSummary(details.Name, details.Type);
    }

    public async Task<TrustDetails> GetTrustDetailsAsync(string uid)
    {
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

        var regionAndTerritory = await GetRegionAndTerritoryAsync(uid);

        var trustDetailsDto = new TrustDetails(
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
            giasGroup.IncorporatedOnOpenDate.ParseAsNullableDate()
        );

        return trustDetailsDto;
    }

    public async Task<TrustGovernance> GetTrustGovernanceAsync(string uid)
    {
        var governors = await academiesDbContext.GiasGovernances
            .Where(g => g.Uid == uid)
            .Select(governance => new Governor(governance.Gid!, governance.Uid!,
                GetFullName(governance.Forename1!, governance.Forename2!, governance.Surname!),
                governance.Role!, governance.AppointingBody!, governance.DateOfAppointment.ParseAsNullableDate(),
                governance.DateTermOfOfficeEndsEnded.ParseAsNullableDate(), null))
            .ToArrayAsync();
        var governersDto = new TrustGovernance(
            governors.Where(governor => governor is { IsCurrentGovernor: true, HasRoleLeadership: true }).ToArray(),
            governors.Where(governor => governor is { IsCurrentGovernor: true, HasRoleMemeber: true })
                .ToArray(),
            governors.Where(governor => governor is { IsCurrentGovernor: true, HasRoleTrustee: true })
                .ToArray(),
            governors.Where(governor => governor.IsCurrentGovernor is false).ToArray()
        );
        return governersDto;
    }

    private async Task<string> GetRegionAndTerritoryAsync(string uid)
    {
        return await academiesDbContext.MstrTrusts
            .Where(m => m.GroupUid == uid)
            .Select(m => m.GORregion)
            .SingleOrDefaultAsync() ?? string.Empty;
    }

    private static string GetFullName(string forename1, string forename2, string surname)
    {
        var fullName = forename1; //Forename1 is always populated

        if (!string.IsNullOrWhiteSpace(forename2))
            fullName += $" {forename2}";

        if (!string.IsNullOrWhiteSpace(surname))
            fullName += $" {surname}";

        return fullName;
    }
}
