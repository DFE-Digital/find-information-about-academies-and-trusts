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

        return textInfo.ToTitleCase(textInfo.ToLower(text));
    }
}
