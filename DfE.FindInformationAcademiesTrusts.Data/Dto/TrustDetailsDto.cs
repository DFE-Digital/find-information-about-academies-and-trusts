namespace DfE.FindInformationAcademiesTrusts.Data.Dto;

public record TrustDetailsDto(
    string Uid,
    string GroupId,
    string? Ukprn,
    string CompaniesHouseNumber,
    string Type,
    string Address,
    string RegionAndTerritory,
    int? SingleAcademyUrn,
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
