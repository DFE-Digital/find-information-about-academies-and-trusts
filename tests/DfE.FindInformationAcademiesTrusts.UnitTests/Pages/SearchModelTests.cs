using DfE.FindInformationAcademiesTrusts.Pages;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages;

public class SearchModelTests
{
    private readonly Mock<ITrustProvider> _mockTrustProvider;
    private readonly SearchModel _sut;

    public SearchModelTests()
    {
        _mockTrustProvider = new Mock<ITrustProvider>();

        _sut = new SearchModel(_mockTrustProvider.Object);
    }

    private readonly Trust[] _fakeTrusts =
    {
        new("trust 1", "Dorthy Inlet, Kingston upon Hull, City of, JY36 9VC", "2044", 0),
        new("trust 2", "Grant Course, North East Lincolnshire, QH96 9WV", "2044", 3),
        new("trust 3", "Abbott Turnpike, East Riding of Yorkshire, BI86 4LZ", "2044", 4)
    };

    [Fact]
    public async Task OnGetAsync_should_search_if_query_parameter()
    {
        const string query = "trust";

        _mockTrustProvider.Setup(s => s.GetTrustsByNameAsync(query).Result)
            .Returns(_fakeTrusts);
        _sut.KeyWords = query;

        await _sut.OnGetAsync();

        _sut.Trusts.Should().BeEquivalentTo(_fakeTrusts);
    }

    [Fact]
    public async Task OnGetAsync_should_default_to_empty_trusts_if_no_query()
    {
        await _sut.OnGetAsync();

        _sut.Trusts.Should().BeEmpty();
    }

    [Fact]
    public async Task OnGetAsync_should_redirect_to_trust_details_if_given_trustId()
    {
        _sut.TrustId = "1234";
        var result = await _sut.OnGetAsync();

        result.Should().BeOfType<RedirectToPageResult>();
        var redirectResult = (RedirectToPageResult)result;
        redirectResult.PageName.Should().Be("/Trusts/Details");
    }

    [Fact]
    public async Task OnGetAsync_should_pass_trustId_to_trust_details_if_given_trustId()
    {
        _sut.TrustId = "1234";
        var result = await _sut.OnGetAsync();

        result.Should().BeOfType<RedirectToPageResult>();
        var redirectResult = (RedirectToPageResult)result;
        redirectResult.RouteValues.Should().ContainKey("Ukprn").WhoseValue.Should().Be("1234");
    }

    [Fact]
    public async Task OnGetPopulateAutocompleteAsync_should_return_trusts_matching_keyword()
    {
        const string query = "trust";

        _mockTrustProvider.Setup(s => s.GetTrustsByNameAsync(query).Result)
            .Returns(_fakeTrusts);
        _sut.KeyWords = query;

        var result = await _sut.OnGetPopulateAutocompleteAsync();

        result.Should().BeOfType<JsonResult>();
        var jsonResult = (JsonResult)result;
        jsonResult.Value.Should().BeEquivalentTo(_fakeTrusts.Select(trust =>
            new SearchModel.AutocompleteEntry(
                trust.Address,
                trust.Name,
                trust.Ukprn
            )));
    }

    [Fact]
    public void KeyWords_property_is_empty_by_default()
    {
        _sut.KeyWords.Should().Be("");
    }

    [Fact]
    public void TrustId_property_is_empty_by_default()
    {
        _sut.TrustId.Should().Be("");
    }

    [Fact]
    public void InputId_should_have_a_fixed_value()
    {
        _sut.InputId.Should().Be("search");
    }
}
