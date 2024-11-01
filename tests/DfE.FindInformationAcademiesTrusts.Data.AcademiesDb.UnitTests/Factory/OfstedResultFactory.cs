namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests.Factory;

public static class OfstedResultFactory
{
    public static OfstedRating OutstandingEstablishmentOfstedRating(DateTime inspectionDate)
    {
        return new OfstedRating(
            OfstedRatingScore.Outstanding, OfstedRatingScore.Outstanding, OfstedRatingScore.Outstanding,
            OfstedRatingScore.Outstanding, OfstedRatingScore.Outstanding, OfstedRatingScore.Outstanding,
            OfstedRatingScore.Outstanding, CategoriesOfConcern.None, SafeguardingScore.Yes, inspectionDate);
    }

    public static OfstedRating GoodEstablishmentOfstedRating(DateTime inspectionDate)
    {
        return new OfstedRating(
            OfstedRatingScore.Good, OfstedRatingScore.Good, OfstedRatingScore.Good,
            OfstedRatingScore.Good, OfstedRatingScore.Good, OfstedRatingScore.Good,
            OfstedRatingScore.Good, CategoriesOfConcern.NoticeToImprove, SafeguardingScore.None, inspectionDate);
    }

    public static OfstedRating RequiresImprovementEstablishmentOfstedRating(DateTime inspectionDate)
    {
        return new OfstedRating(
            OfstedRatingScore.RequiresImprovement, OfstedRatingScore.RequiresImprovement,
            OfstedRatingScore.RequiresImprovement,
            OfstedRatingScore.RequiresImprovement, OfstedRatingScore.RequiresImprovement,
            OfstedRatingScore.RequiresImprovement,
            OfstedRatingScore.RequiresImprovement, CategoriesOfConcern.SpecialMeasures, SafeguardingScore.NotRecorded,
            inspectionDate);
    }

    public static OfstedRating InadequateEstablishmentOfstedRating(DateTime inspectionDate)
    {
        return new OfstedRating(
            OfstedRatingScore.Inadequate, OfstedRatingScore.Inadequate, OfstedRatingScore.Inadequate,
            OfstedRatingScore.Inadequate, OfstedRatingScore.Inadequate, OfstedRatingScore.Inadequate,
            OfstedRatingScore.Inadequate, CategoriesOfConcern.SeriousWeakness, SafeguardingScore.No, inspectionDate);
    }

    public static OfstedRating NoneEstablishmentOfstedRating(DateTime? inspectionDate)
    {
        return new OfstedRating(-1, inspectionDate);
    }

    public static OfstedRating OutstandingFurtherEstablishmentOfstedRating(DateTime inspectionDate)
    {
        return new OfstedRating(
            OfstedRatingScore.Outstanding, OfstedRatingScore.Outstanding, OfstedRatingScore.Outstanding,
            OfstedRatingScore.Outstanding, OfstedRatingScore.Outstanding, OfstedRatingScore.None,
            OfstedRatingScore.None, CategoriesOfConcern.None, SafeguardingScore.Yes, inspectionDate);
    }

    public static OfstedRating GoodFurtherEstablishmentOfstedRating(DateTime inspectionDate)
    {
        return new OfstedRating(
            OfstedRatingScore.Good, OfstedRatingScore.Good, OfstedRatingScore.Good,
            OfstedRatingScore.Good, OfstedRatingScore.Good, OfstedRatingScore.None,
            OfstedRatingScore.None, CategoriesOfConcern.None, SafeguardingScore.None, inspectionDate);
    }

    public static OfstedRating RequiresFurtherImprovementEstablishmentOfstedRating(DateTime inspectionDate)
    {
        return new OfstedRating(
            OfstedRatingScore.RequiresImprovement, OfstedRatingScore.RequiresImprovement,
            OfstedRatingScore.RequiresImprovement,
            OfstedRatingScore.RequiresImprovement, OfstedRatingScore.RequiresImprovement,
            OfstedRatingScore.None,
            OfstedRatingScore.None, CategoriesOfConcern.None, SafeguardingScore.NotRecorded, inspectionDate);
    }

    public static OfstedRating InadequateFurtherEstablishmentOfstedRating(DateTime inspectionDate)
    {
        return new OfstedRating(
            OfstedRatingScore.Inadequate, OfstedRatingScore.Inadequate, OfstedRatingScore.Inadequate,
            OfstedRatingScore.Inadequate, OfstedRatingScore.Inadequate, OfstedRatingScore.None,
            OfstedRatingScore.None, CategoriesOfConcern.None, SafeguardingScore.No, inspectionDate);
    }

    public static OfstedRating NoneFurtherEstablishmentOfstedRating(DateTime? inspectionDate)
    {
        return new OfstedRating(-1, inspectionDate);
    }
}
