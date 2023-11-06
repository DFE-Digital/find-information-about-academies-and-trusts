namespace DfE.FindInformationAcademiesTrusts.Data;

public record Trust(
    string Uid,
    string Name,
    string GroupId,
    string? Ukprn,
    string Type,
    string Address,
    string OpenedDate,
    string CompaniesHouseNumber,
    string RegionAndTerritory
);
