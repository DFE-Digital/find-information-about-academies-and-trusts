namespace DfE.FindInformationAcademiesTrusts.Data.Repositories.Academy;

public record AcademyPupilNumbers(
    string Urn,
    string? EstablishmentName,
    string? PhaseOfEducation,
    AgeRange AgeRange,
    int? NumberOfPupils,
    int? SchoolCapacity
);
