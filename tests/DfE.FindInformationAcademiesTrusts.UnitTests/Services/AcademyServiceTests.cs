using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.Academy;
using DfE.FindInformationAcademiesTrusts.Services.Academy;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Services;

public class AcademyServiceTests
{
    private readonly AcademyService _sut;
    private readonly Mock<IAcademyRepository> _mockAcademyRepository = new();

    public AcademyServiceTests()
    {
        _sut = new AcademyService(_mockAcademyRepository.Object);
    }

    [Fact]
    public async Task GetAcademiesInTrustDetailsAsync_should_return_mapped_result_from_repository()
    {
        const string uid = "1234";
        AcademyDetails[] academies =
        [
            new AcademyDetails("9876", "Academy 1", "Academy converter", "Oxfordshire",
                "(England/Wales) Urban city and town"),
            new AcademyDetails("9876", "Academy 2", "Academy sponsor led", "Lincolnshire",
                "(England/Wales) Rural town and fringe")
        ];

        _mockAcademyRepository.Setup(a => a.GetAcademiesInTrustDetailsAsync(uid))
            .ReturnsAsync(academies);

        var result = await _sut.GetAcademiesInTrustDetailsAsync(uid);

        result.Should().BeOfType<AcademyDetailsServiceModel[]>();
        result.Should().BeEquivalentTo(academies);
    }

    [Fact]
    public async Task GetAcademiesInTrustOfstedAsync_should_return_mapped_result_from_repository()
    {
        const string uid = "1234";
        var academies = new[]
        {
            new AcademyOfsted("1", "Academy 1", new DateTime(2022, 12, 1),
                new OfstedRating(OfstedRatingScore.Good, new DateTime(2023, 1, 1)),
                new OfstedRating(OfstedRatingScore.RequiresImprovement, new DateTime(2023, 2, 1))),
            new AcademyOfsted("2", "Academy 2", new DateTime(2022, 11, 2),
                new OfstedRating(OfstedRatingScore.Good, new DateTime(2023, 1, 2)),
                new OfstedRating(OfstedRatingScore.RequiresImprovement, new DateTime(2023, 3, 1))),
            new AcademyOfsted("3", "Academy 3", new DateTime(2022, 10, 3),
                new OfstedRating(OfstedRatingScore.Good, new DateTime(2023, 1, 3)),
                new OfstedRating(OfstedRatingScore.RequiresImprovement, new DateTime(2023, 4, 1)))
        };

        _mockAcademyRepository.Setup(a => a.GetAcademiesInTrustOfstedAsync(uid))
            .ReturnsAsync(academies);

        var result = await _sut.GetAcademiesInTrustOfstedAsync(uid);

        result.Should().BeOfType<AcademyOfstedServiceModel[]>();
        result.Should().BeEquivalentTo(academies);
    }
}
