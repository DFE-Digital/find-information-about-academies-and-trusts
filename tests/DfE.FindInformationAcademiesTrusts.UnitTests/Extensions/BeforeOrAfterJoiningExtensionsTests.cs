using DfE.FindInformationAcademiesTrusts.Extensions;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Extensions
{
    using DfE.FindInformationAcademiesTrusts.Data.Enums;

    public class BeforeOrAfterJoiningExtensionsTests
    {
        [Theory]
        [InlineData(BeforeOrAfterJoining.Before, "Before joining")]
        [InlineData(BeforeOrAfterJoining.After, "After joining")]
        [InlineData(BeforeOrAfterJoining.NotYetInspected, "Unknown")]
        public void ToDisplayString_ReturnsCorrectString_ForDefinedEnumValues(BeforeOrAfterJoining beforeOrAfterJoining, string expected)
        {
            // Act
            var result = beforeOrAfterJoining.ToDisplayString();

            // Assert
            result.Should().Be(expected);
        }
    }
}
