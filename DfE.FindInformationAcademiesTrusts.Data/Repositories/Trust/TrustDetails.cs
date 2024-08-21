namespace DfE.FindInformationAcademiesTrusts.Data.Repositories.Trust;

public record TrustDetails(
    string Uid,
    string? GroupId,
    string? Ukprn,
    string? CompaniesHouseNumber,
    string Type,
    string Address,
    string RegionAndTerritory,
    DateTime? OpenedDate);
