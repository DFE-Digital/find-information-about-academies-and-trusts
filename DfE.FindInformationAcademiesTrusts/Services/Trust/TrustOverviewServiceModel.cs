namespace DfE.FindInformationAcademiesTrusts.Services.Trust;

public record TrustOverviewServiceModel(
    string Uid,
    string? GroupId,
    string? Ukprn,
    string? CompaniesHouseNumber,
    string Type,
    string Address,
    string RegionAndTerritory,
    string? SingleAcademyTrustAcademyUrn,
    DateTime? OpenedDate,
    int TotalAcademies,
    IReadOnlyDictionary<string, int> AcademiesByLocalAuthority,
    int TotalPupilNumbers,
    int TotalCapacity)
{
    public bool IsMultiAcademyTrust()
    {
        return Type == "Multi-academy trust";
    }

    public bool IsSingleAcademyTrust()
    {
        return Type == "Single-academy trust";
    }

    public int? PercentageFull =>
        TotalCapacity > 0
            ? (int)Math.Round((double)TotalPupilNumbers / TotalCapacity * 100)
            : null;
}
