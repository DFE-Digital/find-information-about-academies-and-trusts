using ClosedXML.Excel;
using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.Academy;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.Trust;
using DfE.FindInformationAcademiesTrusts.Pages;
using DfE.FindInformationAcademiesTrusts.Services.Export;
using DfE.FindInformationAcademiesTrusts.Services.Trust;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Services;

public class ExportServiceTests
{
    private readonly Mock<IDateTimeProvider> _mockDateTimeProvider;
    private readonly Mock<IAcademyRepository> _mockAcademyRepository;
    private readonly Mock<ITrustRepository> _mockTrustRepository;
    private readonly ExportService _sut;

    public ExportServiceTests()
    {
        _mockDateTimeProvider = new Mock<IDateTimeProvider>();
        _mockAcademyRepository = new Mock<IAcademyRepository>();
        _mockTrustRepository = new Mock<ITrustRepository>();

        // Baseline date for testing will be the current date in this instance
        _mockDateTimeProvider.Setup(m => m.Now).Returns(DateTime.Now);

        _sut = new ExportService(_mockAcademyRepository.Object, _mockTrustRepository.Object);
    }

    [Fact]
    public async Task ExportAcademiesToSpreadsheet_ShouldGenerateCorrectHeadersAsync()
    {
        // Arrange            
        var trustSummary = new TrustSummaryServiceModel("1", "Sample Trust", "Multi-academy trust", 0);

        // Act
        var result = await _sut.ExportAcademiesToSpreadsheetAsync(trustSummary.Uid);
        using var workbook = new XLWorkbook(new MemoryStream(result));
        var worksheet = workbook.Worksheet("Academies");

        // Assert
        worksheet.Cell(3, 1).Value.ToString().Should().Be("School Name");
        worksheet.Cell(3, 2).Value.ToString().Should().Be("URN");
        worksheet.Cell(3, 3).Value.ToString().Should().Be("Local Authority");
        worksheet.Cell(3, 4).Value.ToString().Should().Be("Type");
        worksheet.Cell(3, 5).Value.ToString().Should().Be("Rural or Urban");
        worksheet.Cell(3, 6).Value.ToString().Should().Be("Date joined");
        worksheet.Cell(3, 7).Value.ToString().Should().Be("Previous Ofsted Rating");
        worksheet.Cell(3, 8).Value.ToString().Should().Be("Before/After Joining");
        worksheet.Cell(3, 9).Value.ToString().Should().Be("Date of Previous Ofsted");
        worksheet.Cell(3, 10).Value.ToString().Should().Be("Current Ofsted Rating");
        worksheet.Cell(3, 11).Value.ToString().Should().Be("Before/After Joining");
        worksheet.Cell(3, 12).Value.ToString().Should().Be("Date of Current Ofsted");
        worksheet.Cell(3, 13).Value.ToString().Should().Be("Phase of Education");
        worksheet.Cell(3, 14).Value.ToString().Should().Be("Age Range");
        worksheet.Cell(3, 15).Value.ToString().Should().Be("Pupil Numbers");
        worksheet.Cell(3, 16).Value.ToString().Should().Be("Capacity");
        worksheet.Cell(3, 17).Value.ToString().Should().Be("% Full");
        worksheet.Cell(3, 18).Value.ToString().Should().Be("Pupils eligible for Free School Meals");
    }

    [Fact]
    public async Task ExportAcademiesToSpreadsheet_ShouldCorrectlyExtractAcademyDataAsync()
    {
        // Arrange
        var trustSummary = new TrustSummaryServiceModel("1", "Sample Trust", "Multi-academy trust", 1);

        // Trust summary set up
        _mockTrustRepository.Setup(x => x.GetTrustSummaryAsync(trustSummary.Uid)).ReturnsAsync(
            new TrustSummary("Sample Trust", "Multi-academy trust"));
        // Academies details set up
        _mockAcademyRepository.Setup(m => m.GetAcademiesInTrustDetailsAsync(trustSummary.Uid)).ReturnsAsync(
            new AcademyDetails[]
            {
                new("123456", "Academy 1", "Type A", "Local Authority 1", "Urban")
            });
        // Academies ofsted set up
        _mockAcademyRepository.Setup(m => m.GetAcademiesInTrustOfstedAsync(trustSummary.Uid)).ReturnsAsync(
            new AcademyOfsted[]
            {
                new("123456", "Academy 1", DateTime.Now, new OfstedRating(-1, null), new OfstedRating(1, DateTime.Now))
            });
        // Academies pupil number set up
        _mockAcademyRepository.Setup(m => m.GetAcademiesInTrustPupilNumbersAsync(trustSummary.Uid)).ReturnsAsync(
            new AcademyPupilNumbers[]
            {
                new("123456", "Academy 1", "Primary", new AgeRange(5, 11), 500, 600)
            });
        // Academies free school meals
        _mockAcademyRepository.Setup(m
            => m.GetAcademiesInTrustFreeSchoolMealsAsync(trustSummary.Uid)).ReturnsAsync(
            new AcademyFreeSchoolMeals[]
            {
                new("123456", "Academy 1", 20, 1, "Type A", "Primary")
            });

        // Act
        var result = await _sut.ExportAcademiesToSpreadsheetAsync(trustSummary.Uid);
        using var workbook = new XLWorkbook(new MemoryStream(result));
        var worksheet = workbook.Worksheet("Academies");

        // Assert
        worksheet.Cell(4, 1).Value.ToString().Should().Be("Academy 1");
        worksheet.Cell(4, 2).Value.ToString().Should().Be("123456");
        worksheet.Cell(4, 3).Value.ToString().Should().Be("Local Authority 1");
        worksheet.Cell(4, 4).Value.ToString().Should().Be("Type A");
        worksheet.Cell(4, 5).Value.ToString().Should().Be("Urban");
        worksheet.Cell(4, 6).Value.ToString().Should().Be(DateTime.Now.ToString(StringFormatConstants.ViewDate));
        worksheet.Cell(4, 7).Value.ToString().Should().Be("Not yet inspected");
        worksheet.Cell(4, 8).Value.ToString().Should().Be(string.Empty);
        worksheet.Cell(4, 9).Value.ToString().Should().Be(string.Empty);
        worksheet.Cell(4, 10).Value.ToString().Should().Be("Outstanding");
        worksheet.Cell(4, 11).Value.ToString().Should().Be("After Joining");
        worksheet.Cell(4, 12).Value.ToString().Should().Be(DateTime.Now.ToString(StringFormatConstants.ViewDate));
        worksheet.Cell(4, 13).Value.ToString().Should().Be("Primary");
        worksheet.Cell(4, 14).Value.ToString().Should().Be("5 - 11");
        worksheet.Cell(4, 15).Value.ToString().Should().Be("500");
        worksheet.Cell(4, 16).Value.ToString().Should().Be("600");
        worksheet.Cell(4, 17).Value.ToString().Should().Be("83%");
        worksheet.Cell(4, 18).Value.ToString().Should().Be("20%");
    }

    [Fact]
    public async Task ExportAcademiesToSpreadsheet_ShouldHandleEmptyAcademiesAsync()
    {
        // Arrange            
        var trustSummary = new TrustSummaryServiceModel("1", "Empty Trust", "Multi-academy trust", 0);

        // Act
        var result = await _sut.ExportAcademiesToSpreadsheetAsync(trustSummary.Uid);
        using var workbook = new XLWorkbook(new MemoryStream(result));
        var worksheet = workbook.Worksheet("Academies");

        // Assert
        worksheet.LastRowUsed().RowNumber().Should()
            .Be(3); // Last row should be headers as there is no data for the next row
    }

    [Fact]
    public async Task ExportAcademiesToSpreadsheet_ShouldCorrectlyHandleNullValuesAsync()
    {
        // Arrange

        var trustSummary = new TrustSummaryServiceModel("1", "Sample Trust", "Multi-academy trust", 1);

        // Trust summary set up
        _mockTrustRepository.Setup(x => x.GetTrustSummaryAsync(trustSummary.Uid)).ReturnsAsync(
            new TrustSummary("Sample Trust", "Multi-academy trust"));
        // Academies details set up
        _mockAcademyRepository.Setup(m => m.GetAcademiesInTrustDetailsAsync(trustSummary.Uid)).ReturnsAsync(
            new AcademyDetails[]
            {
                new("123456", null, null, null, null)
            });
        // Academies ofsted set up
        _mockAcademyRepository.Setup(m => m.GetAcademiesInTrustOfstedAsync(trustSummary.Uid)).ReturnsAsync(
            new AcademyOfsted[]
            {
                new("123456", null, DateTime.Now, new OfstedRating(-1, null), new OfstedRating(-1, null))
            });
        // Academies pupil number set up
        _mockAcademyRepository.Setup(m => m.GetAcademiesInTrustPupilNumbersAsync(trustSummary.Uid)).ReturnsAsync(
            new AcademyPupilNumbers[]
            {
                new("123456", null, null, new AgeRange(5, 11), null, null)
            });
        // Academies free school meals
        _mockAcademyRepository.Setup(m
            => m.GetAcademiesInTrustFreeSchoolMealsAsync(trustSummary.Uid)).ReturnsAsync(
            new AcademyFreeSchoolMeals[]
            {
                new("123456", null, null, 1, null, null)
            });
        // Act
        var result = await _sut.ExportAcademiesToSpreadsheetAsync(trustSummary.Uid);
        using var workbook = new XLWorkbook(new MemoryStream(result));
        var worksheet = workbook.Worksheet("Academies");

        // Assert
        worksheet.Cell(4, 1).Value.ToString().Should().Be(string.Empty);
        worksheet.Cell(4, 2).Value.ToString().Should().Be("123456");
        worksheet.Cell(4, 3).Value.ToString().Should().Be(string.Empty);
        worksheet.Cell(4, 4).Value.ToString().Should().Be(string.Empty);
        worksheet.Cell(4, 5).Value.ToString().Should().Be(string.Empty);
        worksheet.Cell(4, 6).Value.ToString().Should().Be(DateTime.Now.ToString(StringFormatConstants.ViewDate));
        worksheet.Cell(4, 7).Value.ToString().Should().Be("Not yet inspected");
        worksheet.Cell(4, 8).Value.ToString().Should().Be(string.Empty);
        worksheet.Cell(4, 9).Value.ToString().Should().Be(string.Empty);
        worksheet.Cell(4, 10).Value.ToString().Should().Be("Not yet inspected");
        worksheet.Cell(4, 11).Value.ToString().Should().Be(string.Empty);
        worksheet.Cell(4, 12).Value.ToString().Should().Be(string.Empty);
        worksheet.Cell(4, 13).Value.ToString().Should().Be(string.Empty);
        worksheet.Cell(4, 14).Value.ToString().Should().Be("5 - 11");
        worksheet.Cell(4, 15).Value.ToString().Should().Be(string.Empty);
        worksheet.Cell(4, 16).Value.ToString().Should().Be(string.Empty);
        worksheet.Cell(4, 17).Value.ToString().Should().Be(string.Empty);
        worksheet.Cell(4, 18).Value.ToString().Should().Be(string.Empty);
    }

    [Fact]
    public void IsOfstedRatingBeforeOrAfterJoining_ShouldReturnEmptyString_WhenOfstedRatingScoreIsNone()
    {
        // Arrange
        var ofstedRatingScore = OfstedRatingScore.None;
        var dateJoinedTrust = _mockDateTimeProvider.Object.Now;
        DateTime? inspectionEndDate = dateJoinedTrust.AddDays(-1);

        // Act
        var result =
            ExportService.IsOfstedRatingBeforeOrAfterJoining(ofstedRatingScore, dateJoinedTrust, inspectionEndDate);

        // Assert
        result.Should().Be(string.Empty);
    }

    [Fact]
    public void IsOfstedRatingBeforeOrAfterJoining_ShouldReturnBeforeJoining_WhenInspectionDateIsBeforeJoiningDate()
    {
        // Arrange
        var ofstedRatingScore = OfstedRatingScore.Good;
        var dateJoinedTrust = _mockDateTimeProvider.Object.Now;
        DateTime? inspectionEndDate = dateJoinedTrust.AddDays(-10);

        // Act
        var result =
            ExportService.IsOfstedRatingBeforeOrAfterJoining(ofstedRatingScore, dateJoinedTrust, inspectionEndDate);

        // Assert
        result.Should().Be("Before Joining");
    }

    [Fact]
    public void IsOfstedRatingBeforeOrAfterJoining_ShouldReturnAfterJoining_WhenInspectionDateIsAfterJoiningDate()
    {
        // Arrange
        var ofstedRatingScore = OfstedRatingScore.Good;
        var dateJoinedTrust = _mockDateTimeProvider.Object.Now.AddDays(-10);
        DateTime? inspectionEndDate = dateJoinedTrust.AddDays(5);

        // Act
        var result =
            ExportService.IsOfstedRatingBeforeOrAfterJoining(ofstedRatingScore, dateJoinedTrust, inspectionEndDate);

        // Assert
        result.Should().Be("After Joining");
    }

    [Fact]
    public void IsOfstedRatingBeforeOrAfterJoining_ShouldReturnEmptyString_WhenInspectionDateIsNull()
    {
        // Arrange
        var ofstedRatingScore = OfstedRatingScore.Good;
        var dateJoinedTrust = _mockDateTimeProvider.Object.Now;


        // Act
        var result = ExportService.IsOfstedRatingBeforeOrAfterJoining(ofstedRatingScore, dateJoinedTrust, null);

        // Assert
        result.Should().Be(string.Empty);
    }

    [Theory]
    [InlineData(500, 600, 83)] // Valid case
    [InlineData(300, 300, 100)] // Edge case: full capacity
    [InlineData(0, 300, 0)] // Edge case: 0 pupils
    [InlineData(300, 0, 0)] // Edge case: zero capacity (should return 0)
    [InlineData(null, 300, 0)] // Edge case: null pupils
    [InlineData(300, null, 0)] // Edge case: null capacity
    [InlineData(null, null, 0)] // Edge case: both null
    public void CalculatePercentageFull_ShouldReturnExpectedResult(int? numberOfPupils, int? schoolCapacity,
        float expected)
    {
        // Act
        var result = ExportService.CalculatePercentageFull(numberOfPupils, schoolCapacity);

        // Assert
        Assert.Equal(expected, result);
    }
}
