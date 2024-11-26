using DfE.FindInformationAcademiesTrusts.Data;

namespace DfE.FindInformationAcademiesTrusts.Extensions;

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
            OfstedRatingScore.None => 5,
            OfstedRatingScore.DoesNotApply => 8,
            OfstedRatingScore.NoJudgement => 9,
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
            OfstedRatingScore.None => "Not yet inspected",
            OfstedRatingScore.NoJudgement => "No Judgement",
            OfstedRatingScore.DoesNotApply => "Does not apply",
            _ => "Unknown"
        };
    }
}
