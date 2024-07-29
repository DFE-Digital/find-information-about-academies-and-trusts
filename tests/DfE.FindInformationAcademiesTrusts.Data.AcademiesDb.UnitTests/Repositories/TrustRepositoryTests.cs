using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Gias;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Repositories;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests.Mocks;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.Models;

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
        _ = _mockAcademiesDbContext.CreateGiasGroup(uid, name, type);

        var result = await _sut.GetTrustSummaryAsync(uid);
        result.Should().BeEquivalentTo(new TrustSummary(name, type));
    }

    [Fact]
    public async Task GetTrustSummaryAsync_should_return_empty_values_on_null_group_fields()
    {
        _ = _mockAcademiesDbContext.CreateGiasGroup("2806", null, null);

        var result = await _sut.GetTrustSummaryAsync("2806");
        result.Should().BeEquivalentTo(new TrustSummary(string.Empty, string.Empty));
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
}
