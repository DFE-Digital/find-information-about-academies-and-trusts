using ClosedXML.Excel;
using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.Academy;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.Trust;
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

        _mockDateTimeProvider.Setup(m => m.Now).Returns(DateTime.Now);

        _sut = new ExportService(_mockAcademyRepository.Object, _mockTrustRepository.Object);
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
        worksheet.Cell(4, 6).DataType.Should().Be(ClosedXML.Excel.XLDataType.DateTime);
        worksheet.Cell(4, 6).GetValue<DateTime>().Should().BeCloseTo(now, TimeSpan.FromSeconds(1));

        worksheet.Cell(4, 7).Value.ToString().Should().Be("Not yet inspected");
        worksheet.Cell(4, 8).Value.ToString().Should().Be(string.Empty);
        worksheet.Cell(4, 9).Value.ToString().Should().Be(string.Empty);
        worksheet.Cell(4, 10).Value.ToString().Should().Be("Outstanding");
        worksheet.Cell(4, 11).Value.ToString().Should().Be("After Joining");

        // Current Ofsted date also as a date
        worksheet.Cell(4, 12).DataType.Should().Be(ClosedXML.Excel.XLDataType.DateTime);
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

        worksheet.Cell(4, 7).Value.ToString().Should().Be("Not yet inspected");
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

        worksheet.Cell(4, 7).Value.ToString().Should().Be("Not yet inspected");
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
        worksheet.Cell(3, 1).Value.ToString().Should().Be("School Name");
        worksheet.Cell(3, 2).Value.ToString().Should().Be("Date Joined");
        worksheet.Cell(3, 3).Value.ToString().Should().Be("Date of Current Inspection");
        worksheet.Cell(3, 4).Value.ToString().Should().Be("Before/After Joining");
        worksheet.Cell(3, 5).Value.ToString().Should().Be("Quality of Education");
        worksheet.Cell(3, 6).Value.ToString().Should().Be("Behaviour and Attitudes");
        worksheet.Cell(3, 7).Value.ToString().Should().Be("Personal Development");
        worksheet.Cell(3, 8).Value.ToString().Should().Be("Leadership and Management");
        worksheet.Cell(3, 9).Value.ToString().Should().Be("Early Years Provision");
        worksheet.Cell(3, 10).Value.ToString().Should().Be("Sixth Form Provision");
        worksheet.Cell(3, 11).Value.ToString().Should().Be("Date of Previous Inspection");
        worksheet.Cell(3, 12).Value.ToString().Should().Be("Before/After Joining");
        worksheet.Cell(3, 13).Value.ToString().Should().Be("Previous Quality of Education");
        worksheet.Cell(3, 14).Value.ToString().Should().Be("Previous Behaviour and Attitudes");
        worksheet.Cell(3, 15).Value.ToString().Should().Be("Previous Personal Development");
        worksheet.Cell(3, 16).Value.ToString().Should().Be("Previous Leadership and Management");
        worksheet.Cell(3, 17).Value.ToString().Should().Be("Previous Early Years Provision");
        worksheet.Cell(3, 18).Value.ToString().Should().Be("Previous Sixth Form Provision");
        worksheet.Cell(3, 19).Value.ToString().Should().Be("Effective Safeguarding");
        worksheet.Cell(3, 20).Value.ToString().Should().Be("Category of Concern");
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
        worksheet.Cell(4, 3).DataType.Should().Be(XLDataType.DateTime);
        worksheet.Cell(4, 3).GetValue<DateTime>().Should().Be(currentInspectionDate);

        // Previous Inspection Date as date
        worksheet.Cell(4, 11).DataType.Should().Be(XLDataType.DateTime);
        worksheet.Cell(4, 11).GetValue<DateTime>().Should().Be(previousInspectionDate);
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
            .ReturnsAsync(new AcademyDetails[]
            {
                new("A123", "Academy XYZ", "TypeX", "Local LA", "Urban")
            });

        _mockAcademyRepository.Setup(m => m.GetAcademiesInTrustOfstedAsync("uid"))
            .ReturnsAsync(Array.Empty<AcademyOfsted>());

        var result = await _sut.ExportOfstedDataToSpreadsheetAsync("uid");
        using var workbook = new XLWorkbook(new MemoryStream(result));
        var worksheet = workbook.Worksheet("Ofsted");

        // If no Ofsted data, we get default "Not yet inspected"
        worksheet.Cell(4, 5).Value.ToString().Should().Be("Not yet inspected");
        worksheet.Cell(4, 3).Value.ToString().Should().Be(string.Empty); // date of current inspection empty
    }
}
