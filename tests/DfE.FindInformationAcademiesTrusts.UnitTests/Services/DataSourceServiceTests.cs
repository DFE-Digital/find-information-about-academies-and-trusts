using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Data.Repositories;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.Models;
using DfE.FindInformationAcademiesTrusts.Services;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Services;

public class DataSourceServiceTests
{
    private readonly DataSourceService _sut;
    private readonly Mock<IAcademiesDbDataSourceRepository> _mockAcademiesDbDataSourceRepository = new();
    private readonly Mock<IFreeSchoolMealsAverageProvider> _mockFreeSchoolMealsAverageProvider = new();

    public DataSourceServiceTests()
    {
        _sut = new DataSourceService(_mockAcademiesDbDataSourceRepository.Object,
            _mockFreeSchoolMealsAverageProvider.Object);
    }

    [Fact]
    public async Task GetAsync_Gias_should_call_academiesDbDataSourceRepository()
    {
        var dataSource = new DataSource(Source.Gias, new DateTime(2024, 01, 01), UpdateFrequency.Daily);
        _mockAcademiesDbDataSourceRepository.Setup(d => d.GetGiasUpdatedAsync()).ReturnsAsync(dataSource);

        var result = await _sut.GetAsync(Source.Gias);

        result.Should().BeEquivalentTo(dataSource);
    }

    [Fact]
    public async Task GetAsync_Mstr_should_call_academiesDbDataSourceRepository()
    {
        var dataSource = new DataSource(Source.Mstr, new DateTime(2024, 01, 01), UpdateFrequency.Daily);
        _mockAcademiesDbDataSourceRepository.Setup(d => d.GetMstrUpdatedAsync()).ReturnsAsync(dataSource);

        var result = await _sut.GetAsync(Source.Mstr);

        result.Should().BeEquivalentTo(dataSource);
    }

    [Fact]
    public async Task GetAsync_Cdm_should_call_academiesDbDataSourceRepository()
    {
        var dataSource = new DataSource(Source.Cdm, new DateTime(2024, 01, 01), UpdateFrequency.Daily);
        _mockAcademiesDbDataSourceRepository.Setup(d => d.GetCdmUpdatedAsync()).ReturnsAsync(dataSource);

        var result = await _sut.GetAsync(Source.Cdm);

        result.Should().BeEquivalentTo(dataSource);
    }

    [Fact]
    public async Task GetAsync_Mis_should_call_academiesDbDataSourceRepository()
    {
        var dataSource = new DataSource(Source.Mis, new DateTime(2024, 01, 01), UpdateFrequency.Monthly);
        _mockAcademiesDbDataSourceRepository.Setup(d => d.GetMisEstablishmentsUpdatedAsync()).ReturnsAsync(dataSource);

        var result = await _sut.GetAsync(Source.Mis);

        result.Should().BeEquivalentTo(dataSource);
    }

    [Fact]
    public async Task GetAsync_ExploreEducationStatistics_should_call_freeSchoolMealsAverageProvider()
    {
        var dataSource = new DataSource(Source.ExploreEducationStatistics, new DateTime(2024, 01, 01),
            UpdateFrequency.Annually);
        _mockFreeSchoolMealsAverageProvider.Setup(d => d.GetFreeSchoolMealsUpdated()).Returns(dataSource);

        var result = await _sut.GetAsync(Source.ExploreEducationStatistics);

        result.Should().BeEquivalentTo(dataSource);
    }
}
