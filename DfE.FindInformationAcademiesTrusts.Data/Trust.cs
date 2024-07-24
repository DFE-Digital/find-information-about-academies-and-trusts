namespace DfE.FindInformationAcademiesTrusts.Data;

public record Trust(
    string Uid,
    string Name,
    string GroupId,
    string? Ukprn,
    string Type,
    string Address,
    DateTime? OpenedDate,
    string CompaniesHouseNumber,
    string RegionAndTerritory,
    Academy[] Academies,
    Governor[] Governors,
    Person? TrustRelationshipManager,
    Person? SfsoLead,
    string Status
)
{
    public bool IsMultiAcademyTrust()
    {
        return Type == "Multi-academy trust";
    }

    public bool IsSingleAcademyTrust()
    {
        return Type == "Single-academy trust";
    }
}
