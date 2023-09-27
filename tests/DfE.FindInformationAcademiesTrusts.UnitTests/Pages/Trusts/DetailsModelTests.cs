using DfE.FindInformationAcademiesTrusts.Pages.Trusts;

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
}
