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
            OfstedRatingScore.DoesNotApply => 5,
            OfstedRatingScore.NoJudgement => 6,
            OfstedRatingScore.None => 7,
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
            OfstedRatingScore.NoJudgement => "No judgement",
            OfstedRatingScore.DoesNotApply => "Does not apply",
            _ => "Unknown"
        };
    }
}
