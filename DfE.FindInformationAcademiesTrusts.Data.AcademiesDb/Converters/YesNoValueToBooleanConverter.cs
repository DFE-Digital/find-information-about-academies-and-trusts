namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Converters;

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

public class YesNoValueToBooleanConverter : ValueConverter<bool?, string?>
{
    public YesNoValueToBooleanConverter() : base(
        // writing to the database
        v => (v == true ? "Yes" : v == false ? "No" : null) ?? string.Empty,
        value => ConvertYesNoString(value))
    {

    }

    private static bool? ConvertYesNoString(string? input)
    {
        if (input is null)
        {
            return null;
        }

        return input.Equals("yes", StringComparison.CurrentCultureIgnoreCase);
    }
}