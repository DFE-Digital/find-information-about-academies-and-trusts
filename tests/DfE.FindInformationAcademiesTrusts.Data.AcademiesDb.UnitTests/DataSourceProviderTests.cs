using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Ops;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests.Mocks;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests;

public class DataSourceProviderTests
{
    private readonly DataSourceProvider _sut;
    private readonly List<ApplicationEvent> _applicationEvents;
    private readonly MockAcademiesDbContext _mockAcademiesDbContext = new();
    private static readonly DateTime TestStartTime = new(2023, 12, 12, 06, 54, 12);
    private readonly List<ApplicationSetting> _applicationSettings;

    public DataSourceProviderTests()
    {
        _applicationEvents = _mockAcademiesDbContext.SetupMockDbContextOpsApplicationEvents(TestStartTime);
        _applicationSettings = _mockAcademiesDbContext.SetupMockDbContextOpsApplicationSettings(TestStartTime);
        _sut = new DataSourceProvider(_mockAcademiesDbContext.Object);
    }

    [Fact]
    public async Task GetGiasUpdated_WhenNoEntryExists_ShouldReturnDataSource_WithNullDate()
    {
        _mockAcademiesDbContext.SetupEmptyMockDbContextOpsApplicationEvents();
        var result = await _sut.GetGiasUpdated();

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(new DataSource(Source.Gias,
            null, UpdateFrequency.Daily));
    }

    [Fact]
    public async Task GetGiasUpdated_WhenEntryExists_ShouldReturnDataSource()
    {
        var result = await _sut.GetGiasUpdated();

        result.Should().NotBeNull();
        result.Should()
            .BeEquivalentTo(new DataSource(Source.Gias, TestStartTime.AddDays(-1),
                UpdateFrequency.Daily));
    }

    [Fact]
    public async Task GetGiasUpdated_WhenInvalidEntryExists_ShouldReturnDataSource_WithNullDate()
    {
        _mockAcademiesDbContext.SetupInvalidMockDbContextOpsApplicationEvents(TestStartTime);
        var result = await _sut.GetGiasUpdated();

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(new DataSource(Source.Gias,
            null, UpdateFrequency.Daily));
    }

    [Fact]
    public async Task GetMstrUpdated_WhenNoEntryExists_ShouldReturnDataSource_WithNullDate()
    {
        _mockAcademiesDbContext.SetupEmptyMockDbContextOpsApplicationEvents();
        var result = await _sut.GetMstrUpdated();
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(new DataSource(Source.Mstr,
            null, UpdateFrequency.Daily));
    }

    [Fact]
    public async Task GetMstrUpdated_WhenEntryExists_ShouldReturnDataSource()
    {
        var result = await _sut.GetMstrUpdated();

        result.Should().NotBeNull();
        result.Should()
            .BeEquivalentTo(new DataSource(Source.Mstr, TestStartTime.AddDays(-1),
                UpdateFrequency.Daily));
    }

    [Fact]
    public async Task GetMstrUpdated_WhenInvalidEntryExists_ShouldReturnDataSource_WithNullDate()
    {
        _mockAcademiesDbContext.SetupInvalidMockDbContextOpsApplicationEvents(TestStartTime);
        var result = await _sut.GetMstrUpdated();

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(new DataSource(Source.Mstr,
                null, UpdateFrequency.Daily));
    }

    [Fact]
    public async Task GetCdmUpdated_WhenNoEntryExists_ShouldReturnDataSource_WithNullDate()
    {
        _mockAcademiesDbContext.SetupEmptyMockDbContextOpsApplicationEvents();
        var result = await _sut.GetCdmUpdated();

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(new DataSource(Source.Cdm,
            null, UpdateFrequency.Daily));
    }

    [Fact]
    public async Task GetCdmUpdated_WhenEntryExists_ShouldReturnDataSource()
    {
        var result = await _sut.GetCdmUpdated();

        result.Should().NotBeNull();
        result.Should()
            .BeEquivalentTo(new DataSource(Source.Cdm, TestStartTime.AddDays(-1),
                UpdateFrequency.Daily));
    }

    [Fact]
    public async Task GetCdmUpdated_WhenInvalidEntryExists_ShouldReturnDataSource_WithNullDate()
    {
        _mockAcademiesDbContext.SetupInvalidMockDbContextOpsApplicationEvents(TestStartTime);
        var result = await _sut.GetCdmUpdated();

        result.Should().NotBeNull();
        result.Should()
            .BeEquivalentTo(new DataSource(Source.Cdm,
                null, UpdateFrequency.Daily));
    }

    [Fact]
    public async Task GetMisEstablishmentsUpdated_WhenNoEntryExists_ShouldReturnDataSource_WithNullDate()
    {
        _mockAcademiesDbContext.SetupEmptyMockDbContextOpsApplicationSettings();
        var result = await _sut.GetMisEstablishmentsUpdated();

        result.Should().NotBeNull();
        result.Should()
            .BeEquivalentTo(new DataSource(Source.Mis, null, UpdateFrequency.Monthly));
    }

    [Fact]
    public async Task GetMisEstablishmentsUpdated_WhenEntryExists_ShouldReturnDataSource()
    {
        var result = await _sut.GetMisEstablishmentsUpdated();

        result.Should().NotBeNull();
        result.Should()
            .BeEquivalentTo(new DataSource(Source.Mis,
                TestStartTime.AddDays(-1), UpdateFrequency.Monthly));
    }

    [Fact]
    public async Task GetMisEstablishmentsUpdated_WhenInvalidEntryExists_ShouldReturnDataSource_WithNullDate()
    {
        _mockAcademiesDbContext.SetupInvalidMockDbContextOpsApplicationSettings(TestStartTime);
        var result = await _sut.GetMisEstablishmentsUpdated();

        result.Should().NotBeNull();
        result.Should()
            .BeEquivalentTo(new DataSource(Source.Mis, null, UpdateFrequency.Monthly));
    }

    [Fact]
    public async Task GetMisEstablishmentsFurtherEducationUpdated_WhenNoEntryExists_ShouldReturnDataSource_WithNullDate()
    {
        _mockAcademiesDbContext.SetupEmptyMockDbContextOpsApplicationSettings();
        var result = await _sut.GetMisFurtherEducationEstablishmentsUpdated();

        result.Should().NotBeNull();
        result.Should()
            .BeEquivalentTo(new DataSource(Source.Mis, null, UpdateFrequency.Monthly));
    }

    [Fact]
    public async Task GetMisEstablishmentsFurtherEducationUpdated_WhenEntryExists_ShouldReturnDataSource()
    {
        var result = await _sut.GetMisFurtherEducationEstablishmentsUpdated();

        result.Should().NotBeNull();
        result.Should()
            .BeEquivalentTo(new DataSource(Source.Mis,
                TestStartTime.AddDays(-2), UpdateFrequency.Monthly));
    }

    [Fact]
    public async Task
        GetMisEstablishmentsFurtherEducationUpdated_WhenInvalidEntryExists_ShouldReturnDataSource_WithNullDate()
    {
        _mockAcademiesDbContext.SetupInvalidMockDbContextOpsApplicationSettings(TestStartTime);
        var result = await _sut.GetMisFurtherEducationEstablishmentsUpdated();

        result.Should().NotBeNull();
        result.Should()
            .BeEquivalentTo(new DataSource(Source.Mis, null, UpdateFrequency.Monthly));
    }
}
