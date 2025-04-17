using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Extensions;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Extensions;

public class SchoolCategoryExtensionsTests
{
    [Theory]
    [InlineData(SchoolCategory.LaMaintainedSchool, "School")]
    [InlineData(SchoolCategory.Academy, "Academy")]
    public void ToDisplayString_returns_expected_value(
        SchoolCategory schoolCategory, string expected)
    {
        var result = schoolCategory.ToDisplayString();
        result.Should().Be(expected);
    }

    [Fact]
    public void ToDisplayString__throws_when_mapping_is_invalid()
    {
        var act = () => ((SchoolCategory)9999).ToDisplayString();
        act.Should().Throw<ArgumentException>();
    }
}
