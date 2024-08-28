using DfE.FindInformationAcademiesTrusts.Pages;

namespace DfE.FindInformationAcademiesTrusts.Extensions;

public static class DateTimeExtensions
{
    public static string ShowDateStringOrReplaceWithText(this DateTime? date, string replacementText = "No Data",
        string format = StringFormatConstants.ViewDate)
    {
        if (date.HasValue)
        {
            return date.Value.ToString(format);
        }

        return replacementText;
    }
}
