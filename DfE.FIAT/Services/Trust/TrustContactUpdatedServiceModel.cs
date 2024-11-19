namespace DfE.FIAT.Web.Services.Trust;

public record TrustContactUpdatedServiceModel(
    bool EmailUpdated,
    bool NameUpdated
);
