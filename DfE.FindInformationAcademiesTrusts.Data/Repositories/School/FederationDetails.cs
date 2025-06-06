namespace DfE.FindInformationAcademiesTrusts.Data.Repositories.School;

public record FederationDetails(
    string? FederationName,
    string? FederationUid,
    DateTime? OpenedOnDate,
    Dictionary<string, string>? Schools);