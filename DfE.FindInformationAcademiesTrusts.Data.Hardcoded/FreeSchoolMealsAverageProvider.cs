using System.Diagnostics.CodeAnalysis;
using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.DataSource;

namespace DfE.FindInformationAcademiesTrusts.Data.Hardcoded;

[ExcludeFromCodeCoverage]
public class FreeSchoolMealsAverageProvider : IFreeSchoolMealsAverageProvider
{
    private const int NationalKey = -1;

    public double GetLaAverage(Academy academy)
    {
        var key = GetPhaseTypeKey(academy);
        return FreeSchoolMealsData.Averages2023To24[academy.OldLaCode].PercentOfPupilsByPhase[key];
    }

    public double GetNationalAverage(Academy academy)
    {
        var key = GetPhaseTypeKey(academy);
        return FreeSchoolMealsData.Averages2023To24[NationalKey].PercentOfPupilsByPhase[key];
    }

    public DataSource GetFreeSchoolMealsUpdated()
    {
        return new DataSource(Source.ExploreEducationStatistics, FreeSchoolMealsData.LastUpdated,
            UpdateFrequency.Annually);
    }

    public static ExploreEducationStatisticsPhaseType GetPhaseTypeKey(Academy academy)
    {
        var key = (academy.PhaseOfEducation?.ToLower(), academy.TypeOfEstablishment?.ToLower()) switch
        {
            ("primary" or "middle deemed primary",
                "community school" or "voluntary aided school" or "foundation school" or "voluntary controlled school"
                or "academy sponsor led" or "academy converter" or "city technology college" or "free schools"
                or "university technical college" or "studio schools")
                => ExploreEducationStatisticsPhaseType.StateFundedPrimary,
            ("secondary" or "middle deemed secondary" or "16 plus" or "not applicable" or "all-through",
                "community school" or "voluntary aided school" or "foundation school" or "voluntary controlled school"
                or "academy 16-19 converter" or "academy sponsor led" or "academy converter"
                or "city technology college" or "free schools" or "free schools 16 to 19"
                or "university technical college" or "studio schools" or "academy 16 to 19 sponsor led")
                => ExploreEducationStatisticsPhaseType.StateFundedSecondary,
            (_,
                "foundation special school" or "community special school" or "academy special converter"
                or "academy special sponsor led" or "free schools special")
                => ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool,
            (_,
                "pupil referral unit" or "academy alternative provision sponsor led" or
                "free schools alternative provision" or "academy alternative provision converter")
                => ExploreEducationStatisticsPhaseType.StateFundedApSchool,
            _ => throw new ArgumentOutOfRangeException(nameof(academy),
                $"Can't get ExploreEducationStatisticsPhaseType for [{nameof(academy.PhaseOfEducation)}:{academy.PhaseOfEducation}, {nameof(academy.TypeOfEstablishment)}:{academy.TypeOfEstablishment}]")
        };
        return key;
    }
}
