using DfE.FindInformationAcademiesTrusts.Pages;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages;

public class SearchModelTests
{
    [Fact]
    public async Task OnGetAsync_should_search_if_query_parameter()
    {
        var query = "trust";

        var mockTrustProvider = new Mock<ITrustProvider>();
        mockTrustProvider.Setup(s => s.GetTrustsByNameAsync(query).Result)
            .Returns(FakeTrusts());
        var sut = new SearchModel(mockTrustProvider.Object)
        {
            KeyWords = query
        };

        await sut.OnGetAsync();

        sut.Trusts.Should().BeEquivalentTo(FakeTrusts());
    }

    [Fact]
    public async Task OnGetAsync_should_default_to_empty_trusts_if_no_query()
    {
        var mockTrustProvider = new Mock<ITrustProvider>();
        var sut = new SearchModel(mockTrustProvider.Object);

        await sut.OnGetAsync();

        sut.Trusts.Should().BeEmpty();
    }

    private static Trust[] FakeTrusts()
    {
        return new[]
        {
            new Trust("trust 1", "Dorthy Inlet, Kingston upon Hull, City of, JY36 9VC", "2044", 0),
            new Trust("trust 2", "Grant Course, North East Lincolnshire, QH96 9WV", "2044", 3),
            new Trust("trust 3", "Abbott Turnpike, East Riding of Yorkshire, BI86 4LZ", "2044", 4)
        };
    }
}
