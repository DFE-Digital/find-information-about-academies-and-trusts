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
}
