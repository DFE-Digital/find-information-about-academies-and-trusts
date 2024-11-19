namespace DfE.FIAT.Web.Services.Academy;

public record AcademyDetailsServiceModel(
    string Urn,
    string? EstablishmentName,
    string? LocalAuthority,
    string? TypeOfEstablishment,
    string? UrbanRural
);
