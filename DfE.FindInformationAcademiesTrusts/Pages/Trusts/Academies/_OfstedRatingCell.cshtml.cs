namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.Academies;

internal static class OfstedRatingConstants
{
    public const string NotYetInspected = "Not yet inspected";
}

public class OfstedRatingCellModel
{
    public required DateTime? AcademyJoinedDate { get; init; }
    public required string Rating { get; init; }
    public required DateTime? RatingDate { get; init; }

    public bool HasRating()
    {
        return Rating != OfstedRatingConstants.NotYetInspected;
    }

    public bool IsAfterJoining()
    {
        return RatingDate >= AcademyJoinedDate;
    }

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
