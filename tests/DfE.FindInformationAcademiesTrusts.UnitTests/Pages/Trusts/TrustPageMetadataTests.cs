using DfE.FindInformationAcademiesTrusts.Pages.Trusts;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts;

public class TrustPageMetadataTests
{
    [Fact]
    public void TrustPageMetadata_can_be_composed_in_parts()
    {
        //This is similar to how it is being used in the page object models
        var sut = new TrustPageMetadata("MY TRUST", true);
        sut.BrowserTitle.Should().Be("MY TRUST");

        sut = sut with { PageName = "Page" };
        sut.BrowserTitle.Should().Be("Page - MY TRUST");

        sut = sut with { SubPageName = "Sub page" };
        sut.BrowserTitle.Should().Be("Sub page - Page - MY TRUST");

        sut = sut with { TabName = "The tab" };
        sut.BrowserTitle.Should().Be("The tab - Sub page - Page - MY TRUST");
    }

    [Theory]
    [InlineData("MY TRUST", true, "Page", "Sub page", "The tab", "The tab - Sub page - Page - MY TRUST")]
    [InlineData("MY TRUST", false, "Page", "Sub page", "The tab", "Error: The tab - Sub page - Page - MY TRUST")]
    [InlineData("OTHER TRUST", true, "Page", "Sub page", null, "Sub page - Page - OTHER TRUST")]
    [InlineData("OTHER TRUST", false, "Page", "Sub page", null, "Error: Sub page - Page - OTHER TRUST")]
    [InlineData("OTHER TRUST", true, "Page", null, null, "Page - OTHER TRUST")]
    [InlineData("OTHER TRUST", false, "Page", null, null, "Error: Page - OTHER TRUST")]
    [InlineData("OTHER TRUST", true, null, null, null, "OTHER TRUST")]
    [InlineData("OTHER TRUST", false, null, null, null, "Error: OTHER TRUST")]
    [InlineData("MY TRUST", true, null, null, "The tab", "The tab - MY TRUST")]
    [InlineData("MY TRUST", false, null, null, "The tab", "Error: The tab - MY TRUST")]
    [InlineData("MY TRUST", true, null, "Sub page", "The tab", "The tab - Sub page - MY TRUST")]
    [InlineData("MY TRUST", false, null, "Sub page", "The tab", "Error: The tab - Sub page - MY TRUST")]
    [InlineData("MY TRUST", true, "Page", null, "The tab", "The tab - Page - MY TRUST")]
    [InlineData("MY TRUST", false, "Page", null, "The tab", "Error: The tab - Page - MY TRUST")]
    public void BrowserTitle_should_be_constructed_from_properties_and_modelstate(
        string trustName,
        bool modelStateIsValid,
        string? pageName,
        string? subPageName,
        string? tabName, string expected)
    {
        var sut = new TrustPageMetadata(trustName, modelStateIsValid, pageName, subPageName, tabName);

        sut.BrowserTitle.Should().Be(expected);
    }
}
