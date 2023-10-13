using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests.Mocks;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests;

public class TrustProviderTests
{
    private readonly TrustProvider _sut;
    private readonly List<Group> _groups;

    public TrustProviderTests()
    {
        var mockAcademiesDbContext = new MockAcademiesDbContext();
        _groups = mockAcademiesDbContext.SetupMockDbContextGroups(5);
        _sut = new TrustProvider(mockAcademiesDbContext.Object);
    }

    [Fact]
    public async Task GetTrustsByUidAsync_should_return_a_trust_if_group_found()
    {
        _groups.Add(new Group
            { GroupName = "trust 1", GroupUid = "1234", GroupType = "Multi-academy trust", Ukprn = "my ukprn" });

        var result = await _sut.GetTrustByUidAsync("1234");

        result.Should().BeEquivalentTo(new Trust("1234", "trust 1",
            "my ukprn",
            "Multi-academy trust"));
    }

    [Fact]
    public async Task GetTrustsByUidAsync_should_return_null_when_group_not_found()
    {
        var result = await _sut.GetTrustByUidAsync("987654321");
        result.Should().BeNull();
    }
}
