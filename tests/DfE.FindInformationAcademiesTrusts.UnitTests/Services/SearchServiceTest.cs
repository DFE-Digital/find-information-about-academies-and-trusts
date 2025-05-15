using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.Search;
using DfE.FindInformationAcademiesTrusts.Services.Search;
using NSubstitute;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Services;

public class SearchServiceTest
{
    private readonly SearchService _sut;

    private readonly ITrustSchoolSearchRepository _mockTrustSchoolSearchRepository = Substitute.For<ITrustSchoolSearchRepository>();
    private readonly IStringFormattingUtilities stringFormattingUtilities = new StringFormattingUtilities();
    private int _pageSize = 20;

    public SearchServiceTest()
    {
        _sut = new SearchService(_mockTrustSchoolSearchRepository, stringFormattingUtilities);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public async Task GetSearchResultsForPageAsync_if_keywords_are_null_or_empty_should_return_empty_results(string? text)
    {
        var result = await _sut.GetSearchResultsForPageAsync(text, 1);

        result.ResultsList.Should().BeEmpty();
        result.ResultsOverview.Should().BeEquivalentTo(new SearchResultsOverview());
        await _mockTrustSchoolSearchRepository.Received(0).GetSearchResultsAsync(Arg.Any<string>(), Arg.Any<int>());
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public async Task GetSearchResultsForAutocompleteAsync_if_keywords_are_null_or_empty_should_return_empty_results(string? text)
    {
      var result = await _sut.GetSearchResultsForAutocompleteAsync(text);

      result.Should().BeEmpty();
      await _mockTrustSchoolSearchRepository.Received(0).GetAutoCompleteSearchResultsAsync(Arg.Any<string>());
    }


    [Fact]
    public async Task GetSearchResultsForPageAsync_should_return_correct_mapped_models()
    {
        string searchText = "a";
        
        SearchResult trustResult = new()
        {
            Id = "123456",
            Name = "A Cool Trust",
            IsTrust = true,
            TrustGroupId = "TR123",
            Street = "A street",
            Town = "Town",
            Locality = "Station Road",
            PostCode = "GH1 8JH",
            Type = "Multi-academy"
        };

        SearchResult schoolResult = new()
        {
            Id = "65432",
            Name = "A Cool School",
            IsTrust = false,
            Street = "A street",
            Town = "Town",
            Locality = "Station Road",
            PostCode = "GH1 8JH",
            Type = "Community school"
        };

        SearchResult[] results = [trustResult, schoolResult];

        var expectedTrustResult = new SearchResultServiceModel(trustResult.Id, trustResult.Name, "A street, Station Road, Town, GH1 8JH", trustResult.TrustGroupId, trustResult.Type,
            ResultType.Trust);

        var expectedSchoolResult = new SearchResultServiceModel(schoolResult.Id, schoolResult.Name, "A street, Station Road, Town, GH1 8JH", null, schoolResult.Type,
            ResultType.School);

        _mockTrustSchoolSearchRepository.GetSearchResultsAsync(searchText, _pageSize).Returns((results, new SearchResultCount(results.Length, 1, 1)));

        var pagedSearchResults = await _sut.GetSearchResultsForPageAsync(searchText, 1);

        pagedSearchResults.ResultsList.Count.Should().Be(2);
        pagedSearchResults.ResultsList.PageStatus.TotalResults.Should().Be(2);
        pagedSearchResults.ResultsList.Should().Contain([expectedTrustResult, expectedSchoolResult]);
        pagedSearchResults.ResultsOverview.Should().NotBeNull();
        pagedSearchResults.ResultsOverview!.NumberOfSchools.Should().Be(1);
        pagedSearchResults.ResultsOverview!.NumberOfTrusts.Should().Be(1);
    }

    [Fact]
    public async Task GetSearchResultsForAutocompleteAsync_should_return_correct_mapped_models()
    {
        string searchText = "a";

        SearchResult trustResult = new()
        {
            Id = "123456",
            Name = "A Cool Trust",
            IsTrust = true,
            TrustGroupId = "TR123",
            Street = "A street",
            Town = "Town",
            Locality = "Station Road",
            PostCode = "GH1 8JH",
            Type = "Multi-academy"
        };

        SearchResult schoolResult = new()
        {
            Id = "65432",
            Name = "A Cool School",
            IsTrust = false,
            Street = "A street",
            Town = "Town",
            Locality = "Station Road",
            PostCode = "GH1 8JH",
            Type = "Community school"
        };

        SearchResult[] results = [trustResult, schoolResult];

        var expectedTrustResult = new SearchResultServiceModel(trustResult.Id, trustResult.Name, "A street, Station Road, Town, GH1 8JH", trustResult.TrustGroupId, trustResult.Type,
            ResultType.Trust);

        var expectedSchoolResult = new SearchResultServiceModel(schoolResult.Id, schoolResult.Name, "A street, Station Road, Town, GH1 8JH", null, schoolResult.Type,
            ResultType.School);

        _mockTrustSchoolSearchRepository.GetAutoCompleteSearchResultsAsync(searchText).Returns(results);

        var searchResults = await _sut.GetSearchResultsForAutocompleteAsync(searchText);

        searchResults.Length.Should().Be(2);
        searchResults.Should().Contain([expectedTrustResult, expectedSchoolResult]);
    }
}
