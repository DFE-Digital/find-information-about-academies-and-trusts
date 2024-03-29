﻿using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests.Mocks;
using DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests;

public class DataSourceProviderTests
{
    private readonly DataSourceProvider _sut;
    private readonly MockAcademiesDbContext _mockAcademiesDbContext = new();
    private static readonly DateTime TestStartTime = new(2023, 12, 12, 06, 54, 12);
    private readonly MockLogger<DataSourceProvider> _logger = new();

    public DataSourceProviderTests()
    {
        _mockAcademiesDbContext.SetupMockDbContextOpsApplicationEvents(TestStartTime);
        _mockAcademiesDbContext.SetupMockDbContextOpsApplicationSettings(TestStartTime);
        _sut = new DataSourceProvider(_mockAcademiesDbContext.Object, _logger.Object);
    }

    [Fact]
    public async Task GetGiasUpdated_WhenNoEntryExists_ShouldReturnDataSource_WithNullDate()
    {
        _mockAcademiesDbContext.SetupEmptyMockDbContextOpsApplicationEvents();
        var result = await _sut.GetGiasUpdated();

        _logger.VerifyLogError("Unable to find when GIAS_Daily was last run");

        result.Should().BeEquivalentTo(new DataSource(Source.Gias,
            null, UpdateFrequency.Daily));
    }

    [Fact]
    public async Task GetGiasUpdated_WhenEntryExists_ShouldReturnDataSource()
    {
        var result = await _sut.GetGiasUpdated();

        result.Should()
            .BeEquivalentTo(new DataSource(Source.Gias, TestStartTime.AddDays(-1),
                UpdateFrequency.Daily));
    }

    [Fact]
    public async Task GetGiasUpdated_WhenInvalidEntryExists_ShouldReturnDataSource_WithNullDate()
    {
        _mockAcademiesDbContext.SetupInvalidMockDbContextOpsApplicationEvents(TestStartTime);
        var result = await _sut.GetGiasUpdated();
        _logger.VerifyLogError("Unable to find when GIAS_Daily was last run");
        result.Should().BeEquivalentTo(new DataSource(Source.Gias,
            null, UpdateFrequency.Daily));
    }

    [Fact]
    public async Task GetMstrUpdated_WhenNoEntryExists_ShouldReturnDataSource_WithNullDate()
    {
        _mockAcademiesDbContext.SetupEmptyMockDbContextOpsApplicationEvents();
        var result = await _sut.GetMstrUpdated();
        _logger.VerifyLogError("Unable to find when MSTR_Daily was last run");
        result.Should().BeEquivalentTo(new DataSource(Source.Mstr,
            null, UpdateFrequency.Daily));
    }

    [Fact]
    public async Task GetMstrUpdated_WhenEntryExists_ShouldReturnDataSource()
    {
        var result = await _sut.GetMstrUpdated();

        result.Should()
            .BeEquivalentTo(new DataSource(Source.Mstr, TestStartTime.AddDays(-1),
                UpdateFrequency.Daily));
    }

    [Fact]
    public async Task GetMstrUpdated_WhenInvalidEntryExists_ShouldReturnDataSource_WithNullDate()
    {
        _mockAcademiesDbContext.SetupInvalidMockDbContextOpsApplicationEvents(TestStartTime);
        var result = await _sut.GetMstrUpdated();
        _logger.VerifyLogError("Unable to find when MSTR_Daily was last run");
        result.Should().BeEquivalentTo(new DataSource(Source.Mstr,
            null, UpdateFrequency.Daily));
    }

    [Fact]
    public async Task GetCdmUpdated_WhenNoEntryExists_ShouldReturnDataSource_WithNullDate()
    {
        _mockAcademiesDbContext.SetupEmptyMockDbContextOpsApplicationEvents();
        var result = await _sut.GetCdmUpdated();
        _logger.VerifyLogError("Unable to find when CDM_Daily was last run");

        result.Should().BeEquivalentTo(new DataSource(Source.Cdm,
            null, UpdateFrequency.Daily));
    }

    [Fact]
    public async Task GetCdmUpdated_WhenEntryExists_ShouldReturnDataSource()
    {
        var result = await _sut.GetCdmUpdated();

        result.Should()
            .BeEquivalentTo(new DataSource(Source.Cdm, TestStartTime.AddDays(-1),
                UpdateFrequency.Daily));
    }

    [Fact]
    public async Task GetCdmUpdated_WhenInvalidEntryExists_ShouldReturnDataSource_WithNullDate()
    {
        _mockAcademiesDbContext.SetupInvalidMockDbContextOpsApplicationEvents(TestStartTime);
        var result = await _sut.GetCdmUpdated();
        _logger.VerifyLogError("Unable to find when CDM_Daily was last run");

        result.Should()
            .BeEquivalentTo(new DataSource(Source.Cdm,
                null, UpdateFrequency.Daily));
    }

    [Fact]
    public async Task GetMisEstablishmentsUpdated_WhenNoEntryExists_ShouldReturnDataSource_WithNullDate()
    {
        _mockAcademiesDbContext.SetupEmptyMockDbContextOpsApplicationSettings();
        var result = await _sut.GetMisEstablishmentsUpdated();
        _logger.VerifyLogError("Unable to find when ManagementInformationSchoolTableData was last modified");

        result.Should()
            .BeEquivalentTo(new DataSource(Source.Mis, null, UpdateFrequency.Monthly));
    }

    [Fact]
    public async Task GetMisEstablishmentsUpdated_WhenEntryExists_ShouldReturnDataSource()
    {
        var result = await _sut.GetMisEstablishmentsUpdated();

        result.Should()
            .BeEquivalentTo(new DataSource(Source.Mis,
                TestStartTime.AddDays(-1), UpdateFrequency.Monthly));
    }

    [Fact]
    public async Task GetMisEstablishmentsUpdated_WhenInvalidEntryExists_ShouldReturnDataSource_WithNullDate()
    {
        _mockAcademiesDbContext.SetupInvalidMockDbContextOpsApplicationSettings(TestStartTime);
        var result = await _sut.GetMisEstablishmentsUpdated();

        _logger.VerifyLogError("Unable to find when ManagementInformationSchoolTableData was last modified");
        result.Should()
            .BeEquivalentTo(new DataSource(Source.Mis, null, UpdateFrequency.Monthly));
    }
}
