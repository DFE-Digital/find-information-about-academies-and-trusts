namespace DfE.FIAT.Web.Services.Trust;

public record TrustSummaryServiceModel(
    string Uid,
    string Name,
    string Type,
    int NumberOfAcademies);
