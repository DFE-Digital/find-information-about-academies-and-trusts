using DfE.FindInformationAcademiesTrusts.Extensions;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Extensions;

public class StringExtensionsTest
{
    [Theory]
    [InlineData("UPPER", "upper")]
    [InlineData("inPut", "input")]
    [InlineData("lower", "lower")]
    public void Kebabify_should_lower_text(string linkText, string expected)
    {
        linkText.Kebabify().Should().Be(expected);
    }

    [Theory]
    [InlineData("    ", "")]
    [InlineData("  left", "left")]
    [InlineData("right  ", "right")]
    [InlineData(" middlish  ", "middlish")]
    public void Kebabify_should_trim_text(string linkText, string expected)
    {
        linkText.Kebabify().Should().Be(expected);
    }

    [Theory]
    [InlineData("left right", "left-right")]
    [InlineData("left middle right", "left-middle-right")]
    [InlineData("one with extra   space", "one-with-extra-space")]
    public void Kebabify_should_replace_spaces_with_dashes(string linkText, string expected)
    {
        linkText.Kebabify().Should().Be(expected);
    }

    [Theory]
    [InlineData("(42)", "")]
    [InlineData("left (middle) right", "left-right")]
    [InlineData("left(middle)right", "left-right")]
    [InlineData("left () right", "left-right")]
    [InlineData("(left) middle right", "middle-right")]
    [InlineData("left middle (right)", "left-middle")]
    [InlineData("one with (3)   spaces", "one-with-spaces")]
    public void Kebabify_should_remove_bracketed_text(string linkText, string expected)
    {
        linkText.Kebabify().Should().Be(expected);
    }

    [Theory]
    [InlineData("left--right", "left-right")]
    [InlineData("left----right", "left-right")]
    public void Kebabify_should_dedupe_dashes(string linkText, string expected)
    {
        linkText.Kebabify().Should().Be(expected);
    }

    [Theory]
    [InlineData("-", "")]
    [InlineData("-left", "left")]
    [InlineData("right-", "right")]
    [InlineData("-middlish--", "middlish")]
    public void Kebabify_should_not_start_or_end_with_dash(string linkText, string expected)
    {
        linkText.Kebabify().Should().Be(expected);
    }

    [Theory]
    [InlineData("hello, world!", "hello-world")]
    [InlineData("hel-lo, world!", "hel-lo-world")]
    [InlineData("goodbye: cruel; world...", "goodbye-cruel-world")]
    [InlineData("@hello#$%^&*()_+= world", "hello-world")]
    [InlineData("he\tllo!\nworld?", "helloworld")]
    [InlineData("\"quoted\" 'text'", "quoted-text")]
    [InlineData("   hello, world!  ", "hello-world")]
    public void Kebabify_should_remove_all_punctuation_characters(string input, string expected)
    {
        input.Kebabify().Should().Be(expected);
    }

    [Theory]
    [InlineData("test", "Test")]
    [InlineData("", "")]
    [InlineData(" ", "")]
    [InlineData("my test input", "My Test Input")]
    [InlineData("thIS is A LONG tESt inpUT", "This Is A Long Test Input")]
    [InlineData("tHIS iNPuT'S 'pOinT' IS to <TESt> PUnctuAtion;", "This Input's 'Point' Is To <Test> Punctuation;")]
    [InlineData(null, "")]
    public void ToTitleText_should_return_correct_string(string? inputText, string expected)
    {
        inputText!.ToTitleCase().Should().Be(expected);
    }
}
