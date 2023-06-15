using DfE.FindInformationAcademiesTrusts.Pages;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages;

public class SearchModelTests
{
    [Fact]
    public async Task OnGetAsync_should_search_if_query_parameter()
    {
        var mockTrustSearch = new Mock<ITrustSearch>();
        mockTrustSearch.Setup(s => s.SearchAsync(
            It.IsAny<string>()
        ).Result).Returns(
            new[]
            {
                new Trust("trust 1"),
                new Trust("trust 2"),
                new Trust("trust 3")
            }
        );

        var sut = new SearchModel(mockTrustSearch.Object)
        {
            Query = "trust"
        };

        await sut.OnGetAsync();

        sut.Trusts.Should().HaveCount(3).And.OnlyHaveUniqueItems();
    }
}
