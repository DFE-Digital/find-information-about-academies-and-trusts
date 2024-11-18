using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Gias;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests.Mocks;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests;

public class TrustSearchTests
{
    private readonly TrustSearch _sut;
    private readonly MockAcademiesDbContext _mockAcademiesDbContext = new();
    private readonly Mock<IUtilities> _mockUtilities = new();

    public TrustSearchTests()
    {
        _mockUtilities
            .Setup(u => u.BuildAddressString(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<string>()))
            .Returns(string.Empty);

        _sut = new TrustSearch(_mockAcademiesDbContext.Object, _mockUtilities.Object);
    }

    [Theory]
    [InlineData(20)]
    [InlineData(21)]
    [InlineData(30)]
    public async Task SearchAsync_should_only_return_20_results_when_there_are_more_than_20_matches(int numMatches)
    {
        AddGiasGroupsForSearchTerm("inspire", numMatches);

        var result = await _sut.SearchAsync("inspire");
        result.Should().HaveCount(20);
    }

    [Theory]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(30)]
    public async Task SearchAutocompleteAsync_should_only_return_5_results_when_there_are_more_than_5_matches(
        int numMatches)
    {
        AddGiasGroupsForSearchTerm("inspire", numMatches);

        var result = await _sut.SearchAutocompleteAsync("inspire");
        result.Should().HaveCount(5);
    }

    [Fact]
    public async Task SearchAsync_should_return_the_correct_results_page_when_there_are_more_than_20_matches()
    {
        for (var page = 1; page <= 3; page++)
        {
            for (var i = 0; i < 20; i++)
            {
                _mockAcademiesDbContext.AddGiasGroup(groupName: $"Page {page}");
            }
        }

        var result = await _sut.SearchAsync("Page");
        result.All(entry => entry.Name == "Page 1").Should().Be(true);

        result = await _sut.SearchAsync("Page", 2);
        result.All(entry => entry.Name == "Page 2").Should().Be(true);

        result = await _sut.SearchAsync("Page", 3);
        result.All(entry => entry.Name == "Page 3").Should().Be(true);
    }

    [Fact]
    public async Task SearchAsync_should_return_all_results_when_there_are_less_than_20_matches()
    {
        AddGiasGroupsForSearchTerm("inspire", 19);

        var result = await _sut.SearchAsync("inspire");
        result.Should().HaveCount(19);
    }

    [Fact]
    public async Task SearchAutocompleteAsync_should_return_all_results_when_there_are_less_than_5_matches()
    {
        AddGiasGroupsForSearchTerm("inspire", 4);

        var result = await _sut.SearchAutocompleteAsync("inspire");
        result.Should().HaveCount(4);
    }

    [Fact]
    public async Task SearchAsync_should_return_empty_if_there_is_no_matching_result()
    {
        var result = await _sut.SearchAsync("non existent trust");
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task SearchAutocompleteAsync_should_return_empty_if_there_is_no_matching_result()
    {
        var result = await _sut.SearchAutocompleteAsync("non existent trust");
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task SearchAsync_should_return_a_single_item_if_there_is_one_matching_term()
    {
        AddGiasGroupsForSearchTerm("inspire", 1);

        var result = await _sut.SearchAsync("inspire");
        result.Should().ContainSingle().Which.Name.Should().Contain("inspire");
    }

    [Fact]
    public async Task SearchAutocompleteAsync_should_return_a_single_item_if_there_is_one_matching_term()
    {
        AddGiasGroupsForSearchTerm("inspire", 1);

        var result = await _sut.SearchAutocompleteAsync("inspire");
        result.Should().ContainSingle().Which.Name.Should().Contain("inspire");
    }

    [Fact]
    public async Task SearchAsync_should_map_properties()
    {
        _mockAcademiesDbContext.AddGiasGroup(new GiasGroup
        {
            GroupUid = "1234",
            GroupType = "Multi-academy trust",
            GroupId = "TR01234",
            GroupName = "Inspire 1234"
        });

        var result = await _sut.SearchAsync("Inspire 1234");

        result.Should().ContainSingle()
            .Which.Should().BeEquivalentTo(new TrustSearchEntry(
                "Inspire 1234",
                string.Empty,
                "1234",
                "TR01234"));
    }

    [Fact]
    public async Task SearchAutocompleteAsync_should_map_properties()
    {
        _mockAcademiesDbContext.AddGiasGroup(new GiasGroup
        {
            GroupUid = "1234",
            GroupType = "Multi-academy trust",
            GroupId = "TR01234",
            GroupName = "Inspire 1234"
        });

        var result = await _sut.SearchAutocompleteAsync("Inspire 1234");

        result.Should().ContainSingle()
            .Which.Should().BeEquivalentTo(new TrustSearchEntry(
                "Inspire 1234",
                string.Empty,
                "1234",
                "TR01234"));
    }

    [Fact]
    public async Task SearchAsync_should_set_address_from_utilities()
    {
        const string street = "a street";
        const string locality = "a locality";
        const string town = "a town";
        const string postcode = "a postcode";
        const string expectedAddress = "an address";
        _mockAcademiesDbContext.AddGiasGroup(new GiasGroup
        {
            GroupUid = "1234",
            GroupType = "Multi-academy trust",
            GroupId = "TR01234",
            GroupName = "Inspire 1234",
            GroupContactStreet = street,
            GroupContactLocality = locality,
            GroupContactTown = town,
            GroupContactPostcode = postcode
        });
        _mockUtilities.Setup(u => u.BuildAddressString(street, locality, town, postcode))
            .Returns(expectedAddress);

        var result = await _sut.SearchAsync("Inspire");

        result.Should().ContainSingle()
            .Which.Address.Should().Be(expectedAddress);
    }

    [Fact]
    public async Task SearchAutocompleteAsync_should_set_address_from_utilities()
    {
        const string street = "a street";
        const string locality = "a locality";
        const string town = "a town";
        const string postcode = "a postcode";
        const string expectedAddress = "an address";
        _mockAcademiesDbContext.AddGiasGroup(new GiasGroup
        {
            GroupUid = "1234",
            GroupType = "Multi-academy trust",
            GroupId = "TR01234",
            GroupName = "Inspire 1234",
            GroupContactStreet = street,
            GroupContactLocality = locality,
            GroupContactTown = town,
            GroupContactPostcode = postcode
        });
        _mockUtilities.Setup(u => u.BuildAddressString(street, locality, town, postcode))
            .Returns(expectedAddress);

        var result = await _sut.SearchAutocompleteAsync("Inspire");

        result.Should().ContainSingle()
            .Which.Address.Should().Be(expectedAddress);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task SearchAsync_should_return_empty_if_empty_search_term(string? term)
    {
        var result = await _sut.SearchAsync(term);
        result.Should().BeEmpty();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task SearchAutocompleteAsync_should_return_empty_if_empty_search_term(string? term)
    {
        var result = await _sut.SearchAutocompleteAsync(term);
        result.Should().BeEmpty();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task SearchAsync_should_not_call_database_if_empty_search_term(string? term)
    {
        await _sut.SearchAsync(term);
        _mockAcademiesDbContext.Verify(academiesDbContext => academiesDbContext.Groups, Times.Never);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task SearchAutocompleteAsync_should_not_call_database_if_empty_search_term(string? term)
    {
        await _sut.SearchAutocompleteAsync(term);
        _mockAcademiesDbContext.Verify(academiesDbContext => academiesDbContext.Groups, Times.Never);
    }

    [Theory]
    [InlineData("a")]
    [InlineData("TR0")]
    public async Task SearchAsync_should_return_first_20_trusts_after_sorting_alphabetically_by_name(string searchTerm)
    {
        var names = new[]
        {
            "education trust", "beta trust", "zetta trust", "derbyshire academies", "chelsea learning", "all stars",
            "education trust", "beta trust", "zetta trust", "derbyshire academies", "chelsea learning", "all stars",
            "education trust", "beta trust", "zetta trust", "derbyshire academies", "chelsea learning", "all stars",
            "education trust", "beta trust", "zetta trust", "derbyshire academies", "chelsea learning", "all stars"
        };
        foreach (var name in names)
        {
            _mockAcademiesDbContext.AddGiasGroup(groupName: name);
        }

        var result = await _sut.SearchAsync(searchTerm);

        result.Should()
            .HaveCount(20)
            .And.BeInAscendingOrder(t => t.Name)
            .And.NotContain(t => t.Name == "zetta trust");
    }

    [Theory]
    [InlineData("a")]
    [InlineData("TR0")]
    public async Task SearchAutocompleteAsync_should_return_first_5_trusts_after_sorting_alphabetically_by_name(
        string searchTerm)
    {
        var names = new[]
            { "education trust", "beta trust", "zetta trust", "derbyshire academies", "chelsea learning", "all stars" };
        foreach (var name in names)
        {
            _mockAcademiesDbContext.AddGiasGroup(groupName: name);
        }

        var result = await _sut.SearchAutocompleteAsync(searchTerm);

        result.Should()
            .HaveCount(5)
            .And.BeInAscendingOrder(t => t.Name)
            .And.NotContain(t => t.Name == "zetta trust");
    }

    [Fact]
    public async Task SearchAsync_Should_Return_Trust_When_Searching_By_TrustId()
    {
        // Arrange
        _mockAcademiesDbContext.AddGiasGroup(groupId: "TR01234");

        // Act
        var result = await _sut.SearchAsync("TR01234");

        // Assert
        result.Should().ContainSingle()
            .Which.GroupId.Should().Be("TR01234");
    }

    [Fact]
    public async Task SearchAutocompleteAsync_Should_Return_Trust_When_Searching_By_TrustId()
    {
        // Arrange
        _mockAcademiesDbContext.AddGiasGroup(groupId: "TR01234");

        // Act
        var result = await _sut.SearchAutocompleteAsync("TR01234");

        // Assert
        result.Should().ContainSingle()
            .Which.GroupId.Should().Be("TR01234");
    }

    [Fact]
    public async Task SearchAsync_Should_Return_Empty_When_TrustId_Does_Not_Exist()
    {
        var result = await _sut.SearchAsync("TR99999");

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task SearchAutocompleteAsync_Should_Return_Empty_When_TrustId_Does_Not_Exist()
    {
        var result = await _sut.SearchAutocompleteAsync("TR99999");

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task SearchAsync_Should_Return_Trusts_When_SearchTerm_Matches_Both_TrustId_And_TrustName()
    {
        // Arrange
        _mockAcademiesDbContext.AddGiasGroup(groupId: "TR01234", groupName: "A Trust");
        _mockAcademiesDbContext.AddGiasGroup(groupId: "TR05678", groupName: "Trust 1234");

        // Act
        var result = await _sut.SearchAsync("1234");

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(t => t.GroupId == "TR01234");
        result.Should().Contain(t => t.Name == "Trust 1234");
    }

    [Fact]
    public async Task SearchAutocompleteAsync_Should_Return_Trusts_When_SearchTerm_Matches_Both_TrustId_And_TrustName()
    {
        // Arrange
        _mockAcademiesDbContext.AddGiasGroup(groupId: "TR01234", groupName: "A Trust");
        _mockAcademiesDbContext.AddGiasGroup(groupId: "TR05678", groupName: "Trust 1234");

        // Act
        var result = await _sut.SearchAutocompleteAsync("1234");

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(t => t.GroupId == "TR01234");
        result.Should().Contain(t => t.Name == "Trust 1234");
    }

    [Fact]
    public async Task SearchAsync_Should_Be_Case_Insensitive_When_Searching_By_TrustId()
    {
        // Arrange
        _mockAcademiesDbContext.AddGiasGroup(groupId: "TR01234");

        // Act
        var result = await _sut.SearchAsync("tr01234"); // Lowercase search term

        // Assert
        result.Should().ContainSingle()
            .Which.GroupId.Should().Be("TR01234");
    }

    [Fact]
    public async Task SearchAutocompleteAsync_Should_Be_Case_Insensitive_When_Searching_By_TrustId()
    {
        // Arrange
        _mockAcademiesDbContext.AddGiasGroup(groupId: "TR01234");

        // Act
        var result = await _sut.SearchAutocompleteAsync("tr01234"); // Lowercase search term

        // Assert
        result.Should().ContainSingle()
            .Which.GroupId.Should().Be("TR01234");
    }

    private void AddGiasGroupsForSearchTerm(string term, int num)
    {
        for (var i = 0; i < num; i++)
        {
            _mockAcademiesDbContext.AddGiasGroup(groupName: $"Trust {term} {i}");
        }
    }
}
