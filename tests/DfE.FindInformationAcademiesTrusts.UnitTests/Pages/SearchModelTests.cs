using DfE.FindInformationAcademiesTrusts.Pages;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages;

public class SearchModelTests
{
    [Fact]
    public async Task OnGetAsync_should_search_if_query_parameter()
    {
        var query = "trust";
        var mockTrustSearch = new Mock<ITrustSearch>();
        mockTrustSearch.Setup(s => s.SearchAsync(query).Result)
            .Returns(
                new[]
                {
                    new Trust("trust 1", "Dorthy Inlet, Kingston upon Hull, City of, JY36 9VC"),
                    new Trust("trust 2", "Grant Course, North East Lincolnshire, QH96 9WV"),
                    new Trust("trust 3", "Abbott Turnpike, East Riding of Yorkshire, BI86 4LZ")
                }
            );

        var sut = new SearchModel(mockTrustSearch.Object)
        {
            Query = query
        };

        await sut.OnGetAsync();

        sut.Trusts.Should().BeEquivalentTo(
            new[]
            {
                new Trust("trust 1", "Dorthy Inlet, Kingston upon Hull, City of, JY36 9VC"),
                new Trust("trust 2", "Grant Course, North East Lincolnshire, QH96 9WV"),
                new Trust("trust 3", "Abbott Turnpike, East Riding of Yorkshire, BI86 4LZ")
            }
        );
    }

    [Fact]
    public async Task OnGetAsync_should_default_to_empty_trusts_if_no_query()
    {
        var mockTrustSearch = new Mock<ITrustSearch>();
        var sut = new SearchModel(mockTrustSearch.Object);

        await sut.OnGetAsync();

        sut.Trusts.Should().BeEmpty();
    }
}
