using DfE.FindInformationAcademiesTrusts.Data;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.Academies;

public class OfstedRatingCellModel
{
    public required DateTime? AcademyJoinedDate { get; init; }

    public required OfstedRating OfstedRating { get; init; }

    public bool IsAfterJoining()
    {
        return OfstedRating.InspectionEndDate >= AcademyJoinedDate;
    }

    public string? OfstedRatingDescription => OfstedRating.OfstedRatingScore switch
    {
        OfstedRatingScore.None => "Not yet inspected",
        OfstedRatingScore.Outstanding => "Outstanding",
        OfstedRatingScore.Good => "Good",
        OfstedRatingScore.RequiresImprovement => "Requires improvement",
        OfstedRatingScore.Inadequate => "Inadequate",
        _ => string.Empty
    };

    public string GetTagClasses()
    {
        var tag = "govuk-tag";
        if (!IsAfterJoining()) tag += " govuk-tag--grey";
        return tag;
    }

    public string GetTagText()
    {
        return IsAfterJoining() ? "After joining" : "Before joining";
    }
}
