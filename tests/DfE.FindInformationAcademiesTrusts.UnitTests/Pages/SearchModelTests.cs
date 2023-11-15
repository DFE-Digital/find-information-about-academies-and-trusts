using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Pages;
using DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages;

public class SearchModelTests
{
    private const string SearchTermThatMatchesAllFakeTrusts = "trust";
    private readonly SearchModel _sut;
    private readonly Mock<ITrustSearch> _mockTrustSearch;

    private readonly TrustSearchEntry[] _fakeTrusts =
    {
        new("trust 1", "Dorthy Inlet, Kingston upon Hull, City of, JY36 9VC", "2044", ""),
        new("trust 2", "Grant Course, North East Lincolnshire, QH96 9WV", "2044", ""),
        new("trust 3", "Abbott Turnpike, East Riding of Yorkshire, BI86 4LZ", "2044", "")
    };

    private readonly Trust _fakeTrust;

    public SearchModelTests()
    {
        Mock<ITrustProvider> mockTrustProvider = new();
        _mockTrustSearch = new Mock<ITrustSearch>();

        var dummyTrustFactory = new DummyTrustFactory();
        _fakeTrust = dummyTrustFactory.GetDummyTrust();
        mockTrustProvider.Setup(s => s.GetTrustByUidAsync(_fakeTrust.Uid).Result)
            .Returns(_fakeTrust);
        _mockTrustSearch.Setup(s => s.SearchAsync(SearchTermThatMatchesAllFakeTrusts).Result).Returns(_fakeTrusts);

        _sut = new SearchModel(mockTrustProvider.Object, _mockTrustSearch.Object);
    }

    [Fact]
    public async Task OnGetAsync_should_search_if_query_parameter_provided()
    {
        _sut.KeyWords = SearchTermThatMatchesAllFakeTrusts;

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
    public async Task OnGetAsync_should_redirect_to_trust_details_if_given_uid_and_query_is_trust_name()
    {
        _sut.Uid = _fakeTrust.Uid;
        _sut.KeyWords = _fakeTrust.Name;

        var result = await _sut.OnGetAsync();

        result.Should().BeOfType<RedirectToPageResult>();
        var redirectResult = (RedirectToPageResult)result;
        redirectResult.PageName.Should().Be("/Trusts/Details");
    }

    [Fact]
    public async Task OnGetAsync_should_pass_trustId_to_trust_details_if_given_trustId()
    {
        _sut.Uid = _fakeTrust.Uid;
        _sut.KeyWords = _fakeTrust.Name;

        var result = await _sut.OnGetAsync();

        result.Should().BeOfType<RedirectToPageResult>();
        var redirectResult = (RedirectToPageResult)result;
        redirectResult.RouteValues.Should().ContainKey("Uid").WhoseValue.Should().Be(_fakeTrust.Uid);
    }

    [Fact]
    public async Task OnGetAsync_should_not_redirect_to_trust_details_if_trustId_does_not_match_query()
    {
        var differentFakeTrust = new TrustSearchEntry("other trust", "Some address", "987", "TR0987");
        _mockTrustSearch.Setup(s => s.SearchAsync(differentFakeTrust.Name))
            .ReturnsAsync(new[] { differentFakeTrust });

        _sut.KeyWords = differentFakeTrust.Name;
        _sut.Uid = _fakeTrust.Uid;

        var result = await _sut.OnGetAsync();

        result.Should().BeOfType<PageResult>();
        _sut.Trusts.Should().ContainSingle(t => t == differentFakeTrust);
    }

    [Fact]
    public async Task OnGetPopulateAutocompleteAsync_should_return_trusts_matching_keyword()
    {
        _sut.KeyWords = SearchTermThatMatchesAllFakeTrusts;

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
    public void OnPost_should_always_redirect_to_onGetAsync()
    {
        var result = (RedirectToPageResult)_sut.OnPost();

        result.PageName.Should().BeEquivalentTo("/Search");
        result.RouteValues?["KeyWords"].Should().BeEquivalentTo(string.Empty);
        result.RouteValues?["Uid"].Should().BeEquivalentTo(string.Empty);
    }

    [Fact]
    public void KeyWords_property_is_empty_by_default()
    {
        _sut.KeyWords.Should().Be("");
    }

    [Fact]
    public void TrustId_property_is_empty_by_default()
    {
        _sut.Uid.Should().Be("");
    }

    [Fact]
    public void InputId_should_have_a_fixed_value()
    {
        _sut.InputId.Should().Be("search");
    }
}
