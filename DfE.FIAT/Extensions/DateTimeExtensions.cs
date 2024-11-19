using DfE.FIAT.Web.Pages;

namespace DfE.FIAT.Web.Extensions;

public static class DateTimeExtensions
{
    public static string ShowDateStringOrReplaceWithText(this DateTime? date)
    {
        if (date.HasValue)
        {
            return date.Value.ToString(StringFormatConstants.ViewDate);
        }

        return ViewConstants.NoDataText;
    }
}
