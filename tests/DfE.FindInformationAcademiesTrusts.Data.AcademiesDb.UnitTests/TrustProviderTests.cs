using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests.Mocks;
using DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests;

public class TrustProviderTests
{
    private readonly TrustProvider _sut;
    private readonly List<Group> _groups;
    private readonly List<MstrTrust> _mstrTrusts;
    private readonly List<Establishment> _establishments;
    private readonly Mock<ITrustHelper> _mockTrustHelper = new();
    private readonly Mock<IAcademyHelper> _mockAcademyHelper = new();
    private readonly MockAcademiesDbContext _mockAcademiesDbContext = new();

    public TrustProviderTests()
    {
        _groups = _mockAcademiesDbContext.SetupMockDbContextGroups(5);
        _mstrTrusts = _mockAcademiesDbContext.SetupMockDbContextMstrTrust(5);
        _establishments = _mockAcademiesDbContext.SetupMockDbContextEstablishment(15);

        _sut = new TrustProvider(_mockAcademiesDbContext.Object, _mockTrustHelper.Object, _mockAcademyHelper.Object);
    }

    [Fact]
    public async Task GetTrustsByUidAsync_should_return_a_trust_if_group_and_mstrTrust_found()
    {
        var groupUid = "1234";
        var group = CreateGroup(groupUid);
        var mstrTrust = CreateMstrTrust(groupUid);
        var expectedTrust = DummyTrustFactory.GetDummyTrust(groupUid);

        _mockTrustHelper.Setup(t => t.CreateTrustFrom(group, mstrTrust, It.IsAny<Academy[]>())).Returns(expectedTrust);

        var result = await _sut.GetTrustByUidAsync(groupUid);

        result.Should().Be(expectedTrust);
    }

    [Fact]
    public async Task GetTrustsByUidAsync_should_return_null_when_group_not_found()
    {
        var groupUid = "987654321";
        CreateMstrTrust(groupUid);

        var result = await _sut.GetTrustByUidAsync(groupUid);
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetTrustsByUidAsync_should_return_null_mstrTrust_not_found()
    {
        var groupUid = "987654321";
        CreateGroup(groupUid);

        var result = await _sut.GetTrustByUidAsync(groupUid);
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetTrustsByUidAsync_should_return_null_if_both_group_and_mstrTrust_not_found()
    {
        var result = await _sut.GetTrustByUidAsync("987654321");
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetTrustByUidAsync_should_only_give_academies_linked_to_trust_to_trustHelper()
    {
        const string groupUid = "1234";
        var group = CreateGroup(groupUid);
        var mstrTrust = CreateMstrTrust(groupUid);

        var expectedAcademies = SetUpAcademiesLinkedToTrust(_establishments.Take(3), group);
        SetUpAcademiesLinkedToTrust(_establishments.Skip(5).Take(3), CreateGroup("Some other trust"));

        _mockTrustHelper
            .Setup(t => t.CreateTrustFrom(group, mstrTrust, It.IsAny<Academy[]>()))
            .Returns(DummyTrustFactory.GetDummyTrust(groupUid));

        await _sut.GetTrustByUidAsync(groupUid);

        _mockTrustHelper.Verify(t =>
            t.CreateTrustFrom(group, mstrTrust,
                It.Is<Academy[]>(a => expectedAcademies.SequenceEqual(a)))
        );
    }

    [Fact]
    public async Task
        GetTrustByUidAsync_should_give_empty_academies_array_to_trustHelper_when_no_academies_linked_to_trust()
    {
        const string groupUid = "1234";
        var group = CreateGroup(groupUid);
        var mstrTrust = CreateMstrTrust(groupUid);

        SetUpAcademiesLinkedToTrust(_establishments.Skip(5).Take(3), CreateGroup("Some other trust"));

        _mockTrustHelper
            .Setup(t => t.CreateTrustFrom(group, mstrTrust, It.IsAny<Academy[]>()))
            .Returns(DummyTrustFactory.GetDummyTrust(groupUid));

        await _sut.GetTrustByUidAsync(groupUid);

        _mockTrustHelper.Verify(t => t.CreateTrustFrom(group, mstrTrust, It.Is<Academy[]>(a => !a.Any())));
    }

    private List<Academy> SetUpAcademiesLinkedToTrust(IEnumerable<Establishment> establishmentsLinkedToTrust,
        Group group)
    {
        var establishmentGroupLinks =
            _mockAcademiesDbContext.LinkEstablishmentsToGroup(establishmentsLinkedToTrust, group);
        var academiesLinkedToTrust = new List<Academy>();
        foreach (var (establishment, groupLink) in establishmentGroupLinks)
        {
            var dummyAcademy = DummyAcademyFactory.GetDummyAcademy(establishment.Urn);
            _mockAcademyHelper.Setup(a => a.CreateAcademyFrom(groupLink, establishment))
                .Returns(dummyAcademy);
            academiesLinkedToTrust.Add(dummyAcademy);
        }

        return academiesLinkedToTrust;
    }

    private MstrTrust CreateMstrTrust(string groupUid)
    {
        var mstrTrust = new MstrTrust
        {
            GroupUid = groupUid, GORregion = "North East"
        };
        _mstrTrusts.Add(mstrTrust);
        return mstrTrust;
    }

    private Group CreateGroup(string groupUid)
    {
        var group = new Group
            { GroupName = "trust 1", GroupUid = groupUid, GroupType = "Multi-academy trust", Ukprn = "my ukprn" };
        _groups.Add(group);
        return group;
    }
}
