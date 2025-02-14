namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Converters;

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

public class YesNoValueToBooleanConverter() :
    ValueConverter<bool?, string?>(
        value => ConvertBoolToYesNoString(value),
        value => ConvertYesNoString(value))
{
    private static bool? ConvertYesNoString(string? input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return null;
        }

        return input.Equals("yes", StringComparison.CurrentCultureIgnoreCase);
    }

    private static string? ConvertBoolToYesNoString(bool? input)
    {
        if (input is null)
        {
            return null;
        }

        return input.Value ? "Yes" : "No";
    }
}