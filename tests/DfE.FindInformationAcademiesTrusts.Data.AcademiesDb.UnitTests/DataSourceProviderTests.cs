using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Ops;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests.Mocks;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests;

public class DataSourceProviderTests
{
    private readonly DataSourceProvider _sut;
    private readonly List<ApplicationEvent> _applicationEvents;
    private readonly MockAcademiesDbContext _mockAcademiesDbContext = new();
    private readonly DateTime _testStartTime = DateTime.Now;
    private readonly List<ApplicationSetting> _applicationSettings;

    public DataSourceProviderTests()
    {
        _applicationEvents = _mockAcademiesDbContext.SetupMockDbContextOpsApplicationEvents(_testStartTime);
        _applicationSettings = _mockAcademiesDbContext.SetupMockDbContextOpsApplicationSettings(_testStartTime);
        _sut = new DataSourceProvider(_mockAcademiesDbContext.Object);
    }

    [Fact]
    public async Task GetGiasUpdated_WhenNoEntryExists_ShouldReturnNull()
    {
        _mockAcademiesDbContext.SetupEmptyMockDbContextOpsApplicationEvents();
        var result = await _sut.GetGiasUpdated();

        result.Should().BeNull();
    }

    [Fact]
    public async Task GetGiasUpdated_WhenEntryExists_ShouldReturnDataSource()
    {
        var result = await _sut.GetGiasUpdated();

        result.Should().NotBeNull();
        result.Should()
            .BeEquivalentTo(new DataSource("Get information about schools", _testStartTime.AddDays(-1),
                "Daily"));
    }

    [Fact]
    public async Task GetGiasUpdated_WhenInvalidEntryExists_ShouldReturnNull()
    {
        _mockAcademiesDbContext.SetupInvalidMockDbContextOpsApplicationEvents(_testStartTime);
        var result = await _sut.GetGiasUpdated();

        result.Should().BeNull();
    }

    [Fact]
    public async Task GetMstrUpdated_WhenNoEntryExists_ShouldReturnNull()
    {
        _mockAcademiesDbContext.SetupEmptyMockDbContextOpsApplicationEvents();
        var result = await _sut.GetMstrUpdated();

        result.Should().BeNull();
    }

    [Fact]
    public async Task GetMstrUpdated_WhenEntryExists_ShouldReturnDataSource()
    {
        var result = await _sut.GetMstrUpdated();

        result.Should().NotBeNull();
        result.Should()
            .BeEquivalentTo(new DataSource("Get information about schools", _testStartTime.AddDays(-1),
                "Daily"));
    }

    [Fact]
    public async Task GetMstrUpdated_WhenInvalidEntryExists_ShouldReturnNull()
    {
        _mockAcademiesDbContext.SetupInvalidMockDbContextOpsApplicationEvents(_testStartTime);
        var result = await _sut.GetMstrUpdated();

        result.Should().BeNull();
    }

    [Fact]
    public async Task GetCdmUpdated_WhenNoEntryExists_ShouldReturnNull()
    {
        _mockAcademiesDbContext.SetupEmptyMockDbContextOpsApplicationEvents();
        var result = await _sut.GetCdmUpdated();

        result.Should().BeNull();
    }

    [Fact]
    public async Task GetCdmUpdated_WhenEntryExists_ShouldReturnDataSource()
    {
        var result = await _sut.GetCdmUpdated();

        result.Should().NotBeNull();
        result.Should()
            .BeEquivalentTo(new DataSource("RSD service support team", _testStartTime.AddDays(-1),
                "Daily"));
    }

    [Fact]
    public async Task GetCdmUpdated_WhenInvalidEntryExists_ShouldReturnNull()
    {
        _mockAcademiesDbContext.SetupInvalidMockDbContextOpsApplicationEvents(_testStartTime);
        var result = await _sut.GetCdmUpdated();

        result.Should().BeNull();
    }

    [Fact]
    public async Task GetMisEstablishmentsUpdated_WhenNoEntryExists_ShouldReturnNull()
    {
        _mockAcademiesDbContext.SetupEmptyMockDbContextOpsApplicationSettings();
        var result = await _sut.GetMisEstablishmentsUpdated();

        result.Should().BeNull();
    }

    [Fact]
    public async Task GetMisEstablishmentsUpdated_WhenEntryExists_ShouldReturnDataSource()
    {
        var result = await _sut.GetMisEstablishmentsUpdated();

        result.Should().NotBeNull();
        result.Should()
            .BeEquivalentTo(new DataSource("State-funded school inspections and outcomes: management information",
                _testStartTime.AddDays(-1), "Monthly"));
    }

    [Fact]
    public async Task GetMisEstablishmentsUpdated_WhenInvalidEntryExists_ShouldReturnNull()
    {
        _mockAcademiesDbContext.SetupInvalidMockDbContextOpsApplicationSettings(_testStartTime);
        var result = await _sut.GetMisEstablishmentsUpdated();

        result.Should().BeNull();
    }

    [Fact]
    public async Task GetMisEstablishmentsFurtherEducationUpdated_WhenNoEntryExists_ShouldReturnNull()
    {
        _mockAcademiesDbContext.SetupEmptyMockDbContextOpsApplicationSettings();
        var result = await _sut.GetMisFurtherEducationEstablishmentsUpdated();

        result.Should().BeNull();
    }

    [Fact]
    public async Task GetMisEstablishmentsFurtherEducationUpdated_WhenEntryExists_ShouldReturnDataSource()
    {
        var result = await _sut.GetMisFurtherEducationEstablishmentsUpdated();

        result.Should().NotBeNull();
        result.Should()
            .BeEquivalentTo(new DataSource("State-funded school inspections and outcomes: management information",
                _testStartTime.AddDays(-2), "Monthly"));
    }

    [Fact]
    public async Task GetMisEstablishmentsFurtherEducationUpdated_WhenInvalidEntryExists_ShouldReturnNull()
    {
        _mockAcademiesDbContext.SetupInvalidMockDbContextOpsApplicationSettings(_testStartTime);
        var result = await _sut.GetMisFurtherEducationEstablishmentsUpdated();

        result.Should().BeNull();
    }
}
