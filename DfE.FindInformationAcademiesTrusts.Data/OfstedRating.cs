namespace DfE.FindInformationAcademiesTrusts.Data;

public record OfstedRating(OfstedRatingScore OfstedRatingScore, DateTime? InspectionEndDate)
{
    public static readonly OfstedRating None = new(OfstedRatingScore.None, null);
}

public enum OfstedRatingScore
{
    None = -1,
    Outstanding = 1,
    Good = 2,
    RequiresImprovement = 3,
    Inadequate = 4
}
