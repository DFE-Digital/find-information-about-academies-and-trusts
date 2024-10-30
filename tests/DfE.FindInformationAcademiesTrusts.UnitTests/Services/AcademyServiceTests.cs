using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.Academy;
using DfE.FindInformationAcademiesTrusts.Services.Academy;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Services;

public class AcademyServiceTests
{
    private readonly AcademyService _sut;
    private readonly Mock<IAcademyRepository> _mockAcademyRepository = new();
    private readonly Mock<IFreeSchoolMealsAverageProvider> _mockFreeSchoolMealsAverageProvider = new();

    public AcademyServiceTests()
    {
        _sut = new AcademyService(_mockAcademyRepository.Object, _mockFreeSchoolMealsAverageProvider.Object);
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
                new OfstedRating((int)OfstedRatingScore.Good, new DateTime(2023, 1, 1)),
                new OfstedRating((int)OfstedRatingScore.RequiresImprovement, new DateTime(2023, 2, 1))),
            new AcademyOfsted("2", "Academy 2", new DateTime(2022, 11, 2),
                new OfstedRating((int)OfstedRatingScore.Good, new DateTime(2023, 1, 2)),
                new OfstedRating((int)OfstedRatingScore.RequiresImprovement, new DateTime(2023, 3, 1))),
            new AcademyOfsted("3", "Academy 3", new DateTime(2022, 10, 3),
                new OfstedRating((int)OfstedRatingScore.Good, new DateTime(2023, 1, 3)),
                new OfstedRating((int)OfstedRatingScore.RequiresImprovement, new DateTime(2023, 4, 1)))
        };

        _mockAcademyRepository.Setup(a => a.GetAcademiesInTrustOfstedAsync(uid))
            .ReturnsAsync(academies);

        var result = await _sut.GetAcademiesInTrustOfstedAsync(uid);

        result.Should().BeOfType<AcademyOfstedServiceModel[]>();
        result.Should().BeEquivalentTo(academies);
    }

    [Fact]
    public async Task GetAcademiesInTrustPupilNumbersAsync_should_return_mapped_result_from_repository()
    {
        const string uid = "1234";
        AcademyPupilNumbers[] academies =
        [
            BuildDummyAcademyPupilNumbers("9876", "phase1", new AgeRange(2, 15), 100, 200),
            BuildDummyAcademyPupilNumbers("8765", "phase2", new AgeRange(7, 12), 2, 5)
        ];

        _mockAcademyRepository.Setup(a => a.GetAcademiesInTrustPupilNumbersAsync(uid))
            .ReturnsAsync(academies);

        var result = await _sut.GetAcademiesInTrustPupilNumbersAsync(uid);

        result.Should().BeOfType<AcademyPupilNumbersServiceModel[]>();
        result.Should().BeEquivalentTo(academies);
    }

    private static AcademyPupilNumbers BuildDummyAcademyPupilNumbers(string urn,
        string phaseOfEducation = "test",
        AgeRange? ageRange = null,
        int? numberOfPupils = 300,
        int? schoolCapacity = 400
    )
    {
        return new AcademyPupilNumbers(urn,
            $"Academy {urn}",
            phaseOfEducation,
            ageRange ?? new AgeRange(11, 18),
            numberOfPupils,
            schoolCapacity);
    }

    [Fact]
    public async Task GetAcademiesInTrustFreeSchoolMealsAsync_should_map_relevant_fields_from_repository()
    {
        const string uid = "1234";
        var academy =
            new AcademyFreeSchoolMeals(string.Empty, null, null, 0, string.Empty, string.Empty);
        AcademyFreeSchoolMeals[] academies =
        [
            academy with { Urn = "1", EstablishmentName = "Academy 1", PercentageFreeSchoolMeals = 12.0 },
            academy with { Urn = "2", EstablishmentName = "Academy 2", PercentageFreeSchoolMeals = 0 },
            academy with { Urn = "3", EstablishmentName = "Academy 3", PercentageFreeSchoolMeals = 24.1 },
            academy with { Urn = "4", EstablishmentName = "Academy 4", PercentageFreeSchoolMeals = null },
            academy with { Urn = "5", EstablishmentName = "Academy 5", PercentageFreeSchoolMeals = 100 }
        ];

        _mockAcademyRepository.Setup(a => a.GetAcademiesInTrustFreeSchoolMealsAsync(uid))
            .ReturnsAsync(academies);

        var result = await _sut.GetAcademiesInTrustFreeSchoolMealsAsync(uid);

        result.Should().BeOfType<AcademyFreeSchoolMealsServiceModel[]>();
        result.Should().BeEquivalentTo(academies, options => options
            .Excluding(a => a.LocalAuthorityCode)
            .Excluding(a => a.PhaseOfEducation)
            .Excluding(a => a.TypeOfEstablishment));
    }

    [Fact]
    public async Task
        GetAcademiesInTrustFreeSchoolMealsAsync_should_get_relevant_fields_from_freeSchoolMealsAveragesProvider()
    {
        const string uid = "1234";
        var academy =
            new AcademyFreeSchoolMeals(string.Empty, null, null, 0, string.Empty, string.Empty);
        AcademyFreeSchoolMeals[] academies =
        [
            academy with
            {
                LocalAuthorityCode = 12, PhaseOfEducation = "Primary", TypeOfEstablishment = "Community school"
            },
            academy with
            {
                LocalAuthorityCode = 22, PhaseOfEducation = "Secondary", TypeOfEstablishment = "Academy converter"
            },
            academy with
            {
                LocalAuthorityCode = 34, TypeOfEstablishment = "Foundation special school"
            }
        ];
        _mockAcademyRepository.Setup(a => a.GetAcademiesInTrustFreeSchoolMealsAsync(uid))
            .ReturnsAsync(academies);

        _mockFreeSchoolMealsAverageProvider.Setup(f => f.GetLaAverage(12, "Primary", "Community school"))
            .Returns(22.4);
        _mockFreeSchoolMealsAverageProvider.Setup(f => f.GetLaAverage(22, "Secondary", "Academy converter"))
            .Returns(23.5);
        _mockFreeSchoolMealsAverageProvider.Setup(f => f.GetLaAverage(34, string.Empty, "Foundation special school"))
            .Returns(70);
        _mockFreeSchoolMealsAverageProvider.Setup(f => f.GetNationalAverage("Primary", "Community school"))
            .Returns(12.4);
        _mockFreeSchoolMealsAverageProvider.Setup(f => f.GetNationalAverage("Secondary", "Academy converter"))
            .Returns(13.5);
        _mockFreeSchoolMealsAverageProvider.Setup(f => f.GetNationalAverage(string.Empty, "Foundation special school"))
            .Returns(60);

        var result = await _sut.GetAcademiesInTrustFreeSchoolMealsAsync(uid);

        result[0].LaAveragePercentageFreeSchoolMeals.Should().Be(22.4);
        result[0].NationalAveragePercentageFreeSchoolMeals.Should().Be(12.4);
        result[1].LaAveragePercentageFreeSchoolMeals.Should().Be(23.5);
        result[1].NationalAveragePercentageFreeSchoolMeals.Should().Be(13.5);
        result[2].LaAveragePercentageFreeSchoolMeals.Should().Be(70);
        result[2].NationalAveragePercentageFreeSchoolMeals.Should().Be(60);
    }
}
