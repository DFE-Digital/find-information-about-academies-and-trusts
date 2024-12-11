using DfE.FindInformationAcademiesTrusts.Pages.Trusts;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts;

public class TrustPageMetadataTests
{
    [Fact]
    public void TrustPageMetadata_can_be_composed_in_parts()
    {
        //This is similar to how it is being used in the page object models
        var sut = new TrustPageMetadata("MY TRUST");
        sut.BrowserTitle.Should().Be("MY TRUST");

        sut = sut with { PageName = "Page" };
        sut.BrowserTitle.Should().Be("Page - MY TRUST");

        sut = sut with { SubPageName = "Sub page" };
        sut.BrowserTitle.Should().Be("Sub page - Page - MY TRUST");

        sut = sut with { TabName = "The tab" };
        sut.BrowserTitle.Should().Be("The tab - Sub page - Page - MY TRUST");
    }

    [Theory]
    [InlineData("MY TRUST", "Page", "Sub page", "The tab", "The tab - Sub page - Page - MY TRUST")]
    [InlineData("OTHER TRUST", "Page", "Sub page", null, "Sub page - Page - OTHER TRUST")]
    [InlineData("OTHER TRUST", "Page", null, null, "Page - OTHER TRUST")]
    [InlineData("OTHER TRUST", null, null, null, "OTHER TRUST")]
    [InlineData("MY TRUST", null, null, "The tab", "The tab - MY TRUST")]
    [InlineData("MY TRUST", null, "Sub page", "The tab", "The tab - Sub page - MY TRUST")]
    [InlineData("MY TRUST", "Page", null, "The tab", "The tab - Page - MY TRUST")]
    public void BrowserTitle_only_uses_present_strings(string trustName, string? pageName, string? subPageName,
        string? tabName, string expected)
    {
        var sut = new TrustPageMetadata(trustName, pageName, subPageName, tabName);

        sut.BrowserTitle.Should().Be(expected);
    }
}
