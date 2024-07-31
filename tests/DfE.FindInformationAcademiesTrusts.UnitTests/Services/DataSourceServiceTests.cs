using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Data.Repositories;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.Models;
using DfE.FindInformationAcademiesTrusts.ServiceModels;
using DfE.FindInformationAcademiesTrusts.Services;
using DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Services;

public class DataSourceServiceTests
{
    private readonly DataSourceService _sut;
    private readonly Mock<IAcademiesDbDataSourceRepository> _mockAcademiesDbDataSourceRepository = new();
    private readonly Mock<IFreeSchoolMealsAverageProvider> _mockFreeSchoolMealsAverageProvider = new();
    private readonly MockMemoryCache _mockMemoryCache = new();

    private readonly Dictionary<Source, DataSource> _dummyDataSources = new()
    {
        { Source.Cdm, GetDummyDataSource(Source.Cdm, UpdateFrequency.Daily) },
        { Source.ExploreEducationStatistics, GetDummyDataSource(Source.Cdm, UpdateFrequency.Annually) },
        { Source.Gias, GetDummyDataSource(Source.Gias, UpdateFrequency.Daily) },
        { Source.Mis, GetDummyDataSource(Source.Mis, UpdateFrequency.Monthly) },
        { Source.Mstr, GetDummyDataSource(Source.Mstr, UpdateFrequency.Daily) }
    };

    public DataSourceServiceTests()
    {
        _sut = new DataSourceService(_mockAcademiesDbDataSourceRepository.Object,
            _mockFreeSchoolMealsAverageProvider.Object, _mockMemoryCache.Object);

        _mockAcademiesDbDataSourceRepository.Setup(d => d.GetGiasUpdatedAsync())
            .ReturnsAsync(_dummyDataSources[Source.Gias]);
        _mockAcademiesDbDataSourceRepository.Setup(d => d.GetMstrUpdatedAsync())
            .ReturnsAsync(_dummyDataSources[Source.Mstr]);
        _mockAcademiesDbDataSourceRepository.Setup(d => d.GetCdmUpdatedAsync())
            .ReturnsAsync(_dummyDataSources[Source.Cdm]);
        _mockAcademiesDbDataSourceRepository.Setup(d => d.GetMisEstablishmentsUpdatedAsync())
            .ReturnsAsync(_dummyDataSources[Source.Mis]);
        _mockFreeSchoolMealsAverageProvider.Setup(d => d.GetFreeSchoolMealsUpdated())
            .Returns(_dummyDataSources[Source.ExploreEducationStatistics]);
    }

    private static DataSource GetDummyDataSource(Source source, UpdateFrequency updateFrequency)
    {
        return new DataSource(source, new DateTime(2024, 01, 01), updateFrequency);
    }

    [Fact]
    public async Task GetAsync_uncached_Gias_should_call_academiesDbDataSourceRepository()
    {
        var result = await _sut.GetAsync(Source.Gias);

        result.Should().BeEquivalentTo(_dummyDataSources[Source.Gias]);
        _mockAcademiesDbDataSourceRepository.Verify(d => d.GetGiasUpdatedAsync(), Times.Once);
    }

    [Fact]
    public async Task GetAsync_uncached_Mstr_should_call_academiesDbDataSourceRepository()
    {
        var result = await _sut.GetAsync(Source.Mstr);

        result.Should().BeEquivalentTo(_dummyDataSources[Source.Mstr]);
        _mockAcademiesDbDataSourceRepository.Verify(d => d.GetMstrUpdatedAsync(), Times.Once);
    }

    [Fact]
    public async Task GetAsync_uncachedCdm_should_call_academiesDbDataSourceRepository()
    {
        var result = await _sut.GetAsync(Source.Cdm);

        result.Should().BeEquivalentTo(_dummyDataSources[Source.Cdm]);
        _mockAcademiesDbDataSourceRepository.Verify(d => d.GetCdmUpdatedAsync(), Times.Once);
    }

    [Fact]
    public async Task GetAsync_uncached_Mis_should_call_academiesDbDataSourceRepository()
    {
        var result = await _sut.GetAsync(Source.Mis);

        result.Should().BeEquivalentTo(_dummyDataSources[Source.Mis]);
        _mockAcademiesDbDataSourceRepository.Verify(d => d.GetMisEstablishmentsUpdatedAsync(), Times.Once);
    }

    [Fact]
    public async Task GetAsync_uncached_ExploreEducationStatistics_should_call_freeSchoolMealsAverageProvider()
    {
        var result = await _sut.GetAsync(Source.ExploreEducationStatistics);

        result.Should().BeEquivalentTo(_dummyDataSources[Source.ExploreEducationStatistics]);
        _mockFreeSchoolMealsAverageProvider.Verify(f => f.GetFreeSchoolMealsUpdated(), Times.Once);
    }

    [Theory]
    [InlineData(Source.Cdm, UpdateFrequency.Daily)]
    [InlineData(Source.ExploreEducationStatistics, UpdateFrequency.Annually)]
    [InlineData(Source.Gias, UpdateFrequency.Daily)]
    [InlineData(Source.Mis, UpdateFrequency.Monthly)]
    [InlineData(Source.Mstr, UpdateFrequency.Daily)]
    public async Task GetAsync_cached_should_return_cached_result(Source source, UpdateFrequency updateFrequency)
    {
        var dataSource = new DataSourceServiceModel(source, new DateTime(2024, 01, 01), updateFrequency);
        _mockMemoryCache.AddMockCacheEntry(source, dataSource);

        var result = await _sut.GetAsync(source);

        result.Should().BeEquivalentTo(dataSource);
    }

    [Theory]
    [InlineData(Source.Cdm, UpdateFrequency.Daily)]
    [InlineData(Source.ExploreEducationStatistics, UpdateFrequency.Annually)]
    [InlineData(Source.Gias, UpdateFrequency.Daily)]
    [InlineData(Source.Mis, UpdateFrequency.Monthly)]
    [InlineData(Source.Mstr, UpdateFrequency.Daily)]
    public async Task GetAsync_uncached_should_cache_result(Source source, UpdateFrequency updateFrequency)
    {
        var expectedCacheTimeSpan =
            updateFrequency is UpdateFrequency.Daily ? TimeSpan.FromHours(1) : TimeSpan.FromDays(1);

        await _sut.GetAsync(source);

        _mockMemoryCache.Verify(m => m.CreateEntry(source), Times.Once);

        var cachedEntry = _mockMemoryCache.MockCacheEntries[source];

        cachedEntry.Value.Should().BeEquivalentTo(_dummyDataSources[source]);
        cachedEntry.AbsoluteExpirationRelativeToNow.Should().Be(expectedCacheTimeSpan);
    }

    [Fact]
    public async Task GetAsync_uncached_should_not_cache_source_with_null_lastUpdated()
    {
        _mockAcademiesDbDataSourceRepository.Setup(d => d.GetGiasUpdatedAsync())
            .ReturnsAsync(new DataSource(Source.Gias, null, UpdateFrequency.Daily));

        await _sut.GetAsync(Source.Gias);

        _mockMemoryCache.Verify(m => m.CreateEntry(It.IsAny<Source>()), Times.Never);
    }
}
