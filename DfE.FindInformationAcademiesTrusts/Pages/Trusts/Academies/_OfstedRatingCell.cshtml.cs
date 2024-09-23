using DfE.FindInformationAcademiesTrusts.Data;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.Academies;

public class OfstedRatingCellModel
{
    public required DateTime? AcademyJoinedDate { get; init; }

    public required OfstedRating OfstedRating { get; init; }

    public string? IdPrefix { get; set; }

    public bool IsAfterJoining => OfstedRating.InspectionDate >= AcademyJoinedDate;

    public string? OfstedRatingDescription => OfstedRating.OfstedRatingScore switch
    {
        OfstedRatingScore.None => "Not yet inspected",
        OfstedRatingScore.Outstanding => "Outstanding",
        OfstedRatingScore.Good => "Good",
        OfstedRatingScore.RequiresImprovement => "Requires improvement",
        OfstedRatingScore.Inadequate => "Inadequate",
        _ => string.Empty
    };
    public int OfstedRatingSortValue
    {
        get
        {
            if (OfstedRating.OfstedRatingScore == OfstedRatingScore.None) return 5;
            return (int)OfstedRating.OfstedRatingScore;
        }
    }

    public string TagClasses
    {
        get
        {
            var tag = "govuk-tag";
            if (!IsAfterJoining) tag += " govuk-tag--grey";
            return tag;
        }
    }

    public string TagText => IsAfterJoining ? "After joining" : "Before joining";
}
