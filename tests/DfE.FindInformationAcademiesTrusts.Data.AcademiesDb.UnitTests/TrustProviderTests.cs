using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Factories;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Gias;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Mstr;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests.Mocks;
using DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests;

public class TrustProviderTests
{
    private readonly TrustProvider _sut;
    private readonly List<GiasGroup> _giasGroups;
    private readonly List<MstrTrust> _mstrTrusts;
    private readonly List<GiasEstablishment> _giasEstablishments;
    private readonly List<GiasGovernance> _giasGovernances;
    private readonly Mock<ITrustFactory> _mockTrustFactory = new();
    private readonly Mock<IAcademyFactory> _mockAcademyFactory = new();
    private readonly Mock<IGovernorFactory> _mockGovernorFactory = new();
    private readonly MockAcademiesDbContext _mockAcademiesDbContext = new();

    public TrustProviderTests()
    {
        _giasGroups = _mockAcademiesDbContext.SetupMockDbContextGiasGroups(5);
        _mstrTrusts = _mockAcademiesDbContext.SetupMockDbContextMstrTrust(5);
        _giasEstablishments = _mockAcademiesDbContext.SetupMockDbContextGiasEstablishment(15);
        _giasGovernances = _mockAcademiesDbContext.SetupMockDbContextGiasGovernance(20, "Some other trust");

        _sut = new TrustProvider(_mockAcademiesDbContext.Object, _mockTrustFactory.Object, _mockAcademyFactory.Object,
            _mockGovernorFactory.Object);
    }

    [Fact]
    public async Task GetTrustsByUidAsync_should_return_a_trust_if_giasGroup_and_mstrTrust_found()
    {
        var groupUid = "1234";
        var giasGroup = CreateGiasGroup(groupUid);
        var mstrTrust = CreateMstrTrust(groupUid);
        var expectedTrust = DummyTrustFactory.GetDummyTrust(groupUid);

        _mockTrustFactory
            .Setup(t => t.CreateTrustFrom(giasGroup, mstrTrust, It.IsAny<Academy[]>(), Array.Empty<Governor>()))
            .Returns(expectedTrust);

        var result = await _sut.GetTrustByUidAsync(groupUid);

        result.Should().Be(expectedTrust);
    }

    [Fact]
    public async Task GetTrustsByUidAsync_should_return_null_when_giasGroup_not_found()
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
        var giasGroup = CreateGiasGroup(groupUid);
        var expectedTrust = DummyTrustFactory.GetDummyTrust(groupUid);

        _mockTrustFactory.Setup(t =>
                t.CreateTrustFrom(giasGroup, It.IsAny<MstrTrust>(), It.IsAny<Academy[]>(), Array.Empty<Governor>()))
            .Returns(expectedTrust);

        var result = await _sut.GetTrustByUidAsync(groupUid);

        result.Should().Be(expectedTrust);
    }

    [Fact]
    public async Task GetTrustsByUidAsync_should_return_null_if_both_giasGroup_and_mstrTrust_not_found()
    {
        var result = await _sut.GetTrustByUidAsync("987654321");
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetTrustByUidAsync_should_only_give_academies_linked_to_trust_to_trustFactory()
    {
        const string groupUid = "1234";
        var giasGroup = CreateGiasGroup(groupUid);
        var mstrTrust = CreateMstrTrust(groupUid);

        var expectedAcademies = SetUpAcademiesLinkedToTrust(_giasEstablishments.Take(3), giasGroup);
        SetUpAcademiesLinkedToTrust(_giasEstablishments.Skip(5).Take(3), CreateGiasGroup("Some other trust"));

        _mockTrustFactory
            .Setup(t => t.CreateTrustFrom(giasGroup, mstrTrust, It.IsAny<Academy[]>(), Array.Empty<Governor>()))
            .Returns(DummyTrustFactory.GetDummyTrust(groupUid));

        await _sut.GetTrustByUidAsync(groupUid);

        _mockTrustFactory.Verify(t =>
            t.CreateTrustFrom(giasGroup, mstrTrust,
                It.Is<Academy[]>(a => expectedAcademies.SequenceEqual(a)), Array.Empty<Governor>())
        );
    }

    [Fact]
    public async Task
        GetTrustByUidAsync_should_give_empty_academies_array_to_trustFactory_when_no_academies_linked_to_trust()
    {
        const string groupUid = "1234";
        var giasGroup = CreateGiasGroup(groupUid);
        var mstrTrust = CreateMstrTrust(groupUid);

        SetUpAcademiesLinkedToTrust(_giasEstablishments.Skip(5).Take(3), CreateGiasGroup("Some other trust"));

        _mockTrustFactory
            .Setup(t => t.CreateTrustFrom(giasGroup, mstrTrust, It.IsAny<Academy[]>(), Array.Empty<Governor>()))
            .Returns(DummyTrustFactory.GetDummyTrust(groupUid));

        await _sut.GetTrustByUidAsync(groupUid);

        _mockTrustFactory.Verify(t =>
            t.CreateTrustFrom(giasGroup, mstrTrust, It.Is<Academy[]>(a => !a.Any()), Array.Empty<Governor>()));
    }

    [Fact]
    public async Task GetTrustByUidAsync_should_only_give_governors_linked_to_trust_to_trustFactory()
    {
        const string groupUid = "1234";
        var giasGroup = CreateGiasGroup(groupUid);
        var mstrTrust = CreateMstrTrust(groupUid);

        var expectedGovernors = SetUpGovernorsLinkedToTrust(5, groupUid);

        _mockTrustFactory
            .Setup(t => t.CreateTrustFrom(giasGroup, mstrTrust, Array.Empty<Academy>(), It.IsAny<Governor[]>()))
            .Returns(DummyTrustFactory.GetDummyTrust(groupUid));

        await _sut.GetTrustByUidAsync(groupUid);

        _mockTrustFactory.Verify(t =>
            t.CreateTrustFrom(giasGroup, mstrTrust, Array.Empty<Academy>(),
                It.Is<Governor[]>(g => expectedGovernors.SequenceEqual(g)))
        );
    }

    [Fact]
    public async Task
        GetTrustByUidAsync_should_give_empty_governors_array_to_trustFactory_when_no_governors_linked_to_trust()
    {
        const string groupUid = "1234";
        var giasGroup = CreateGiasGroup(groupUid);

        _mockTrustFactory
            .Setup(t => t.CreateTrustFrom(giasGroup, null, Array.Empty<Academy>(), Array.Empty<Governor>()))
            .Returns(DummyTrustFactory.GetDummyTrust(groupUid));

        await _sut.GetTrustByUidAsync(groupUid);

        _mockTrustFactory.Verify(t =>
            t.CreateTrustFrom(giasGroup, null, It.IsAny<Academy[]>(), It.Is<Governor[]>(g => !g.Any())));
    }

    private List<Academy> SetUpAcademiesLinkedToTrust(IEnumerable<GiasEstablishment> giasEstablishmentsLinkedToTrust,
        GiasGroup giasGroup)
    {
        var establishmentGroupLinks =
            _mockAcademiesDbContext.LinkGiasEstablishmentsToGiasGroup(giasEstablishmentsLinkedToTrust, giasGroup);
        var academiesLinkedToTrust = new List<Academy>();
        foreach (var (giasEstablishment, giasGroupLink) in establishmentGroupLinks)
        {
            var dummyAcademy = DummyAcademyFactory.GetDummyAcademy(giasEstablishment.Urn);
            _mockAcademyFactory.Setup(a => a.CreateAcademyFrom(giasGroupLink, giasEstablishment))
                .Returns(dummyAcademy);
            academiesLinkedToTrust.Add(dummyAcademy);
        }

        return academiesLinkedToTrust;
    }

    private List<Governor> SetUpGovernorsLinkedToTrust(int num, string groupUid)
    {
        var newGiasGovernances = new List<GiasGovernance>();
        for (var i = 0; i < num; i++)
        {
            newGiasGovernances.Add(new GiasGovernance
            {
                Uid = groupUid,
                Forename1 = $"Governor {i}"
            });
        }

        _giasGovernances.AddRange(newGiasGovernances);

        var governorsLinkedToTrust = new List<Governor>();

        var dummyGovernorFactory = new DummyGovernorFactory();
        foreach (var giasGovernance in newGiasGovernances)
        {
            var dummyGovernor = dummyGovernorFactory.GetDummyGovernor(groupUid);
            _mockGovernorFactory.Setup(g => g.CreateFrom(giasGovernance))
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

    private GiasGroup CreateGiasGroup(string groupUid)
    {
        var giasGroup = new GiasGroup
            { GroupName = "trust 1", GroupUid = groupUid, GroupType = "Multi-academy trust", Ukprn = "my ukprn" };
        _giasGroups.Add(giasGroup);
        return giasGroup;
    }
}
