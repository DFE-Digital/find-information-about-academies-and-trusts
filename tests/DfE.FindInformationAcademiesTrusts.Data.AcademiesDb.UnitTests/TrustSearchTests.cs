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
        _mockAcademiesDbContext.AddGiasGroupsForSearchTerm("inspire", numMatches);

        var result = await _sut.SearchAsync("inspire");
        result.Should().HaveCount(20);
    }

    [Fact]
    public async Task SearchAsync_should_return_the_correct_results_page_when_there_are_more_than_20_matches()
    {
        const int matches = 60;
        var groups = _mockAcademiesDbContext.AddGiasGroups(matches);
        for (var i = 0; i < groups.Count; i++)
        {
            groups[i].GroupName = "Page " + Math.Ceiling((double)(i + 1) / 20);
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
        _mockAcademiesDbContext.AddGiasGroupsForSearchTerm("inspire", 19);

        var result = await _sut.SearchAsync("inspire");
        result.Should().HaveCount(19);
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
        _mockAcademiesDbContext.AddGiasGroupsForSearchTerm("inspire", 1);

        var result = await _sut.SearchAsync("inspire");
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

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public async Task SearchAsync_should_return_empty_if_empty_search_term(string term)
    {
        var result = await _sut.SearchAsync(term);
        result.Should().BeEmpty();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public async Task SearchAsync_should_not_call_database_if_empty_search_term(string term)
    {
        await _sut.SearchAsync(term);
        _mockAcademiesDbContext.Verify(academiesDbContext => academiesDbContext.Groups, Times.Never);
    }

    [Fact]
    public async Task SearchAsync_should_return_trusts_sorted_alphabetically_by_trust_name()
    {
        var groups = _mockAcademiesDbContext.AddGiasGroups(5);
        var names = new[] { "education", "abbey", "educations", "aldridge trust", "abbey trust" };
        for (var i = 0; i < names.Length; i++)
        {
            groups[i].GroupName = names[i];
        }

        var result = await _sut.SearchAsync("a");
        result.Should().BeInAscendingOrder(t => t.Name);
    }

    [Fact]
    public async Task SearchAsync_should_only_return_single_and_multi_academy_trusts()
    {
        var groups = _mockAcademiesDbContext.AddGiasGroupsForSearchTerm("inspire", 5);

        groups[0].GroupType = "Federation";
        groups[1].GroupType = "Single-academy trust";
        groups[2].GroupType = "Multi-academy trust";
        groups[3].GroupType = "Trust";
        groups[4].GroupType = "School sponsor";

        var result = await _sut.SearchAsync("inspire");
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task SearchAsync_should_not_return_groups_with_a_null_GroupUid()
    {
        var groups = _mockAcademiesDbContext.AddGiasGroupsForSearchTerm("inspire", 5);

        groups[0].GroupUid = null;

        var result = await _sut.SearchAsync("inspire");
        result.Should().HaveCount(4);
    }

    [Fact]
    public async Task SearchAsync_should_not_return_groups_with_a_null_GroupId()
    {
        var groups = _mockAcademiesDbContext.AddGiasGroupsForSearchTerm("inspire", 5);

        groups[0].GroupId = null;

        var result = await _sut.SearchAsync("inspire");
        result.Should().HaveCount(4);
    }

    [Fact]
    public async Task SearchAsync_should_not_return_groups_with_a_null_GroupName()
    {
        var groups = _mockAcademiesDbContext.AddGiasGroupsForSearchTerm("inspire", 5);

        groups[0].GroupName = null;

        var result = await _sut.SearchAsync("inspire");
        result.Should().HaveCount(4);
    }
    [Fact]
    public async Task SearchAsync_Should_Return_Trust_When_Searching_By_TrustId()
    {
        // Arrange
        _mockAcademiesDbContext.AddGiasGroup(new GiasGroup
        {
            GroupUid = "1234",
            GroupType = "Multi-academy trust",
            GroupId = "TR01234",
            GroupName = "Inspire Trust",
            GroupContactStreet = "12 Abbey Road",
            GroupContactLocality = "Dorthy Inlet",
            GroupContactTown = "East Park",
            GroupContactPostcode = "JY36 9VC"
        });

        // Act
        var result = await _sut.SearchAsync("TR01234");

        // Assert
        result.Should().ContainSingle()
            .Which.Should().BeEquivalentTo(new TrustSearchEntry(
                "Inspire Trust",
                "12 Abbey Road, Dorthy Inlet, East Park, JY36 9VC",
                "1234",
                "TR01234"));
    }

    [Fact]
    public async Task SearchAsync_Should_Return_Empty_When_TrustId_Does_Not_Exist()
    {
        // Arrange
        _mockAcademiesDbContext.AddGiasGroup(new GiasGroup
        {
            GroupUid = "1234",
            GroupType = "Multi-academy trust",
            GroupId = "TR01234",
            GroupName = "Inspire Trust",
            GroupContactStreet = "12 Abbey Road",
            GroupContactLocality = "Dorthy Inlet",
            GroupContactTown = "East Park",
            GroupContactPostcode = "JY36 9VC"
        });

        // Act
        var result = await _sut.SearchAsync("TR99999");

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task SearchAsync_Should_Return_Trusts_When_SearchTerm_Matches_Both_TrustId_And_TrustName()
    {
        // Arrange
        _mockAcademiesDbContext.AddGiasGroup(new GiasGroup
        {
            GroupUid = "1234",
            GroupType = "Multi-academy trust",
            GroupId = "TR01234",
            GroupName = "TR01234 Academy",
            GroupContactStreet = "34 Baker Street",
            GroupContactLocality = "Another Town",
            GroupContactTown = "West Park",
            GroupContactPostcode = "AB12 3CD"
        });

        _mockAcademiesDbContext.AddGiasGroup(new GiasGroup
        {
            GroupUid = "5678",
            GroupType = "Multi-academy trust",
            GroupId = "TR05678",
            GroupName = "TR01234 Academy",
            GroupContactStreet = "56 High Street",
            GroupContactLocality = "Somewhere",
            GroupContactTown = "North Park",
            GroupContactPostcode = "CD34 5EF"
        });

        // Act
        var result = await _sut.SearchAsync("TR01234");

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(t => t.GroupId == "TR01234");
        result.Should().Contain(t => t.Name == "TR01234 Academy");
    }

    [Fact]
    public async Task SearchAsync_Should_Be_Case_Insensitive_When_Searching_By_TrustId()
    {
        // Arrange
        _mockAcademiesDbContext.AddGiasGroup(new GiasGroup
        {
            GroupUid = "1234",
            GroupType = "Multi-academy trust",
            GroupId = "TR01234",
            GroupName = "Inspire Trust",
            GroupContactStreet = "12 Abbey Road",
            GroupContactLocality = "Dorthy Inlet",
            GroupContactTown = "East Park",
            GroupContactPostcode = "JY36 9VC"
        });

        // Act
        var result = await _sut.SearchAsync("tr01234"); // Lowercase search term

        // Assert
        result.Should().ContainSingle()
            .Which.GroupId.Should().Be("TR01234");
    }

    [Fact]
    public async Task SearchAsync_Should_Not_Throw_Exception_When_GroupId_Is_Null()
    {
        // Arrange
        _mockAcademiesDbContext.AddGiasGroup(new GiasGroup
        {
            GroupUid = "1234",
            GroupType = "Multi-academy trust",
            GroupId = null,
            GroupName = "Inspire Trust",
            GroupContactStreet = "12 Abbey Road",
            GroupContactLocality = "Dorthy Inlet",
            GroupContactTown = "East Park",
            GroupContactPostcode = "JY36 9VC"
        });

        // Act
        Func<Task> act = async () => await _sut.SearchAsync("Inspire");

        // Assert
        await act.Should().NotThrowAsync();
    }
}
