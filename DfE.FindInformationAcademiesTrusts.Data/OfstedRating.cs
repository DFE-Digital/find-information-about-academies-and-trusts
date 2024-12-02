namespace DfE.FindInformationAcademiesTrusts.Data;

public record OfstedRating(
    OfstedRatingScore OverallEffectiveness,
    OfstedRatingScore QualityOfEducation,
    OfstedRatingScore BehaviourAndAttitudes,
    OfstedRatingScore PersonalDevelopment,
    OfstedRatingScore EffectivenessOfLeadershipAndManagement,
    OfstedRatingScore EarlyYearsProvision,
    OfstedRatingScore SixthFormProvision,
    CategoriesOfConcern CategoryOfConcern,
    SafeguardingScore SafeguardingIsEffective,
    DateTime? InspectionDate)
{
    public static readonly OfstedRating NotInspected = new(OfstedRatingScore.NotInspected,
        OfstedRatingScore.NotInspected, OfstedRatingScore.NotInspected, OfstedRatingScore.NotInspected,
        OfstedRatingScore.NotInspected, OfstedRatingScore.NotInspected, OfstedRatingScore.NotInspected,
        CategoriesOfConcern.NotInspected, SafeguardingScore.NotInspected, null);

    public OfstedRating(int? overallEffectiveness, DateTime? inspectionDate)
        : this(
            (OfstedRatingScore?)overallEffectiveness ?? OfstedRatingScore.NotInspected,
            OfstedRatingScore.NotInspected,
            OfstedRatingScore.NotInspected,
            OfstedRatingScore.NotInspected,
            OfstedRatingScore.NotInspected,
            OfstedRatingScore.NotInspected,
            OfstedRatingScore.NotInspected,
            CategoriesOfConcern.NotInspected,
            SafeguardingScore.NotInspected,
            inspectionDate
        )
    {
    }

    public static SafeguardingScore ConvertStringToSafeguardingScore(string? input)
    {
        switch (input)
        {
            case SafeguardingScoreString.Yes:
                return SafeguardingScore.Yes;
            case SafeguardingScoreString.No:
                return SafeguardingScore.No;
            case SafeguardingScoreString.Nine:
                return SafeguardingScore.NotRecorded;
            default:
                return SafeguardingScore.NotInspected;
        }
    }
}

public static class SafeguardingScoreString
{
    public const string Yes = "Yes";
    public const string No = "No";
    public const string Nine = "9";
}

public enum OfstedRatingScore
{
    NotInspected = -1,
    InsufficientEvidence = 0,
    Outstanding = 1,
    Good = 2,
    RequiresImprovement = 3,
    Inadequate = 4,
    DoesNotApply = 8,
    NoJudgement = 9
}

public enum SafeguardingScore
{
    NotInspected = -1,
    Yes,
    No,
    NotRecorded = 9
}

public enum CategoriesOfConcern
{
    NotInspected = -1,
    DoesNotApply,
    NoConcerns,
    SpecialMeasures,
    SeriousWeakness,
    NoticeToImprove
}
