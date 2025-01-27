using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Extensions;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.Ofsted;

public record SingleHeadlineGradeCellsModel(
    OfstedRating SingleHeadlineGradeRating,
    BeforeOrAfterJoining BeforeOrAfterJoining,
    bool IsCurrent)
{
    public bool HasInspection => SingleHeadlineGradeRating.OverallEffectiveness != OfstedRatingScore.NotInspected;

    public string TagLabel => BeforeOrAfterJoining switch
    {
        BeforeOrAfterJoining.Before => "Before joining",
        BeforeOrAfterJoining.After => "After joining",
        _ => "Unknown"
    };

    public bool IsBeforeJoining => BeforeOrAfterJoining == BeforeOrAfterJoining.Before;

    public bool RatingShouldBeBold => SingleHeadlineGradeRating.OverallEffectiveness switch
    {
        OfstedRatingScore.Outstanding
            or OfstedRatingScore.Good
            or OfstedRatingScore.RequiresImprovement
            or OfstedRatingScore.Inadequate => true,
        _ => false
    };

    public string InspectionDateText => HasInspection
        ? SingleHeadlineGradeRating.InspectionDate.ShowDateStringOrReplaceWithText()
        : OfstedRatingScore.NotInspected.ToDisplayString(IsCurrent);

    public string InspectionDateSort =>
        SingleHeadlineGradeRating.InspectionDate?.ToString(StringFormatConstants.SortableDateFormat)
        ?? "0";

    public string SingleHeadlineGradeText => SingleHeadlineGradeRating.OverallEffectiveness.ToDisplayString(IsCurrent);
    public int SingleHeadlineGradeSort => SingleHeadlineGradeRating.OverallEffectiveness.ToDataSortValue();
}
