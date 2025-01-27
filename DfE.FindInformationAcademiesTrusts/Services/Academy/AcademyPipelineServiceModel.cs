using DfE.FindInformationAcademiesTrusts.Data;

namespace DfE.FindInformationAcademiesTrusts.Services.Academy;

public record AcademyPipelineServiceModel(
    string? Urn,
    string? EstablishmentName,
    AgeRange? AgeRange,
    string? LocalAuthority,
    string? ProjectType,
    DateTime? ChangeDate
);
