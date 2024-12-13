using DfE.FIAT.Data;

namespace DfE.FIAT.Web.Extensions;

public static class OfstedRatingScoreExtensions
{
    public static int ToDataSortValue(this OfstedRatingScore rating)
    {
        return rating switch
        {
            OfstedRatingScore.Outstanding => 1,
            OfstedRatingScore.Good => 2,
            OfstedRatingScore.RequiresImprovement => 3,
            OfstedRatingScore.Inadequate => 4,
            OfstedRatingScore.InsufficientEvidence => 5,
            OfstedRatingScore.NoJudgement => 6,
            OfstedRatingScore.DoesNotApply => 7,
            OfstedRatingScore.NotInspected => 8,
            _ => -1
        };
    }

    public static string ToDisplayString(this OfstedRatingScore rating)
    {
        return rating switch
        {
            OfstedRatingScore.Outstanding => "Outstanding",
            OfstedRatingScore.Good => "Good",
            OfstedRatingScore.RequiresImprovement => "Requires improvement",
            OfstedRatingScore.Inadequate => "Inadequate",
            OfstedRatingScore.InsufficientEvidence => "Insufficient evidence",
            OfstedRatingScore.NoJudgement => "No judgement",
            OfstedRatingScore.DoesNotApply => "Does not apply",
            OfstedRatingScore.NotInspected => "Not yet inspected",
            _ => "Unknown"
        };
    }
}
