using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Repositories;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests.Mocks;
using DfE.FindInformationAcademiesTrusts.Data.RepositoryDto;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests.Repositories;

public class TrustRepositoryTests
{
    private readonly TrustRepository _sut;
    private readonly MockAcademiesDbContext _mockAcademiesDbContext = new();

    public TrustRepositoryTests()
    {
        _mockAcademiesDbContext.SetupMockDbContextGiasGroups(5);
        _sut = new TrustRepository(_mockAcademiesDbContext.Object);
    }

    [Theory]
    [InlineData("2806", "My Trust", "Multi-academy trust")]
    [InlineData("9008", "Another Trust", "Single-academy trust")]
    [InlineData("9008", "Trust with no academies", "Multi-academy trust")]
    public async Task GetTrustSummaryAsync_should_return_trustSummary_if_found(string uid, string name, string type)
    {
        _ = _mockAcademiesDbContext.CreateGiasGroup(uid, name, type);

        var result = await _sut.GetTrustSummaryAsync(uid);
        result.Should().BeEquivalentTo(new TrustSummaryRepoDto(name, type));
    }

    [Fact]
    public async Task GetTrustSummaryAsync_should_return_empty_values_on_null_group_fields()
    {
        _ = _mockAcademiesDbContext.CreateGiasGroup("2806", null, null);

        var result = await _sut.GetTrustSummaryAsync("2806");
        result.Should().BeEquivalentTo(new TrustSummaryRepoDto(string.Empty, string.Empty));
    }
}
