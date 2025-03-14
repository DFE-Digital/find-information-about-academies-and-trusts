using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;
using Moq;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReceivedExtensions;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Services;

public class DataSourceServiceTests
{
    private readonly DataSourceService _sut;
    private readonly IDataSourceRepository _mockDataSourceRepository = Substitute.For<IDataSourceRepository>();
    private readonly IFreeSchoolMealsAverageProvider _mockFreeSchoolMealsAverageProvider = Substitute.For<IFreeSchoolMealsAverageProvider>();
    private readonly MockMemoryCache _mockMemoryCache = new();

    private readonly Dictionary<Source, Data.Repositories.DataSource.DataSource> _dummyDataSources = new()
    {
        { Source.Cdm, GetDummyDataSource(Source.Cdm, UpdateFrequency.Daily) },
        { Source.Complete, GetDummyDataSource(Source.Complete, UpdateFrequency.Daily) },
        { Source.ExploreEducationStatistics, GetDummyDataSource(Source.Cdm, UpdateFrequency.Annually) },
        { Source.Gias, GetDummyDataSource(Source.Gias, UpdateFrequency.Daily) },
        { Source.ManageFreeSchoolProjects, GetDummyDataSource(Source.ManageFreeSchoolProjects, UpdateFrequency.Daily) },
        { Source.Mis, GetDummyDataSource(Source.Mis, UpdateFrequency.Monthly) },
        { Source.Mstr, GetDummyDataSource(Source.Mstr, UpdateFrequency.Daily) },
        { Source.Prepare, GetDummyDataSource(Source.Prepare, UpdateFrequency.Daily) }
    };

    public DataSourceServiceTests()
    {
        _sut = new DataSourceService(_mockDataSourceRepository,
            _mockFreeSchoolMealsAverageProvider, _mockMemoryCache.Object);

        Source[] supportedAcademiesDbSources =
        [
            Source.Cdm, Source.Complete, Source.Gias, Source.ManageFreeSchoolProjects, Source.Mis, Source.Mstr,
            Source.Prepare
        ];
        
        _mockDataSourceRepository.GetAsync(Arg.Is<Source>(source => supportedAcademiesDbSources.Contains(source)))
            .Returns(callInfo => _dummyDataSources[callInfo.Arg<Source>()]);
        
        _mockDataSourceRepository.GetAsync(Arg.Is<Source>(source => !supportedAcademiesDbSources.Contains(source)))
            .ThrowsAsync(new ArgumentOutOfRangeException());
        
        _mockFreeSchoolMealsAverageProvider.GetFreeSchoolMealsUpdated().Returns(_dummyDataSources[Source.ExploreEducationStatistics]);
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
        _mockFreeSchoolMealsAverageProvider.Received(1).GetFreeSchoolMealsUpdated();
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
    [InlineData(Source.Complete, UpdateFrequency.Daily)]
    [InlineData(Source.ExploreEducationStatistics, UpdateFrequency.Annually)]
    [InlineData(Source.Gias, UpdateFrequency.Daily)]
    [InlineData(Source.ManageFreeSchoolProjects, UpdateFrequency.Daily)]
    [InlineData(Source.Mis, UpdateFrequency.Monthly)]
    [InlineData(Source.Mstr, UpdateFrequency.Daily)]
    [InlineData(Source.Prepare, UpdateFrequency.Daily)]
    public async Task GetAsync_uncached_should_call_dataSourceRepository(Source source, UpdateFrequency updateFrequency)
    {
        var expectedCacheTimeSpan =
            updateFrequency is UpdateFrequency.Daily ? TimeSpan.FromHours(1) : TimeSpan.FromDays(1);

        await _sut.GetAsync(source);

        _mockMemoryCache.Object.Received(1).CreateEntry(source);
        
        var cachedEntry = _mockMemoryCache.MockCacheEntries[source];

        cachedEntry.Value.Should().BeEquivalentTo(_dummyDataSources[source]);
        cachedEntry.AbsoluteExpirationRelativeToNow.Should().Be(expectedCacheTimeSpan);
    }

    [Theory]
    [InlineData(Source.Cdm)]
    [InlineData(Source.Complete)]
    [InlineData(Source.Gias)]
    [InlineData(Source.ManageFreeSchoolProjects)]
    [InlineData(Source.Mis)]
    [InlineData(Source.Mstr)]
    [InlineData(Source.Prepare)]
    public async Task GetAsync_uncached_should_call_academiesDbDataSourceRepository(Source source)
    {
        var result = await _sut.GetAsync(source);

        result.Should().BeEquivalentTo(_dummyDataSources[source]);
        await _mockDataSourceRepository.Received(1).GetAsync(source);
    }

    [Theory]
    [InlineData(Source.Cdm, UpdateFrequency.Daily)]
    [InlineData(Source.Complete, UpdateFrequency.Daily)]
    [InlineData(Source.ExploreEducationStatistics, UpdateFrequency.Annually)]
    [InlineData(Source.Gias, UpdateFrequency.Daily)]
    [InlineData(Source.ManageFreeSchoolProjects, UpdateFrequency.Daily)]
    [InlineData(Source.Mis, UpdateFrequency.Monthly)]
    [InlineData(Source.Mstr, UpdateFrequency.Daily)]
    [InlineData(Source.Prepare, UpdateFrequency.Daily)]
    public async Task GetAsync_uncached_should_cache_result(Source source, UpdateFrequency updateFrequency)
    {
        var expectedCacheTimeSpan =
            updateFrequency is UpdateFrequency.Daily ? TimeSpan.FromHours(1) : TimeSpan.FromDays(1);

        await _sut.GetAsync(source);
        
        _mockMemoryCache.Object.Received(1).CreateEntry(source);

        var cachedEntry = _mockMemoryCache.MockCacheEntries[source];

        cachedEntry.Value.Should().BeEquivalentTo(_dummyDataSources[source]);
        cachedEntry.AbsoluteExpirationRelativeToNow.Should().Be(expectedCacheTimeSpan);
    }

    [Fact]
    public async Task GetAsync_uncached_should_not_cache_source_with_null_lastUpdated()
    {
        _mockDataSourceRepository.GetAsync(Source.Gias).Returns(
            Task.FromResult(new Data.Repositories.DataSource.DataSource(Source.Gias, null, UpdateFrequency.Daily)));

        await _sut.GetAsync(Source.Gias);
        
        _mockMemoryCache.Object.DidNotReceive().CreateEntry(It.IsAny<Source>());
    }

    [Fact]
    public async Task GetAsync_should_throw_when_unknown_source()
    {
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _sut.GetAsync((Source)999));
    }
}
