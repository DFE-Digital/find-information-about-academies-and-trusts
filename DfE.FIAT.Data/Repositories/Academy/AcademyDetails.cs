namespace DfE.FIAT.Data.Repositories.Academy;

public record AcademyDetails(
    string Urn,
    string? EstablishmentName,
    string? TypeOfEstablishment,
    string? LocalAuthority,
    string? UrbanRural
);
