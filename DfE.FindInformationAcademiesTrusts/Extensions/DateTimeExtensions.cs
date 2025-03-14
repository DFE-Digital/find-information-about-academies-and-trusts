﻿using DfE.FindInformationAcademiesTrusts.Pages;

namespace DfE.FindInformationAcademiesTrusts.Extensions;

public static class DateTimeExtensions
{
    public static string ShowDateStringOrReplaceWithText(this DateTime? date,
        string replacementText = ViewConstants.NoDataText)
    {
        if (date.HasValue)
        {
            return date.Value.ToString(StringFormatConstants.DisplayDateFormat);
        }

        return replacementText;
    }
}
