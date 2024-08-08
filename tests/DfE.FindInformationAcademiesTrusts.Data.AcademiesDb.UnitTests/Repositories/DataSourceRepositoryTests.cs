﻿using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Repositories;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests.Mocks;
using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.Models;
using DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests.Repositories;

public class DataSourceRepositoryTests
{
    private readonly DataSourceRepository _sut;
    private readonly MockAcademiesDbContext _mockAcademiesDbContext = new();
    private readonly MockLogger<DataSourceRepository> _logger = new();

    public DataSourceRepositoryTests()
    {
        _sut = new DataSourceRepository(_mockAcademiesDbContext.Object, _logger.Object);

        var someDateTime = new DateTime(2020, 01, 01);
        _mockAcademiesDbContext.AddApplicationEvent("Unrelated event", someDateTime);
        _mockAcademiesDbContext.AddApplicationSetting("Unrelated setting", someDateTime);
    }

    [Fact]
    public async Task GetAsync_Throws_WhenUnsupportedDataSource()
    {
        var action = () => _sut.GetAsync(Source.ExploreEducationStatistics);

        await action.Should().ThrowAsync<ArgumentOutOfRangeException>();
    }

    [Theory]
    [InlineData(Source.Cdm, UpdateFrequency.Daily)]
    [InlineData(Source.Gias, UpdateFrequency.Daily)]
    [InlineData(Source.Mis, UpdateFrequency.Monthly)]
    [InlineData(Source.Mstr, UpdateFrequency.Daily)]
    public async Task GetAsync_WhenEntryExists_ShouldReturnLatestSuccessfullyFinishedDataSourceUpdate(Source source,
        UpdateFrequency updateFrequency)
    {
        var inProgressUpdateTime = new DateTime(2023, 12, 12, 06, 54, 12);
        var erroredUpdateTime = inProgressUpdateTime.AddHours(-1);
        var lastSuccessfulUpdateTime = inProgressUpdateTime.AddDays(-1);
        var oldSuccessfulUpdateTime = inProgressUpdateTime.AddDays(-2);

        AddSuccessfulDataSourceUpdates(lastSuccessfulUpdateTime);
        AddSuccessfulDataSourceUpdates(oldSuccessfulUpdateTime);
        AddInProgressDataSourceUpdates(inProgressUpdateTime);
        AddErroredDataSourceUpdates(erroredUpdateTime);

        var result = await _sut.GetAsync(source);

        result.Should().BeEquivalentTo(new DataSource(source, lastSuccessfulUpdateTime, updateFrequency));
    }

    [Theory]
    [InlineData(Source.Cdm, "Unable to find when CDM_Daily was last run", UpdateFrequency.Daily)]
    [InlineData(Source.Gias, "Unable to find when GIAS_Daily was last run", UpdateFrequency.Daily)]
    [InlineData(Source.Mis, "Unable to find when ManagementInformationSchoolTableData was last modified",
        UpdateFrequency.Monthly)]
    [InlineData(Source.Mstr, "Unable to find when MSTR_Daily was last run", UpdateFrequency.Daily)]
    public async Task GetAsync_WhenNoEntryForPipelineExists_ShouldReturnDataSource_WithNullDate_AndLogError(
        Source source, string expectedErrorMessage, UpdateFrequency updateFrequency)
    {
        AddSuccessfulDataSourceUpdatesExceptFor(source);

        var result = await _sut.GetAsync(source);

        _logger.VerifyLogError(expectedErrorMessage);
        result.Should().BeEquivalentTo(new DataSource(source, null, updateFrequency));
    }

    private void AddInProgressDataSourceUpdates(DateTime updateTime)
    {
        _mockAcademiesDbContext.AddApplicationEvent("GIAS_Daily", updateTime, "Started");
        _mockAcademiesDbContext.AddApplicationEvent("MSTR_Daily", updateTime, "Started");
        _mockAcademiesDbContext.AddApplicationEvent("CDM_Daily", updateTime, "Started");
        //MIS does not have an in progress update status
    }

    private void AddErroredDataSourceUpdates(DateTime updateTime)
    {
        _mockAcademiesDbContext.AddApplicationEvent("GIAS_Daily", updateTime, eventType: 'E');
        _mockAcademiesDbContext.AddApplicationEvent("MSTR_Daily", updateTime, eventType: 'E');
        _mockAcademiesDbContext.AddApplicationEvent("CDM_Daily", updateTime, eventType: 'E');
        //MIS does not have an errored update status
    }

    private void AddSuccessfulDataSourceUpdates(DateTime updateTime)
    {
        _mockAcademiesDbContext.AddApplicationEvent("GIAS_Daily", updateTime);
        _mockAcademiesDbContext.AddApplicationEvent("MSTR_Daily", updateTime);
        _mockAcademiesDbContext.AddApplicationEvent("CDM_Daily", updateTime);
        _mockAcademiesDbContext.AddApplicationSetting("ManagementInformationSchoolTableData CSV Filename", updateTime);
    }

    private void AddSuccessfulDataSourceUpdatesExceptFor(Source source)
    {
        var lastUpdateTime = new DateTime(2023, 12, 12, 06, 54, 12);

        if (source is not Source.Gias) _mockAcademiesDbContext.AddApplicationEvent("GIAS_Daily", lastUpdateTime);
        if (source is not Source.Mstr) _mockAcademiesDbContext.AddApplicationEvent("MSTR_Daily", lastUpdateTime);
        if (source is not Source.Cdm) _mockAcademiesDbContext.AddApplicationEvent("CDM_Daily", lastUpdateTime);
        if (source is not Source.Mis)
            _mockAcademiesDbContext.AddApplicationSetting("ManagementInformationSchoolTableData CSV Filename",
                lastUpdateTime);
    }
}
