using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Extensions;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Factories;

public interface ITrustFactory
{
    Trust CreateTrustFrom(Group group, MstrTrust? mstrTrust, Academy[] academies);
}

public class TrustFactory : ITrustFactory
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
