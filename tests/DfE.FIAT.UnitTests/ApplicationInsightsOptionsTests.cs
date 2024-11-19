using DfE.FIAT.Web.Options;

namespace DfE.FIAT.UnitTests;

public class ApplicationInsightsOptionsTests
{
    [Fact]
    public void Configuration_section_should_be_ApplicationInsights()
    {
        ApplicationInsightsOptions.ConfigurationSection.Should().Be("ApplicationInsights");
    }

    [Fact]
    public void ConnectionString_should_default_to_null()
    {
        ApplicationInsightsOptions sut = new();
        sut.ConnectionString.Should().BeNull();
    }
}
