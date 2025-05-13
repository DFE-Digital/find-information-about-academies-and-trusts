using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Extensions;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Extensions;

public class AgeRangeExtensionsTests
{
    [Theory]
    [InlineData(0, 100, "0 to 100")]
    [InlineData(2, 9, "2 to 9")]
    [InlineData(11, 18, "11 to 18")]
    public void ToFullDisplayString_should_use_min_and_max(int min, int max, string expected)
    {
        new AgeRange(min, max).ToFullDisplayString().Should().Be(expected);
    }
}
