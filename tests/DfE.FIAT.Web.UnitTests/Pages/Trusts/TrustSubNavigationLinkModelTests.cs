using DfE.FIAT.Web.Pages.Trusts;

namespace DfE.FIAT.Web.UnitTests.Pages.Trusts;

public class TrustSubNavigationLinkModelTests
{
    private readonly TrustSubNavigationLinkModel _baseLinkModel = new("", "", "", "", false);

    [Theory]
    [InlineData("Link Text", "Service Name", "service-name-link-text-subnav")]
    [InlineData("Link", "Page", "page-link-subnav")]
    [InlineData("Things (12)", "Page", "page-things-subnav")]
    [InlineData("Historic things (0)", "Other Page", "other-page-historic-things-subnav")]
    public void TestId_should_kebabify_service_name_and_link_text(string linkText, string serviceName,
        string expected)
    {
        var sut = _baseLinkModel with { LinkText = linkText, ServiceName = serviceName };

        sut.TestId.Should().Be(expected);
    }
}
