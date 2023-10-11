using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests.Mocks;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests;

public class TrustSearchTests
{
    private readonly ITrustSearch _sut;
    private readonly Mock<IAcademiesDbContext> _mockAcademiesDbContext;
    private readonly Mock<ITrustHelper> _mockTrustHelper;

    public TrustSearchTests()
    {
        _mockAcademiesDbContext = new Mock<IAcademiesDbContext>();
        _mockTrustHelper = new Mock<ITrustHelper>();

        SetupMockDbContextGroups(3);

        _sut = new TrustSearch(_mockAcademiesDbContext.Object, _mockTrustHelper.Object);
    }

    [Theory]
    [InlineData(20)]
    [InlineData(21)]
    [InlineData(30)]
    public async Task SearchAsync_should_only_return_20_results_when_there_are_more_than_20_matches(int numMatches)
    {
        SetupMockDbContextGroups(numMatches);

        var result = await _sut.SearchAsync("trust");
        result.Should().HaveCount(20);
    }

    [Fact]
    public async Task SearchAsync_should_return_all_results_when_there_are_less_than_20_matches()
    {
        SetupMockDbContextGroups(19);

        var result = await _sut.SearchAsync("trust");
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
        var result = await _sut.SearchAsync("trust 1");
        result.Should().ContainSingle().Which.Name.Should().Be("trust 1");
    }

    [Fact]
    public async Task SearchAsync_should_return_multiple_trusts_if_more_than_one_match()
    {
        var result = await _sut.SearchAsync("trust");
        result.Should().HaveCount(3).And.OnlyHaveUniqueItems();
    }

    [Fact]
    public async Task SearchAsync_should_return_trust_with_Uid()
    {
        var result = await _sut.SearchAsync("trust 1");
        result.Should().ContainSingle().Which.Uid.Should().Be("1");
    }

    [Fact]
    public async Task SearchAsync_should_return_trust_with_GroupId()
    {
        var result = await _sut.SearchAsync("trust 1");
        result.Should().ContainSingle().Which.GroupId.Should().Be("TR01");
    }

    [Fact]
    public async Task SearchAsync_should_return_trust_address_formatted_as_string()
    {
        var groups = SetupMockDbContextGroups(3);
        var fakeTrusts = new[]
        {
            "12 Abbey Road, Dorthy Inlet, East Park, Kingston upon Hull, JY36 9VC",
            "",
            "Dorthy Inlet"
        };

        for (var i = 0; i < groups.Count; i++)
        {
            var group = groups[i];
            var address = fakeTrusts[i];
            _mockTrustHelper.Setup(trustHelper => trustHelper.BuildAddressString(group))
                .Returns(address);
        }

        var result = (await _sut.SearchAsync("trust")).ToArray();

        for (var i = 0; i < result.Length; i++)
        {
            result[i].Address.Should().Be(fakeTrusts[i]);
        }
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task SearchAsync_should_return_empty_if_empty_search_term(string term)
    {
        var result = await _sut.SearchAsync(term);
        result.Should().BeEmpty();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task SearchAsync_should_not_call_database_if_empty_search_term(string term)
    {
        _ = await _sut.SearchAsync(term);
        _mockAcademiesDbContext.Verify(academiesDbContext => academiesDbContext.Groups, Times.Never);
    }

    [Fact]
    public async Task SearchAsync_should_return_trusts_sorted_alphabetically_by_trust_name()
    {
        var names = new[] { "education", "abbey", "educations", "aldridge trust", "abbey trust" };
        var groups = new List<Group>();
        foreach (var name in names)
        {
            groups.Add(new Group { GroupName = name });
        }

        _mockAcademiesDbContext.Setup(academiesDbContext => academiesDbContext.Groups)
            .Returns(MockDbContext.GetMock(groups));

        var result = await _sut.SearchAsync("a");
        result.Should().BeInAscendingOrder(t => t.Name);
    }

    [Fact]
    public async Task SearchAsync_should_only_return_single_and_multi_academy_trusts()
    {
        var groups = SetupMockDbContextGroups(5);

        groups[0].GroupType = "Federation";
        groups[1].GroupType = "Single-academy trust";
        groups[2].GroupType = "Multi-academy trust";
        groups[3].GroupType = "Trust";
        groups[4].GroupType = "School sponsor";

        var result = await _sut.SearchAsync("trust");
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task SearchAsync_should_not_return_groups_with_a_null_GroupUid()
    {
        var groups = SetupMockDbContextGroups(5);

        groups[0].GroupUid = null;

        var result = await _sut.SearchAsync("trust");
        result.Should().HaveCount(4);
    }

    [Fact]
    public async Task SearchAsync_should_not_return_groups_with_a_null_GroupId()
    {
        var groups = SetupMockDbContextGroups(5);

        groups[0].GroupId = null;

        var result = await _sut.SearchAsync("trust");
        result.Should().HaveCount(4);
    }

    [Fact]
    public async Task SearchAsync_should_not_return_groups_with_a_null_GroupName()
    {
        var groups = SetupMockDbContextGroups(5);

        groups[0].GroupName = null;

        var result = await _sut.SearchAsync("trust");
        result.Should().HaveCount(4);
    }

    private List<Group> SetupMockDbContextGroups(int numMatches)
    {
        var groups = new List<Group>();
        for (var i = 0; i < numMatches; i++)
        {
            groups.Add(new Group
            {
                GroupName = $"trust {i}", GroupUid = $"{i}", GroupId = $"TR0{i}", GroupType = "Multi-academy trust"
            });
        }

        _mockAcademiesDbContext.Setup(academiesDbContext => academiesDbContext.Groups)
            .Returns(MockDbContext.GetMock(groups));

        return groups;
    }
}
