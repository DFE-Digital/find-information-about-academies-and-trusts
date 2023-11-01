using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests.Mocks;
using DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests;

public class TrustProviderTests
{
    private readonly TrustProvider _sut;
    private readonly List<Group> _groups;
    private readonly Mock<ITrustHelper> _mockTrustHelper;

    public TrustProviderTests()
    {
        var mockAcademiesDbContext = new MockAcademiesDbContext();
        _groups = mockAcademiesDbContext.SetupMockDbContextGroups(5);
        _mockTrustHelper = new Mock<ITrustHelper>();

        _sut = new TrustProvider(mockAcademiesDbContext.Object, _mockTrustHelper.Object);
    }

    [Fact]
    public async Task GetTrustsByUidAsync_should_return_a_trust_if_group_found()
    {
        var group = new Group
            { GroupName = "trust 1", GroupUid = "1234", GroupType = "Multi-academy trust", Ukprn = "my ukprn" };
        _groups.Add(group);
        var expectedTrust = DummyTrustFactory.GetDummyTrust(group.GroupUid);

        _mockTrustHelper.Setup(t => t.CreateTrustFromGroup(group)).Returns(expectedTrust);

        var result = await _sut.GetTrustByUidAsync("1234");

        result.Should().Be(expectedTrust);
    }

    [Fact]
    public async Task GetTrustsByUidAsync_should_return_null_when_group_not_found()
    {
        var result = await _sut.GetTrustByUidAsync("987654321");
        result.Should().BeNull();
    }
}
