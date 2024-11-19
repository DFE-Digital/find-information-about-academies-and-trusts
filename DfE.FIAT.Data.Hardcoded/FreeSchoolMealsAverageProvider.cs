using DfE.FIAT.Data.Enums;
using DfE.FIAT.Data.Repositories.DataSource;
using System.Diagnostics.CodeAnalysis;

namespace DfE.FIAT.Data.Hardcoded;

[ExcludeFromCodeCoverage]
public class FreeSchoolMealsAverageProvider : IFreeSchoolMealsAverageProvider
{
    private const int NationalKey = -1;

    public double GetLaAverage(int localAuthorityCode, string? phaseOfEducation, string? typeOfEstablishment)
    {
        var key = GetPhaseTypeKey(phaseOfEducation, typeOfEstablishment);
        return FreeSchoolMealsData.Averages2023To24[localAuthorityCode].PercentOfPupilsByPhase[key];
    }

    public double GetNationalAverage(string? phaseOfEducation, string? typeOfEstablishment)
    {
        var key = GetPhaseTypeKey(phaseOfEducation, typeOfEstablishment);
        return FreeSchoolMealsData.Averages2023To24[NationalKey].PercentOfPupilsByPhase[key];
    }

    public DataSource GetFreeSchoolMealsUpdated()
    {
        return new DataSource(Source.ExploreEducationStatistics, FreeSchoolMealsData.LastUpdated,
            UpdateFrequency.Annually);
    }

    public static ExploreEducationStatisticsPhaseType GetPhaseTypeKey(string? phaseOfEducation,
        string? typeOfEstablishment)
    {
        return (phaseOfEducation: phaseOfEducation?.ToLower(),
                typeOfEstablishment: typeOfEstablishment?.ToLower()) switch
        {
            {
                phaseOfEducation: "primary" or "middle deemed primary",
                typeOfEstablishment: "community school" or "voluntary aided school" or "foundation school"
                or "voluntary controlled school" or "academy sponsor led" or "academy converter"
                or "city technology college" or "free schools" or "university technical college" or "studio schools"
            } => ExploreEducationStatisticsPhaseType.StateFundedPrimary,

            {
                phaseOfEducation: "secondary" or "middle deemed secondary" or "16 plus" or "not applicable"
                or "all-through",
                typeOfEstablishment: "community school" or "voluntary aided school" or "foundation school"
                or "voluntary controlled school" or "academy 16-19 converter" or "academy sponsor led"
                or "academy converter" or "city technology college" or "free schools" or "free schools 16 to 19"
                or "university technical college" or "studio schools" or "academy 16 to 19 sponsor led"
            } => ExploreEducationStatisticsPhaseType.StateFundedSecondary,

            {
                typeOfEstablishment: "foundation special school" or "community special school"
                or "academy special converter" or "academy special sponsor led" or "free schools special"
            } => ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool,

            {
                typeOfEstablishment: "pupil referral unit" or "academy alternative provision sponsor led"
                or "free schools alternative provision" or "academy alternative provision converter"
            } => ExploreEducationStatisticsPhaseType.StateFundedApSchool,

            _ => throw new ArgumentOutOfRangeException(nameof(phaseOfEducation),
                $"Can't get ExploreEducationStatisticsPhaseType for [{nameof(phaseOfEducation)}:{phaseOfEducation}, {nameof(typeOfEstablishment)}:{typeOfEstablishment}]")
        };
    }
}
