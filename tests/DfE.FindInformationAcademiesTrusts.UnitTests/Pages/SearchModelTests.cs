using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Pages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages;

public class SearchModelTests
{
    private readonly Mock<ITrustProvider> _mockTrustProvider;
    private readonly SearchModel _sut;
    private readonly Mock<ITrustSearch> _mockTrustSearch;

    public SearchModelTests()
    {
        _mockTrustProvider = new Mock<ITrustProvider>();
        _mockTrustSearch = new Mock<ITrustSearch>();

        _sut = new SearchModel(_mockTrustProvider.Object, _mockTrustSearch.Object);
    }

    private readonly TrustSearchEntry[] _fakeTrusts =
    {
        new("trust 1", "Dorthy Inlet, Kingston upon Hull, City of, JY36 9VC", "2044", "", 0),
        new("trust 2", "Grant Course, North East Lincolnshire, QH96 9WV", "2044", "", 3),
        new("trust 3", "Abbott Turnpike, East Riding of Yorkshire, BI86 4LZ", "2044", "", 4)
    };

    private readonly Trust _fakeTrust =
        new("trust 1", "2044", "Multi-academy trust");

    private const string _trustId = "1234";

    [Fact]
    public async Task OnGetAsync_should_search_if_query_parameter()
    {
        const string query = "trust";

        _mockTrustSearch.Setup(s => s.SearchAsync(query).Result).Returns(_fakeTrusts);
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
        _sut.TrustId = _trustId;
        _sut.KeyWords = "trust 1";
        _mockTrustProvider.Setup(s => s.GetTrustByUkprnAsync(_trustId).Result)
            .Returns(_fakeTrust);

        var result = await _sut.OnGetAsync();

        result.Should().BeOfType<RedirectToPageResult>();
        var redirectResult = (RedirectToPageResult)result;
        redirectResult.PageName.Should().Be("/Trusts/Details");
    }

    [Fact]
    public async Task OnGetAsync_should_pass_trustId_to_trust_details_if_given_trustId()
    {
        _sut.TrustId = _trustId;
        _sut.KeyWords = "trust 1";
        _mockTrustProvider.Setup(s => s.GetTrustByUkprnAsync(_trustId).Result)
            .Returns(_fakeTrust);

        var result = await _sut.OnGetAsync();

        result.Should().BeOfType<RedirectToPageResult>();
        var redirectResult = (RedirectToPageResult)result;
        redirectResult.RouteValues.Should().ContainKey("Ukprn").WhoseValue.Should().Be(_trustId);
    }

    [Fact]
    public async Task OnGetAsync_should_not_redirect_to_trust_details_if_trustId_does_not_match_query()
    {
        const string query = "trust 3";

        _mockTrustSearch.Setup(s => s.SearchAsync(query).Result).Returns(_fakeTrusts);
        _mockTrustProvider.Setup(s => s.GetTrustByUkprnAsync(_trustId).Result)
            .Returns(_fakeTrust);

        _mockTrustSearch.Setup(s => s.SearchAsync(query).Result).Returns(_fakeTrusts);
        _sut.KeyWords = query;
        _sut.TrustId = _trustId;

        var result = await _sut.OnGetAsync();

        result.Should().BeOfType<PageResult>();
        _sut.Trusts.Should().BeEquivalentTo(_fakeTrusts);
    }

    [Fact]
    public async Task OnGetPopulateAutocompleteAsync_should_return_trusts_matching_keyword()
    {
        const string query = "trust";
        _mockTrustSearch.Setup(s => s.SearchAsync(query).Result).Returns(_fakeTrusts);
        _sut.KeyWords = query;

        var result = await _sut.OnGetPopulateAutocompleteAsync();

        result.Should().BeOfType<JsonResult>();
        var jsonResult = (JsonResult)result;
        jsonResult.Value.Should().BeEquivalentTo(_fakeTrusts.Select(trust =>
            new SearchModel.AutocompleteEntry(
                trust.Address,
                trust.Name,
                trust.Uid
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
