namespace DfE.FindInformationAcademiesTrusts.Data;

public record Academy(
    string Urn,
    string? EstablishmentName,
    string? LocalAuthority,
    string? UrbanRural,
    string? PhaseOfEducation,
    string? NumberOfPupils,
    string? SchoolCapacity,
    string? PercentageFreeSchoolMeals,
    AgeRange? AgeRange,
    OfstedRating? CurrentOfstedRating,
    OfstedRating? PreviousOfstedRating
);
