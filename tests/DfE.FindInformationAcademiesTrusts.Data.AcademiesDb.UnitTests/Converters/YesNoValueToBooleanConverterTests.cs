using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Converters;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests.Converters;

public class YesNoValueToBooleanConverterTests
{
    [Theory]
    [MemberData(nameof(stringCombinations))]
    public void ConvertFrom_ShouldCorrectlyParseYesOrNo_IntoBool(string? stringValue, bool? expected)
    {
        var result = new YesNoValueToBooleanConverter().ConvertFromProvider(stringValue);
        result.Should().Be(expected);
    }

    public static TheoryData<string?, bool?> stringCombinations => new()
    {
        { "yes", true },
        { "Yes", true },
        { "YES", true },
        { "no", false },
        { "No", false },
        { "NO", false },
        { "", null },
        { null, null }
    };

    [Theory]
    [MemberData(nameof(boolCombinations))]
    public void ConvertTo_ShouldCorrectlyParseBool_IntoYesNoString(bool? boolValue, string? expected)
    {
        var result = new YesNoValueToBooleanConverter().ConvertToProvider(boolValue);
        result.Should().Be(expected);
    }

    public static TheoryData<bool?, string?> boolCombinations => new()
    {
        { true, "Yes" },
        { false, "No" },
        { null, null }
    };
}