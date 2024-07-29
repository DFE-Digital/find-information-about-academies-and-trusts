using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Gias;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Repositories;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests.Mocks;
using DfE.FindInformationAcademiesTrusts.Data.Dto;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests;

public class TrustServiceTests
{
    private readonly TrustService _sut;
    private readonly MockAcademiesDbContext _mockAcademiesDbContext = new();
    private readonly Mock<IAcademyRepository> _mockAcademyRepository = new();

    public TrustServiceTests()
    {
        _mockAcademiesDbContext.SetupMockDbContextGiasGroups(5);
        _mockAcademiesDbContext.SetupMockDbContextMstrTrust(5);

        _sut = new TrustService(_mockAcademiesDbContext.Object, _mockAcademyRepository.Object);
    }

    [Fact]
    public async Task GetTrustSummaryAsync_should_return_null_if_no_giasGroup_found()
    {
        var result = await _sut.GetTrustSummaryAsync("this uid doesn't exist");
        result.Should().BeNull();
    }

    [Theory]
    [InlineData("2806", "My Trust", "Multi-academy trust", 3)]
    [InlineData("9008", "Another Trust", "Single-academy trust", 1)]
    [InlineData("9008", "Trust with no academies", "Multi-academy trust", 0)]
    public async Task GetTrustSummaryAsync_should_return_trustSummary_if_found(string uid, string name, string type,
        int numAcademies)
    {
        _mockAcademiesDbContext.CreateGiasGroup(uid, name, type);
        _mockAcademyRepository.Setup(a => a.GetNumberOfAcademiesInTrustAsync(uid))
            .ReturnsAsync(numAcademies);

        var result = await _sut.GetTrustSummaryAsync(uid);
        result.Should().BeEquivalentTo(new TrustSummaryDto(uid, name, type, numAcademies));
    }

    [Fact]
    public async Task GetTrustSummaryAsync_should_return_empty_values_on_null_group_fields()
    {
        _ = _mockAcademiesDbContext.CreateGiasGroup("2806", null, null);

        var result = await _sut.GetTrustSummaryAsync("2806");
        result.Should().BeEquivalentTo(new TrustSummaryDto("2806", string.Empty, string.Empty, 0));
    }

    [Fact]
    public async Task GetTrustDetailsAsync_should_get_regionAndTerritory_from_mstrTrusts()
    {
        _ = _mockAcademiesDbContext.CreateGiasGroup("2806");
        _ = _mockAcademiesDbContext.CreateMstrTrust("2806", "My Region");

        var result = await _sut.GetTrustDetailsAsync("2806");

        result.RegionAndTerritory.Should().Be("My Region");
    }

    [Fact]
    public async Task GetTrustDetailsAsync_should_set_regionAndTerritory_to_empty_string_when_mstrTrust_not_available()
    {
        _ = _mockAcademiesDbContext.CreateGiasGroup("2806");

        var result = await _sut.GetTrustDetailsAsync("2806");

        result.RegionAndTerritory.Should().BeEmpty();
    }

    [Fact]
    public async Task
        GetTrustDetailsAsync_should_set_regionAndTerritory_to_empty_string_when_GORregion_in_mstrTrust_null()
    {
        _ = _mockAcademiesDbContext.CreateGiasGroup("2806");
        _ = _mockAcademiesDbContext.CreateMstrTrust("2806", null);

        var result = await _sut.GetTrustDetailsAsync("2806");

        result.RegionAndTerritory.Should().BeEmpty();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("123456")]
    [InlineData("567890")]
    public async Task GetTrustDetailsAsync_should_get_singleAcademyUrn_from_academy_repository(string? singleAcademyUrn)
    {
        _ = _mockAcademiesDbContext.CreateGiasGroup("2806");
        _mockAcademyRepository.Setup(a => a.GetUrnForSingleAcademyTrustAsync("2806"))
            .ReturnsAsync(singleAcademyUrn);

        var result = await _sut.GetTrustDetailsAsync("2806");

        result.SingleAcademyUrn.Should().Be(singleAcademyUrn);
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
        var giasGroup = new GiasGroup
        {
            GroupUid = "2806",
            GroupType = "Multi-academy trust",
            GroupContactStreet = groupContactStreet,
            GroupContactLocality = groupContactLocality,
            GroupContactTown = groupContactTown,
            GroupContactPostcode = groupContactPostcode
        };
        _mockAcademiesDbContext.AddGiasGroup(giasGroup);

        var result = await _sut.GetTrustDetailsAsync("2806");

        result.Address.Should().Be(expectedAddress);
    }

    [Fact]
    public async Task GetTrustDetailsAsync_should_set_properties_from_giasGroup()
    {
        var giasGroup = new GiasGroup
        {
            GroupUid = "2806",
            GroupId = "TR0012",
            Ukprn = "10012345",
            GroupType = "Multi-academy trust",
            CompaniesHouseNumber = "123456",
            IncorporatedOnOpenDate = "28/06/2007"
        };
        _mockAcademiesDbContext.AddGiasGroup(giasGroup);

        var result = await _sut.GetTrustDetailsAsync("2806");

        result.Should().BeEquivalentTo(new TrustDetailsDto("2806",
            "TR0012",
            "10012345",
            "123456",
            "Multi-academy trust",
            "",
            "",
            null,
            new DateTime(2007, 6, 28)
        ));
    }
}
