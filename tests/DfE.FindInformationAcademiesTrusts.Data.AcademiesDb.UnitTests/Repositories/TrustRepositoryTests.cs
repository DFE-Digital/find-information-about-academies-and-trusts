using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Gias;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Repositories;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests.Mocks;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.Trust;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests.Repositories;

public class TrustRepositoryTests
{
    private readonly TrustRepository _sut;
    private readonly MockAcademiesDbContext _mockAcademiesDbContext = new();

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
    public async Task GetTrustGovernanceAsync_ShouldReturnEmpty_WithNoGovernenceSet()
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
        var startDate = DateTime.Today.AddYears(-1);
        var endDate = DateTime.Today.AddYears(1);
        var input = new GiasGovernance
        {
            Gid = "9999",
            Uid = "1234",
            Role = role,
            Forename1 = "First",
            Forename2 = "Second",
            Surname = "Last",
            DateOfAppointment = startDate.ToShortDateString(),
            DateTermOfOfficeEndsEnded = endDate.ToShortDateString(),
            AppointingBody = "Nick Warms"
        };
        var output = new Governor(
            "9999",
            "1234",
            Role: role,
            FullName: "First Second Last",
            DateOfAppointment: startDate,
            DateOfTermEnd: endDate,
            AppointingBody: "Nick Warms",
            Email: null
        );


        _mockAcademiesDbContext.AddGiasGovernance(input);
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
        var startDate = DateTime.Today.AddYears(-1);
        var endDate = DateTime.Today.AddYears(1);
        var input = new GiasGovernance
        {
            Gid = "9999",
            Uid = "1234",
            Role = role,
            Forename1 = "First",
            Forename2 = "Second",
            Surname = "Last",
            DateOfAppointment = startDate.ToShortDateString(),
            DateTermOfOfficeEndsEnded = endDate.ToShortDateString(),
            AppointingBody = "Nick Warms"
        };

        _mockAcademiesDbContext.AddGiasGovernance(input);
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
        var startDate = DateTime.Today.AddYears(-3);
        var endDate = DateTime.Today.AddYears(-1);
        var input = new GiasGovernance
        {
            Gid = "9999",
            Uid = "1234",
            Role = role,
            Forename1 = "First",
            Forename2 = "Second",
            Surname = "Last",
            DateOfAppointment = startDate.ToShortDateString(),
            DateTermOfOfficeEndsEnded = endDate.ToShortDateString(),
            AppointingBody = "Nick Warms"
        };
        var output = new Governor(
            "9999",
            "1234",
            Role: role,
            FullName: "First Second Last",
            DateOfAppointment: startDate,
            DateOfTermEnd: endDate,
            AppointingBody: "Nick Warms",
            Email: null
        );


        _mockAcademiesDbContext.AddGiasGovernance(input);
        var result = await _sut.GetTrustGovernanceAsync("1234");

        var historicMembers = new List<Governor> { output };

        result.Should().BeEquivalentTo(
            new TrustGovernance([], [], [], historicMembers.ToArray())
        );
    }

    [Fact]
    public async Task GetTrustGovernanceAsync_ShouldSortHistoricAndCurrentGoverors()
    {
        var startDate = DateTime.Today.AddYears(-3);
        var yesterday = DateTime.Today.AddDays(-1);
        var today = DateTime.Today;
        var tomorrow = DateTime.Today.AddDays(1);
        var currentMember = new GiasGovernance
        {
            Gid = "9999",
            Uid = "1234",
            Role = "Member",
            Forename1 = "First",
            Forename2 = "Second",
            Surname = "Last",
            DateOfAppointment = startDate.ToShortDateString(),
            DateTermOfOfficeEndsEnded = today.ToShortDateString(),
            AppointingBody = "Nick Warms"
        };
        var currentMemberOutput = new Governor(
            "9999",
            "1234",
            Role: "Member",
            FullName: "First Second Last",
            DateOfAppointment: startDate,
            DateOfTermEnd: today,
            AppointingBody: "Nick Warms",
            Email: null
        );
        var currentMember2 = new GiasGovernance
        {
            Gid = "9998",
            Uid = "1234",
            Role = "Member",
            Forename1 = "First",
            Forename2 = "Second",
            Surname = "Last",
            DateOfAppointment = startDate.ToShortDateString(),
            DateTermOfOfficeEndsEnded = tomorrow.ToShortDateString(),
            AppointingBody = "Nick Warms"
        };
        var currentMember2Output = new Governor(
            "9998",
            "1234",
            Role: "Member",
            FullName: "First Second Last",
            DateOfAppointment: startDate,
            DateOfTermEnd: tomorrow,
            AppointingBody: "Nick Warms",
            Email: null
        );
        var historicMember = new GiasGovernance
        {
            Gid = "9997",
            Uid = "1234",
            Role = "Member",
            Forename1 = "First",
            Forename2 = "Second",
            Surname = "Last",
            DateOfAppointment = startDate.ToShortDateString(),
            DateTermOfOfficeEndsEnded = yesterday.ToShortDateString(),
            AppointingBody = "Nick Warms"
        };
        var historicMemberOutput = new Governor(
            "9997",
            "1234",
            Role: "Member",
            FullName: "First Second Last",
            DateOfAppointment: startDate,
            DateOfTermEnd: yesterday,
            AppointingBody: "Nick Warms",
            Email: null
        );


        _mockAcademiesDbContext.AddGiasGovernance(currentMember);
        _mockAcademiesDbContext.AddGiasGovernance(currentMember2);
        _mockAcademiesDbContext.AddGiasGovernance(historicMember);
        var result = await _sut.GetTrustGovernanceAsync("1234");


        result.Should().BeEquivalentTo(
            new TrustGovernance([], [currentMemberOutput, currentMember2Output], [], [historicMemberOutput])
        );
    }
}
