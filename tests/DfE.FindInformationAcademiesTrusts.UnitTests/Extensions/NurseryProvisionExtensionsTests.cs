namespace DfE.FindInformationAcademiesTrusts.UnitTests.Extensions
{
    using DfE.FindInformationAcademiesTrusts.Services.School;

    public class NurseryProvisionExtensionsTests
    {
        [Theory]
        [InlineData(NurseryProvision.HasClasses, "Yes")]
        [InlineData(NurseryProvision.NoClasses, "No")]
        [InlineData(NurseryProvision.NotRecorded, "Not Recorded")]
        public void ToText_ReturnsCorrectString_ForDefinedEnumValues(NurseryProvision provision, string expected)
        {
            var result = provision.ToText();
            result.Should().Be(expected);
        }
    }
}
