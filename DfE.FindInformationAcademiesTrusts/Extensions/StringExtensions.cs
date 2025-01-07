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

    public static string Kebabify(this string text)
    {
        var transformedText = text.Trim();

        transformedText = BracketedDigitsRegex().Replace(transformedText, "-");
        transformedText = SpacesRegex().Replace(transformedText, "-");
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

    public static string ReplaceWhitespaces(this string input, string replacement)
    {
        return Regex.Replace(input.Trim().ToLowerInvariant(), @"\s+", replacement, RegexOptions.Compiled,
            TimeSpan.FromMilliseconds(500));
    }

    public static string RemovePunctuation(this string input)
    {
        return Regex.Replace(input.Trim().ToLowerInvariant(), @"[^\s\w]|_+", "", RegexOptions.Compiled,
            TimeSpan.FromMilliseconds(500));
    }
}
