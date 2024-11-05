namespace DfE.FindInformationAcademiesTrusts.Data;

public record OfstedRating(
    OfstedRatingScore OverallEffectiveness,
    OfstedRatingScore QualityOfEducation,
    OfstedRatingScore BehaviourAndAttitudues,
    OfstedRatingScore PersonalDevelopment,
    OfstedRatingScore EffectivenessOfLeadershipAndManagement,
    OfstedRatingScore EarlyYearsProvision,
    OfstedRatingScore SixthFormProvision,
    CategoriesOfConcern CategoryOfConcern,
    SafeguardingScore SafeguradingIsEffective,
    DateTime? InspectionDate)
{
    public static readonly OfstedRating None = new(OfstedRatingScore.None, OfstedRatingScore.None,
        OfstedRatingScore.None, OfstedRatingScore.None,
        OfstedRatingScore.None, OfstedRatingScore.None, OfstedRatingScore.None, CategoriesOfConcern.None,
        SafeguardingScore.None, null);

    public OfstedRating(int? overallEffectiveness, DateTime? inspectionDate)
        : this(
            (OfstedRatingScore?)overallEffectiveness ?? OfstedRatingScore.None,
            OfstedRatingScore.None,
            OfstedRatingScore.None,
            OfstedRatingScore.None,
            OfstedRatingScore.None,
            OfstedRatingScore.None,
            OfstedRatingScore.None,
            CategoriesOfConcern.None,
            SafeguardingScore.None,
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
                return SafeguardingScore.None;
        }
    }

    public static CategoriesOfConcern ConvertStringToCategoriesOfConcern(string? input)
    {
        switch (input)
        {
            case CategoriesOfConcernString.SpecialMeasures:
                return CategoriesOfConcern.SpecialMeasures;
            case CategoriesOfConcernString.SeriousWeakness:
                return CategoriesOfConcern.SeriousWeakness;
            case CategoriesOfConcernString.NoticeToImprove:
                return CategoriesOfConcern.NoticeToImprove;
            default:
                return CategoriesOfConcern.None;
        }
    }
}

public static class SafeguardingScoreString
{
    public const string Yes = "Yes";
    public const string No = "No";
    public const string Nine = "9";
}

public static class CategoriesOfConcernString
{
    public const string SpecialMeasures = "SM";
    public const string SeriousWeakness = "SWK";
    public const string NoticeToImprove = "NTI";
}

public enum OfstedRatingScore
{
    None = -1,
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
    None = -1,
    Yes,
    No,
    NotRecorded = 9
}

public enum CategoriesOfConcern
{
    None = -1,
    SpecialMeasures,
    SeriousWeakness,
    NoticeToImprove
}
