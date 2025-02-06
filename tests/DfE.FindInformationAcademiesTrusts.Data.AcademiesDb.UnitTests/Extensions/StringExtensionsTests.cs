using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Extensions;
using DfE.FindInformationAcademiesTrusts.Extensions;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests.Extensions;

public class StringExtensionsTests
{
    [Theory]
    [InlineData("not a date string")]
    [InlineData(" 01/01/2023 ")]
    [InlineData(" 01-01-2023 ")]
    [InlineData("\"01/01/2023\"")]
    [InlineData("\"01-01-2023\"")]
    [InlineData("01/01/20")]
    [InlineData("01-01-20")]
    [InlineData("2021/01/20")]
    [InlineData("2021-01-20")]
    [InlineData("20\\01\\2021")]
    public void ParseAsNullableDate_should_throw_argumentexception_when_unknown_date_string(string input)
    {
        var action = () => input.ParseAsNullableDate();

        action.Should().Throw<ArgumentException>().WithMessage($"Cannot parse date in unknown format - {input}");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("    ")]
    public void ParseAsNullableDate_should_return_null_when_passed_null_or_empty(string? input)
    {
        var result = input.ParseAsNullableDate();

        result.Should().BeNull();
    }

    [Theory]
    [MemberData(nameof(DateValues))]
    public void ParseAsNullableDate_should_return_correctly_parsed_date(string? input, DateTime? expected)
    {
        var result = input.ParseAsNullableDate();
        result.Should().Be(expected);
    }

    public static TheoryData<string?, DateTime?> DateValues =>
        new()
        {
            { "01/01/2023", new DateTime(2023, 01, 01) },
            { "01/12/2019", new DateTime(2019, 12, 01) },
            { "12/01/2017", new DateTime(2017, 01, 12) },
            { "29/02/2016", new DateTime(2016, 02, 29) },
            { "30/01/2015", new DateTime(2015, 01, 30) },
            { "01-01-2023", new DateTime(2023, 01, 01) },
            { "01-12-2019", new DateTime(2019, 12, 01) },
            { "12-01-2017", new DateTime(2017, 01, 12) },
            { "29-02-2016", new DateTime(2016, 02, 29) },
            { "30-01-2015", new DateTime(2015, 01, 30) }
        };

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("    ")]
    public void ParseAsNullableInt_should_return_null_when_passed_null_or_empty(string? input)
    {
        var result = input.ParseAsNullableInt();

        result.Should().BeNull();
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

    [Fact]
    public void DefaultIfNull_should_return_the_string_if_not_null()
    {
        const string? text = "Test string";
        text.DefaultIfNull("Fallback string").Should().Be("Test string");
    }

    [Fact]
    public void DefaultIfNull_should_return_the_default_string_if_null()
    {
        const string? text = null;
        text.DefaultIfNull("Fallback string").Should().Be("Fallback string");
    }

    [Theory]
    [InlineData("t")]
    [InlineData("test")]
    [InlineData("12ee")]
    [InlineData("%")]
    public void ParseAsNullableInt_should_return_null_when_passed_value_that_cannot_be_parsed(string? input)
    {
        var result = input.ParseAsNullableInt();

        result.Should().BeNull();
    }

    [Theory]
    [InlineData("0", 0)]
    [InlineData("1", 1)]
    [InlineData("1000", 1000)]
    [InlineData("99", 99)]
    public void ParseAsNullableInt_should_return_correctly_parsed_int(string? input, int? expected)
    {
        var result = input.ParseAsNullableInt();
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("0", 0D)]
    [InlineData("1", 1D)]
    [InlineData("1.5", 1.5)]
    [InlineData("1000", 1000D)]
    [InlineData("99.99", 99.99)]
    [InlineData("0.01", 0.01)]
    [InlineData("", null)]
    [InlineData("test", null)]
    public void ParseAsNullableDouble_should_return_correctly_parsed_double(string? input, double? expected)
    {
        var result = input.ParseAsNullableDouble();
        result.Should().BeApproximately(expected, 0.01);
    }
}
