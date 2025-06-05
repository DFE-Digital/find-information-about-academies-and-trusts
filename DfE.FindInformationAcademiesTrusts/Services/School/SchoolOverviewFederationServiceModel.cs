namespace DfE.FindInformationAcademiesTrusts.Services.School;

public record SchoolOverviewFederationServiceModel(
    string? Name,
    string? FederationUid,
    DateTime? OpenedOnDate,
    Dictionary<string, string> Schools);