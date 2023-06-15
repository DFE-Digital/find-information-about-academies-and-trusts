using DfE.FindInformationAcademiesTrusts.Pages;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages;

public class SearchModelTests
{
    [Fact]
    public async Task OnGetAsync_should_search_if_query_parameter()
    {
        var query = "trust";
        var mockTrustSearch = new Mock<ITrustSearch>();
        mockTrustSearch.Setup(s => s.SearchAsync(
            It.Is<string>(q => q == query)
        ).Result).Returns(
            new[]
            {
                new Trust("trust 1"),
                new Trust("trust 2"),
                new Trust("trust 3")
            }
        ).Verifiable();

        var sut = new SearchModel(mockTrustSearch.Object)
        {
            Query = query
        };

        await sut.OnGetAsync();

        sut.Trusts.Should().HaveCount(3).And.OnlyHaveUniqueItems();
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
