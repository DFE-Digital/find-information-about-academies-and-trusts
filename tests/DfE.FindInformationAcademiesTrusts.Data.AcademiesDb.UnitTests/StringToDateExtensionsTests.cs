namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests;

public class StringToDateExtensionsTests
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

    public static IEnumerable<object[]> DateValues =>
        new List<object[]>
        {
            new object[] { "01/01/2023", new DateTime(2023, 01, 01) },
            new object[] { "01/12/2019", new DateTime(2019, 12, 01) },
            new object[] { "12/01/2017", new DateTime(2017, 01, 12) },
            new object[] { "29/02/2016", new DateTime(2016, 02, 29) },
            new object[] { "30/01/2015", new DateTime(2015, 01, 30) },
            new object[] { "01-01-2023", new DateTime(2023, 01, 01) },
            new object[] { "01-12-2019", new DateTime(2019, 12, 01) },
            new object[] { "12-01-2017", new DateTime(2017, 01, 12) },
            new object[] { "29-02-2016", new DateTime(2016, 02, 29) },
            new object[] { "30-01-2015", new DateTime(2015, 01, 30) }
        };
}
