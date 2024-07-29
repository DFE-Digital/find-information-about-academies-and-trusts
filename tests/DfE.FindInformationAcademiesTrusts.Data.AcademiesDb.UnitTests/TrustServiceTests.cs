using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Repositories;
using DfE.FindInformationAcademiesTrusts.Data.Dto;
using DfE.FindInformationAcademiesTrusts.Data.RepositoryDto;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests;

public class TrustServiceTests
{
    private readonly TrustService _sut;
    private readonly Mock<IAcademyRepository> _mockAcademyRepository = new();
    private readonly Mock<ITrustRepository> _mockTrustRepository = new();

    public TrustServiceTests()
    {
        _sut = new TrustService(_mockAcademyRepository.Object,
            _mockTrustRepository.Object);
    }

    [Fact]
    public async Task GetTrustSummaryAsync_should_return_null_if_no_giasGroup_found()
    {
        _mockTrustRepository.Setup(t => t.GetTrustSummaryAsync("this uid doesn't exist"))
            .ReturnsAsync((TrustSummaryRepoDto?)null);

        var result = await _sut.GetTrustSummaryAsync("this uid doesn't exist");
        result.Should().BeNull();
    }

    [Theory]
    [InlineData("2806", "My Trust", "Multi-academy trust", 3)]
    [InlineData("9008", "Another Trust", "Single-academy trust", 1)]
    [InlineData("9008", "Trust with no academies", "Multi-academy trust", 0)]
    public async Task GetTrustSummaryAsync_should_return_trustSummary_if_found(string uid, string name, string type,
        int numAcademies)
    {
        _mockTrustRepository.Setup(t => t.GetTrustSummaryAsync(uid)).ReturnsAsync(new TrustSummaryRepoDto(name, type));
        _mockAcademyRepository.Setup(a => a.GetNumberOfAcademiesInTrustAsync(uid))
            .ReturnsAsync(numAcademies);

        var result = await _sut.GetTrustSummaryAsync(uid);
        result.Should().BeEquivalentTo(new TrustSummaryDto(uid, name, type, numAcademies));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("123456")]
    [InlineData("567890")]
    public async Task GetTrustDetailsAsync_should_get_singleAcademyUrn_from_academy_repository(string? singleAcademyUrn)
    {
        _mockAcademyRepository.Setup(a => a.GetUrnForSingleAcademyTrustAsync("2806"))
            .ReturnsAsync(singleAcademyUrn);
        _mockTrustRepository.Setup(t => t.GetTrustDetailsAsync("2806"))
            .ReturnsAsync(new TrustDetailsRepoDto("2806",
                "TR0012",
                "10012345",
                "123456",
                "Multi-academy trust",
                "123 Fairyland Drive, Gotham, GT12 1AB",
                "Oxfordshire",
                new DateTime(2007, 6, 28)));

        var result = await _sut.GetTrustDetailsAsync("2806");

        result.SingleAcademyUrn.Should().Be(singleAcademyUrn);
    }

    [Fact]
    public async Task GetTrustDetailsAsync_should_set_properties_from_TrustRepo()
    {
        _mockTrustRepository.Setup(t => t.GetTrustDetailsAsync("2806"))
            .ReturnsAsync(new TrustDetailsRepoDto("2806",
                "TR0012",
                "10012345",
                "123456",
                "Multi-academy trust",
                "123 Fairyland Drive, Gotham, GT12 1AB",
                "Oxfordshire",
                new DateTime(2007, 6, 28)));

        var result = await _sut.GetTrustDetailsAsync("2806");

        result.Should().BeEquivalentTo(new TrustDetailsDto("2806",
            "TR0012",
            "10012345",
            "123456",
            "Multi-academy trust",
            "123 Fairyland Drive, Gotham, GT12 1AB",
            "Oxfordshire",
            null,
            new DateTime(2007, 6, 28)
        ));
    }
}
