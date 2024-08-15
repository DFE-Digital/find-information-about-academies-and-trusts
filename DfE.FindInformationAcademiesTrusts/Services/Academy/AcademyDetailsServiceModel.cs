namespace DfE.FindInformationAcademiesTrusts.Services.Academy;

public record AcademyDetailsServiceModel(
    string Urn,
    string? EstablishmentName,
    string? LocalAuthority,
    string? TypeOfEstablishment,
    string? UrbanRural
);
