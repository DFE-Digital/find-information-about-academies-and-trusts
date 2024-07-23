namespace DfE.FindInformationAcademiesTrusts.Data;

public record TrustSummaryDto(
    string Uid,
    string Name,
    string Type,
    int NumberOfAcademies);
