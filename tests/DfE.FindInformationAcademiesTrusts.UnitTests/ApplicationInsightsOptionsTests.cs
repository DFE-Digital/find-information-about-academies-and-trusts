namespace DfE.FindInformationAcademiesTrusts.UnitTests;

public class ApplicationInsightsOptionsTests
{
    [Fact]
    public void ApplicationInsightsOptions_Should_Return_Correct_Section()
    {
        var expectedConfigurationSection = "ApplicationInsights";
        var sut = ApplicationInsightsOptions.ConfigurationSection;
        sut.Should().BeEquivalentTo(expectedConfigurationSection);
    }
}
