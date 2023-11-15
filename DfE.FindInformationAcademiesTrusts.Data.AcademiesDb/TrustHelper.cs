using System.Globalization;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb;

public interface ITrustHelper
{
    Trust CreateTrustFrom(Group group, MstrTrust mstrTrust, Academy[] academies);
    string BuildAddressString(Group group);
}

public class TrustHelper : ITrustHelper
{
    public Trust CreateTrustFrom(Group group, MstrTrust mstrTrust, Academy[] academies)
    {
        return new Trust(
            group.GroupUid!,
            group.GroupName ?? string.Empty,
            group.GroupId ?? string.Empty,
            group.Ukprn,
            group.GroupType ?? string.Empty,
            BuildAddressString(group),
            ParseAsDate(group.IncorporatedOnOpenDate),
            group.CompaniesHouseNumber ?? string.Empty,
            mstrTrust.GORregion ?? string.Empty,
            academies,
            Array.Empty<Governor>(),
            null,
            null
        );
    }

    public string BuildAddressString(Group group)
    {
        return string.Join(", ", new[]
        {
            group.GroupContactStreet,
            group.GroupContactLocality,
            group.GroupContactTown,
            group.GroupContactPostcode
        }.Where(s => !string.IsNullOrWhiteSpace(s)));
    }

    private static DateTime? ParseAsDate(string? date)
    {
        if (string.IsNullOrEmpty(date)) return null;
        var newDate = DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture);

        return newDate;
    }
}
