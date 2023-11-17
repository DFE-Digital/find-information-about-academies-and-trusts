using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Factories;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Gias;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Mstr;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests.Mocks;
using DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests;

public class TrustProviderTests
{
    private readonly TrustProvider _sut;
    private readonly List<Group> _groups;
    private readonly List<MstrTrust> _mstrTrusts;
    private readonly List<Establishment> _establishments;
    private readonly List<Governance> _governances;
    private readonly Mock<ITrustFactory> _mockTrustFactory = new();
    private readonly Mock<IAcademyFactory> _mockAcademyFactory = new();
    private readonly Mock<IGovernorFactory> _mockGovernorFactory = new();
    private readonly MockAcademiesDbContext _mockAcademiesDbContext = new();

    public TrustProviderTests()
    {
        _groups = _mockAcademiesDbContext.SetupMockDbContextGroups(5);
        _mstrTrusts = _mockAcademiesDbContext.SetupMockDbContextMstrTrust(5);
        _establishments = _mockAcademiesDbContext.SetupMockDbContextEstablishment(15);
        _governances = _mockAcademiesDbContext.SetupMockDbContextGovernance(20, "Some other trust");


        _sut = new TrustProvider(_mockAcademiesDbContext.Object, _mockTrustFactory.Object, _mockAcademyFactory.Object,
            _mockGovernorFactory.Object);
    }

    [Fact]
    public async Task GetTrustsByUidAsync_should_return_a_trust_if_group_and_mstrTrust_found()
    {
        var groupUid = "1234";
        var group = CreateGroup(groupUid);
        var mstrTrust = CreateMstrTrust(groupUid);
        var expectedTrust = DummyTrustFactory.GetDummyTrust(groupUid);

        _mockTrustFactory
            .Setup(t => t.CreateTrustFrom(group, mstrTrust, It.IsAny<Academy[]>(), Array.Empty<Governor>()))
            .Returns(expectedTrust);

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
    public async Task GetTrustsByUidAsync_should_return_a_trust_if_mstrTrust_not_found()
    {
        var groupUid = "987654321";
        var group = CreateGroup(groupUid);
        var expectedTrust = DummyTrustFactory.GetDummyTrust(groupUid);

        _mockTrustFactory.Setup(t =>
                t.CreateTrustFrom(group, It.IsAny<MstrTrust>(), It.IsAny<Academy[]>(), Array.Empty<Governor>()))
            .Returns(expectedTrust);

        var result = await _sut.GetTrustByUidAsync(groupUid);

        result.Should().Be(expectedTrust);
    }

    [Fact]
    public async Task GetTrustsByUidAsync_should_return_null_if_both_group_and_mstrTrust_not_found()
    {
        var result = await _sut.GetTrustByUidAsync("987654321");
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetTrustByUidAsync_should_only_give_academies_linked_to_trust_to_trustFactory()
    {
        const string groupUid = "1234";
        var group = CreateGroup(groupUid);
        var mstrTrust = CreateMstrTrust(groupUid);

        var expectedAcademies = SetUpAcademiesLinkedToTrust(_establishments.Take(3), group);
        SetUpAcademiesLinkedToTrust(_establishments.Skip(5).Take(3), CreateGroup("Some other trust"));

        _mockTrustFactory
            .Setup(t => t.CreateTrustFrom(group, mstrTrust, It.IsAny<Academy[]>(), Array.Empty<Governor>()))
            .Returns(DummyTrustFactory.GetDummyTrust(groupUid));

        await _sut.GetTrustByUidAsync(groupUid);

        _mockTrustFactory.Verify(t =>
            t.CreateTrustFrom(group, mstrTrust,
                It.Is<Academy[]>(a => expectedAcademies.SequenceEqual(a)), Array.Empty<Governor>())
        );
    }

    [Fact]
    public async Task
        GetTrustByUidAsync_should_give_empty_academies_array_to_trustFactory_when_no_academies_linked_to_trust()
    {
        const string groupUid = "1234";
        var group = CreateGroup(groupUid);
        var mstrTrust = CreateMstrTrust(groupUid);

        SetUpAcademiesLinkedToTrust(_establishments.Skip(5).Take(3), CreateGroup("Some other trust"));

        _mockTrustFactory
            .Setup(t => t.CreateTrustFrom(group, mstrTrust, It.IsAny<Academy[]>(), Array.Empty<Governor>()))
            .Returns(DummyTrustFactory.GetDummyTrust(groupUid));

        await _sut.GetTrustByUidAsync(groupUid);

        _mockTrustFactory.Verify(t =>
            t.CreateTrustFrom(group, mstrTrust, It.Is<Academy[]>(a => !a.Any()), Array.Empty<Governor>()));
    }

    [Fact]
    public async Task GetTrustByUidAsync_should_only_give_governors_linked_to_trust_to_trustFactory()
    {
        const string groupUid = "1234";
        var group = CreateGroup(groupUid);
        var mstrTrust = CreateMstrTrust(groupUid);

        var expectedGovernors = SetUpGovernorsLinkedToTrust(5, groupUid);

        _mockTrustFactory
            .Setup(t => t.CreateTrustFrom(group, mstrTrust, Array.Empty<Academy>(), It.IsAny<Governor[]>()))
            .Returns(DummyTrustFactory.GetDummyTrust(groupUid));

        await _sut.GetTrustByUidAsync(groupUid);

        _mockTrustFactory.Verify(t =>
            t.CreateTrustFrom(group, mstrTrust, Array.Empty<Academy>(),
                It.Is<Governor[]>(g => expectedGovernors.SequenceEqual(g)))
        );
    }

    [Fact]
    public async Task
        GetTrustByUidAsync_should_give_empty_governors_array_to_trustFactory_when_no_governors_linked_to_trust()
    {
        const string groupUid = "1234";
        var group = CreateGroup(groupUid);

        _mockTrustFactory
            .Setup(t => t.CreateTrustFrom(group, null, Array.Empty<Academy>(), Array.Empty<Governor>()))
            .Returns(DummyTrustFactory.GetDummyTrust(groupUid));

        await _sut.GetTrustByUidAsync(groupUid);

        _mockTrustFactory.Verify(t =>
            t.CreateTrustFrom(group, null, It.IsAny<Academy[]>(), It.Is<Governor[]>(g => !g.Any())));
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
            _mockAcademyFactory.Setup(a => a.CreateAcademyFrom(groupLink, establishment))
                .Returns(dummyAcademy);
            academiesLinkedToTrust.Add(dummyAcademy);
        }

        return academiesLinkedToTrust;
    }

    private List<Governor> SetUpGovernorsLinkedToTrust(int num, string groupUid)
    {
        var newGovernances = new List<Governance>();
        for (var i = 0; i < num; i++)
        {
            newGovernances.Add(new Governance
            {
                Uid = groupUid,
                Forename1 = $"Governor {i}"
            });
        }

        _governances.AddRange(newGovernances);

        var governorsLinkedToTrust = new List<Governor>();

        var dummyGovernorFactory = new DummyGovernorFactory();
        foreach (var governance in newGovernances)
        {
            var dummyGovernor = dummyGovernorFactory.GetDummyGovernor(groupUid);
            _mockGovernorFactory.Setup(g => g.CreateFrom(governance))
                .Returns(dummyGovernor);
            governorsLinkedToTrust.Add(dummyGovernor);
        }

        return governorsLinkedToTrust;
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
