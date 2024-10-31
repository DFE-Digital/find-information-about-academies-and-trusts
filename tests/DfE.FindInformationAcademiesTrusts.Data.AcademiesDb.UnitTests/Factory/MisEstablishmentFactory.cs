using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Mis;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests.Factory;

public static class MisEstablishmentFactory
{
    public static MisEstablishment CreateMisEstablishment(int urn, int? grade, string? categoriesOfConcern,
        string? safeguarding, string? dateString, bool usePreviousScores = false)

    {
        if (usePreviousScores)
        {
            return new MisEstablishment
            {
                Urn = urn, PreviousFullInspectionOverallEffectiveness = grade.ToString(),
                PreviousQualityOfEducation = grade, PreviousBehaviourAndAttitudes = grade,
                PreviousPersonalDevelopment = grade, PreviousEffectivenessOfLeadershipAndManagement = grade,
                PreviousEarlyYearsProvisionWhereApplicable = grade,
                PreviousSixthFormProvisionWhereApplicable = grade.ToString(),
                PreviousCategoryOfConcern = categoriesOfConcern, PreviousSafeguardingIsEffective = safeguarding,
                PreviousInspectionStartDate = dateString
            };
        }

        return new MisEstablishment
        {
            Urn = urn, OverallEffectiveness = grade, QualityOfEducation = grade, BehaviourAndAttitudes = grade,
            PersonalDevelopment = grade, EffectivenessOfLeadershipAndManagement = grade,
            EarlyYearsProvisionWhereApplicable = grade,
            SixthFormProvisionWhereApplicable = grade, CategoryOfConcern = categoriesOfConcern,
            SafeguardingIsEffective = safeguarding, InspectionStartDate = dateString
        };
    }
}
