using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Cdm;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Gias;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Mstr;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Repositories;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests.Mocks;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.Trust;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests.Repositories;

public class TrustRepositoryTests
{
    private readonly TrustRepository _sut;
    private readonly MockAcademiesDbContext _mockAcademiesDbContext = new();

    private readonly DateTime _lastYear = DateTime.Today.AddYears(-1);
    private readonly DateTime _nextYear = DateTime.Today.AddYears(1);
    private readonly DateTime _lastDecade = DateTime.Today.AddYears(-10);
    private readonly DateTime _yesterday = DateTime.Today.AddDays(-1);
    private readonly DateTime _today = DateTime.Today;
    private readonly DateTime _tomorrow = DateTime.Today.AddDays(1);

    public TrustRepositoryTests()
    {
        _sut = new TrustRepository(_mockAcademiesDbContext.Object);
    }

    [Theory]
    [InlineData("2806", "My Trust", "Multi-academy trust")]
    [InlineData("9008", "Another Trust", "Single-academy trust")]
    [InlineData("9008", "Trust with no academies", "Multi-academy trust")]
    public async Task GetTrustSummaryAsync_should_return_trustSummary_if_found(string uid, string name, string type)
    {
        _ = _mockAcademiesDbContext.AddGiasGroup(uid, name, type);

        var result = await _sut.GetTrustSummaryAsync(uid);
        result.Should().BeEquivalentTo(new TrustSummary(name, type));
    }

    [Fact]
    public async Task GetTrustSummaryAsync_should_return_empty_values_on_null_group_fields()
    {
        _ = _mockAcademiesDbContext.AddGiasGroup("2806", null, null);

        var result = await _sut.GetTrustSummaryAsync("2806");
        result.Should().BeEquivalentTo(new TrustSummary(string.Empty, string.Empty));
    }

    [Fact]
    public async Task GetTrustDetailsAsync_should_get_regionAndTerritory_from_mstrTrusts()
    {
        _ = _mockAcademiesDbContext.AddGiasGroup("2806");
        _ = _mockAcademiesDbContext.AddMstrTrust("2806", "My Region");

        var result = await _sut.GetTrustDetailsAsync("2806");

        result.RegionAndTerritory.Should().Be("My Region");
    }

    [Fact]
    public async Task GetTrustDetailsAsync_should_set_regionAndTerritory_to_empty_string_when_mstrTrust_not_available()
    {
        _ = _mockAcademiesDbContext.AddGiasGroup("2806");

        var result = await _sut.GetTrustDetailsAsync("2806");

        result.RegionAndTerritory.Should().BeEmpty();
    }

    [Fact]
    public async Task
        GetTrustDetailsAsync_should_set_regionAndTerritory_to_empty_string_when_GORregion_in_mstrTrust_null()
    {
        _ = _mockAcademiesDbContext.AddGiasGroup("2806");
        _ = _mockAcademiesDbContext.AddMstrTrust("2806", null);

        var result = await _sut.GetTrustDetailsAsync("2806");

        result.RegionAndTerritory.Should().BeEmpty();
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
    public async Task GetTrustDetailsAsync_should_build_address_from_giasGroup(string? groupContactStreet,
        string? groupContactLocality, string? groupContactTown, string? groupContactPostcode,
        string? expectedAddress)
    {
        _mockAcademiesDbContext.AddGiasGroup(new GiasGroup
        {
            GroupUid = "2806",
            GroupType = "Multi-academy trust",
            GroupContactStreet = groupContactStreet,
            GroupContactLocality = groupContactLocality,
            GroupContactTown = groupContactTown,
            GroupContactPostcode = groupContactPostcode
        });

        var result = await _sut.GetTrustDetailsAsync("2806");

        result.Address.Should().Be(expectedAddress);
    }

    [Fact]
    public async Task GetTrustDetailsAsync_should_set_properties_from_giasGroup()
    {
        _mockAcademiesDbContext.AddGiasGroup(new GiasGroup
        {
            GroupUid = "2806",
            GroupId = "TR0012",
            Ukprn = "10012345",
            GroupType = "Multi-academy trust",
            CompaniesHouseNumber = "123456",
            IncorporatedOnOpenDate = "28/06/2007"
        });

        var result = await _sut.GetTrustDetailsAsync("2806");

        result.Should().BeEquivalentTo(new TrustDetails("2806",
            "TR0012",
            "10012345",
            "123456",
            "Multi-academy trust",
            "",
            "",
            new DateTime(2007, 6, 28)
        ));
    }

    [Fact]
    public async Task GetTrustGovernanceAsync_ShouldReturnEmpty_WithNoGovernanceSet()
    {
        var result = await _sut.GetTrustGovernanceAsync("1234");

        result.Should().BeEquivalentTo(new TrustGovernance([], [], [], []));
    }

    [Theory]
    [InlineData("Member")]
    [InlineData("Trustee")]
    [InlineData("Accounting Officer")]
    [InlineData("Chief Financial Officer")]
    [InlineData("Chair of Trustees")]
    public async Task GetTrustGovernanceAsync_ShouldReturnValidGovernors_WithTheCorrectRole(string role)
    {
        var output = CreateGovernor("1234", "9999", _lastYear, _nextYear, role, email: null);

        var result = await _sut.GetTrustGovernanceAsync("1234");

        var members = new List<Governor>();
        var trustees = new List<Governor>();
        var trustLeadership = new List<Governor>();

        switch (role)
        {
            case "Member":
                members.Add(output);
                break;
            case "Trustee":
                trustees.Add(output);
                break;
            case "Accounting Officer":
            case "Chief Financial Officer":
            case "Chair of Trustees":
                trustLeadership.Add(output);
                break;
        }

        result.Should().BeEquivalentTo(
            new TrustGovernance(trustLeadership.ToArray(), members.ToArray(), trustees.ToArray(), [])
        );
    }

    [Theory]
    [InlineData("Shared Chair of Local Governing Body - Group")]
    [InlineData("Shared Local Governor - Group")]
    [InlineData("Governor")]
    [InlineData("Local Govenor")]
    public async Task GetTrustGovernanceAsync_ShouldReturnNoGovernors_WithTheIncorrectRole(string role)
    {
        _ = CreateGovernor("1234", "9999", _lastYear, _nextYear, role);
        var result = await _sut.GetTrustGovernanceAsync("1234");

        result.Should().BeEquivalentTo(
            new TrustGovernance([], [], [], [])
        );
    }

    [Theory]
    [InlineData("Member")]
    [InlineData("Trustee")]
    [InlineData("Accounting Officer")]
    [InlineData("Chief Financial Officer")]
    [InlineData("Chair of Trustees")]
    [InlineData("Shared Chair of Local Governing Body - Group")]
    [InlineData("Shared Local Governor - Group")]
    [InlineData("Governor")]
    [InlineData("Local Govenor")]
    public async Task GetTrustGovernanceAsync_ShouldReturnHistoricGovernors_WhenTheyExist_IrrespectiveOfRole(
        string role)
    {
        var output = CreateGovernor("1234", "9999", _lastDecade, _lastYear, role, email: null);

        var result = await _sut.GetTrustGovernanceAsync("1234");

        result.Should().BeEquivalentTo(
            new TrustGovernance([], [], [], [output])
        );
    }

    [Fact]
    public async Task GetTrustGovernanceAsync_ShouldSortHistoricAndCurrentGovernors()
    {
        var currentMember1 = CreateGovernor("1234", "9999", _lastDecade, _today, email: null);
        var currentMember2 = CreateGovernor("1234", "9998", _lastDecade, _tomorrow, email: null);
        var historicMember = CreateGovernor("1234", "9997", _lastDecade, _yesterday, email: null);

        var result = await _sut.GetTrustGovernanceAsync("1234");

        result.Should().BeEquivalentTo(
            new TrustGovernance([], [currentMember1, currentMember2], [], [historicMember])
        );
    }

    [Fact]
    public async Task GetTrustContactsAsync_Should_Return_Valid_TrustRelationshipManager_WhenOneIsPresentForTheTrust()
    {
        var trm = CreateTrustRelationshipManager("1234", "Trust Relationship Manager", "trm@testemail.com");
        var result = await _sut.GetTrustContactsAsync("1234");
        result.TrustRelationshipManager.Should().BeEquivalentTo(trm);
    }

    [Fact]
    public async Task GetTrustContactsAsync_Should_Return_Valid_SFSOLead_WhenOneIsPresentForTheTrust()
    {
        var sfsoLead = CreateSfsoLead("1234", "SFSO Lead", "sfsolead@testemail.com");
        var result = await _sut.GetTrustContactsAsync("1234");
        result.SfsoLead.Should().BeEquivalentTo(sfsoLead);
    }

    [Fact]
    public async Task GetTrustContactsAsync_Should_Return_Valid_ChairOfTrustees_WhenOneIsPresentForTheTrust()
    {
        var governor = CreateGovernor("1234", "9876", null, _tomorrow, "Chair of Trustees");
        var result = await _sut.GetTrustContactsAsync("1234");
        result.ChairOfTrustees.Should().NotBeNull();
        result.ChairOfTrustees!.FullName.Should().Be(governor.FullName);
        result.ChairOfTrustees!.Email.Should().Be(governor.Email);
    }

    [Fact]
    public async Task GetTrustContactsAsync_Should_Return_Valid_ChiefFinancialOfficer_WhenOneIsPresentForTheTrust()
    {
        var governor = CreateGovernor("1234", "9876", null, _tomorrow, "Chief Financial Officer");
        var result = await _sut.GetTrustContactsAsync("1234");
        result.ChiefFinancialOfficer.Should().NotBeNull();
        result.ChiefFinancialOfficer!.FullName.Should().Be(governor.FullName);
        result.ChiefFinancialOfficer!.Email.Should().Be(governor.Email);
    }

    [Fact]
    public async Task GetTrustContactsAsync_Should_Return_Valid_AccountingOfficer_WhenOneIsPresentForTheTrust()
    {
        var governor = CreateGovernor("1234", "9876", null, _tomorrow, "Accounting Officer");
        var result = await _sut.GetTrustContactsAsync("1234");
        result.AccountingOfficer.Should().NotBeNull();
        result.AccountingOfficer!.FullName.Should().Be(governor.FullName);
        result.AccountingOfficer!.Email.Should().Be(governor.Email);
    }

    [Fact]
    public async Task GetTrustContactsAsync_Should_Return_null_WhenNoDataIsPresent()
    {
        var result = await _sut.GetTrustContactsAsync("9876");
        result.TrustRelationshipManager.Should().BeNull();
        result.SfsoLead.Should().BeNull();
        result.ChairOfTrustees.Should().BeNull();
        result.ChiefFinancialOfficer.Should().BeNull();
        result.AccountingOfficer.Should().BeNull();
    }

    [Fact]
    public async Task GetTrustContactsAsync_Should_Return_null_WhenOtherGovernorsArePresent()
    {
        _ = CreateGovernor("1234", "9876", null, _tomorrow); //Incorrect role
        _ = CreateGovernor("9876", "9876", null, _tomorrow, "Chief Financial Officer"); //Incorrect uid
        var result = await _sut.GetTrustContactsAsync("1234");
        result.TrustRelationshipManager.Should().BeNull();
        result.SfsoLead.Should().BeNull();
        result.ChairOfTrustees.Should().BeNull();
        result.ChiefFinancialOfficer.Should().BeNull();
        result.AccountingOfficer.Should().BeNull();
    }

    [Fact]
    public async Task GetTrustContactsAsync_Should_Return_CorrectDetails_EvenWithoutMatch_in_MstrTrustGovernance_table()
    {
        var input = new GiasGovernance
        {
            Gid = "9999",
            Uid = "1234",
            Role = "Chair of Trustees",
            Forename1 = "First",
            Forename2 = "Second",
            Surname = "Last",
            DateOfAppointment = _lastYear.ToString("dd/MM/yyyy"),
            DateTermOfOfficeEndsEnded = _nextYear.ToString("dd/MM/yyyy"),
            AppointingBody = "Nick Warms"
        };

        _mockAcademiesDbContext.AddGiasGovernance(input);
        var result = await _sut.GetTrustContactsAsync("1234");
        result.ChairOfTrustees.Should().NotBeNull();
        result.ChairOfTrustees!.FullName.Should().Be("First Second Last");
        result.ChairOfTrustees!.Email.Should().BeNull();
    }

    [Fact]
    public async Task GetTrustContactsAsync_ShouldOnlyReturnCurrentGovernors()
    {
        _ = CreateGovernor("1234", "9999", _lastYear, _yesterday, "Chair of Trustees");
        var today = CreateGovernor("1234", "9998", _lastYear, _today, "Chief Financial Officer");
        var tomorrow = CreateGovernor("1234", "9997", _lastYear, _tomorrow, "Accounting Officer");

        var result = await _sut.GetTrustContactsAsync("1234");
        result.ChairOfTrustees.Should().BeNull();
        result.ChiefFinancialOfficer.Should().NotBeNull();
        result.ChiefFinancialOfficer!.FullName.Should().Be(today.FullName);
        result.ChiefFinancialOfficer!.Email.Should().Be(today.Email);
        result.AccountingOfficer.Should().NotBeNull();
        result.AccountingOfficer!.FullName.Should().Be(tomorrow.FullName);
        result.AccountingOfficer!.Email.Should().Be(tomorrow.Email);
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

        var cdmAccount = _mockAcademiesDbContext.AddCdmAccount(groupUid);
        var cdmSystemuser = _mockAcademiesDbContext.AddCdmSystemuser(fullName, email);

        accountSetup(cdmAccount, cdmSystemuser);

        return person;
    }

    private Governor CreateGovernor(string uid, string gid, DateTime? startDate,
        DateTime? endDate, string role = "Member", string forename1 = "First", string forename2 = "Second",
        string surname = "Last", string appointingBody = "Nick Warms", string? email = "test@email.com")
    {
        var fullName = forename1; //Forename1 is always populated

        if (!string.IsNullOrWhiteSpace(forename2))
            fullName += $" {forename2}";

        if (!string.IsNullOrWhiteSpace(surname))
            fullName += $" {surname}";

        var giasGovernance = new GiasGovernance
        {
            Gid = gid,
            Uid = uid,
            Role = role,
            Forename1 = forename1,
            Forename2 = forename2,
            Surname = surname,
            DateOfAppointment = startDate?.ToString("dd/MM/yyyy"),
            DateTermOfOfficeEndsEnded = endDate?.ToString("dd/MM/yyyy"),
            AppointingBody = appointingBody
        };

        var governor = new Governor(
            gid,
            uid,
            Role: role,
            FullName: fullName,
            DateOfAppointment: startDate,
            DateOfTermEnd: endDate,
            AppointingBody: "Nick Warms",
            Email: email
        );

        var mstrTrustGovernance = new MstrTrustGovernance
        {
            Gid = gid,
            Forename1 = forename1,
            Forename2 = forename2,
            Surname = surname,
            DateOfAppointment = startDate?.ToString("dd/MM/yyyy"),
            DateTermOfOfficeEndsEnded = endDate?.ToString("dd/MM/yyyy"),
            AppointingBody = appointingBody,
            Email = email
        };

        _mockAcademiesDbContext.AddGiasGovernance(giasGovernance);
        _mockAcademiesDbContext.AddMstrTrustGovernance(mstrTrustGovernance);

        return governor;
    }
}
