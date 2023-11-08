namespace DfE.FindInformationAcademiesTrusts.Data;

public enum OfstedRatingScore
{
    Outstanding,
    Good
}

public record OfstedRating(OfstedRatingScore OfstedRatingScore, DateTime InspectionEndDate);
