using DfE.FIAT.Data;

namespace DfE.FIAT.Web.Extensions;

public static class CategoriesOfConcernExtensions
{
    public static string ToDataSortValue(this CategoriesOfConcern rating)
    {
        return rating.ToDisplayString().ToLowerInvariant().Trim();
    }

    public static string ToDisplayString(this CategoriesOfConcern rating)
    {
        return rating switch
        {
            CategoriesOfConcern.NoConcerns => "None",
            CategoriesOfConcern.SpecialMeasures => "Special measures",
            CategoriesOfConcern.SeriousWeakness => "Serious weakness",
            CategoriesOfConcern.NoticeToImprove => "Notice to improve",
            CategoriesOfConcern.NotInspected => "Not yet inspected",
            CategoriesOfConcern.DoesNotApply => "Does not apply",
            _ => "Unknown"
        };
    }
}
