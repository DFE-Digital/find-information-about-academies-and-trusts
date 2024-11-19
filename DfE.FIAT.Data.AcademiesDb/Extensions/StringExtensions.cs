using System.Globalization;
using System.Text.RegularExpressions;

namespace DfE.FIAT.Data.AcademiesDb.Extensions;

public static class StringExtensions
{
    private static readonly Regex SlashRegex = new(@"^\d\d/\d\d/\d\d\d\d$", RegexOptions.NonBacktracking);
    private static readonly Regex DashRegex = new(@"^\d\d\-\d\d\-\d\d\d\d$", RegexOptions.NonBacktracking);

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

    public static int? ParseAsNullableInt(this string? numberString)
    {
        return int.TryParse(numberString, out var number) ? number : null;
    }

    public static double? ParseAsNullableDouble(this string? numberString)
    {
        return double.TryParse(numberString, out var number) ? number : null;
    }
}
