using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Academies;
using DfE.FindInformationAcademiesTrusts.Services.Academy;
using DfE.FindInformationAcademiesTrusts.Services.Export;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts.Academies;

public class PupilNumbersModelTests
{
    private readonly PupilNumbersModel _sut;
    private readonly MockDataSourceService _mockDataSourceService = new();
    private readonly Mock<ITrustService> _mockTrustRepository = new();
    private readonly Mock<IExportService> _mockExportService = new();
    private readonly Mock<DateTimeProvider> _mockDateTimeProvider = new();
    private readonly Mock<IAcademyService> _mockAcademyService = new();

    public PupilNumbersModelTests()
    {
        MockLogger<PupilNumbersModel> logger = new();
        var testTrustUid = "1234";
        var testTrustName = "Test Trust";
        var testTrustType = "SAT";

        _mockTrustRepository.Setup(t => t.GetTrustSummaryAsync(testTrustUid))
            .ReturnsAsync(new TrustSummaryServiceModel(testTrustUid, testTrustName, testTrustType,
                1));
        _mockAcademyService.Setup(t => t.GetAcademiesInTrustPupilNumbersAsync(testTrustUid))
            .ReturnsAsync([
                new AcademyPupilNumbersServiceModel(testTrustUid, testTrustName, "Phase",
                    new AgeRange("11", "16"), 100, 200)
            ]);

        _sut = new PupilNumbersModel(_mockDataSourceService.Object, logger.Object,
               _mockTrustRepository.Object, _mockAcademyService.Object, _mockExportService.Object, _mockDateTimeProvider.Object)
        { Uid = "1234" };

    }

    [Fact]
    public void PageTitle_should_be_AcademiesPupilNumbers()
    {
        _sut.PageTitle.Should().Be("Academies pupil numbers");
    }


    [Fact]
    public void TabName_should_be_PupilNumbers()
    {
        _sut.TabName.Should().Be("Pupil numbers");
    }

    [Fact]
    public void PageName_should_be_AcademiesInThisTrust()
    {
        _sut.PageName.Should().Be("Academies in this trust");
    }

    [Theory]
    [InlineData("Primary", 5, 11, "Primary0511")]
    [InlineData("Primary", 5, 9, "Primary0509")]
    [InlineData("Primary", 0, 7, "Primary0007")]
    [InlineData("16 plus", 16, 19, "16 plus1619")]
    [InlineData("Secondary", 10, 18, "Secondary1018")]
    public void PhaseAndAgeRangeSortValue_should_be_amalgamation_of_Phase_and_age_range_properties(string phase,
        int minAge, int maxAge, string expected)
    {
        var ageRange = new AgeRange(minAge, maxAge);
        var testAcademyUrn = "1234";
        var testAcademyName = "Test Academy";
        var testAcademyNumberOfPupils = 100;
        var testAcademySchoolCapacity = 100;

        var result = PupilNumbersModel.PhaseAndAgeRangeSortValue(new AcademyPupilNumbersServiceModel(testAcademyUrn,
            testAcademyName, phase, ageRange,
            testAcademyNumberOfPupils, testAcademySchoolCapacity));
        result.Should().Be(expected);
    }

    [Fact]
    public async Task OnGetAsync_returns_NotFoundResult_if_Trust_is_not_found()
    {
        _mockTrustRepository.Setup(t => t.GetTrustSummaryAsync("1234")).ReturnsAsync((TrustSummaryServiceModel?)null);
        var result = await _sut.OnGetAsync();
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task OnGetAsync_sets_correct_data_source_list()
    {
        await _sut.OnGetAsync();
        _mockDataSourceService.Verify(e => e.GetAsync(Source.Gias), Times.Once);
        _sut.DataSources.Should().ContainSingle();
        _sut.DataSources[0].Fields.Should().Contain(new List<string> { "Pupil numbers" });
    }
}
