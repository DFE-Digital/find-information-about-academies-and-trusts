using DfE.FIAT.Data;
using DfE.FIAT.Web.Pages;
using DfE.FIAT.Web.Services.Trust;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DfE.FIAT.Web.UnitTests.Pages;

public class SearchModelTests
{
    private const string SearchTermThatMatchesAllFakeTrusts = "trust";
    private readonly SearchModel _sut;
    private readonly Mock<ITrustSearch> _mockTrustSearch;

    private readonly TrustSummaryServiceModel _fakeTrust = new("1234", "My Trust", "Multi-academy trust", 3);

    private readonly TrustSearchEntry[] _fakeTrusts =
    [
        new TrustSearchEntry("trust 1", "Dorthy Inlet, Kingston upon Hull, City of, JY36 9VC", "2044", ""),
        new TrustSearchEntry("trust 2", "Grant Course, North East Lincolnshire, QH96 9WV", "2044", ""),
        new TrustSearchEntry("trust 3", "Abbott Turnpike, East Riding of Yorkshire, BI86 4LZ", "2044", "")
    ];

    public SearchModelTests()
    {
        Mock<ITrustService> mockTrustService = new();
        _mockTrustSearch = new Mock<ITrustSearch>();

        mockTrustService.Setup(s => s.GetTrustSummaryAsync(_fakeTrust.Uid).Result)
            .Returns(_fakeTrust);

        _mockTrustSearch.Setup(s => s.SearchAsync(It.IsAny<string?>(), It.IsAny<int>()))
            .ReturnsAsync(PaginatedList<TrustSearchEntry>.Empty());
        _mockTrustSearch.Setup(s => s.SearchAsync(SearchTermThatMatchesAllFakeTrusts, It.IsAny<int>()))
            .ReturnsAsync(new PaginatedList<TrustSearchEntry>(_fakeTrusts, _fakeTrusts.Length, 1, 1));
        _mockTrustSearch.Setup(s => s.SearchAutocompleteAsync(It.IsAny<string?>()))
            .ReturnsAsync([]);
        _mockTrustSearch.Setup(s => s.SearchAutocompleteAsync(SearchTermThatMatchesAllFakeTrusts))
            .ReturnsAsync(_fakeTrusts);

        _sut = new SearchModel(mockTrustService.Object, _mockTrustSearch.Object);
    }

    [Fact]
    public async Task OnGetAsync_should_search_with_query_parameter_provided()
    {
        _sut.KeyWords = SearchTermThatMatchesAllFakeTrusts;

        await _sut.OnGetAsync();

        _sut.Trusts.Should().BeEquivalentTo(_fakeTrusts);
    }

    [Fact]
    public async Task OnGetAsync_should_return_no_results_page_if_no_trusts_found_for_term()
    {
        _sut.KeyWords = "no matching trusts";

        await _sut.OnGetAsync();

        _sut.Trusts.Should().BeEmpty();
        _sut.PaginationRouteData["Keywords"].Should().Be("no matching trusts");
    }

    [Fact]
    public async Task OnGetAsync_should_return_no_results_page_if_no_keyword_given()
    {
        _sut.KeyWords = null;
        await _sut.OnGetAsync();

        _sut.Trusts.Should().BeEmpty();
        _sut.PaginationRouteData["Keywords"].Should().Be(string.Empty);
    }

    [Fact]
    public async Task OnGetAsync_should_redirect_to_trust_overview_if_given_uid_and_query_is_trust_name()
    {
        _sut.Uid = _fakeTrust.Uid;
        _sut.KeyWords = _fakeTrust.Name;

        var result = await _sut.OnGetAsync();

        var redirectResult = result.Should().BeOfType<RedirectToPageResult>().Subject;
        redirectResult.PageName.Should().Be("/Trusts/Overview/TrustDetails");
        redirectResult.RouteValues.Should().ContainKey("Uid").WhoseValue.Should().Be(_fakeTrust.Uid);
    }

    [Fact]
    public async Task OnGetAsync_should_not_redirect_to_trust_overview_if_trustId_does_not_match_query()
    {
        var differentFakeTrust = new TrustSearchEntry("other trust", "Some address", "987", "TR0987");
        _mockTrustSearch.Setup(s => s.SearchAsync(differentFakeTrust.Name, It.IsAny<int>()))
            .ReturnsAsync(new PaginatedList<TrustSearchEntry>(new[] { differentFakeTrust }, 1, 1, 1));

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

        var jsonResult = result.Should().BeOfType<JsonResult>().Subject;
        jsonResult.Value.Should().BeEquivalentTo(_fakeTrusts.Select(trust =>
            new SearchModel.AutocompleteEntry(
                trust.Address,
                trust.Name,
                trust.Uid
            )
        ));
    }

    [Theory]
    [InlineData("no matching trusts")]
    [InlineData("")]
    [InlineData(null)]
    public async Task OnGetPopulateAutocompleteAsync_should_return_empty_json_when_no_matching_keyword(string? keywords)
    {
        _sut.KeyWords = keywords;

        var result = await _sut.OnGetPopulateAutocompleteAsync();

        using var _ = new AssertionScope();

        var jsonResult = result.Should().BeOfType<JsonResult>().Subject;
        jsonResult.Value.Should()
            .BeAssignableTo<IEnumerable<SearchModel.AutocompleteEntry>>()
            .Which.Should().BeEmpty();
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
    public void ShowHeaderSearch_should_be_false()
    {
        _sut.ShowHeaderSearch.Should().BeFalse();
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
        _sut.PageSearchFormInputId.Should().Be("search");
    }

    [Fact]
    public void SearchPageNumber_should_default_to_1()
    {
        _sut.PageNumber.Should().Be(1);
    }

    [Fact]
    public void PageName_should_be_search()
    {
        _sut.PageName.Should().Be("Search");
    }

    [Fact]
    public void PaginationRouteData_should_default_to_empty()
    {
        _sut.PaginationRouteData.Should().BeEquivalentTo(new Dictionary<string, string>());
    }

    [Fact]
    public void PageStatus_should_default_to_empty()
    {
        _sut.PageStatus.Should().BeEquivalentTo(new PageStatus(0, 0, 0));
    }

    [Fact]
    public async Task When_a_different_page_is_requested_return_a_different_page()
    {
        _mockTrustSearch.Setup(s => s.SearchAsync(SearchTermThatMatchesAllFakeTrusts, 1))
            .ReturnsAsync(new PaginatedList<TrustSearchEntry>(_fakeTrusts, 4, 1, 3));
        var differentFakeTrust =
            new TrustSearchEntry(SearchTermThatMatchesAllFakeTrusts, "Some address", "987", "TR0987");
        _mockTrustSearch.Setup(s => s.SearchAsync(differentFakeTrust.Name, 2))
            .ReturnsAsync(new PaginatedList<TrustSearchEntry>(new[] { differentFakeTrust }, 4, 2, 3));

        _sut.KeyWords = SearchTermThatMatchesAllFakeTrusts;
        _sut.PageNumber = 1;

        var result = await _sut.OnGetAsync();
        result.Should().BeOfType<PageResult>();
        _sut.Trusts.Should().BeEquivalentTo(_fakeTrusts);
        _sut.PaginationRouteData["Keywords"].Should().Be(SearchTermThatMatchesAllFakeTrusts);

        _sut.PageNumber = 2;
        result = await _sut.OnGetAsync();
        result.Should().BeOfType<PageResult>();
        _sut.Trusts.Should().ContainSingle(t => t == differentFakeTrust);
        _sut.PaginationRouteData["Keywords"].Should().Be(SearchTermThatMatchesAllFakeTrusts);
    }

    [Fact]
    public void Title_Should_Be_Search_When_Keywords_Are_Empty()
    {
        _sut.Title.Should().BeEquivalentTo("Search");
    }

    [Fact]
    public void Title_Should_Include_The_Keywords_When_they_Are_Set()
    {
        _sut.KeyWords = "Test";
        _sut.Title.Should().BeEquivalentTo("Search - Test");
    }

    [Fact]
    public void Title_Should_Include_The_Keywords_When_they_Are_Set_And_There_Is_Only_one_page()
    {
        _sut.KeyWords = "Test";
        _sut.Trusts = new PaginatedList<TrustSearchEntry>(Array.Empty<TrustSearchEntry>(), 1, 1, 1);
        _sut.Title.Should().BeEquivalentTo("Search - Test");
    }

    [Fact]
    public void Title_Should_Include_The_Keywords_And_Page_Number_When_They_Are_Set()
    {
        _sut.KeyWords = "Test";
        _sut.Trusts = new PaginatedList<TrustSearchEntry>(Array.Empty<TrustSearchEntry>(), 46, 2, 20);
        _sut.Title.Should().BeEquivalentTo("Search (page 2 of 3) - Test");
    }
}
