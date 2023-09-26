using DfE.FindInformationAcademiesTrusts.Pages.Trusts;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages;

public class DetailsModelTests
{
    [Fact]
    public async void OnGetAsync_should_fetch_a_trust_by_ukprn()
    {
        var mockTrustProvider = new Mock<ITrustProvider>();
        mockTrustProvider.Setup(s => s.GetTrustByUkprnAsync("1234").Result)
            .Returns(new Trust("test", "test", "Multi-academy trust"));
        var sut = new DetailsModel(mockTrustProvider.Object)
        {
            Ukprn = "1234"
        };

        await sut.OnGetAsync();
        sut.Trust.Should().BeEquivalentTo(new Trust("test", "test", "Multi-academy trust"));
    }

    [Fact]
    public async void Ukprn_should_be_empty_string_by_default()
    {
        var mockTrustProvider = new Mock<ITrustProvider>();
        var sut = new DetailsModel(mockTrustProvider.Object);

        await sut.OnGetAsync();
        sut.Ukprn.Should().BeEquivalentTo(string.Empty);
    }

    [Fact]
    public void PageName_should_be_Details()
    {
        var mockTrustProvider = new Mock<ITrustProvider>();
        var sut = new DetailsModel(mockTrustProvider.Object);
        sut.PageName.Should().Be("Details");
    }

    [Fact]
    public void PageSection_should_be_AboutTheTrust()
    {
        var mockTrustProvider = new Mock<ITrustProvider>();
        var sut = new DetailsModel(mockTrustProvider.Object);
        sut.Section.Should().Be("About the trust");
    }

    [Fact]
    public async void OnGetAsync_should_return_not_found_result_if_trust_is_not_found()
    {
        var mockTrustProvider = new Mock<ITrustProvider>();
        mockTrustProvider.Setup(s => s.GetTrustByUkprnAsync("1111").Result)
            .Returns((Trust?)null);

        var sut = new DetailsModel(mockTrustProvider.Object)
        {
            Ukprn = "1111"
        };
        var result = await sut.OnGetAsync();
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async void OnGetAsync_should_return_not_found_result_if_Ukprn_is_not_provided()
    {
        var mockTrustProvider = new Mock<ITrustProvider>();

        var sut = new DetailsModel(mockTrustProvider.Object);
        var result = await sut.OnGetAsync();
        result.Should().BeOfType<NotFoundResult>();
    }
}
