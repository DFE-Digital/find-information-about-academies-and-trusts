using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.School;
using DfE.FindInformationAcademiesTrusts.Services.School;
using DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Services;

public class SchoolServiceTests
{
    private readonly SchoolService _sut;
    private readonly ISchoolRepository _mockSchoolRepository = Substitute.For<ISchoolRepository>();
    private readonly MockMemoryCache _mockMemoryCache = new();

    public SchoolServiceTests()
    {
        _sut = new SchoolService(_mockMemoryCache.Object, _mockSchoolRepository);
    }

    [Fact]
    public async Task GetSchoolSummaryAsync_cached_should_return_cached_result()
    {
        const string urn = "123456";
        const string key = $"{nameof(SchoolService.GetSchoolSummaryAsync)}:{urn}";
        var cachedResult = new SchoolSummaryServiceModel(urn, "Chill primary school", "Academy sponsor led",
            SchoolCategory.LaMaintainedSchool);
        _mockMemoryCache.AddMockCacheEntry(key, cachedResult);

        var result = await _sut.GetSchoolSummaryAsync(urn);
        result.Should().Be(cachedResult);

        await _mockSchoolRepository.DidNotReceive().GetSchoolSummaryAsync(urn);
    }

    [Fact]
    public async Task GetSchoolSummaryAsync_should_return_null_if_not_found()
    {
        _mockSchoolRepository.GetSchoolSummaryAsync("this urn doesn't exist").Returns((SchoolSummary?)null);

        var result = await _sut.GetSchoolSummaryAsync("this urn doesn't exist");
        result.Should().BeNull();
    }

    [Theory]
    [InlineData("280689", "My School", "Foundation school", SchoolCategory.LaMaintainedSchool)]
    [InlineData("900855", "My Academy", "Academy converter", SchoolCategory.Academy)]
    public async Task GetSchoolSummaryAsync_should_return_schoolSummary_if_found(string urn, string name, string type,
        SchoolCategory category)
    {
        _mockSchoolRepository.GetSchoolSummaryAsync(urn).Returns(new SchoolSummary(name, type, category));

        var result = await _sut.GetSchoolSummaryAsync(urn);
        result.Should().BeEquivalentTo(new SchoolSummaryServiceModel(urn, name, type, category));
    }

    [Theory]
    [InlineData("280689", "My School", "Foundation school", SchoolCategory.LaMaintainedSchool)]
    [InlineData("900855", "My Academy", "Academy converter", SchoolCategory.Academy)]
    public async Task GetSchoolSummaryAsync_uncached_should_cache_result(string urn, string name, string type,
        SchoolCategory category)
    {
        var key = $"{nameof(SchoolService.GetSchoolSummaryAsync)}:{urn}";

        _mockSchoolRepository.GetSchoolSummaryAsync(urn).Returns(new SchoolSummary(name, type, category));

        await _sut.GetSchoolSummaryAsync(urn);

        _mockMemoryCache.Object.Received(1).CreateEntry(key);

        var cachedEntry = _mockMemoryCache.MockCacheEntries[key];

        cachedEntry.Value.Should().BeEquivalentTo(new SchoolSummaryServiceModel(urn, name, type, category));
        cachedEntry.SlidingExpiration.Should().Be(TimeSpan.FromMinutes(10));
    }
}
