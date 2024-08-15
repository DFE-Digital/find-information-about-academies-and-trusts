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
}
