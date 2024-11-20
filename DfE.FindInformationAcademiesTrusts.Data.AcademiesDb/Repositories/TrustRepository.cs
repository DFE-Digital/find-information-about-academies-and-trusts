using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Contexts;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Extensions;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Gias;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.Trust;
using Microsoft.EntityFrameworkCore;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Repositories;

public class TrustRepository(
    IAcademiesDbContext academiesDbContext,
    IStringFormattingUtilities stringFormattingUtilities) : ITrustRepository
{
    public async Task<TrustSummary?> GetTrustSummaryAsync(string uid)
    {
        var details = await academiesDbContext.Groups
            .Where(g => g.GroupUid == uid)
            .Select(g => new
                {
                    Name = g.GroupName!,
                    Type = g.GroupType!
                }
            ) //GroupName and GroupType will never be null due to EF query filters
            .SingleOrDefaultAsync();

        return details is null ? null : new TrustSummary(details.Name, details.Type);
    }

    public async Task<TrustOverview> GetTrustOverviewAsync(string uid)
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

        var trustOverview = new TrustOverview(
            giasGroup.GroupUid!, //Searched by this field so it must be present
            giasGroup.GroupId!, //Enforced by EF filter
            giasGroup.Ukprn,
            giasGroup.CompaniesHouseNumber,
            giasGroup.GroupType!, //Enforced by EF filter
            stringFormattingUtilities.BuildAddressString(
                giasGroup.GroupContactStreet,
                giasGroup.GroupContactLocality,
                giasGroup.GroupContactTown,
                giasGroup.GroupContactPostcode
            ),
            regionAndTerritory,
            giasGroup.IncorporatedOnOpenDate.ParseAsNullableDate()
        );

        return trustOverview;
    }

    public async Task<TrustGovernance> GetTrustGovernanceAsync(string uid, string? urn = null)
    {
        IQueryable<GiasGovernance> query = academiesDbContext.GiasGovernances;

        var governors = await FilterBySatOrMat(uid, urn, query)
            .Select(governance => new Governor(
                governance.Gid!,
                governance.Uid!,
                GetFullName(governance.Forename1!, governance.Forename2!, governance.Surname!),
                governance.Role!,
                governance.AppointingBody!,
                governance.DateOfAppointment.ParseAsNullableDate(),
                governance.DateTermOfOfficeEndsEnded.ParseAsNullableDate(),
                null))
            .ToArrayAsync();

        var governersDto = new TrustGovernance(
            governors.Where(g => g.IsCurrentGovernor && g.HasRoleLeadership).ToArray(),
            governors.Where(g => g.IsCurrentGovernor && g.HasRoleMember).ToArray(),
            governors.Where(g => g.IsCurrentGovernor && g.HasRoleTrustee).ToArray(),
            governors.Where(g => !g.IsCurrentGovernor).ToArray()
        );

        return governersDto;
    }

    public static IQueryable<GiasGovernance> FilterBySatOrMat(string uid, string? urn, IQueryable<GiasGovernance> query)
    {
        if (!string.IsNullOrEmpty(urn))
        {
            // Use urn if it's provided as that means this is a Single Academy Trust (SAT)
            return query.Where(g => g.Urn == urn);
        }

        return query.Where(g => g.Uid == uid);
    }

    public async Task<TrustContacts> GetTrustContactsAsync(string uid, string? urn = null)
    {
        var governanceContacts = await GetGovernanceContactsAsync(uid, urn);

        return new TrustContacts(
            governanceContacts.GetValueOrDefault("Accounting Officer"),
            governanceContacts.GetValueOrDefault("Chair of Trustees"),
            governanceContacts.GetValueOrDefault("Chief Financial Officer"));
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

    private async Task<Dictionary<string, Person>> GetGovernanceContactsAsync(string uid, string? urn = null)
    {
        string[] roles = { "Chair of Trustees", "Accounting Officer", "Chief Financial Officer" };

        IQueryable<GiasGovernance> query = academiesDbContext.GiasGovernances;

        query = FilterBySatOrMat(uid, urn, query);

        var governors = (await query
                .Where(governance => roles.Contains(governance.Role))
                .Select(governance => new
                {
                    governance.Gid,
                    FullName = GetFullName(governance.Forename1!, governance.Forename2!, governance.Surname!),
                    EndDate = governance.DateTermOfOfficeEndsEnded.ParseAsNullableDate(),
                    Role = governance.Role!
                })
                .ToArrayAsync())
            .Where(g => g.EndDate == null || g.EndDate >= DateTime.Today).ToArray();

        var gids = governors.Select(g => g.Gid).ToArray();

        var governorEmails = await academiesDbContext.TadTrustGovernances
            .Where(tadTrustGovernance => gids.Contains(tadTrustGovernance.Gid))
            .Select(tadTrustGovernance => new { tadTrustGovernance.Gid, tadTrustGovernance.Email }).ToArrayAsync();

        return governors.ToDictionary(
            governor => governor.Role,
            governor => new Person(
                governor.FullName,
                governorEmails.SingleOrDefault(governorEmail => governorEmail.Gid == governor.Gid)?.Email)
        );
    }
}
