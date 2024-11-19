using DfE.FIAT.Data.AcademiesDb.Models.Mis;

namespace DfE.FIAT.Data.AcademiesDb.UnitTests.Factory;

public static class MisFurtherEstablishmentFactory
{
    public static MisFurtherEducationEstablishment CreateMisFurtherEducationEstablishment(int urn, int? grade,
        string? safeguarding, string? dateString, bool usePreviousScores = false)

    {
        if (usePreviousScores)
        {
            return new MisFurtherEducationEstablishment
            {
                ProviderUrn = urn, PreviousOverallEffectiveness = grade,
                PreviousQualityOfEducation = grade, PreviousBehaviourAndAttitudes = grade,
                PreviousPersonalDevelopment = grade, PreviousEffectivenessOfLeadershipAndManagement = grade,
                PreviousSafeguarding = safeguarding,
                PreviousLastDayOfInspection = dateString
            };
        }

        return new MisFurtherEducationEstablishment
        {
            ProviderUrn = urn, OverallEffectiveness = grade, QualityOfEducation = grade, BehaviourAndAttitudes = grade,
            PersonalDevelopment = grade, EffectivenessOfLeadershipAndManagement = grade,
            IsSafeguardingEffective = safeguarding, LastDayOfInspection = dateString
        };
    }
}
