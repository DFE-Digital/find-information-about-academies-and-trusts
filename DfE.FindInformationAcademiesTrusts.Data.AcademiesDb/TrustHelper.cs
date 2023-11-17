using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb;

public interface ITrustHelper
{
    Trust CreateTrustFrom(Group group, MstrTrust? mstrTrust, Academy[] academies);
}

public class TrustHelper : ITrustHelper
{
    public Trust CreateTrustFrom(Group group, MstrTrust? mstrTrust, Academy[] academies)
    {
        return new Trust(
            group.GroupUid!,
            group.GroupName ?? string.Empty,
            group.GroupId ?? string.Empty,
            group.Ukprn,
            group.GroupType ?? string.Empty,
            group.BuildAddressString(),
            group.IncorporatedOnOpenDate.ParseAsNullableDate(),
            group.CompaniesHouseNumber ?? string.Empty,
            mstrTrust?.GORregion ?? string.Empty,
            academies,
            Array.Empty<Governor>(),
            null,
            null
        );
    }
}
