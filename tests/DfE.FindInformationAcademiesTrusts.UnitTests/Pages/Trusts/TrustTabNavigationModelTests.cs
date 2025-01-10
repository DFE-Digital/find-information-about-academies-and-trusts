using DfE.FindInformationAcademiesTrusts.Pages.Trusts;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts;

public class TrustTabNavigationModelTests
{
    private readonly TrustTabNavigationLinkModel _baseLinkModel = new("", "", "", "", false);

    [Theory]
    [InlineData("Link Text", "Service Name", "service-name-link-text-tab")]
    [InlineData("Link", "Page", "page-link-tab")]
    [InlineData("Things (12)", "Page", "page-things-tab")]
    [InlineData("Historic things (0)", "Other Page", "other-page-historic-things-tab")]
    public void TestId_should_kebabify_service_name_and_link_text(string linkText, string subPageName,
        string expected)
    {
        var sut = _baseLinkModel with { LinkText = linkText, SubNavName = subPageName };

        sut.TestId.Should().Be(expected);
    }
}
