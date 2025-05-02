﻿using ClosedXML.Excel;
using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Exceptions;
using DfE.FindInformationAcademiesTrusts.Services.Academy;
using DfE.FindInformationAcademiesTrusts.Services.Export;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using static DfE.FindInformationAcademiesTrusts.Services.Export.ExportColumns;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Services.ExportServices
{

    public class OfstedDataExportServiceTests
    {
        private readonly string trustUid = "1";
        private readonly ITrustService _mockTrustService;
        private readonly IAcademyService _mockAcademyService;

        private readonly OfstedDataExportService _sut;
        private readonly TrustSummaryServiceModel trustSummary;

        public OfstedDataExportServiceTests()
        {
            _mockTrustService = Substitute.For<ITrustService>();
            _mockAcademyService = Substitute.For<IAcademyService>();

            trustSummary = new TrustSummaryServiceModel("1", "Sample Trust", "Multi-academy trust", 0);

            _mockTrustService.GetTrustSummaryAsync(trustUid).Returns(trustSummary);

            _sut = new OfstedDataExportService(_mockAcademyService, _mockTrustService);
        }

        [Fact]
        public async Task ExportOfstedDataToSpreadsheet_ShouldGenerateCorrectHeadersAsync()
        {
            var result = await _sut.BuildAsync(trustUid);
            using var workbook = new XLWorkbook(new MemoryStream(result));
            var worksheet = workbook.Worksheet("Ofsted");

            // Verify headers on row 3
            worksheet.AssertSpreadsheetMatches(3,
            [
                "School Name", "Date joined", "Current single headline grade", "Before/After Joining",
                "Date of Current Inspection", "Previous single headline grade", "Before/After Joining",
                "Date of previous inspection", "Quality of Education", "Behaviour and Attitudes",
                "Personal Development",
                "Leadership and Management", "Early Years Provision", "Sixth Form Provision",
                "Previous Quality of Education", "Previous Behaviour and Attitudes", "Previous Personal Development",
                "Previous Leadership and Management", "Previous Early Years Provision", "Previous Sixth Form Provision",
                "Effective Safeguarding", "Category of Concern"
            ]);
        }

        [Fact]
        public async Task IfTrustDetailsDontExist_ShouldThrow()
        {
            string unknownTrustId = "Unknown";

            var result = async () => { await _sut.BuildAsync(unknownTrustId); };

            await result.Should().ThrowAsync<DataIntegrityException>().WithMessage($"Trust summary not found for UID {unknownTrustId}");
        }

        [Fact]
        public async Task ExportOfstedDataToSpreadsheet_ShouldWriteTrustInformationAsync()
        {
            var result = await _sut.BuildAsync(trustUid);
            using var workbook = new XLWorkbook(new MemoryStream(result));
            var worksheet = workbook.Worksheet("Ofsted");

            worksheet.CellValue(1, 1).Should().Be(trustSummary.Name);
            worksheet.CellValue(2, 1).Should().Be(trustSummary.Type);
        }

        [Fact]
        public async Task ExportOfstedDataToSpreadsheet_ShouldHandleEmptyAcademiesAsync()
        {
            _mockAcademyService.GetAcademiesInTrustDetailsAsync(trustUid).Returns([]);

            var result = await _sut.BuildAsync(trustUid);
            using var workbook = new XLWorkbook(new MemoryStream(result));
            var worksheet = workbook.Worksheet("Ofsted");

            var lastUsedRow = worksheet.LastRowUsed()?.RowNumber() ?? 0;
            lastUsedRow.Should()
                .Be(3); // If no data rows are present, we expect the last used row to be the headers row (3)
        }

        [Fact]
        public async Task ExportOfstedDataToSpreadsheet_ShouldCorrectlyExtractDataAsync()
        {
            var joinedDate = new DateTime(2020, 1, 1);
            var currentInspectionDate = new DateTime(2021, 5, 20);
            var previousInspectionDate = new DateTime(2019, 12, 31);

            _mockAcademyService.GetAcademiesInTrustDetailsAsync(trustUid)
                .Returns([new("A123", "Academy XYZ", "TypeX", "Local LA", "Urban", DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-10)))]);

            _mockAcademyService.GetAcademiesInTrustOfstedAsync(trustUid)
                .Returns([
                    new("A123", "Academy XYZ", joinedDate,
                        new OfstedRating((int)OfstedRatingScore.Good, previousInspectionDate),
                        new OfstedRating((int)OfstedRatingScore.Outstanding, currentInspectionDate))
                ]);

            var result = await _sut.BuildAsync(trustUid);
            using var workbook = new XLWorkbook(new MemoryStream(result));
            var worksheet = workbook.Worksheet("Ofsted");

            // Row with data is row 4
            worksheet.CellValue(4, OfstedColumns.SchoolName).Should().Be("Academy XYZ");
            
            // Date Joined as date
            worksheet.Cell(4, OfstedColumns.DateJoined).DataType.Should().Be(XLDataType.DateTime);
            worksheet.Cell(4, OfstedColumns.DateJoined).GetValue<DateTime>().Should().Be(joinedDate);

            //Current Single headline grade
            worksheet.CellValue(4, OfstedColumns.CurrentSingleHeadlineGrade).Should().Be("Outstanding");
            worksheet.CellValue(4, OfstedColumns.CurrentBeforeAfterJoining).Should().Be("After joining");

            // Current Inspection Date as date
            worksheet.Cell(4, OfstedColumns.DateOfCurrentInspection).DataType.Should().Be(XLDataType.DateTime);
            worksheet.Cell(4, OfstedColumns.DateOfCurrentInspection).GetValue<DateTime>().Should().Be(currentInspectionDate);

            //Current Single headline grade
            worksheet.CellValue(4, OfstedColumns.PreviousSingleHeadlineGrade).Should().Be("Good");
            worksheet.CellValue(4, OfstedColumns.PreviousBeforeAfterJoining).Should().Be("Before joining");

            // Previous Inspection Date as date
            worksheet.Cell(4, OfstedColumns.DateOfPreviousInspection).DataType.Should().Be(XLDataType.DateTime);
            worksheet.Cell(4, OfstedColumns.DateOfPreviousInspection).GetValue<DateTime>().Should().Be(previousInspectionDate);

            var expectedCurrentRow = _sut.TrustRows + _sut.HeaderRows + 1;

            _sut.CurrentRow.Should().Be(expectedCurrentRow);
        }
        
        [Fact]
        public async Task ExportOfstedDataToSpreadsheet_ShouldHandleNoOfstedDataAsync()
        {
            _mockAcademyService.GetAcademiesInTrustDetailsAsync(trustUid).Returns([new ("A123", "Academy XYZ", "Local LA", "TypeX", "Urban", DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-10)))]);

            _mockAcademyService.GetAcademiesInTrustOfstedAsync(trustUid).Returns([]);

            var result = await _sut.BuildAsync(trustUid);
            using var workbook = new XLWorkbook(new MemoryStream(result));
            var worksheet = workbook.Worksheet("Ofsted");


            //Current inspection
            worksheet.CellValue(4, OfstedColumns.DateOfCurrentInspection).Should().Be(string.Empty);
            worksheet.CellValue(4, OfstedColumns.CurrentBeforeAfterJoining).Should().Be(string.Empty);
            worksheet.CellValue(4, OfstedColumns.CurrentQualityOfEducation).Should().Be("Not yet inspected");
            worksheet.CellValue(4, OfstedColumns.CurrentBehaviourAndAttitudes).Should().Be("Not yet inspected");
            worksheet.CellValue(4, OfstedColumns.CurrentPersonalDevelopment).Should().Be("Not yet inspected");
            worksheet.CellValue(4, OfstedColumns.CurrentLeadershipAndManagement).Should().Be("Not yet inspected");
            worksheet.CellValue(4, OfstedColumns.CurrentEarlyYearsProvision).Should().Be("Not yet inspected");
            worksheet.CellValue(4, OfstedColumns.CurrentSixthFormProvision).Should().Be("Not yet inspected");

            //Previous inspection
            worksheet.CellValue(4, OfstedColumns.DateOfPreviousInspection).Should().Be(string.Empty);
            worksheet.CellValue(4, OfstedColumns.PreviousBeforeAfterJoining).Should().Be(string.Empty);
            worksheet.CellValue(4, OfstedColumns.PreviousQualityOfEducation).Should().Be("Not inspected");
            worksheet.CellValue(4, OfstedColumns.PreviousBehaviourAndAttitudes).Should().Be("Not inspected");
            worksheet.CellValue(4, OfstedColumns.PreviousPersonalDevelopment).Should().Be("Not inspected");
            worksheet.CellValue(4, OfstedColumns.PreviousLeadershipAndManagement).Should().Be("Not inspected");
            worksheet.CellValue(4, OfstedColumns.PreviousEarlyYearsProvision).Should().Be("Not inspected");
            worksheet.CellValue(4, OfstedColumns.PreviousSixthFormProvision).Should().Be("Not inspected");

            //Safeguarding and concerns
            worksheet.CellValue(4, OfstedColumns.EffectiveSafeguarding).Should().Be("Not yet inspected");
            worksheet.CellValue(4, OfstedColumns.CategoryOfConcern).Should().Be("Not yet inspected");
        }

        [Fact]
        public async Task ExportOfstedDataToSpreadsheet_ShouldCorrectlyHandleNullValuesAsync()
        {
            var joinedDate = new DateTime(2020, 1, 1);
            var currentInspectionDate = new DateTime(2021, 5, 20);
            var previousInspectionDate = new DateTime(2019, 12, 31);

            _mockAcademyService.GetAcademiesInTrustDetailsAsync(trustUid)
                .Returns([new("A123", null, "TypeX", "Local LA", "Urban", DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-10)))]);

            _mockAcademyService.GetAcademiesInTrustOfstedAsync(trustUid)
                .Returns([
                    new("A123", "Academy XYZ", joinedDate,
                        new OfstedRating((int)OfstedRatingScore.Good, previousInspectionDate),
                        new OfstedRating((int)OfstedRatingScore.Outstanding, currentInspectionDate))
                ]);

            var result = await _sut.BuildAsync(trustUid);
            using var workbook = new XLWorkbook(new MemoryStream(result));
            var worksheet = workbook.Worksheet("Ofsted");

            worksheet.CellValue(4, OfstedColumns.SchoolName).Should().Be(string.Empty);

        }
    }
}
