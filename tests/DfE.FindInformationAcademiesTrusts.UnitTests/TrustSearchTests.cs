namespace DfE.FindInformationAcademiesTrusts.UnitTests;

using static FluentActions;

public class TrustSearchTests
{
    private readonly ITrustSearch _sut;

    public TrustSearchTests()
    {
        var mockTrustsProvider = new Mock<ITrustProvider>();
        mockTrustsProvider.Setup(t => t.GetTrustsAsync().Result).Returns(
            new[]
            {
                new Trust("trust 1"),
                new Trust("trust 2"),
                new Trust("trust 3")
            }
        );
        _sut = new TrustSearch(mockTrustsProvider.Object);
    }

    [Fact]
    public async Task SearchAsync_should_return_empty_if_there_is_no_matching_result()
    {
        var result = await _sut.SearchAsync("non existent trust");
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task SearchAsync_should_return_a_single_item_if_there_is_one_matching_term()
    {
        var result = await _sut.SearchAsync("trust 1");
        result.Should().ContainSingle().Which.Should().Be("trust 1");
    }

    [Fact]
    public async Task SearchAsync_should_return_multiple_trusts_if_more_than_one_match()
    {
        var result = await _sut.SearchAsync("trust");
        result.Should().BeEquivalentTo("trust 1", "trust 2", "trust 3");
    }

    [Theory]
    [InlineData("Trust 1")]
    [InlineData("trusT 1")]
    [InlineData("TRUST 1")]
    public async Task SearchAsync_should_be_case_insensitive(string term)
    {
        var result = await _sut.SearchAsync(term);
        result.Should().ContainSingle().Which.Should().Be("trust 1");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task SearchAsync_should_throw_error_if_empty_search_term(string term)
    {
        await Invoking(() => _sut.SearchAsync(term)).Should().ThrowAsync<ArgumentException>()
            .WithMessage("No search term provided");
    }
}
