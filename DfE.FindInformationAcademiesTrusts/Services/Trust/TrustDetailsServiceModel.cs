namespace DfE.FindInformationAcademiesTrusts.Services.Trust;

public record TrustDetailsServiceModel(
    string Uid,
    string? GroupId,
    string? Ukprn,
    string? CompaniesHouseNumber,
    string Type,
    string Address,
    string RegionAndTerritory,
    string? SingleAcademyTrustAcademyUrn,
    DateTime? OpenedDate)
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
