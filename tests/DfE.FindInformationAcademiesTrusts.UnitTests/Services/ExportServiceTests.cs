using ClosedXML.Excel;
using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.Academy;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.Trust;
using DfE.FindInformationAcademiesTrusts.Services.Academy;
using DfE.FindInformationAcademiesTrusts.Services.Export;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using FluentAssertions.Execution;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Services;

public class ExportServiceTests
{
    private readonly IDateTimeProvider _mockDateTimeProvider;
    private readonly IAcademyRepository _mockAcademyRepository;
    private readonly ITrustRepository _mockTrustRepository;
    private readonly IAcademyService _mockAcademyService;
    private readonly ExportService _sut;

    public ExportServiceTests()
    {
        _mockDateTimeProvider = Substitute.For<IDateTimeProvider>();
        _mockAcademyRepository = Substitute.For<IAcademyRepository>();
        _mockTrustRepository = Substitute.For<ITrustRepository>();
        _mockAcademyService = Substitute.For<IAcademyService>();

        _mockDateTimeProvider.Now.Returns(DateTime.Now);

        _sut = new ExportService(_mockAcademyRepository, _mockTrustRepository,
            _mockAcademyService);
    }

    [Fact]
    public async Task ExportAcademiesToSpreadsheet_ShouldGenerateCorrectHeadersAsync()
    {
        var trustSummary = new TrustSummaryServiceModel("1", "Sample Trust", "Multi-academy trust", 0);

        var result = await _sut.ExportAcademiesToSpreadsheetAsync(trustSummary.Uid);
        using var workbook = new XLWorkbook(new MemoryStream(result));
        var worksheet = workbook.Worksheet("Academies");

        AssertSpreadsheetMatches(worksheet, 3,
        [
            "School Name", "URN", "Local authority", "Type", "Rural or Urban", "Date joined", "Current Ofsted Rating",
            "Before/After Joining", "Date of Current Ofsted", "Previous Ofsted Rating", "Before/After Joining",
            "Date of Previous Ofsted", "Phase of Education",
            "Age range", "Pupil Numbers",
            "Capacity", "% Full", "Pupils eligible for Free School Meals",
            "LA average pupils eligible for Free School Meals", "National average pupils eligible for Free School Meals"
        ]);
    }

    [Fact]
    public async Task ExportAcademiesToSpreadsheet_ShouldCorrectlyExtractAcademyDataAsync()
    {
        var trustSummary = new TrustSummaryServiceModel("1", "Sample Trust", "Multi-academy trust", 1);

        _mockTrustRepository.GetTrustSummaryAsync(trustSummary.Uid)!.Returns(
            Task.FromResult(new TrustSummary("Sample Trust", "Multi-academy trust")));

        var now = DateTime.Now;
        _mockAcademyRepository.GetAcademiesInTrustDetailsAsync(trustSummary.Uid).Returns(
            Task.FromResult(new AcademyDetails[]
            {
                new("123456", "Academy 1", "Type A", "Local Authority 1", "Urban")
            }));
        _mockAcademyRepository.GetAcademiesInTrustOfstedAsync(trustSummary.Uid).Returns(
            Task.FromResult(new AcademyOfsted[]
            {
                new("123456", "Academy 1", now, new OfstedRating(-1, null), new OfstedRating(1, now))
            }));
        _mockAcademyRepository.GetAcademiesInTrustPupilNumbersAsync(trustSummary.Uid).Returns(
            Task.FromResult(new AcademyPupilNumbers[]
            {
                new("123456", "Academy 1", "Primary", new AgeRange(5, 11), 500, 600)
            }));
        _mockAcademyRepository.GetAcademiesInTrustFreeSchoolMealsAsync(trustSummary.Uid).Returns(
            Task.FromResult(new AcademyFreeSchoolMeals[]
            {
                new("123456", "Academy 1", 20, 1, "Type A", "Primary")
            }));
        _mockAcademyService.GetAcademiesInTrustFreeSchoolMealsAsync(trustSummary.Uid).Returns(
            Task.FromResult(new AcademyFreeSchoolMealsServiceModel[]
            {
                new("123456", "Academy 1", 20, 25, 15)
            })
        );

        var result = await _sut.ExportAcademiesToSpreadsheetAsync(trustSummary.Uid);
        using var workbook = new XLWorkbook(new MemoryStream(result));
        var worksheet = workbook.Worksheet("Academies");

        AcademyCell(worksheet, 4, AcademyColumns.SchoolName).Should().Be("Academy 1");
        AcademyCell(worksheet, 4, AcademyColumns.Urn).Should().Be("123456");
        AcademyCell(worksheet, 4, AcademyColumns.LocalAuthority).Should().Be("Local Authority 1");
        AcademyCell(worksheet, 4, AcademyColumns.Type).Should().Be("Type A");
        AcademyCell(worksheet, 4, AcademyColumns.RuralOrUrban).Should().Be("Urban");

        // Check date joined is set as a date
        worksheet.Cell(4, 6).DataType.Should().Be(XLDataType.DateTime);
        worksheet.Cell(4, 6).GetValue<DateTime>().Should().BeCloseTo(now, TimeSpan.FromSeconds(1));
        
        AcademyCell(worksheet, 4, AcademyColumns.CurrentOfstedRating).Should().Be("Outstanding");
        AcademyCell(worksheet, 4, AcademyColumns.CurrentBeforeAfterJoining).Should().Be("After Joining");
        // Current Ofsted date also as a date
        worksheet.Cell(4, 9).DataType.Should().Be(XLDataType.DateTime);
        worksheet.Cell(4, 9).GetValue<DateTime>().Should().BeCloseTo(now, TimeSpan.FromSeconds(1));
        
        
        AcademyCell(worksheet, 4, AcademyColumns.PreviousOfstedRating).Should().Be("Not inspected");
        AcademyCell(worksheet, 4, AcademyColumns.PreviousBeforeAfterJoining).Should().Be(string.Empty);
        AcademyCell(worksheet, 4, AcademyColumns.DateOfPreviousInspection).Should().Be(string.Empty);
        
        AcademyCell(worksheet, 4, AcademyColumns.PhaseOfEducation).Should().Be("Primary");
        AcademyCell(worksheet, 4, AcademyColumns.AgeRange).Should().Be("5 - 11");
        AcademyCell(worksheet, 4, AcademyColumns.PupilNumbers).Should().Be("500");
        AcademyCell(worksheet, 4, AcademyColumns.Capacity).Should().Be("600");
        AcademyCell(worksheet, 4, AcademyColumns.PercentFull).Should().Be("83%");
        AcademyCell(worksheet, 4, AcademyColumns.PupilsEligibleFreeSchoolMeals).Should().Be("20%");
        AcademyCell(worksheet, 4, AcademyColumns.LaPupilsEligibleFreeSchoolMeals).Should().Be("25%");
        AcademyCell(worksheet, 4, AcademyColumns.NationalPupilsEligibleFreeSchoolMeals).Should().Be("15%");
    }

    [Fact]
    public async Task ExportAcademiesToSpreadsheet_ShouldHandleEmptyAcademiesAsync()
    {
        var trustSummary = new TrustSummaryServiceModel("1", "Empty Trust", "Multi-academy trust", 0);

        var result = await _sut.ExportAcademiesToSpreadsheetAsync(trustSummary.Uid);
        using var workbook = new XLWorkbook(new MemoryStream(result));
        var worksheet = workbook.Worksheet("Academies");

        var lastUsedRow = worksheet.LastRowUsed()?.RowNumber() ?? 0;
        lastUsedRow.Should()
            .Be(3); // If no data rows are present, we expect the last used row to be the headers row (3)
    }

    [Fact]
    public async Task ExportAcademiesToSpreadsheet_ShouldCorrectlyHandleNullValuesAsync()
    {
        var trustSummary = new TrustSummaryServiceModel("1", "Sample Trust", "Multi-academy trust", 1);

        _mockTrustRepository.GetTrustSummaryAsync(trustSummary.Uid)!.Returns(
            Task.FromResult(new TrustSummary("Sample Trust", "Multi-academy trust")));

        _mockAcademyRepository.GetAcademiesInTrustDetailsAsync(trustSummary.Uid).Returns(
            Task.FromResult(new AcademyDetails[]
            {
                new("123456", null, null, null, null)
            }));

        var now = DateTime.Now;
        _mockAcademyRepository.GetAcademiesInTrustOfstedAsync(trustSummary.Uid).Returns(
            Task.FromResult(new AcademyOfsted[]
            {
                new("123456", null, now, new OfstedRating(-1, null), new OfstedRating(-1, null))
            }));

        _mockAcademyRepository.GetAcademiesInTrustPupilNumbersAsync(trustSummary.Uid).Returns(
            Task.FromResult(new AcademyPupilNumbers[]
            {
                new("123456", null, null, new AgeRange(5, 11), null, null)
            }));

        _mockAcademyRepository.GetAcademiesInTrustFreeSchoolMealsAsync(trustSummary.Uid).Returns(
            Task.FromResult(new AcademyFreeSchoolMeals[]
            {
                new("123456", null, null, 1, null, null)
            }));

        _mockAcademyService.GetAcademiesInTrustFreeSchoolMealsAsync(trustSummary.Uid).Returns(
            Task.FromResult(new AcademyFreeSchoolMealsServiceModel[]
            {
                new("123456", null, null, 25, 35)
            }));

        var result = await _sut.ExportAcademiesToSpreadsheetAsync(trustSummary.Uid);
        using var workbook = new XLWorkbook(new MemoryStream(result));
        var worksheet = workbook.Worksheet("Academies");

        AcademyCell(worksheet, 4, AcademyColumns.SchoolName).Should().Be(string.Empty);
        AcademyCell(worksheet, 4, AcademyColumns.Urn).Should().Be("123456");
        AcademyCell(worksheet, 4, AcademyColumns.LocalAuthority).Should().Be(string.Empty);
        AcademyCell(worksheet, 4, AcademyColumns.Type).Should().Be(string.Empty);
        AcademyCell(worksheet, 4, AcademyColumns.RuralOrUrban).Should().Be(string.Empty);
        
        worksheet.Cell(4, 6).GetValue<DateTime>().Should().BeCloseTo(now, TimeSpan.FromSeconds(1));
        
        AcademyCell(worksheet, 4, AcademyColumns.CurrentOfstedRating).Should().Be("Not yet inspected");
        AcademyCell(worksheet, 4, AcademyColumns.CurrentBeforeAfterJoining).Should().Be(string.Empty);
        AcademyCell(worksheet, 4, AcademyColumns.DateOfCurrentInspection).Should().Be(string.Empty);
        
        AcademyCell(worksheet, 4, AcademyColumns.PreviousOfstedRating).Should().Be("Not inspected");
        AcademyCell(worksheet, 4, AcademyColumns.PreviousBeforeAfterJoining).Should().Be(string.Empty);
        AcademyCell(worksheet, 4, AcademyColumns.DateOfPreviousInspection).Should().Be(string.Empty);
        
        AcademyCell(worksheet, 4, AcademyColumns.PhaseOfEducation).Should().Be(string.Empty);
        AcademyCell(worksheet, 4, AcademyColumns.AgeRange).Should().Be("5 - 11");
        AcademyCell(worksheet, 4, AcademyColumns.PupilNumbers).Should().Be(string.Empty);
        AcademyCell(worksheet, 4, AcademyColumns.Capacity).Should().Be(string.Empty);
        AcademyCell(worksheet, 4, AcademyColumns.PercentFull).Should().Be(string.Empty);
        AcademyCell(worksheet, 4, AcademyColumns.PupilsEligibleFreeSchoolMeals).Should().Be(string.Empty);
        AcademyCell(worksheet, 4, AcademyColumns.LaPupilsEligibleFreeSchoolMeals).Should().Be("25%");
        AcademyCell(worksheet, 4, AcademyColumns.NationalPupilsEligibleFreeSchoolMeals).Should().Be("35%");
    }

    [Fact]
    public void IsOfstedRatingBeforeOrAfterJoining_ShouldReturnEmptyString_WhenOfstedRatingScoreIsNone()
    {
        var ofstedRatingScore = OfstedRatingScore.NotInspected;
        var dateJoinedTrust = _mockDateTimeProvider.Now;
        DateTime? inspectionEndDate = dateJoinedTrust.AddDays(-1);

        var result =
            ExportService.IsOfstedRatingBeforeOrAfterJoining(ofstedRatingScore, dateJoinedTrust, inspectionEndDate);

        result.Should().Be(string.Empty);
    }

    [Fact]
    public void IsOfstedRatingBeforeOrAfterJoining_ShouldReturnBeforeJoining_WhenInspectionDateIsBeforeJoiningDate()
    {
        var ofstedRatingScore = OfstedRatingScore.Good;
        var dateJoinedTrust = _mockDateTimeProvider.Now;
        DateTime? inspectionEndDate = dateJoinedTrust.AddDays(-10);

        var result =
            ExportService.IsOfstedRatingBeforeOrAfterJoining(ofstedRatingScore, dateJoinedTrust, inspectionEndDate);

        result.Should().Be("Before Joining");
    }

    [Fact]
    public void IsOfstedRatingBeforeOrAfterJoining_ShouldReturnAfterJoining_WhenInspectionDateIsAfterJoiningDate()
    {
        var ofstedRatingScore = OfstedRatingScore.Good;
        var dateJoinedTrust = _mockDateTimeProvider.Now.AddDays(-10);
        DateTime? inspectionEndDate = dateJoinedTrust.AddDays(5);

        var result =
            ExportService.IsOfstedRatingBeforeOrAfterJoining(ofstedRatingScore, dateJoinedTrust, inspectionEndDate);

        result.Should().Be("After Joining");
    }

    [Fact]
    public void IsOfstedRatingBeforeOrAfterJoining_ShouldReturnEmptyString_WhenInspectionDateIsNull()
    {
        var ofstedRatingScore = OfstedRatingScore.Good;
        var dateJoinedTrust = _mockDateTimeProvider.Now;

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
        _mockTrustRepository.GetTrustSummaryAsync(uid).Returns(Task.FromResult<TrustSummary?>(null));

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

        _mockTrustRepository.GetTrustSummaryAsync(trustSummary.Uid)!.Returns(Task.FromResult(new TrustSummary("Sample Trust",
            "Multi-academy trust")));
        _mockAcademyRepository.GetAcademiesInTrustDetailsAsync(trustSummary.Uid).Returns(Task.FromResult(new AcademyDetails[]
        {
            new(academyUrn, "Academy 1", "Type A", "Local Authority 1", "Urban")
        }));
        _mockAcademyRepository.GetAcademiesInTrustOfstedAsync(trustSummary.Uid).Returns(Task.FromResult(Array.Empty<AcademyOfsted>()));

        var result = await _sut.ExportAcademiesToSpreadsheetAsync(trustSummary.Uid);
        using var workbook = new XLWorkbook(new MemoryStream(result));
        var worksheet = workbook.Worksheet("Academies");
        
        AcademyCell(worksheet, 4, AcademyColumns.CurrentOfstedRating).Should().Be("Not yet inspected");
        AcademyCell(worksheet, 4, AcademyColumns.CurrentBeforeAfterJoining).Should().Be(string.Empty);
        AcademyCell(worksheet, 4, AcademyColumns.DateOfCurrentInspection).Should().Be(string.Empty);
        AcademyCell(worksheet, 4, AcademyColumns.PreviousOfstedRating).Should().Be("Not inspected");
        AcademyCell(worksheet, 4, AcademyColumns.PreviousBeforeAfterJoining).Should().Be(string.Empty);
        AcademyCell(worksheet, 4, AcademyColumns.DateOfPreviousInspection).Should().Be(string.Empty);
    }

    [Fact]
    public async Task ExportAcademiesToSpreadsheet_ShouldHandleMissingPupilNumbersDataAsync()
    {
        var trustSummary = new TrustSummaryServiceModel("1", "Sample Trust", "Multi-academy trust", 1);
        var academyUrn = "123456";

        _mockTrustRepository.GetTrustSummaryAsync(trustSummary.Uid)!.Returns(Task.FromResult(new TrustSummary(
            "Sample Trust",
            "Multi-academy trust")));
        _mockAcademyRepository.GetAcademiesInTrustDetailsAsync(trustSummary.Uid).Returns(Task.FromResult(new AcademyDetails[]
        {
            new(academyUrn, "Academy 1", "Type A", "Local Authority 1", "Urban")
        }));
        _mockAcademyRepository.GetAcademiesInTrustPupilNumbersAsync(trustSummary.Uid).Returns(
            Task.FromResult(Array.Empty<AcademyPupilNumbers>()));

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

        _mockTrustRepository.GetTrustSummaryAsync(trustSummary.Uid)!.Returns(Task.FromResult(new TrustSummary("Sample Trust",
            "Multi-academy trust")));
        _mockAcademyRepository.GetAcademiesInTrustDetailsAsync(trustSummary.Uid).Returns(Task.FromResult(new AcademyDetails[]
        {
            new(academyUrn, "Academy 1", "Type A", "Local Authority 1", "Urban")
        }));
        _mockAcademyRepository.GetAcademiesInTrustPupilNumbersAsync(trustSummary.Uid).Returns(Task.FromResult(new AcademyPupilNumbers[]
        {
            new(academyUrn, "Academy 1", "Primary", new AgeRange(5, 11), 0, 300)
        }));

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

        _mockTrustRepository.GetTrustSummaryAsync(trustSummary.Uid)!.Returns(Task.FromResult(new TrustSummary("Sample Trust",
            "Multi-academy trust")));
        _mockAcademyRepository.GetAcademiesInTrustDetailsAsync(trustSummary.Uid).Returns(Task.FromResult(new AcademyDetails[]
        {
            new(academyUrn, "Academy 1", "Type A", "Local Authority 1", "Urban")
        }));
        _mockAcademyRepository.GetAcademiesInTrustFreeSchoolMealsAsync(trustSummary.Uid).Returns(
            Task.FromResult(Array.Empty<AcademyFreeSchoolMeals>()));

        var result = await _sut.ExportAcademiesToSpreadsheetAsync(trustSummary.Uid);
        using var workbook = new XLWorkbook(new MemoryStream(result));
        var worksheet = workbook.Worksheet("Academies");

        worksheet.Cell(4, 18).Value.ToString().Should().Be(string.Empty);
    }

    [Fact]
    public async Task ExportAcademiesToSpreadsheet_ShouldHandleMissingFreeSchoolMealsAveragesAsync()
    {
        var trustSummary = new TrustSummaryServiceModel("1", "Sample Trust", "Multi-academy trust", 1);
        var academyUrn = "123456";
        _mockTrustRepository.GetTrustSummaryAsync(trustSummary.Uid)!.Returns(Task.FromResult(new TrustSummary("Sample Trust",
            "Multi-academy trust")));
        _mockAcademyRepository.GetAcademiesInTrustDetailsAsync(trustSummary.Uid).Returns(Task.FromResult(new AcademyDetails[]
        {
            new(academyUrn, "Academy 1", "Type A", "Local Authority 1", "Urban")
        }));
        _mockAcademyService.GetAcademiesInTrustFreeSchoolMealsAsync(trustSummary.Uid).Returns(
            Task.FromResult(Array.Empty<AcademyFreeSchoolMealsServiceModel>()));
        
        var result = await _sut.ExportAcademiesToSpreadsheetAsync(trustSummary.Uid);
        using var workbook = new XLWorkbook(new MemoryStream(result));
        var worksheet = workbook.Worksheet("Academies");

        AcademyCell(worksheet, 4, AcademyColumns.LaPupilsEligibleFreeSchoolMeals).Should().Be(string.Empty);
        AcademyCell(worksheet, 4, AcademyColumns.NationalPupilsEligibleFreeSchoolMeals).Should().Be(string.Empty);
    }

    [Fact]
    public void IsOfstedRatingBeforeOrAfterJoining_ShouldReturnAfterJoining_WhenInspectionDateIsEqualToJoiningDate()
    {
        var ofstedRatingScore = OfstedRatingScore.Good;
        var dateJoinedTrust = _mockDateTimeProvider.Now;
        DateTime? inspectionEndDate = dateJoinedTrust;

        var result =
            ExportService.IsOfstedRatingBeforeOrAfterJoining(ofstedRatingScore, dateJoinedTrust, inspectionEndDate);

        result.Should().Be("After Joining");
    }

    [Fact]
    public async Task ExportOfstedDataToSpreadsheet_ShouldGenerateCorrectHeadersAsync()
    {
        _mockTrustRepository.GetTrustSummaryAsync("some-uid")!.Returns(
            Task.FromResult(new TrustSummary("Some Trust", "Multi-academy trust")));

        var result = await _sut.ExportOfstedDataToSpreadsheetAsync("some-uid");
        using var workbook = new XLWorkbook(new MemoryStream(result));
        var worksheet = workbook.Worksheet("Ofsted");

        // Verify headers on row 3
        AssertSpreadsheetMatches(worksheet, 3,
        [
            "School Name", "Date joined", "Current single headline grade", "Before/After Joining",
            "Date of Current Inspection", "Previous single headline grade", "Before/After Joining",
            "Date of previous inspection", "Quality of Education", "Behaviour and Attitudes", "Personal Development",
            "Leadership and Management", "Early Years Provision", "Sixth Form Provision",
            "Previous Quality of Education", "Previous Behaviour and Attitudes", "Previous Personal Development",
            "Previous Leadership and Management", "Previous Early Years Provision", "Previous Sixth Form Provision",
            "Effective Safeguarding", "Category of Concern"
        ]);
    }

    [Fact]
    public async Task ExportOfstedDataToSpreadsheet_ShouldWriteTrustInformationAsync()
    {
        _mockTrustRepository.GetTrustSummaryAsync("uid")!.Returns(
            Task.FromResult(new TrustSummary("My Trust", "Multi-academy trust")));

        var result = await _sut.ExportOfstedDataToSpreadsheetAsync("uid");
        using var workbook = new XLWorkbook(new MemoryStream(result));
        var worksheet = workbook.Worksheet("Ofsted");

        worksheet.Cell(1, 1).Value.ToString().Should().Be("My Trust");
        worksheet.Cell(2, 1).Value.ToString().Should().Be("Multi-academy trust");
    }

    [Fact]
    public async Task ExportOfstedDataToSpreadsheet_ShouldHandleEmptyAcademiesAsync()
    {
        _mockTrustRepository.GetTrustSummaryAsync("uid")!.Returns(
            Task.FromResult(new TrustSummary("Empty Trust", "Multi-academy trust")));
        _mockAcademyRepository.GetAcademiesInTrustDetailsAsync("uid")
            .Returns(Task.FromResult(Array.Empty<AcademyDetails>()));

        var result = await _sut.ExportOfstedDataToSpreadsheetAsync("uid");
        using var workbook = new XLWorkbook(new MemoryStream(result));
        var worksheet = workbook.Worksheet("Ofsted");

        var lastUsedRow = worksheet.LastRowUsed()?.RowNumber() ?? 0;
        lastUsedRow.Should()
            .Be(3); // If no data rows are present, we expect the last used row to be the headers row (3)
    }

    [Fact]
    public async Task ExportOfstedDataToSpreadsheet_ShouldWriteDateCellsAsDatesAsync()
    {
        _mockTrustRepository.GetTrustSummaryAsync("uid")!
            .Returns(Task.FromResult(new TrustSummary("Test Trust", "Multi-academy trust")));

        var joinedDate = new DateTime(2020, 1, 1);
        var currentInspectionDate = new DateTime(2021, 5, 20);
        var previousInspectionDate = new DateTime(2019, 12, 31);

        _mockAcademyRepository.GetAcademiesInTrustDetailsAsync("uid")
            .Returns(Task.FromResult(new AcademyDetails[]
            {
                new("A123", "Academy XYZ", "TypeX", "Local LA", "Urban")
            }));

        _mockAcademyRepository.GetAcademiesInTrustOfstedAsync("uid")
            .Returns(Task.FromResult(new AcademyOfsted[]
            {
                new("A123", "Academy XYZ", joinedDate,
                    new OfstedRating((int)OfstedRatingScore.Good, previousInspectionDate),
                    new OfstedRating((int)OfstedRatingScore.Outstanding, currentInspectionDate))
            }));

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
        _mockTrustRepository.GetTrustSummaryAsync("uid").Returns(Task.FromResult<TrustSummary?>(null));

        var result = await _sut.ExportOfstedDataToSpreadsheetAsync("uid");
        using var workbook = new XLWorkbook(new MemoryStream(result));
        var worksheet = workbook.Worksheet("Ofsted");

        worksheet.Cell(1, 1).Value.ToString().Should().Be(string.Empty);
        worksheet.Cell(2, 1).Value.ToString().Should().Be(string.Empty);
    }

    [Fact]
    public async Task ExportOfstedDataToSpreadsheet_ShouldHandleNoOfstedDataAsync()
    {
        _mockTrustRepository.GetTrustSummaryAsync("uid")!.Returns(
            Task.FromResult(new TrustSummary("Test Trust", "Multi-academy trust")));

        _mockAcademyRepository.GetAcademiesInTrustDetailsAsync("uid").Returns(Task.FromResult<AcademyDetails[]>(
        [
            new AcademyDetails("A123", "Academy XYZ", "TypeX", "Local LA", "Urban")
        ]));

        _mockAcademyRepository.GetAcademiesInTrustOfstedAsync("uid").Returns(Task.FromResult<AcademyOfsted[]>([]));

        var result = await _sut.ExportOfstedDataToSpreadsheetAsync("uid");
        using var workbook = new XLWorkbook(new MemoryStream(result));
        var worksheet = workbook.Worksheet("Ofsted");

        using var scope = new AssertionScope();

        //Current inspection
        OfstedCell(worksheet, 4, OfstedColumns.DateOfCurrentInspection).Should().Be(string.Empty);
        OfstedCell(worksheet, 4, OfstedColumns.CurrentBeforeAfterJoining).Should().Be(string.Empty);
        OfstedCell(worksheet, 4, OfstedColumns.CurrentQualityOfEducation).Should().Be("Not yet inspected");
        OfstedCell(worksheet, 4, OfstedColumns.CurrentBehaviourAndAttitudes).Should().Be("Not yet inspected");
        OfstedCell(worksheet, 4, OfstedColumns.CurrentPersonalDevelopment).Should().Be("Not yet inspected");
        OfstedCell(worksheet, 4, OfstedColumns.CurrentLeadershipAndManagement).Should().Be("Not yet inspected");
        OfstedCell(worksheet, 4, OfstedColumns.CurrentEarlyYearsProvision).Should().Be("Not yet inspected");
        OfstedCell(worksheet, 4, OfstedColumns.CurrentSixthFormProvision).Should().Be("Not yet inspected");

        //Previous inspection
        OfstedCell(worksheet, 4, OfstedColumns.DateOfPreviousInspection).Should().Be(string.Empty);
        OfstedCell(worksheet, 4, OfstedColumns.PreviousBeforeAfterJoining).Should().Be(string.Empty);
        OfstedCell(worksheet, 4, OfstedColumns.PreviousQualityOfEducation).Should().Be("Not inspected");
        OfstedCell(worksheet, 4, OfstedColumns.PreviousBehaviourAndAttitudes).Should().Be("Not inspected");
        OfstedCell(worksheet, 4, OfstedColumns.PreviousPersonalDevelopment).Should().Be("Not inspected");
        OfstedCell(worksheet, 4, OfstedColumns.PreviousLeadershipAndManagement).Should().Be("Not inspected");
        OfstedCell(worksheet, 4, OfstedColumns.PreviousEarlyYearsProvision).Should().Be("Not inspected");
        OfstedCell(worksheet, 4, OfstedColumns.PreviousSixthFormProvision).Should().Be("Not inspected");

        //Safeguarding and concerns
        OfstedCell(worksheet, 4, OfstedColumns.EffectiveSafeguarding).Should().Be("Not yet inspected");
        OfstedCell(worksheet, 4, OfstedColumns.CategoryOfConcern).Should().Be("Not yet inspected");
    }

    [Fact]
    public async Task ExportPipelineAcademiesToSpreadsheet_ShouldCorrectlyExtractPipelineAcademyDataAsync()
    {
        const string uid = "1";
        const string trustReferenceNumber = "TRN1111";

        _mockTrustRepository.GetTrustSummaryAsync(uid)!.Returns(Task.FromResult(new TrustSummary("Sample Trust", "Multi-academy trust")));
        _mockTrustRepository.GetTrustReferenceNumberAsync(uid).Returns(Task.FromResult(trustReferenceNumber));

        _mockAcademyService.GetAcademiesPipelinePreAdvisoryAsync(trustReferenceNumber)
            .Returns(Task.FromResult<AcademyPipelineServiceModel[]>(
            [
                new AcademyPipelineServiceModel("1", "Academy 1", new AgeRange(4, 11), "Local Authority 1",
                    "Pre-advisory", new DateTime(2025, 2, 19))
            ]));
        _mockAcademyService.GetAcademiesPipelinePostAdvisoryAsync(trustReferenceNumber)
            .Returns(Task.FromResult<AcademyPipelineServiceModel[]>(
            [
                new AcademyPipelineServiceModel("2", "Academy 2", new AgeRange(2, 11), "Local Authority 2",
                    "Post-advisory", new DateTime(2026, 2, 20))
            ]));
        _mockAcademyService.GetAcademiesPipelineFreeSchoolsAsync(trustReferenceNumber)
            .Returns(Task.FromResult<AcademyPipelineServiceModel[]>(
            [
                new AcademyPipelineServiceModel("3", "Academy 3", new AgeRange(11, 18), "Local Authority 3",
                    "Free school", new DateTime(2025, 6, 21))
            ]));

        var result = await _sut.ExportPipelineAcademiesToSpreadsheetAsync(uid);

        using var workbook = new XLWorkbook(new MemoryStream(result));
        var worksheet = workbook.Worksheet("Pipeline Academies");

        AssertSpreadsheetMatches(worksheet, 1,
            ["Sample Trust"],
            ["Multi-academy trust"],
            [],
            ["Pre-advisory academies"],
            [
                "School Name", "URN", "Age range", "Local authority", "Project type",
                "Proposed conversion or transfer date"
            ],
            ["Academy 1", "1", "4 - 11", "Local Authority 1", "Pre-advisory", new DateTime(2025, 2, 19)],
            [],
            ["Post-advisory academies"],
            [
                "School Name", "URN", "Age range", "Local authority", "Project type",
                "Proposed conversion or transfer date"
            ],
            ["Academy 2", "2", "2 - 11", "Local Authority 2", "Post-advisory", new DateTime(2026, 2, 20)],
            [],
            ["Free schools"],
            ["School Name", "URN", "Age range", "Local authority", "Project type", "Provisional opening date"],
            ["Academy 3", "3", "11 - 18", "Local Authority 3", "Free school", new DateTime(2025, 6, 21)]
        );
    }

    [Fact]
    public async Task ExportPipelineAcademiesToSpreadsheetAsync_ShouldArrangeAcademiesByNameAlphabetically()
    {
        const string uid = "1";
        const string trustReferenceNumber = "TRN1111";

        _mockTrustRepository.GetTrustSummaryAsync(uid)!.Returns(Task.FromResult(new TrustSummary("Sample Trust", "Multi-academy trust")));
        _mockTrustRepository.GetTrustReferenceNumberAsync(uid).Returns(Task.FromResult(trustReferenceNumber));

        _mockAcademyService.GetAcademiesPipelinePreAdvisoryAsync(trustReferenceNumber)
            .Returns(Task.FromResult<AcademyPipelineServiceModel[]>(
            [
                new AcademyPipelineServiceModel("1", "z Academy", new AgeRange(4, 10), "Local Authority 1",
                    "Pre-advisory", new DateTime(2023, 2, 1)),
                new AcademyPipelineServiceModel("2", "B Academy", new AgeRange(5, 11), "Local Authority 2",
                    "Pre-advisory", new DateTime(2024, 3, 2)),
                new AcademyPipelineServiceModel("3", "S Academy 1", new AgeRange(6, 12), "Local Authority 3",
                    "Pre-advisory", new DateTime(2025, 4, 3)),
                new AcademyPipelineServiceModel("4", "S Academy 2", new AgeRange(7, 13), "Local Authority 4",
                    "Pre-advisory", new DateTime(2026, 5, 4))
            ]));
        _mockAcademyService.GetAcademiesPipelinePostAdvisoryAsync(trustReferenceNumber)
            .Returns(Task.FromResult<AcademyPipelineServiceModel[]>(
            [
                new AcademyPipelineServiceModel("5", "zz Academy", new AgeRange(4, 10), "Local Authority 1",
                    "Post-advisory", new DateTime(2023, 2, 1)),
                new AcademyPipelineServiceModel("6", "Bb Academy 2", new AgeRange(5, 11), "Local Authority 2",
                    "Post-advisory", new DateTime(2024, 3, 2)),
                new AcademyPipelineServiceModel("7", "Bb Academy 1", new AgeRange(6, 12), "Local Authority 3",
                    "Post-advisory", new DateTime(2025, 4, 3)),
                new AcademyPipelineServiceModel("8", "Ss Academy", new AgeRange(7, 13), "Local Authority 4",
                    "Post-advisory", new DateTime(2026, 5, 4))
            ]));
        _mockAcademyService.GetAcademiesPipelineFreeSchoolsAsync(trustReferenceNumber)
            .Returns(Task.FromResult<AcademyPipelineServiceModel[]>(
            [
                new AcademyPipelineServiceModel("9", "Aaa Academy", new AgeRange(4, 10), "Local Authority 1",
                    "Free school", new DateTime(2023, 2, 1)),
                new AcademyPipelineServiceModel("10", "Zzz Academy", new AgeRange(5, 11), "Local Authority 2",
                    "Free school", new DateTime(2024, 3, 2)),
                new AcademyPipelineServiceModel("11", "Xxx Academy", new AgeRange(6, 12), "Local Authority 3",
                    "Free school", new DateTime(2025, 4, 3)),
                new AcademyPipelineServiceModel("12", "Fff Academy", new AgeRange(7, 13), "Local Authority 4",
                    "Free school", new DateTime(2026, 5, 4))
            ]));

        var result = await _sut.ExportPipelineAcademiesToSpreadsheetAsync(uid);

        using var workbook = new XLWorkbook(new MemoryStream(result));
        var worksheet = workbook.Worksheet("Pipeline Academies");

        AssertSpreadsheetMatches(worksheet, 1,
            ["Sample Trust"],
            ["Multi-academy trust"],
            [],
            ["Pre-advisory academies"],
            [
                "School Name", "URN", "Age range", "Local authority", "Project type",
                "Proposed conversion or transfer date"
            ],
            ["B Academy", "2", "5 - 11", "Local Authority 2", "Pre-advisory", new DateTime(2024, 3, 2)],
            ["S Academy 1", "3", "6 - 12", "Local Authority 3", "Pre-advisory", new DateTime(2025, 4, 3)],
            ["S Academy 2", "4", "7 - 13", "Local Authority 4", "Pre-advisory", new DateTime(2026, 5, 4)],
            ["z Academy", "1", "4 - 10", "Local Authority 1", "Pre-advisory", new DateTime(2023, 2, 1)],
            [],
            ["Post-advisory academies"],
            [
                "School Name", "URN", "Age range", "Local authority", "Project type",
                "Proposed conversion or transfer date"
            ],
            ["Bb Academy 1", "7", "6 - 12", "Local Authority 3", "Post-advisory", new DateTime(2025, 4, 3)],
            ["Bb Academy 2", "6", "5 - 11", "Local Authority 2", "Post-advisory", new DateTime(2024, 3, 2)],
            ["Ss Academy", "8", "7 - 13", "Local Authority 4", "Post-advisory", new DateTime(2026, 5, 4)],
            ["zz Academy", "5", "4 - 10", "Local Authority 1", "Post-advisory", new DateTime(2023, 2, 1)],
            [],
            ["Free schools"],
            ["School Name", "URN", "Age range", "Local authority", "Project type", "Provisional opening date"],
            ["Aaa Academy", "9", "4 - 10", "Local Authority 1", "Free school", new DateTime(2023, 2, 1)],
            ["Fff Academy", "12", "7 - 13", "Local Authority 4", "Free school", new DateTime(2026, 5, 4)],
            ["Xxx Academy", "11", "6 - 12", "Local Authority 3", "Free school", new DateTime(2025, 4, 3)],
            ["Zzz Academy", "10", "5 - 11", "Local Authority 2", "Free school", new DateTime(2024, 3, 2)]
        );
    }

    /// <summary>
    /// Asserts that the given strings are present in the expected places in the spreadsheet.
    /// Does not look at any cells other than the ones specified.
    /// </summary>
    private static void AssertSpreadsheetMatches(IXLWorksheet worksheet, int startingRow,
        params object[][] expectedValues)
    {
        for (var rowNumber = 0; rowNumber < expectedValues.Length; rowNumber++)
        {
            for (var columnNumber = 0; columnNumber < expectedValues[rowNumber].Length; columnNumber++)
            {
                var actualCell = worksheet.Cell(rowNumber + startingRow, columnNumber + 1); //the worksheet is 1-indexed

                switch (expectedValues[rowNumber][columnNumber])
                {
                    case DateTime expectedCellValue:
                        actualCell.DataType.Should().Be(XLDataType.DateTime);
                        actualCell.GetValue<DateTime>().Should().Be(expectedCellValue);
                        break;

                    case string expectedCellValue:
                        actualCell.Value.ToString().Should().Be(expectedCellValue);
                        break;

                    default: throw new ArgumentOutOfRangeException(nameof(expectedValues));
                }
            }
        }
    }

    [Fact]
    public async Task ExportPipelineAcademiesToSpreadsheet_ShouldCorrectlyHandleNullValuesAsync()
    {
        var trustSummary = new TrustSummaryServiceModel("1", "Sample Trust", "Multi-academy trust", 1);
        var trustReferenceNumber = "TRN1111";

        _mockTrustRepository.GetTrustSummaryAsync(trustSummary.Uid)!.Returns(
            Task.FromResult(new TrustSummary("Sample Trust", "Multi-academy trust")));

        _mockTrustRepository.GetTrustReferenceNumberAsync(trustSummary.Uid).Returns(
            Task.FromResult("TRN1111")
        );

        _mockAcademyService.GetAcademiesPipelinePreAdvisoryAsync(trustReferenceNumber).Returns(
            Task.FromResult(new AcademyPipelineServiceModel[]
                {
                    new("1", null, null,
                        null, "Pre-advisory", null)
                }
            ));

        var result = await _sut.ExportPipelineAcademiesToSpreadsheetAsync(trustSummary.Uid);
        using var workbook = new XLWorkbook(new MemoryStream(result));
        var worksheet = workbook.Worksheet("Pipeline Academies");

        AssertSpreadsheetMatches(worksheet, 1,
            [trustSummary.Name],
            [trustSummary.Type],
            [],
            ["Pre-advisory academies"],
            [
                "School Name", "URN", "Age range", "Local authority", "Project type",
                "Proposed conversion or transfer date"
            ],
            [string.Empty, "1", "Unconfirmed", string.Empty, "Pre-advisory", "Unconfirmed"],
            [],
            ["Post-advisory academies"],
            [
                "School Name", "URN", "Age range", "Local authority", "Project type",
                "Proposed conversion or transfer date"
            ],
            [],
            ["Free schools"],
            ["School Name", "URN", "Age range", "Local authority", "Project type", "Provisional opening date"]
        );
    }

    [Fact]
    public async Task ExportPipelineAcademiesToSpreadsheet_ShouldWriteTrustInformationAsync()
    {
        _mockTrustRepository.GetTrustSummaryAsync("uid")!.Returns(
            Task.FromResult(new TrustSummary("My Trust", "Multi-academy trust")));

        var result = await _sut.ExportPipelineAcademiesToSpreadsheetAsync("uid");
        using var workbook = new XLWorkbook(new MemoryStream(result));
        var worksheet = workbook.Worksheet("Pipeline Academies");

        worksheet.Cell(1, 1).Value.ToString().Should().Be("My Trust");
        worksheet.Cell(2, 1).Value.ToString().Should().Be("Multi-academy trust");
    }

    [Fact]
    public async Task ExportPipelineAcademiesToSpreadsheet_ShouldHandleNullTrustSummaryAsync()
    {
        _mockTrustRepository.GetTrustSummaryAsync("uid").Returns(Task.FromResult<TrustSummary?>(null));

        var result = await _sut.ExportPipelineAcademiesToSpreadsheetAsync("uid");
        using var workbook = new XLWorkbook(new MemoryStream(result));
        var worksheet = workbook.Worksheet("Pipeline Academies");

        worksheet.Cell(1, 1).Value.ToString().Should().Be(string.Empty);
        worksheet.Cell(2, 1).Value.ToString().Should().Be(string.Empty);
    }

    private static string OfstedCell(IXLWorksheet worksheet, int rowNumber, OfstedColumns column)
    {
        return worksheet.Cell(rowNumber, (int)column).Value.ToString();
    }

    private static string AcademyCell(IXLWorksheet worksheet, int rowNumber, AcademyColumns column)
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

    private enum AcademyColumns
    {
        SchoolName = 1,
        Urn = 2,
        LocalAuthority = 3,
        Type = 4,
        RuralOrUrban = 5,
        DateJoined = 6,
        CurrentOfstedRating = 7,
        CurrentBeforeAfterJoining = 8,
        DateOfCurrentInspection = 9,
        PreviousOfstedRating = 10,
        PreviousBeforeAfterJoining = 11,
        DateOfPreviousInspection = 12,
        PhaseOfEducation = 13,
        AgeRange = 14,
        PupilNumbers = 15,
        Capacity = 16,
        PercentFull = 17,
        PupilsEligibleFreeSchoolMeals = 18,
        LaPupilsEligibleFreeSchoolMeals = 19,
        NationalPupilsEligibleFreeSchoolMeals = 20,
    }
}
