using DfE.FIAT.Data;
using DfE.FIAT.Data.Enums;
using DfE.FIAT.Data.Repositories.DataSource;
using DfE.FIAT.Web.Services.DataSource;
using DfE.FIAT.Web.UnitTests.Mocks;

namespace DfE.FIAT.Web.UnitTests.Services;

public class DataSourceServiceTests
{
    private readonly DataSourceService _sut;
    private readonly Mock<IDataSourceRepository> _mockDataSourceRepository = new();
    private readonly Mock<IFreeSchoolMealsAverageProvider> _mockFreeSchoolMealsAverageProvider = new();
    private readonly MockMemoryCache _mockMemoryCache = new();

    private readonly Dictionary<Source, Data.Repositories.DataSource.DataSource> _dummyDataSources = new()
    {
        { Source.Cdm, GetDummyDataSource(Source.Cdm, UpdateFrequency.Daily) },
        { Source.ExploreEducationStatistics, GetDummyDataSource(Source.Cdm, UpdateFrequency.Annually) },
        { Source.Gias, GetDummyDataSource(Source.Gias, UpdateFrequency.Daily) },
        { Source.Mis, GetDummyDataSource(Source.Mis, UpdateFrequency.Monthly) },
        { Source.Mstr, GetDummyDataSource(Source.Mstr, UpdateFrequency.Daily) }
    };

    public DataSourceServiceTests()
    {
        _sut = new DataSourceService(_mockDataSourceRepository.Object,
            _mockFreeSchoolMealsAverageProvider.Object, _mockMemoryCache.Object);

        _mockDataSourceRepository.Setup(d => d.GetAsync(It.IsIn(Source.Cdm, Source.Gias, Source.Mis, Source.Mstr)))
            .ReturnsAsync((Source source) => _dummyDataSources[source]);
        _mockDataSourceRepository.Setup(d => d.GetAsync(It.IsNotIn(Source.Cdm, Source.Gias, Source.Mis, Source.Mstr)))
            .ThrowsAsync(new ArgumentOutOfRangeException());

        _mockFreeSchoolMealsAverageProvider.Setup(d => d.GetFreeSchoolMealsUpdated())
            .Returns(_dummyDataSources[Source.ExploreEducationStatistics]);
    }

    private static Data.Repositories.DataSource.DataSource GetDummyDataSource(Source source,
        UpdateFrequency updateFrequency)
    {
        return new Data.Repositories.DataSource.DataSource(source, new DateTime(2024, 01, 01), updateFrequency);
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
    public async Task GetAsync_uncached_should_call_dataSourceRepository(Source source, UpdateFrequency updateFrequency)
    {
        var expectedCacheTimeSpan =
            updateFrequency is UpdateFrequency.Daily ? TimeSpan.FromHours(1) : TimeSpan.FromDays(1);

        await _sut.GetAsync(source);

        _mockMemoryCache.Verify(m => m.CreateEntry(source), Times.Once);

        var cachedEntry = _mockMemoryCache.MockCacheEntries[source];

        cachedEntry.Value.Should().BeEquivalentTo(_dummyDataSources[source]);
        cachedEntry.AbsoluteExpirationRelativeToNow.Should().Be(expectedCacheTimeSpan);
    }

    [Theory]
    [InlineData(Source.Cdm)]
    [InlineData(Source.Gias)]
    [InlineData(Source.Mis)]
    [InlineData(Source.Mstr)]
    public async Task GetAsync_uncached_should_call_academiesDbDataSourceRepository(Source source)
    {
        var result = await _sut.GetAsync(source);

        result.Should().BeEquivalentTo(_dummyDataSources[source]);
        _mockDataSourceRepository.Verify(d => d.GetAsync(source), Times.Once);
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
        _mockDataSourceRepository.Setup(d => d.GetAsync(Source.Gias))
            .ReturnsAsync(new Data.Repositories.DataSource.DataSource(Source.Gias, null, UpdateFrequency.Daily));

        await _sut.GetAsync(Source.Gias);

        _mockMemoryCache.Verify(m => m.CreateEntry(It.IsAny<Source>()), Times.Never);
    }
}
