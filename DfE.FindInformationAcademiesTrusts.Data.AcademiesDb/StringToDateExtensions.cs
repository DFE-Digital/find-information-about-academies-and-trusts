using System.Globalization;
using System.Text.RegularExpressions;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb;

public static class StringToDateExtensions
{
    private static readonly Regex SlashRegex = new(@"^\d\d/\d\d/\d\d\d\d$");
    private static readonly Regex DashRegex = new(@"^\d\d\-\d\d\-\d\d\d\d$");

    public static DateTime? ParseAsNullableDate(this string? dateString)
    {
        if (string.IsNullOrWhiteSpace(dateString)) return null;

        if (SlashRegex.IsMatch(dateString))
        {
            return DateTime.ParseExact(dateString, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        }

        if (DashRegex.IsMatch(dateString))
        {
            return DateTime.ParseExact(dateString, "dd-MM-yyyy", CultureInfo.InvariantCulture);
        }

        throw new ArgumentException($"Cannot parse date in unknown format - {dateString}");
    }
}
