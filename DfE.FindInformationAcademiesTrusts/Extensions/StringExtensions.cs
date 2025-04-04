using DfE.FindInformationAcademiesTrusts.Pages;
using System.Globalization;
using System.Text.RegularExpressions;

namespace DfE.FindInformationAcademiesTrusts.Extensions;

public static partial class StringExtensions
{
    [GeneratedRegex(@" *\([^)]*\) *", RegexOptions.Compiled)]
    private static partial Regex BracketedDigitsRegex();

    [GeneratedRegex(" +", RegexOptions.Compiled)]
    private static partial Regex SpacesRegex();

    [GeneratedRegex("-+", RegexOptions.Compiled)]
    private static partial Regex DashesRegex();

    [GeneratedRegex(@"[^-\w]|_+", RegexOptions.Compiled)]
    private static partial Regex NonDashPunctuationRegex();

    public static string Kebabify(this string text)
    {
        var transformedText = text.Trim();

        transformedText = BracketedDigitsRegex().Replace(transformedText, "-");
        transformedText = SpacesRegex().Replace(transformedText, "-");
        transformedText = NonDashPunctuationRegex().Replace(transformedText, "");
        transformedText = DashesRegex().Replace(transformedText, "-");

        transformedText = transformedText.Trim('-');
        transformedText = transformedText.ToLowerInvariant();

        return transformedText;
    }

    public static string ToTitleCase(this string text)
    {
        var textInfo = CultureInfo.CurrentCulture.TextInfo;

        if (string.IsNullOrWhiteSpace(text)) return string.Empty;

        return textInfo.ToTitleCase(textInfo.ToLower(text));
    }

    public static string DefaultIfNullOrWhiteSpace(this string? input, string defaultValue = ViewConstants.NoDataText)
    {
        return string.IsNullOrWhiteSpace(input) ? defaultValue : input;
    }
}
