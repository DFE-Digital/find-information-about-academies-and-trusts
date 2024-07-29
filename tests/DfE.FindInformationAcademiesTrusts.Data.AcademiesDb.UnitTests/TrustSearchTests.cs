using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Gias;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests.Mocks;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests;

public class TrustSearchTests
{
    private readonly TrustSearch _sut;
    private readonly MockAcademiesDbContext _mockAcademiesDbContext;

    public TrustSearchTests()
    {
        _mockAcademiesDbContext = new MockAcademiesDbContext();

        _sut = new TrustSearch(_mockAcademiesDbContext.Object);
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
            GroupName = "Inspire 1234",
            GroupContactStreet = "12 Abbey Road",
            GroupContactLocality = "Dorthy Inlet",
            GroupContactTown = "East Park",
            GroupContactPostcode = "JY36 9VC"
        });

        var result = await _sut.SearchAsync("Inspire 1234");

        result.Should().ContainSingle()
            .Which.Should().BeEquivalentTo(new TrustSearchEntry(
                "Inspire 1234",
                "12 Abbey Road, Dorthy Inlet, East Park, JY36 9VC",
                "1234",
                "TR01234"));
    }

    [Theory]
    [InlineData("12 Abbey Road", "Dorthy Inlet", "East Park", "JY36 9VC",
        "12 Abbey Road, Dorthy Inlet, East Park, JY36 9VC")]
    [InlineData(null, "Dorthy Inlet", "East Park", "JY36 9VC", "Dorthy Inlet, East Park, JY36 9VC")]
    [InlineData("12 Abbey Road", null, "East Park", "JY36 9VC", "12 Abbey Road, East Park, JY36 9VC")]
    [InlineData("12 Abbey Road", "Dorthy Inlet", null, "JY36 9VC", "12 Abbey Road, Dorthy Inlet, JY36 9VC")]
    [InlineData("12 Abbey Road", "Dorthy Inlet", "East Park", null, "12 Abbey Road, Dorthy Inlet, East Park")]
    [InlineData(null, null, null, null, "")]
    [InlineData("", "     ", "", null, "")]
    [InlineData("12 Abbey Road", "  ", " ", "JY36 9VC", "12 Abbey Road, JY36 9VC")]
    public async Task SearchAsync_should_return_trust_address_formatted_as_string(string? groupContactStreet,
        string? groupContactLocality, string? groupContactTown, string? groupContactPostcode, string? expectedAddress)
    {
        _mockAcademiesDbContext.AddGiasGroup(new GiasGroup
        {
            GroupUid = "1234",
            GroupType = "Multi-academy trust",
            GroupId = "TR01234",
            GroupName = "Inspire 1234",
            GroupContactStreet = groupContactStreet,
            GroupContactLocality = groupContactLocality,
            GroupContactTown = groupContactTown,
            GroupContactPostcode = groupContactPostcode
        });

        var result = await _sut.SearchAsync("Inspire");

        result.Single().Address.Should().Be(expectedAddress);
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
}
