using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Repositories;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests.Mocks;
using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.Models;
using DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests.Repositories;

public class DataSourceRepositoryTests
{
    private readonly DataSourceRepository _sut;
    private readonly MockAcademiesDbContext _mockAcademiesDbContext = new();
    private static readonly DateTime TestStartTime = new(2023, 12, 12, 06, 54, 12);
    private readonly MockLogger<DataSourceRepository> _logger = new();

    public DataSourceRepositoryTests()
    {
        _mockAcademiesDbContext.SetupMockDbContextOpsApplicationEvents(TestStartTime);
        _mockAcademiesDbContext.SetupMockDbContextOpsApplicationSettings(TestStartTime);
        _sut = new DataSourceRepository(_mockAcademiesDbContext.Object, _logger.Object);
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
    public async Task GetAsync_WhenEntryExists_ShouldReturnDataSource(Source source, UpdateFrequency updateFrequency)
    {
        var result = await _sut.GetAsync(source);

        result.Should()
            .BeEquivalentTo(new DataSource(source, TestStartTime.AddDays(-1), updateFrequency));
    }

    [Theory]
    [InlineData(Source.Cdm, "Unable to find when CDM_Daily was last run", UpdateFrequency.Daily)]
    [InlineData(Source.Gias, "Unable to find when GIAS_Daily was last run", UpdateFrequency.Daily)]
    [InlineData(Source.Mis, "Unable to find when ManagementInformationSchoolTableData was last modified",
        UpdateFrequency.Monthly)]
    [InlineData(Source.Mstr, "Unable to find when MSTR_Daily was last run", UpdateFrequency.Daily)]
    public async Task GetAsync_WhenNoEntryExists_ShouldReturnDataSource_WithNullDate(Source source,
        string expectedErrorMessage, UpdateFrequency updateFrequency)
    {
        _mockAcademiesDbContext.SetupEmptyMockDbContextOpsApplicationEvents();
        _mockAcademiesDbContext.SetupEmptyMockDbContextOpsApplicationSettings();
        var result = await _sut.GetAsync(source);

        _logger.VerifyLogError(expectedErrorMessage);

        result.Should().BeEquivalentTo(new DataSource(source, null, updateFrequency));
    }

    [Theory]
    [InlineData(Source.Cdm, "Unable to find when CDM_Daily was last run", UpdateFrequency.Daily)]
    [InlineData(Source.Gias, "Unable to find when GIAS_Daily was last run", UpdateFrequency.Daily)]
    [InlineData(Source.Mis, "Unable to find when ManagementInformationSchoolTableData was last modified",
        UpdateFrequency.Monthly)]
    [InlineData(Source.Mstr, "Unable to find when MSTR_Daily was last run", UpdateFrequency.Daily)]
    public async Task GetAsync_WhenInvalidEntryExists_ShouldReturnDataSource_WithNullDate(Source source,
        string expectedErrorMessage, UpdateFrequency updateFrequency)
    {
        _mockAcademiesDbContext.SetupInvalidMockDbContextOpsApplicationEvents(TestStartTime);
        _mockAcademiesDbContext.SetupInvalidMockDbContextOpsApplicationSettings(TestStartTime);
        var result = await _sut.GetAsync(source);
        _logger.VerifyLogError(expectedErrorMessage);
        result.Should().BeEquivalentTo(new DataSource(source, null, updateFrequency));
    }
}
