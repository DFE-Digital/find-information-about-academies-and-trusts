using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Factories;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Cdm;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Gias;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Mstr;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests.Mocks;
using DfE.FindInformationAcademiesTrusts.Data.UnitTests.Mocks;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests;

public class TrustProviderTests
{
    private readonly TrustProvider _sut;
    private readonly List<GiasEstablishment> _giasEstablishments;
    private readonly List<GiasGovernance> _giasGovernances;
    private readonly List<MstrTrustGovernance> _mstrTrustGovernances;
    private readonly Mock<ITrustFactory> _mockTrustFactory = new();
    private readonly Mock<IAcademyFactory> _mockAcademyFactory = new();
    private readonly Mock<IGovernorFactory> _mockGovernorFactory = new();
    private readonly Mock<IPersonFactory> _mockPersonFactory = new();
    private readonly MockAcademiesDbContext _mockAcademiesDbContext = new();
    private readonly string _groupUidToGet = "1234";
    private readonly GiasGroup _giasGroupInDb;
    private readonly MstrTrust _mstrTrustInDb;

    public TrustProviderTests()
    {
        _mockAcademiesDbContext.SetupMockDbContextGiasGroups(5);
        _mockAcademiesDbContext.SetupMockDbContextMstrTrust(5);
        _giasEstablishments = _mockAcademiesDbContext.SetupMockDbContextGiasEstablishment(15);
        _giasGovernances = _mockAcademiesDbContext.SetupMockDbContextGiasGovernance(20, "Some other trust");
        _mstrTrustGovernances = _mockAcademiesDbContext.SetupMockDbContextMstrTrustGovernance(20);
        SetUpAcademiesLinkedToTrust(_giasEstablishments.Skip(10).Take(3),
            _mockAcademiesDbContext.CreateGiasGroup("Some other trust"));
        SetUpGovernorsLinkedToTrust(5, "Some other trust");

        _sut = new TrustProvider(_mockAcademiesDbContext.Object, _mockTrustFactory.Object, _mockAcademyFactory.Object,
            _mockGovernorFactory.Object, _mockPersonFactory.Object);

        _giasGroupInDb = _mockAcademiesDbContext.CreateGiasGroup(_groupUidToGet);
        _mstrTrustInDb = _mockAcademiesDbContext.CreateMstrTrust(_groupUidToGet);

        _mockTrustFactory
            .Setup(t => t.CreateTrustFrom(It.IsAny<GiasGroup>(), It.IsAny<MstrTrust>(),
                It.IsAny<Academy[]>(), It.IsAny<Governor[]>(),
                It.IsAny<Person>(), It.IsAny<Person>()))
            .Returns((GiasGroup g, MstrTrust _, Academy[] _, Governor[] _, Person _, Person _) =>
                DummyTrustFactory.GetDummyTrust(g.GroupUid!));
    }

    [Fact]
    public async Task GetTrustsByUidAsync_should_return_a_trust_if_giasGroup_and_mstrTrust_found()
    {
        var expectedTrust = DummyTrustFactory.GetDummyTrust(_groupUidToGet);

        _mockTrustFactory
            .Setup(t => t.CreateTrustFrom(_giasGroupInDb, _mstrTrustInDb,
                It.IsAny<Academy[]>(), It.IsAny<Governor[]>(),
                It.IsAny<Person>(), It.IsAny<Person>()))
            .Returns(expectedTrust);

        var result = await _sut.GetTrustByUidAsync(_groupUidToGet);

        result.Should().Be(expectedTrust);
    }

    [Fact]
    public async Task GetTrustsByUidAsync_should_return_null_when_giasGroup_not_found()
    {
        const string groupUidWithoutGiasGroup = "987654321";
        _mockAcademiesDbContext.CreateMstrTrust(groupUidWithoutGiasGroup);

        var result = await _sut.GetTrustByUidAsync(groupUidWithoutGiasGroup);
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetTrustsByUidAsync_should_return_a_trust_if_mstrTrust_not_found()
    {
        const string groupUidWithoutMstrTrust = "987654321";
        var giasGroup = _mockAcademiesDbContext.CreateGiasGroup(groupUidWithoutMstrTrust);
        var expectedTrust = DummyTrustFactory.GetDummyTrust(groupUidWithoutMstrTrust);

        _mockTrustFactory.Setup(t =>
                t.CreateTrustFrom(giasGroup, null,
                    It.IsAny<Academy[]>(), It.IsAny<Governor[]>(),
                    It.IsAny<Person>(), It.IsAny<Person>()))
            .Returns(expectedTrust);

        var result = await _sut.GetTrustByUidAsync(groupUidWithoutMstrTrust);

        result.Should().Be(expectedTrust);
    }

    [Fact]
    public async Task GetTrustsByUidAsync_should_return_null_if_both_giasGroup_and_mstrTrust_not_found()
    {
        var result = await _sut.GetTrustByUidAsync("this uid doesn't exist");
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetTrustByUidAsync_should_only_give_academies_linked_to_trust_to_trustFactory()
    {
        var expectedAcademies = SetUpAcademiesLinkedToTrust(_giasEstablishments.Take(3), _giasGroupInDb);

        await _sut.GetTrustByUidAsync(_groupUidToGet);

        _mockTrustFactory.Verify(t =>
            t.CreateTrustFrom(_giasGroupInDb, _mstrTrustInDb,
                It.Is<Academy[]>(a => expectedAcademies.SequenceEqual(a)), Array.Empty<Governor>(), null, null)
        );
    }

    [Fact]
    public async Task
        GetTrustByUidAsync_should_give_empty_academies_array_to_trustFactory_when_no_academies_linked_to_trust()
    {
        await _sut.GetTrustByUidAsync(_groupUidToGet);

        _mockTrustFactory.Verify(t =>
            t.CreateTrustFrom(_giasGroupInDb, _mstrTrustInDb, It.Is<Academy[]>(a => !a.Any()), Array.Empty<Governor>(),
                null,
                null));
    }

    [Fact]
    public async Task GetTrustByUidAsync_should_only_give_governors_linked_to_trust_to_trustFactory()
    {
        var expectedGovernors = SetUpGovernorsLinkedToTrust(5, _groupUidToGet);

        await _sut.GetTrustByUidAsync(_groupUidToGet);

        _mockTrustFactory.Verify(t =>
            t.CreateTrustFrom(_giasGroupInDb, _mstrTrustInDb, Array.Empty<Academy>(),
                It.Is<Governor[]>(g => expectedGovernors.SequenceEqual(g)), null, null)
        );
    }

    [Fact]
    public async Task
        GetTrustByUidAsync_should_give_empty_governors_array_to_trustFactory_when_no_governors_linked_to_trust()
    {
        await _sut.GetTrustByUidAsync(_groupUidToGet);

        _mockTrustFactory.Verify(t =>
            t.CreateTrustFrom(_giasGroupInDb, _mstrTrustInDb, It.IsAny<Academy[]>(), It.Is<Governor[]>(g => !g.Any()),
                null, null));
    }

    [Fact]
    public async Task GetTrustByUidAsync_should_give_null_to_trustFactory_when_no_linked_trustRelationshipManager()
    {
        await _sut.GetTrustByUidAsync(_groupUidToGet);

        _mockTrustFactory.Verify(t =>
            t.CreateTrustFrom(_giasGroupInDb, _mstrTrustInDb, It.IsAny<Academy[]>(), It.IsAny<Governor[]>(),
                null, null));
    }

    [Fact]
    public async Task GetTrustByUidAsync_should_give_linked_trustRelationshipManager_to_trustFactory()
    {
        var expectedTrustRelationshipManager =
            CreateTrustRelationshipManager(_groupUidToGet, "trustRelationshipManager", "trm@education.gov.uk");
        await _sut.GetTrustByUidAsync(_groupUidToGet);

        _mockTrustFactory.Verify(t =>
            t.CreateTrustFrom(_giasGroupInDb, _mstrTrustInDb, It.IsAny<Academy[]>(), It.IsAny<Governor[]>(),
                expectedTrustRelationshipManager, null));
    }

    [Fact]
    public async Task GetTrustByUidAsync_should_give_null_to_trustFactory_when_no_linked_sfsoLead()
    {
        await _sut.GetTrustByUidAsync(_groupUidToGet);

        _mockTrustFactory.Verify(t =>
            t.CreateTrustFrom(_giasGroupInDb, _mstrTrustInDb, It.IsAny<Academy[]>(), It.IsAny<Governor[]>(),
                null, null));
    }

    [Fact]
    public async Task GetTrustByUidAsync_should_give_linked_sfsoLead_to_trustFactory()
    {
        var expectedSfsoLead =
            CreateSfsoLead(_groupUidToGet, "SFSO Lead", "sfsoLead@education.gov.uk");
        await _sut.GetTrustByUidAsync(_groupUidToGet);

        _mockTrustFactory.Verify(t =>
            t.CreateTrustFrom(_giasGroupInDb, _mstrTrustInDb, It.IsAny<Academy[]>(), It.IsAny<Governor[]>(),
                null, expectedSfsoLead));
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
            _mockAcademyFactory.Setup(a => a.CreateAcademyFrom(giasGroupLink, giasEstablishment, null))
                .Returns(dummyAcademy);
            academiesLinkedToTrust.Add(dummyAcademy);
        }

        return academiesLinkedToTrust;
    }

    private List<Governor> SetUpGovernorsLinkedToTrust(int num, string groupUid)
    {
        var numExistingGovernances = _giasGovernances.Count;
        var governorsLinkedToTrust = new List<Governor>();

        for (var i = 0; i < num; i++)
        {
            var gid = (i + numExistingGovernances).ToString();
            var giasGovernance = new GiasGovernance
            {
                Gid = gid,
                Uid = groupUid,
                Forename1 = $"Governor {i}"
            };
            var mstrTrustGovernance = new MstrTrustGovernance
            {
                Gid = gid,
                Forename1 = $"Governor {i}",
                Email = $"governor{i}@trust{groupUid}.com"
            };

            _giasGovernances.Add(giasGovernance);
            _mstrTrustGovernances.Add(mstrTrustGovernance);

            var dummyGovernor = DummyGovernorFactory.GetDummyGovernor(gid, groupUid);
            _mockGovernorFactory.Setup(g => g.CreateFrom(giasGovernance, mstrTrustGovernance))
                .Returns(dummyGovernor);
            governorsLinkedToTrust.Add(dummyGovernor);
        }

        return governorsLinkedToTrust;
    }

    private Person CreateTrustRelationshipManager(string groupUid, string fullName, string email)
    {
        return CreatePerson(groupUid, fullName, email,
            (cdmAccount, cdmSystemuser) => cdmAccount.SipTrustrelationshipmanager = cdmSystemuser.Systemuserid);
    }

    private Person CreateSfsoLead(string groupUid, string fullName, string email)
    {
        return CreatePerson(groupUid, fullName, email,
            (cdmAccount, cdmSystemuser) => cdmAccount.SipAmsdlead = cdmSystemuser.Systemuserid);
    }

    private Person CreatePerson(string groupUid, string fullName, string email,
        Action<CdmAccount, CdmSystemuser> accountSetup)
    {
        var person = new Person(fullName, email);

        var cdmAccount = _mockAcademiesDbContext.CreateCdmAccount(groupUid);
        var cdmSystemuser = _mockAcademiesDbContext.CreateCdmSystemuser(fullName, email);

        accountSetup(cdmAccount, cdmSystemuser);

        _mockPersonFactory.Setup(p => p.CreateFrom(cdmSystemuser)).Returns(person);

        return person;
    }
}
