namespace DfE.FindInformationAcademiesTrusts.Data.RepositoryDto;

public record TrustDetailsRepoDto(
    string Uid,
    string? GroupId,
    string? Ukprn,
    string? CompaniesHouseNumber,
    string Type,
    string Address,
    string RegionAndTerritory,
    DateTime? OpenedDate);
