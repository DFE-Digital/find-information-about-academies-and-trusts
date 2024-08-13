namespace DfE.FindInformationAcademiesTrusts.ServiceModels;

public record AcademyDetailsServiceModel(
    string Urn,
    string? EstablishmentName,
    string? LocalAuthority,
    string? TypeOfEstablishment,
    string? UrbanRural
);
