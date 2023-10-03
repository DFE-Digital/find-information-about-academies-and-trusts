using DfE.FindInformationAcademiesTrusts.Data;

namespace DfE.FindInformationAcademiesTrusts.UnitTests;

using static FluentActions;

public class TrustSearchTests
{
    private readonly ITrustSearch _sut;

    private readonly TrustSearchEntry[] _fakeTrusts =
    {
        new("trust 1", "Dorthy Inlet, Kingston upon Hull, City of, JY36 9VC", "2044", 0),
        new("trust 2", "Grant Course, North East Lincolnshire, QH96 9WV", "2044", 3),
        new("trust 3", "Abbott Turnpike, East Riding of Yorkshire, BI86 4LZ", "2044", 24)
    };

    public TrustSearchTests()
    {
        var mockTrustsProvider = new Mock<ITrustProvider>();
        mockTrustsProvider.Setup(t => t.GetTrustsAsync().Result).Returns(_fakeTrusts);
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
        result.Should().ContainSingle().Which.Name.Should().Be("trust 1");
    }

    [Fact]
    public async Task SearchAsync_should_return_multiple_trusts_if_more_than_one_match()
    {
        var result = await _sut.SearchAsync("trust");
        result.Should().HaveCount(3).And.OnlyHaveUniqueItems();
    }

    [Theory]
    [InlineData("Trust 1")]
    [InlineData("trusT 1")]
    [InlineData("TRUST 1")]
    public async Task SearchAsync_should_be_case_insensitive(string term)
    {
        var result = await _sut.SearchAsync(term);
        result.Should().ContainSingle().Which.Name.Should().Be("trust 1");
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
