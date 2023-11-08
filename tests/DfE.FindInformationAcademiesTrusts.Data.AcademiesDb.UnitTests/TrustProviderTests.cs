using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests.Mocks;
using DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests;

public class TrustProviderTests
{
    private readonly TrustProvider _sut;
    private readonly List<Group> _groups;
    private readonly List<MstrTrust> _mstrTrusts;
    private readonly Mock<ITrustHelper> _mockTrustHelper;

    public TrustProviderTests()
    {
        var mockAcademiesDbContext = new MockAcademiesDbContext();
        _groups = mockAcademiesDbContext.SetupMockDbContextGroups(5);
        _mstrTrusts = mockAcademiesDbContext.SetupMockDbContextMstrTrust(5);
        _mockTrustHelper = new Mock<ITrustHelper>();
        _mockTrustHelper.Setup(t => t.CreateTrustFrom(It.IsAny<Group>(), It.IsAny<MstrTrust>()))
            .Returns(DummyTrustFactory.GetDummyTrust("999"));

        _sut = new TrustProvider(mockAcademiesDbContext.Object, _mockTrustHelper.Object);
    }

    [Fact]
    public async Task GetTrustsByUidAsync_should_return_a_trust_if_group_and_mstrTrust_found()
    {
        var group = new Group
            { GroupName = "trust 1", GroupUid = "1234", GroupType = "Multi-academy trust", Ukprn = "my ukprn" };
        _groups.Add(group);
        var mstrTrust = new MstrTrust
        {
            GroupUid = "1234", GORregion = "North East"
        };
        _mstrTrusts.Add(mstrTrust);
        var expectedTrust = DummyTrustFactory.GetDummyTrust(group.GroupUid);

        _mockTrustHelper.Setup(t => t.CreateTrustFrom(group, mstrTrust)).Returns(expectedTrust);

        var result = await _sut.GetTrustByUidAsync("1234");

        result.Should().Be(expectedTrust);
    }

    [Fact]
    public async Task GetTrustsByUidAsync_should_return_null_when_group_not_found()
    {
        var groupUid = "987654321";
        var mstrTrust = new MstrTrust
        {
            GroupUid = groupUid, GORregion = "North East"
        };
        _mstrTrusts.Add(mstrTrust);

        var result = await _sut.GetTrustByUidAsync(groupUid);
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetTrustsByUidAsync_should_return_null_mstrTrust_not_found()
    {
        var groupUid = "987654321";
        var group = new Group
        {
            GroupUid = groupUid
        };
        _groups.Add(group);
        var result = await _sut.GetTrustByUidAsync(groupUid);
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetTrustsByUidAsync_should_return_null_if_both_group_and_mstrTrust_not_found()
    {
        var result = await _sut.GetTrustByUidAsync("987654321");
        result.Should().BeNull();
    }
}
