namespace DfE.FindInformationAcademiesTrusts.Data;

public record Academy(
    int Urn,
    DateTime DateAcademyJoinedTrust,
    string? EstablishmentName,
    string? TypeOfEstablishment,
    string? LocalAuthority,
    string? UrbanRural,
    string? PhaseOfEducation,
    int? NumberOfPupils,
    int? SchoolCapacity,
    double? PercentageFreeSchoolMeals,
    AgeRange AgeRange,
    OfstedRating CurrentOfstedRating,
    OfstedRating PreviousOfstedRating,
    int OldLaCode)
{
    public float? PercentageFull
    {
        get
        {
            if (this is { NumberOfPupils: not null, SchoolCapacity: not null } &&
                SchoolCapacity != 0)
            {
                return (float)Math.Round((int)NumberOfPupils / (float)SchoolCapacity * 100);
            }

            return null;
        }
    }
}
