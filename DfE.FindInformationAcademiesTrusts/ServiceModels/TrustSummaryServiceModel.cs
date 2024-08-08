namespace DfE.FindInformationAcademiesTrusts.ServiceModels;

public record TrustSummaryServiceModel(
    string Uid,
    string Name,
    string Type,
    int NumberOfAcademies);
