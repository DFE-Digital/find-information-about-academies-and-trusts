namespace DfE.FindInformationAcademiesTrusts.Services.Academy;

public record AcademySummaryServiceModel(
    string Urn,
    string Name,
    string? LocalAuthority,
    string? TypeOfEstablishment
);

