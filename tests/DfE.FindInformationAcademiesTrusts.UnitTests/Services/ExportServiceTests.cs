using ClosedXML.Excel;
using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.Academy;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.Trust;
using DfE.FindInformationAcademiesTrusts.Services.Academy;
using DfE.FindInformationAcademiesTrusts.Services.Export;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using DocumentFormat.OpenXml.Wordprocessing;
using FluentAssertions.Execution;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Services;

public class ExportServiceTests
{
    private readonly Mock<IDateTimeProvider> _mockDateTimeProvider;
    private readonly Mock<IAcademyRepository> _mockAcademyRepository;
    private readonly Mock<ITrustRepository> _mockTrustRepository;
    private readonly Mock<IAcademyService> _mockAcademyService;
    private readonly ExportService _sut;

    public ExportServiceTests()
    {
        _mockDateTimeProvider = new Mock<IDateTimeProvider>();
        _mockAcademyRepository = new Mock<IAcademyRepository>();
        _mockTrustRepository = new Mock<ITrustRepository>();
        _mockAcademyService = new Mock<IAcademyService>();

        _mockDateTimeProvider.Setup(m => m.Now).Returns(DateTime.Now);

        _sut = new ExportService(_mockAcademyRepository.Object, _mockTrustRepository.Object, _mockAcademyService.Object);
    }

    [Fact]
    public async Task ExportAcademiesToSpreadsheet_ShouldGenerateCorrectHeadersAsync()
    {
        var trustSummary = new TrustSummaryServiceModel("1", "Sample Trust", "Multi-academy trust", 0);

        var result = await _sut.ExportAcademiesToSpreadsheetAsync(trustSummary.Uid);
        using var workbook = new XLWorkbook(new MemoryStream(result));
        var worksheet = workbook.Worksheet("Academies");

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
        var trustSummary = new TrustSummaryServiceModel("1", "Sample Trust", "Multi-academy trust", 1);

        _mockTrustRepository.Setup(x => x.GetTrustSummaryAsync(trustSummary.Uid)).ReturnsAsync(
            new TrustSummary("Sample Trust", "Multi-academy trust"));

        var now = DateTime.Now;
        _mockAcademyRepository.Setup(m => m.GetAcademiesInTrustDetailsAsync(trustSummary.Uid)).ReturnsAsync(
            new AcademyDetails[]
            {
                new("123456", "Academy 1", "Type A", "Local Authority 1", "Urban")
            });
        _mockAcademyRepository.Setup(m => m.GetAcademiesInTrustOfstedAsync(trustSummary.Uid)).ReturnsAsync(
            new AcademyOfsted[]
            {
                new("123456", "Academy 1", now, new OfstedRating(-1, null), new OfstedRating(1, now))
            });
        _mockAcademyRepository.Setup(m => m.GetAcademiesInTrustPupilNumbersAsync(trustSummary.Uid)).ReturnsAsync(
            new AcademyPupilNumbers[]
            {
                new("123456", "Academy 1", "Primary", new AgeRange(5, 11), 500, 600)
            });
        _mockAcademyRepository.Setup(m => m.GetAcademiesInTrustFreeSchoolMealsAsync(trustSummary.Uid)).ReturnsAsync(
            new AcademyFreeSchoolMeals[]
            {
                new("123456", "Academy 1", 20, 1, "Type A", "Primary")
            });

        var result = await _sut.ExportAcademiesToSpreadsheetAsync(trustSummary.Uid);
        using var workbook = new XLWorkbook(new MemoryStream(result));
        var worksheet = workbook.Worksheet("Academies");

        worksheet.Cell(4, 1).Value.ToString().Should().Be("Academy 1");
        worksheet.Cell(4, 2).Value.ToString().Should().Be("123456");
        worksheet.Cell(4, 3).Value.ToString().Should().Be("Local Authority 1");
        worksheet.Cell(4, 4).Value.ToString().Should().Be("Type A");
        worksheet.Cell(4, 5).Value.ToString().Should().Be("Urban");

        // Check date joined is set as a date
        worksheet.Cell(4, 6).DataType.Should().Be(XLDataType.DateTime);
        worksheet.Cell(4, 6).GetValue<DateTime>().Should().BeCloseTo(now, TimeSpan.FromSeconds(1));

        worksheet.Cell(4, 7).Value.ToString().Should().Be("Not inspected");
        worksheet.Cell(4, 8).Value.ToString().Should().Be(string.Empty);
        worksheet.Cell(4, 9).Value.ToString().Should().Be(string.Empty);
        worksheet.Cell(4, 10).Value.ToString().Should().Be("Outstanding");
        worksheet.Cell(4, 11).Value.ToString().Should().Be("After Joining");

        // Current Ofsted date also as a date
        worksheet.Cell(4, 12).DataType.Should().Be(XLDataType.DateTime);
        worksheet.Cell(4, 12).GetValue<DateTime>().Should().BeCloseTo(now, TimeSpan.FromSeconds(1));

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
        var trustSummary = new TrustSummaryServiceModel("1", "Empty Trust", "Multi-academy trust", 0);

        var result = await _sut.ExportAcademiesToSpreadsheetAsync(trustSummary.Uid);
        using var workbook = new XLWorkbook(new MemoryStream(result));
        var worksheet = workbook.Worksheet("Academies");

        var lastUsedRow = worksheet.LastRowUsed()?.RowNumber() ?? 0;
        lastUsedRow.Should().Be(3); // If no data rows are present, we expect the last used row to be the headers row (3)
    }

    [Fact]
    public async Task ExportAcademiesToSpreadsheet_ShouldCorrectlyHandleNullValuesAsync()
    {
        var trustSummary = new TrustSummaryServiceModel("1", "Sample Trust", "Multi-academy trust", 1);

        _mockTrustRepository.Setup(x => x.GetTrustSummaryAsync(trustSummary.Uid)).ReturnsAsync(
            new TrustSummary("Sample Trust", "Multi-academy trust"));

        _mockAcademyRepository.Setup(m => m.GetAcademiesInTrustDetailsAsync(trustSummary.Uid)).ReturnsAsync(
            new AcademyDetails[]
            {
                new("123456", null, null, null, null)
            });

        var now = DateTime.Now;
        _mockAcademyRepository.Setup(m => m.GetAcademiesInTrustOfstedAsync(trustSummary.Uid)).ReturnsAsync(
            new AcademyOfsted[]
            {
                new("123456", null, now, new OfstedRating(-1, null), new OfstedRating(-1, null))
            });

        _mockAcademyRepository.Setup(m => m.GetAcademiesInTrustPupilNumbersAsync(trustSummary.Uid)).ReturnsAsync(
            new AcademyPupilNumbers[]
            {
                new("123456", null, null, new AgeRange(5, 11), null, null)
            });

        _mockAcademyRepository.Setup(m => m.GetAcademiesInTrustFreeSchoolMealsAsync(trustSummary.Uid)).ReturnsAsync(
            new AcademyFreeSchoolMeals[]
            {
                new("123456", null, null, 1, null, null)
            });

        var result = await _sut.ExportAcademiesToSpreadsheetAsync(trustSummary.Uid);
        using var workbook = new XLWorkbook(new MemoryStream(result));
        var worksheet = workbook.Worksheet("Academies");

        worksheet.Cell(4, 1).Value.ToString().Should().Be(string.Empty);
        worksheet.Cell(4, 2).Value.ToString().Should().Be("123456");
        worksheet.Cell(4, 3).Value.ToString().Should().Be(string.Empty);
        worksheet.Cell(4, 4).Value.ToString().Should().Be(string.Empty);
        worksheet.Cell(4, 5).Value.ToString().Should().Be(string.Empty);

        // Date joined as date
        worksheet.Cell(4, 6).DataType.Should().Be(XLDataType.DateTime);
        worksheet.Cell(4, 6).GetValue<DateTime>().Should().BeCloseTo(now, TimeSpan.FromSeconds(1));

        worksheet.Cell(4, 7).Value.ToString().Should().Be("Not inspected");
        worksheet.Cell(4, 8).Value.ToString().Should().Be(string.Empty);
        worksheet.Cell(4, 9).Value.ToString().Should().Be(string.Empty);
        worksheet.Cell(4, 10).Value.ToString().Should().Be("Not yet inspected");
        worksheet.Cell(4, 11).Value.ToString().Should().Be(string.Empty);

        // Current Ofsted date is null, so cell should be empty string
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
        var ofstedRatingScore = OfstedRatingScore.NotInspected;
        var dateJoinedTrust = _mockDateTimeProvider.Object.Now;
        DateTime? inspectionEndDate = dateJoinedTrust.AddDays(-1);

        var result =
            ExportService.IsOfstedRatingBeforeOrAfterJoining(ofstedRatingScore, dateJoinedTrust, inspectionEndDate);

        result.Should().Be(string.Empty);
    }

    [Fact]
    public void IsOfstedRatingBeforeOrAfterJoining_ShouldReturnBeforeJoining_WhenInspectionDateIsBeforeJoiningDate()
    {
        var ofstedRatingScore = OfstedRatingScore.Good;
        var dateJoinedTrust = _mockDateTimeProvider.Object.Now;
        DateTime? inspectionEndDate = dateJoinedTrust.AddDays(-10);

        var result =
            ExportService.IsOfstedRatingBeforeOrAfterJoining(ofstedRatingScore, dateJoinedTrust, inspectionEndDate);

        result.Should().Be("Before Joining");
    }

    [Fact]
    public void IsOfstedRatingBeforeOrAfterJoining_ShouldReturnAfterJoining_WhenInspectionDateIsAfterJoiningDate()
    {
        var ofstedRatingScore = OfstedRatingScore.Good;
        var dateJoinedTrust = _mockDateTimeProvider.Object.Now.AddDays(-10);
        DateTime? inspectionEndDate = dateJoinedTrust.AddDays(5);

        var result =
            ExportService.IsOfstedRatingBeforeOrAfterJoining(ofstedRatingScore, dateJoinedTrust, inspectionEndDate);

        result.Should().Be("After Joining");
    }

    [Fact]
    public void IsOfstedRatingBeforeOrAfterJoining_ShouldReturnEmptyString_WhenInspectionDateIsNull()
    {
        var ofstedRatingScore = OfstedRatingScore.Good;
        var dateJoinedTrust = _mockDateTimeProvider.Object.Now;

        var result = ExportService.IsOfstedRatingBeforeOrAfterJoining(ofstedRatingScore, dateJoinedTrust, null);

        result.Should().Be(string.Empty);
    }

    [Theory]
    [InlineData(500, 600, 83)]
    [InlineData(300, 300, 100)]
    [InlineData(0, 300, 0)]
    [InlineData(300, 0, 0)]
    [InlineData(null, 300, 0)]
    [InlineData(300, null, 0)]
    [InlineData(null, null, 0)]
    public void CalculatePercentageFull_ShouldReturnExpectedResult(int? numberOfPupils, int? schoolCapacity,
        float expected)
    {
        var result = ExportService.CalculatePercentageFull(numberOfPupils, schoolCapacity);
        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task ExportAcademiesToSpreadsheet_ShouldHandleNullTrustSummaryAsync()
    {
        var uid = "some-uid";
        _mockTrustRepository.Setup(x => x.GetTrustSummaryAsync(uid))
            .ReturnsAsync((TrustSummary?)null);

        var result = await _sut.ExportAcademiesToSpreadsheetAsync(uid);
        using var workbook = new XLWorkbook(new MemoryStream(result));
        var worksheet = workbook.Worksheet("Academies");

        worksheet.Cell(1, 1).Value.ToString().Should().Be(string.Empty);
        worksheet.Cell(2, 1).Value.ToString().Should().Be(string.Empty);
    }

    [Fact]
    public async Task ExportAcademiesToSpreadsheet_ShouldHandleMissingOfstedDataAsync()
    {
        var trustSummary = new TrustSummaryServiceModel("1", "Sample Trust", "Multi-academy trust", 1);
        var academyUrn = "123456";

        _mockTrustRepository.Setup(x => x.GetTrustSummaryAsync(trustSummary.Uid))
            .ReturnsAsync(new TrustSummary("Sample Trust", "Multi-academy trust"));
        _mockAcademyRepository.Setup(m => m.GetAcademiesInTrustDetailsAsync(trustSummary.Uid))
            .ReturnsAsync(new AcademyDetails[]
            {
                new(academyUrn, "Academy 1", "Type A", "Local Authority 1", "Urban")
            });
        _mockAcademyRepository.Setup(m => m.GetAcademiesInTrustOfstedAsync(trustSummary.Uid))
            .ReturnsAsync(Array.Empty<AcademyOfsted>());

        var result = await _sut.ExportAcademiesToSpreadsheetAsync(trustSummary.Uid);
        using var workbook = new XLWorkbook(new MemoryStream(result));
        var worksheet = workbook.Worksheet("Academies");

        worksheet.Cell(4, 7).Value.ToString().Should().Be("Not inspected");
        worksheet.Cell(4, 8).Value.ToString().Should().Be(string.Empty);
        worksheet.Cell(4, 9).Value.ToString().Should().Be(string.Empty);
        worksheet.Cell(4, 10).Value.ToString().Should().Be("Not yet inspected");
        worksheet.Cell(4, 11).Value.ToString().Should().Be(string.Empty);
        worksheet.Cell(4, 12).Value.ToString().Should().Be(string.Empty);
    }

    [Fact]
    public async Task ExportAcademiesToSpreadsheet_ShouldHandleMissingPupilNumbersDataAsync()
    {
        var trustSummary = new TrustSummaryServiceModel("1", "Sample Trust", "Multi-academy trust", 1);
        var academyUrn = "123456";

        _mockTrustRepository.Setup(x => x.GetTrustSummaryAsync(trustSummary.Uid))
            .ReturnsAsync(new TrustSummary("Sample Trust", "Multi-academy trust"));
        _mockAcademyRepository.Setup(m => m.GetAcademiesInTrustDetailsAsync(trustSummary.Uid))
            .ReturnsAsync(new AcademyDetails[]
            {
                new(academyUrn, "Academy 1", "Type A", "Local Authority 1", "Urban")
            });
        _mockAcademyRepository.Setup(m => m.GetAcademiesInTrustPupilNumbersAsync(trustSummary.Uid))
            .ReturnsAsync(Array.Empty<AcademyPupilNumbers>());

        var result = await _sut.ExportAcademiesToSpreadsheetAsync(trustSummary.Uid);
        using var workbook = new XLWorkbook(new MemoryStream(result));
        var worksheet = workbook.Worksheet("Academies");

        worksheet.Cell(4, 15).Value.ToString().Should().Be(string.Empty);
        worksheet.Cell(4, 16).Value.ToString().Should().Be(string.Empty);
        worksheet.Cell(4, 17).Value.ToString().Should().Be(string.Empty);
    }

    [Fact]
    public async Task ExportAcademiesToSpreadsheet_ShouldHandleZeroPercentageFullAsync()
    {
        var trustSummary = new TrustSummaryServiceModel("1", "Sample Trust", "Multi-academy trust", 1);
        var academyUrn = "123456";

        _mockTrustRepository.Setup(x => x.GetTrustSummaryAsync(trustSummary.Uid))
            .ReturnsAsync(new TrustSummary("Sample Trust", "Multi-academy trust"));
        _mockAcademyRepository.Setup(m => m.GetAcademiesInTrustDetailsAsync(trustSummary.Uid))
            .ReturnsAsync(new AcademyDetails[]
            {
                new(academyUrn, "Academy 1", "Type A", "Local Authority 1", "Urban")
            });
        _mockAcademyRepository.Setup(m => m.GetAcademiesInTrustPupilNumbersAsync(trustSummary.Uid))
            .ReturnsAsync(new AcademyPupilNumbers[]
            {
                new(academyUrn, "Academy 1", "Primary", new AgeRange(5, 11), 0, 300)
            });

        var result = await _sut.ExportAcademiesToSpreadsheetAsync(trustSummary.Uid);
        using var workbook = new XLWorkbook(new MemoryStream(result));
        var worksheet = workbook.Worksheet("Academies");

        worksheet.Cell(4, 17).Value.ToString().Should().Be(string.Empty);
    }

    [Fact]
    public async Task ExportAcademiesToSpreadsheet_ShouldHandleMissingFreeSchoolMealsDataAsync()
    {
        var trustSummary = new TrustSummaryServiceModel("1", "Sample Trust", "Multi-academy trust", 1);
        var academyUrn = "123456";

        _mockTrustRepository.Setup(x => x.GetTrustSummaryAsync(trustSummary.Uid))
            .ReturnsAsync(new TrustSummary("Sample Trust", "Multi-academy trust"));
        _mockAcademyRepository.Setup(m => m.GetAcademiesInTrustDetailsAsync(trustSummary.Uid))
            .ReturnsAsync(new AcademyDetails[]
            {
                new(academyUrn, "Academy 1", "Type A", "Local Authority 1", "Urban")
            });
        _mockAcademyRepository.Setup(m => m.GetAcademiesInTrustFreeSchoolMealsAsync(trustSummary.Uid))
            .ReturnsAsync(Array.Empty<AcademyFreeSchoolMeals>());

        var result = await _sut.ExportAcademiesToSpreadsheetAsync(trustSummary.Uid);
        using var workbook = new XLWorkbook(new MemoryStream(result));
        var worksheet = workbook.Worksheet("Academies");

        worksheet.Cell(4, 18).Value.ToString().Should().Be(string.Empty);
    }

    [Fact]
    public void IsOfstedRatingBeforeOrAfterJoining_ShouldReturnAfterJoining_WhenInspectionDateIsEqualToJoiningDate()
    {
        var ofstedRatingScore = OfstedRatingScore.Good;
        var dateJoinedTrust = _mockDateTimeProvider.Object.Now;
        DateTime? inspectionEndDate = dateJoinedTrust;

        var result =
            ExportService.IsOfstedRatingBeforeOrAfterJoining(ofstedRatingScore, dateJoinedTrust, inspectionEndDate);

        result.Should().Be("After Joining");
    }

    [Fact]
    public async Task ExportOfstedDataToSpreadsheet_ShouldGenerateCorrectHeadersAsync()
    {
        _mockTrustRepository.Setup(x => x.GetTrustSummaryAsync("some-uid")).ReturnsAsync(
            new TrustSummary("Some Trust", "Multi-academy trust"));

        var result = await _sut.ExportOfstedDataToSpreadsheetAsync("some-uid");
        using var workbook = new XLWorkbook(new MemoryStream(result));
        var worksheet = workbook.Worksheet("Ofsted");

        // Verify headers on row 3
        Cell(worksheet, 3, OfstedColumns.SchoolName).Should().Be("School Name");
        Cell(worksheet, 3, OfstedColumns.DateJoined).Should().Be("Date Joined");
        Cell(worksheet, 3, OfstedColumns.CurrentSingleHeadlineGrade).Should().Be("Current single headline grade");
        Cell(worksheet, 3, OfstedColumns.CurrentBeforeAfterJoining).Should().Be("Before/After Joining");
        Cell(worksheet, 3, OfstedColumns.DateOfCurrentInspection).Should().Be("Date of Current Inspection");
        Cell(worksheet, 3, OfstedColumns.PreviousSingleHeadlineGrade).Should().Be("Previous single headline grade");
        Cell(worksheet, 3, OfstedColumns.PreviousBeforeAfterJoining).Should().Be("Before/After Joining");
        Cell(worksheet, 3, OfstedColumns.DateOfPreviousInspection).Should().Be("Date of previous inspection");
        Cell(worksheet, 3, OfstedColumns.CurrentQualityOfEducation).Should().Be("Quality of Education");
        Cell(worksheet, 3, OfstedColumns.CurrentBehaviourAndAttitudes).Should().Be("Behaviour and Attitudes");
        Cell(worksheet, 3, OfstedColumns.CurrentPersonalDevelopment).Should().Be("Personal Development");
        Cell(worksheet, 3, OfstedColumns.CurrentLeadershipAndManagement).Should().Be("Leadership and Management");
        Cell(worksheet, 3, OfstedColumns.CurrentEarlyYearsProvision).Should().Be("Early Years Provision");
        Cell(worksheet, 3, OfstedColumns.CurrentSixthFormProvision).Should().Be("Sixth Form Provision");
        Cell(worksheet, 3, OfstedColumns.PreviousQualityOfEducation).Should().Be("Previous Quality of Education");
        Cell(worksheet, 3, OfstedColumns.PreviousBehaviourAndAttitudes).Should().Be("Previous Behaviour and Attitudes");
        Cell(worksheet, 3, OfstedColumns.PreviousPersonalDevelopment).Should().Be("Previous Personal Development");
        Cell(worksheet, 3, OfstedColumns.PreviousLeadershipAndManagement).Should().Be("Previous Leadership and Management");
        Cell(worksheet, 3, OfstedColumns.PreviousEarlyYearsProvision).Should().Be("Previous Early Years Provision");
        Cell(worksheet, 3, OfstedColumns.PreviousSixthFormProvision).Should().Be("Previous Sixth Form Provision");
        Cell(worksheet, 3, OfstedColumns.EffectiveSafeguarding).Should().Be("Effective Safeguarding");
        Cell(worksheet, 3, OfstedColumns.CategoryOfConcern).Should().Be("Category of Concern");
    }

    [Fact]
    public async Task ExportOfstedDataToSpreadsheet_ShouldWriteTrustInformationAsync()
    {
        _mockTrustRepository.Setup(x => x.GetTrustSummaryAsync("uid")).ReturnsAsync(
            new TrustSummary("My Trust", "Multi-academy trust"));

        var result = await _sut.ExportOfstedDataToSpreadsheetAsync("uid");
        using var workbook = new XLWorkbook(new MemoryStream(result));
        var worksheet = workbook.Worksheet("Ofsted");

        worksheet.Cell(1, 1).Value.ToString().Should().Be("My Trust");
        worksheet.Cell(2, 1).Value.ToString().Should().Be("Multi-academy trust");
    }

    [Fact]
    public async Task ExportOfstedDataToSpreadsheet_ShouldHandleEmptyAcademiesAsync()
    {
        _mockTrustRepository.Setup(x => x.GetTrustSummaryAsync("uid")).ReturnsAsync(
            new TrustSummary("Empty Trust", "Multi-academy trust"));
        _mockAcademyRepository.Setup(m => m.GetAcademiesInTrustDetailsAsync("uid"))
            .ReturnsAsync(Array.Empty<AcademyDetails>());

        var result = await _sut.ExportOfstedDataToSpreadsheetAsync("uid");
        using var workbook = new XLWorkbook(new MemoryStream(result));
        var worksheet = workbook.Worksheet("Ofsted");

        var lastUsedRow = worksheet.LastRowUsed()?.RowNumber() ?? 0;
        lastUsedRow.Should().Be(3); // If no data rows are present, we expect the last used row to be the headers row (3)
    }

    [Fact]
    public async Task ExportOfstedDataToSpreadsheet_ShouldWriteDateCellsAsDatesAsync()
    {
        _mockTrustRepository.Setup(x => x.GetTrustSummaryAsync("uid"))
            .ReturnsAsync(new TrustSummary("Test Trust", "Multi-academy trust"));

        var joinedDate = new DateTime(2020, 1, 1);
        var currentInspectionDate = new DateTime(2021, 5, 20);
        var previousInspectionDate = new DateTime(2019, 12, 31);

        _mockAcademyRepository.Setup(m => m.GetAcademiesInTrustDetailsAsync("uid"))
            .ReturnsAsync(new AcademyDetails[]
            {
                new("A123", "Academy XYZ", "TypeX", "Local LA", "Urban")
            });

        _mockAcademyRepository.Setup(m => m.GetAcademiesInTrustOfstedAsync("uid"))
            .ReturnsAsync(new AcademyOfsted[]
            {
                new("A123", "Academy XYZ", joinedDate,
                    new OfstedRating((int)OfstedRatingScore.Good, previousInspectionDate),
                    new OfstedRating((int)OfstedRatingScore.Outstanding, currentInspectionDate))
            });

        var result = await _sut.ExportOfstedDataToSpreadsheetAsync("uid");
        using var workbook = new XLWorkbook(new MemoryStream(result));
        var worksheet = workbook.Worksheet("Ofsted");

        // Row with data is row 4
        worksheet.Cell(4, 1).Value.ToString().Should().Be("Academy XYZ");

        // Date Joined as date
        worksheet.Cell(4, 2).DataType.Should().Be(XLDataType.DateTime);
        worksheet.Cell(4, 2).GetValue<DateTime>().Should().Be(joinedDate);

        // Current Inspection Date as date
        worksheet.Cell(4, 5).DataType.Should().Be(XLDataType.DateTime);
        worksheet.Cell(4, 5).GetValue<DateTime>().Should().Be(currentInspectionDate);

        // Previous Inspection Date as date
        worksheet.Cell(4, 8).DataType.Should().Be(XLDataType.DateTime);
        worksheet.Cell(4, 8).GetValue<DateTime>().Should().Be(previousInspectionDate);
    }

    [Fact]
    public async Task ExportOfstedDataToSpreadsheet_ShouldHandleNullTrustSummaryAsync()
    {
        _mockTrustRepository.Setup(x => x.GetTrustSummaryAsync("uid"))
            .ReturnsAsync((TrustSummary?)null);

        var result = await _sut.ExportOfstedDataToSpreadsheetAsync("uid");
        using var workbook = new XLWorkbook(new MemoryStream(result));
        var worksheet = workbook.Worksheet("Ofsted");

        worksheet.Cell(1, 1).Value.ToString().Should().Be(string.Empty);
        worksheet.Cell(2, 1).Value.ToString().Should().Be(string.Empty);
    }

    [Fact]
    public async Task ExportOfstedDataToSpreadsheet_ShouldHandleNoOfstedDataAsync()
    {
        _mockTrustRepository.Setup(x => x.GetTrustSummaryAsync("uid"))
            .ReturnsAsync(new TrustSummary("Test Trust", "Multi-academy trust"));

        _mockAcademyRepository.Setup(m => m.GetAcademiesInTrustDetailsAsync("uid"))
            .ReturnsAsync([
                new AcademyDetails("A123", "Academy XYZ", "TypeX", "Local LA", "Urban")
            ]);

        _mockAcademyRepository.Setup(m => m.GetAcademiesInTrustOfstedAsync("uid")).ReturnsAsync([]);

        var result = await _sut.ExportOfstedDataToSpreadsheetAsync("uid");
        using var workbook = new XLWorkbook(new MemoryStream(result));
        var worksheet = workbook.Worksheet("Ofsted");

        using var scope = new AssertionScope();

        //Current inspection
        Cell(worksheet, 4, OfstedColumns.DateOfCurrentInspection).Should().Be(string.Empty);
        Cell(worksheet, 4, OfstedColumns.CurrentBeforeAfterJoining).Should().Be(string.Empty);
        Cell(worksheet, 4, OfstedColumns.CurrentQualityOfEducation).Should().Be("Not yet inspected");
        Cell(worksheet, 4, OfstedColumns.CurrentBehaviourAndAttitudes).Should().Be("Not yet inspected");
        Cell(worksheet, 4, OfstedColumns.CurrentPersonalDevelopment).Should().Be("Not yet inspected");
        Cell(worksheet, 4, OfstedColumns.CurrentLeadershipAndManagement).Should().Be("Not yet inspected");
        Cell(worksheet, 4, OfstedColumns.CurrentEarlyYearsProvision).Should().Be("Not yet inspected");
        Cell(worksheet, 4, OfstedColumns.CurrentSixthFormProvision).Should().Be("Not yet inspected");

        //Previous inspection
        Cell(worksheet, 4, OfstedColumns.DateOfPreviousInspection).Should().Be(string.Empty);
        Cell(worksheet, 4, OfstedColumns.PreviousBeforeAfterJoining).Should().Be(string.Empty);
        Cell(worksheet, 4, OfstedColumns.PreviousQualityOfEducation).Should().Be("Not inspected");
        Cell(worksheet, 4, OfstedColumns.PreviousBehaviourAndAttitudes).Should().Be("Not inspected");
        Cell(worksheet, 4, OfstedColumns.PreviousPersonalDevelopment).Should().Be("Not inspected");
        Cell(worksheet, 4, OfstedColumns.PreviousLeadershipAndManagement).Should().Be("Not inspected");
        Cell(worksheet, 4, OfstedColumns.PreviousEarlyYearsProvision).Should().Be("Not inspected");
        Cell(worksheet, 4, OfstedColumns.PreviousSixthFormProvision).Should().Be("Not inspected");

        //Safeguarding and concerns
        Cell(worksheet, 4, OfstedColumns.EffectiveSafeguarding).Should().Be("Not yet inspected");
        Cell(worksheet, 4, OfstedColumns.CategoryOfConcern).Should().Be("Not yet inspected");
    }

    [Fact]
    public async Task ExportPipelineAcademiesToSpreadsheet_ShouldGenerateCorrectHeadersAsync()
    {
        var trustSummary = new TrustSummaryServiceModel("1", "Sample Trust", "Multi-academy trust", 0);
        
        var result = await _sut.ExportPipelineAcademiesToSpreadsheetAsync(trustSummary.Uid);
        using var workbook = new XLWorkbook(new MemoryStream(result));
        var worksheet = workbook.Worksheet("Pipeline Academies");
        
        worksheet.Cell(3, 1).Value.ToString().Should().Be("School Name");
        worksheet.Cell(3, 2).Value.ToString().Should().Be("URN");
        worksheet.Cell(3, 3).Value.ToString().Should().Be("Age range");
        worksheet.Cell(3, 4).Value.ToString().Should().Be("Local authority");
        worksheet.Cell(3, 5).Value.ToString().Should().Be("Project type");
        worksheet.Cell(3, 6).Value.ToString().Should().Be("Provisional opening date");
    }

    [Fact]
    public async Task ExportPipelineAcademiesToSpreadsheet_ShouldCorrectlyExtractPipelineAcademyDataAsync()
    {
        var trustSummary = new TrustSummaryServiceModel("1", "Sample Trust", "Multi-academy trust", 1);
        var trustReferenceNumber = "TRN1111";
        var now = DateTime.Now;
        
        _mockTrustRepository.Setup(x => x.GetTrustSummaryAsync(trustSummary.Uid)).ReturnsAsync(
            new TrustSummary("Sample Trust", "Multi-academy trust"));

        _mockTrustRepository.Setup(m => m.GetTrustReferenceNumberAsync(trustSummary.Uid)).ReturnsAsync(
                "TRN1111"
            );

        _mockAcademyService.Setup(m => m.GetAcademiesPipelinePreAdvisoryAsync(trustReferenceNumber)).ReturnsAsync(
            new AcademyPipelineServiceModel[]
            {
                new(Urn: "1", EstablishmentName: "Academy 1", AgeRange: new AgeRange(4, 11),
                    LocalAuthority: "Local Authority 1", ProjectType: "Pre-advisory", ChangeDate: now),
            }
        );

        _mockAcademyService.Setup(m => m.GetAcademiesPipelinePostAdvisoryAsync(trustReferenceNumber)).ReturnsAsync(
            new AcademyPipelineServiceModel[]
            {
                new(Urn: "2", EstablishmentName: "Academy 2", AgeRange: new AgeRange(2, 11),
                    LocalAuthority: "Local Authority 2", ProjectType: "Post-advisory", ChangeDate: now),
            }
        );

        _mockAcademyService.Setup(m => m.GetAcademiesPipelineFreeSchoolsAsync(trustReferenceNumber)).ReturnsAsync(
            new AcademyPipelineServiceModel[]
            {
                new(Urn: "3", EstablishmentName: "Academy 3", AgeRange: new AgeRange(11, 18),
                    LocalAuthority: "Local Authority 3", ProjectType: "Free school", ChangeDate: now),
            }
        );

        var result = await _sut.ExportPipelineAcademiesToSpreadsheetAsync(trustSummary.Uid);
        using var workbook = new XLWorkbook(new MemoryStream(result));
        var worksheet = workbook.Worksheet("Pipeline Academies");
        
        // Pre-advisory
        worksheet.Cell(5, 1).Value.ToString().Should().Be("Academy 1");
        worksheet.Cell(5, 2).Value.ToString().Should().Be("1");
        worksheet.Cell(5, 3).Value.ToString().Should().Be("4 - 11");
        worksheet.Cell(5, 4).Value.ToString().Should().Be("Local Authority 1");
        worksheet.Cell(5, 5).Value.ToString().Should().Be("Pre-advisory");
        worksheet.Cell(5, 6).DataType.Should().Be(XLDataType.DateTime);
        worksheet.Cell(5, 6).GetValue<DateTime>().Should().BeCloseTo(now, TimeSpan.FromSeconds(1));
        
        // Post-advisory
        worksheet.Cell(8, 1).Value.ToString().Should().Be("Academy 2");
        worksheet.Cell(8, 2).Value.ToString().Should().Be("2");
        worksheet.Cell(8, 3).Value.ToString().Should().Be("2 - 11");
        worksheet.Cell(8, 4).Value.ToString().Should().Be("Local Authority 2");
        worksheet.Cell(8, 5).Value.ToString().Should().Be("Post-advisory");
        worksheet.Cell(8, 6).DataType.Should().Be(XLDataType.DateTime);
        worksheet.Cell(8, 6).GetValue<DateTime>().Should().BeCloseTo(now, TimeSpan.FromSeconds(1));
        
        // Free schools
        worksheet.Cell(11, 1).Value.ToString().Should().Be("Academy 3");
        worksheet.Cell(11, 2).Value.ToString().Should().Be("3");
        worksheet.Cell(11, 3).Value.ToString().Should().Be("11 - 18");
        worksheet.Cell(11, 4).Value.ToString().Should().Be("Local Authority 3");
        worksheet.Cell(11, 5).Value.ToString().Should().Be("Free school");
        worksheet.Cell(11, 6).DataType.Should().Be(XLDataType.DateTime);
             worksheet.Cell(11, 6).GetValue<DateTime>().Should().BeCloseTo(now, TimeSpan.FromSeconds(1));
         }

    [Fact]
    public async Task ExportPipelineAcademiesToSpreadsheet_ShouldCorrectlyHandleNullValuesAsync()
    {
        var trustSummary = new TrustSummaryServiceModel("1", "Sample Trust", "Multi-academy trust", 1);
        var trustReferenceNumber = "TRN1111";
        
        _mockTrustRepository.Setup(x => x.GetTrustSummaryAsync(trustSummary.Uid)).ReturnsAsync(
            new TrustSummary("Sample Trust", "Multi-academy trust"));

        _mockTrustRepository.Setup(m => m.GetTrustReferenceNumberAsync(trustSummary.Uid)).ReturnsAsync(
                "TRN1111"
            );

        _mockAcademyService.Setup(m => m.GetAcademiesPipelinePreAdvisoryAsync(trustReferenceNumber)).ReturnsAsync(
            new AcademyPipelineServiceModel[]
            {
                new(Urn: "1", EstablishmentName: null, AgeRange: null,
                    LocalAuthority: null, ProjectType: "Pre-advisory", ChangeDate: null),
            }
        );

        var result = await _sut.ExportPipelineAcademiesToSpreadsheetAsync(trustSummary.Uid);
        using var workbook = new XLWorkbook(new MemoryStream(result));
        var worksheet = workbook.Worksheet("Pipeline Academies");
        
        worksheet.Cell(5, 1).Value.ToString().Should().Be(string.Empty);
        worksheet.Cell(5, 2).Value.ToString().Should().Be("1");
        worksheet.Cell(5, 3).Value.ToString().Should().Be("Unconfirmed");
        worksheet.Cell(5, 4).Value.ToString().Should().Be(string.Empty);
        worksheet.Cell(5, 5).Value.ToString().Should().Be("Pre-advisory");
        worksheet.Cell(5, 6).Value.ToString().Should().Be("Unconfirmed");
    }

    [Fact]
    public async Task ExportPipelineAcademiesToSpreadsheet_ShouldWriteTrustInformationAsync()
    {
        _mockTrustRepository.Setup(x => x.GetTrustSummaryAsync("uid")).ReturnsAsync(
            new TrustSummary("My Trust", "Multi-academy trust"));

        var result = await _sut.ExportPipelineAcademiesToSpreadsheetAsync("uid");
        using var workbook = new XLWorkbook(new MemoryStream(result));
        var worksheet = workbook.Worksheet("Pipeline Academies");

        worksheet.Cell(1, 1).Value.ToString().Should().Be("My Trust");
        worksheet.Cell(2, 1).Value.ToString().Should().Be("Multi-academy trust");
    }
    
    [Fact]
    public async Task ExportPipelineAcademiesToSpreadsheet_ShouldHandleNullTrustSummaryAsync()
    {
        _mockTrustRepository.Setup(x => x.GetTrustSummaryAsync("uid"))
            .ReturnsAsync((TrustSummary?)null);

        var result = await _sut.ExportPipelineAcademiesToSpreadsheetAsync("uid");
        using var workbook = new XLWorkbook(new MemoryStream(result));
        var worksheet = workbook.Worksheet("Pipeline Academies");

        worksheet.Cell(1, 1).Value.ToString().Should().Be(string.Empty);
        worksheet.Cell(2, 1).Value.ToString().Should().Be(string.Empty);
    }

    [Fact]
    public async Task ExportPipelineAcademiesToSpreadsheet_ShouldWritePipelineSectionHeadingsAsync()
    {
        var trustSummary = new TrustSummaryServiceModel("1", "Sample Trust", "Multi-academy trust", 1);
        var trustReferenceNumber = "TRN1111";
        var now = DateTime.Now;
        
        _mockTrustRepository.Setup(x => x.GetTrustSummaryAsync(trustSummary.Uid)).ReturnsAsync(
            new TrustSummary("Sample Trust", "Multi-academy trust"));

        _mockTrustRepository.Setup(m => m.GetTrustReferenceNumberAsync(trustSummary.Uid)).ReturnsAsync(
            "TRN1111"
        );

        _mockAcademyService.Setup(m => m.GetAcademiesPipelinePreAdvisoryAsync(trustReferenceNumber)).ReturnsAsync(
            new AcademyPipelineServiceModel[]
            {
                new(Urn: "1", EstablishmentName: "Academy 1", AgeRange: new AgeRange(4, 11),
                    LocalAuthority: "Local Authority 1", ProjectType: "Pre-advisory", ChangeDate: now),
            }
        );

        _mockAcademyService.Setup(m => m.GetAcademiesPipelinePostAdvisoryAsync(trustReferenceNumber)).ReturnsAsync(
            new AcademyPipelineServiceModel[]
            {
                new(Urn: "2", EstablishmentName: "Academy 2", AgeRange: new AgeRange(2, 11),
                    LocalAuthority: "Local Authority 2", ProjectType: "Post-advisory", ChangeDate: now),
            }
        );

        _mockAcademyService.Setup(m => m.GetAcademiesPipelineFreeSchoolsAsync(trustReferenceNumber)).ReturnsAsync(
            new AcademyPipelineServiceModel[]
            {
                new(Urn: "3", EstablishmentName: "Academy 3", AgeRange: new AgeRange(11, 18),
                    LocalAuthority: "Local Authority 3", ProjectType: "Free school", ChangeDate: now),
            }
        );

        var result = await _sut.ExportPipelineAcademiesToSpreadsheetAsync(trustSummary.Uid);
        using var workbook = new XLWorkbook(new MemoryStream(result));
        var worksheet = workbook.Worksheet("Pipeline Academies");
        
        worksheet.Cell(4, 1).Value.ToString().Should().Be("Pre-advisory academies");
        worksheet.Cell(7, 1).Value.ToString().Should().Be("Post-advisory academies");
        worksheet.Cell(10, 1).Value.ToString().Should().Be("Free schools");
    }

    private static string Cell(IXLWorksheet worksheet, int rowNumber, OfstedColumns column)
    {
        return worksheet.Cell(rowNumber, (int)column).Value.ToString();
    }

    private enum OfstedColumns
    {
        SchoolName = 1,
        DateJoined = 2,
        CurrentSingleHeadlineGrade = 3,
        CurrentBeforeAfterJoining = 4,
        DateOfCurrentInspection = 5,
        PreviousSingleHeadlineGrade = 6,
        PreviousBeforeAfterJoining = 7,
        DateOfPreviousInspection = 8,
        CurrentQualityOfEducation = 9,
        CurrentBehaviourAndAttitudes = 10,
        CurrentPersonalDevelopment = 11,
        CurrentLeadershipAndManagement = 12,
        CurrentEarlyYearsProvision = 13,
        CurrentSixthFormProvision = 14,
        PreviousQualityOfEducation = 15,
        PreviousBehaviourAndAttitudes = 16,
        PreviousPersonalDevelopment = 17,
        PreviousLeadershipAndManagement = 18,
        PreviousEarlyYearsProvision = 19,
        PreviousSixthFormProvision = 20,
        EffectiveSafeguarding = 21,
        CategoryOfConcern = 22
    }
}
