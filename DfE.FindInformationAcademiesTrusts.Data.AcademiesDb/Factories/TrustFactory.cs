using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Extensions;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Gias;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Mstr;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Factories;

public interface ITrustFactory
{
    Trust CreateTrustFrom(GiasGroup giasGroup, MstrTrust? mstrTrust, Academy[] academies, Governor[] governors,
        Person? trustRelationshipManager, Person? sfsoLead);
}

public class TrustFactory : ITrustFactory
{
    public Trust CreateTrustFrom(GiasGroup giasGroup, MstrTrust? mstrTrust, Academy[] academies, Governor[] governors,
        Person? trustRelationshipManager, Person? sfsoLead)
    {
        return new Trust(
            giasGroup.GroupUid!,
            giasGroup.GroupName ?? string.Empty,
            giasGroup.GroupId ?? string.Empty,
            giasGroup.Ukprn,
            giasGroup.GroupType ?? string.Empty,
            giasGroup.BuildAddressString(),
            giasGroup.IncorporatedOnOpenDate.ParseAsNullableDate(),
            giasGroup.CompaniesHouseNumber ?? string.Empty,
            mstrTrust?.GORregion ?? string.Empty,
            academies,
            governors,
            trustRelationshipManager,
            sfsoLead,
            giasGroup.GroupStatus ?? string.Empty
        );
    }
}
