using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests.Mocks;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests;

public class TrustSearchTests
{
    private readonly ITrustSearch _sut;

    private readonly TrustSearchEntry[] _fakeTrusts =
    {
        new("trust 1", "Dorthy Inlet, Kingston upon Hull, City of, JY36 9VC", "2044", 0),
        new("trust 2", "Grant Course, North East Lincolnshire, QH96 9WV", "2044", 3),
        new("trust 3", "Abbott Turnpike, East Riding of Yorkshire, BI86 4LZ", "2044", 24)
    };

    private readonly Mock<IAcademiesDbContext> _mockAcademiesDbContext;

    public TrustSearchTests()
    {
        _mockAcademiesDbContext = new Mock<IAcademiesDbContext>();

        var groups = new List<Group>
        {
            new() { GroupName = "trust 1" },
            new() { GroupName = "trust 2" },
            new() { GroupName = "trust 3" }
        };
        _mockAcademiesDbContext.Setup(academiesDbContext => academiesDbContext.Groups)
            .Returns(MockDbContext.GetMock(groups));
        _sut = new TrustSearch(_mockAcademiesDbContext.Object);
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
}
