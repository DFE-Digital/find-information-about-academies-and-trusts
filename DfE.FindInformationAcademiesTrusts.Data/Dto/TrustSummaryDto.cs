namespace DfE.FindInformationAcademiesTrusts.Data.Dto;

public record TrustSummaryDto(
    string Uid,
    string Name,
    string Type,
    int NumberOfAcademies);
