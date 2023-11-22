namespace DfE.FindInformationAcademiesTrusts.Data;

public record Academy(
    int Urn,
    DateTime DateAcademyJoinedTrust,
    string? EstablishmentName,
    string? TypeOfEstablishment,
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