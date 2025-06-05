namespace DfE.FindInformationAcademiesTrusts.Data.Repositories.School;

public record FederationDetails(
    string? Name,
    string? FederationUid,
    DateTime? OpenedOnDate,
    Dictionary<string, string>? Schools);