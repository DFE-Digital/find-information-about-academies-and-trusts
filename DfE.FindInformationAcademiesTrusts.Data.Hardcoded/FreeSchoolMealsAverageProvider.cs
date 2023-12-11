namespace DfE.FindInformationAcademiesTrusts.Data.Hardcoded;

public class FreeSchoolMealsAverageProvider : IFreeSchoolMealsAverageProvider
{
    private const int NationalKey = -1;


    public double GetLaAverage(Academy academy)
    {
        var key = GetPhaseTypeKey(academy);
        return FreeSchoolMealsData.Averages2022To23[academy.OldLaCode].PercentOfPupilsByPhase[key];
    }

    public double GetNationalAverage(Academy academy)
    {
        var key = GetPhaseTypeKey(academy);
        return FreeSchoolMealsData.Averages2022To23[NationalKey].PercentOfPupilsByPhase[key];
    }

    public static ExploreEducationStatisticsPhaseType GetPhaseTypeKey(Academy academy)
    {
        var key = (academy.PhaseOfEducation, academy.TypeOfEstablishment) switch
        {
            ("Primary" or "Middle Deemed Primary", "Community School" or "Voluntary Aided School" or "Foundation School"
                or
                "Voluntary Controlled School" or "Academy Sponsor Led" or "Academy Converter"
                or "City Technology College" or
                "Free Schools" or "University technical college"
                or "Studio schools") => ExploreEducationStatisticsPhaseType
                    .StateFundedPrimary,
            ("Secondary" or "Middle Deemed Secondary" or "16 Plus" or "Not applicable" or "All Through" or "All-Through"
                or
                "All-through" or "all-through", "Community School" or "Voluntary Aided School" or "Foundation School" or
                "Voluntary Controlled School" or "Academy 16-19 Converter" or "Academy Sponsor Led"
                or "Academy Converter" or
                "City Technology College" or
                "Free Schools" or "Free schools 16 to 19" or "University technical college" or "Studio schools" or
                "Academy 16 to 19 sponsor led") => ExploreEducationStatisticsPhaseType.StateFundedSecondary,
            (_, "Foundation Special School" or "Community Special School" or "Academy Special Converter"
                or "Academy Special Sponsor Led" or "Free Schools Special") => ExploreEducationStatisticsPhaseType
                    .StateFundedSpecialSchool,
            (_, "Pupil Referral Unit" or "Academy Alternative Provision Sponsor Led" or
                "Free schools alternative provision"
                or "Academy Alternative Provision Converter") => ExploreEducationStatisticsPhaseType
                    .StateFundedApSchool,
            _ => throw new ArgumentOutOfRangeException(nameof(academy))
        };
        return key;
    }
}
