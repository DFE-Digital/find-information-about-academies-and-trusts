using ClosedXML.Excel;
using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Services.Export;
using DfE.FindInformationAcademiesTrusts.Services.Trust;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Services
{
    public class ExportServiceTests
    {
        private readonly ExportService _sut = new();

        [Fact]
        public void ExportAcademiesToSpreadsheetUsingProvider_ShouldGenerateCorrectHeaders()
        {
            // Arrange
            var trust = new Trust("1",
                                  "Sample Trust",
                                  "1001",
                                  "12345",
                                  "Multi-academy trust",
                                  "Address",
                                  DateTime.Now,
                                  "123456",
                                  "Region",
                                  [],
                                  [],
                                  null,
                                  null,
                                  "Open");
            var trustSummary = new TrustSummaryServiceModel("1", "Sample Trust", "Multi-academy trust", 0);

            // Act
            var result = _sut.ExportAcademiesToSpreadsheetUsingProvider(trust, trustSummary);
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
            worksheet.Cell(3, 8).Value.ToString().Should().Be("Date of Previous Ofsted");
            worksheet.Cell(3, 9).Value.ToString().Should().Be("Before/After Joining");
            worksheet.Cell(3, 10).Value.ToString().Should().Be("Current Ofsted Rating");
            worksheet.Cell(3, 11).Value.ToString().Should().Be("Date of Current Ofsted");
            worksheet.Cell(3, 12).Value.ToString().Should().Be("Before/After Joining");
            worksheet.Cell(3, 13).Value.ToString().Should().Be("Phase of Education");
            worksheet.Cell(3, 14).Value.ToString().Should().Be("Pupil Numbers");
            worksheet.Cell(3, 15).Value.ToString().Should().Be("Capacity");
            worksheet.Cell(3, 16).Value.ToString().Should().Be("% Full");
            worksheet.Cell(3, 17).Value.ToString().Should().Be("Pupils eligible for Free School Meals");
        }

        [Fact]
        public void ExportAcademiesToSpreadsheetUsingProvider_ShouldCorrectlyExtractAcademyData()
        {
            // Arrange
            var academy = new Academy(
                123456,
                DateTime.Now,
                "Academy 1",
                "Type A",
                "Local Authority 1",
                "Urban",
                "Primary",
                500,
                600,
                20,
                new AgeRange(5, 11),
                new OfstedRating(OfstedRatingScore.Outstanding, DateTime.Now),
                OfstedRating.None,
                100);
            var trust = new Trust(
                "1",
                "Sample Trust",
                "1001",
                "12345",
                "Multi-academy trust",
                "Address",
                DateTime.Now,
                "123456",
                "Region",
                [academy],
                [],
                null,
                null,
                "Open");
            var trustSummary = new TrustSummaryServiceModel("1", "Sample Trust", "Multi-academy trust", 1);

            // Act
            var result = _sut.ExportAcademiesToSpreadsheetUsingProvider(trust, trustSummary);
            using var workbook = new XLWorkbook(new MemoryStream(result));
            var worksheet = workbook.Worksheet("Academies");

            // Assert
            worksheet.Cell(4, 1).Value.ToString().Should().Be("Academy 1");
            worksheet.Cell(4, 2).Value.ToString().Should().Be("123456");
            worksheet.Cell(4, 3).Value.ToString().Should().Be("Local Authority 1");
            worksheet.Cell(4, 4).Value.ToString().Should().Be("Type A");
            worksheet.Cell(4, 5).Value.ToString().Should().Be("Urban");
            worksheet.Cell(4, 6).Value.ToString().Should().Be(DateTime.Now.ToString("yyyy-MM-dd"));
            worksheet.Cell(4, 7).Value.ToString().Should().Be("None");
            worksheet.Cell(4, 8).Value.ToString().Should().Be(string.Empty);
            worksheet.Cell(4, 9).Value.ToString().Should().Be("After Joining");
            worksheet.Cell(4, 10).Value.ToString().Should().Be("Outstanding");
            worksheet.Cell(4, 11).Value.ToString().Should().Be(DateTime.Now.ToString("yyyy-MM-dd"));
            worksheet.Cell(4, 12).Value.ToString().Should().Be("After Joining");
            worksheet.Cell(4, 13).Value.ToString().Should().Be("Primary");
            worksheet.Cell(4, 14).Value.ToString().Should().Be("500");
            worksheet.Cell(4, 15).Value.ToString().Should().Be("600");
            worksheet.Cell(4, 16).Value.ToString().Should().Be("83%");
            worksheet.Cell(4, 17).Value.ToString().Should().Be("20%");
        }

        [Fact]
        public void ExportAcademiesToSpreadsheetUsingProvider_ShouldHandleEmptyAcademies()
        {
            // Arrange
            var trust = new Trust("1",
                                  "Empty Trust",
                                  "1001",
                                  "12345",
                                  "Multi-academy trust",
                                  "Address",
                                  DateTime.Now,
                                  "123456",
                                  "Region",
                                  [],
                                  [],
                                  null,
                                  null,
                                  "Open");
            var trustSummary = new TrustSummaryServiceModel("1", "Empty Trust", "Multi-academy trust", 0);

            // Act
            var result = _sut.ExportAcademiesToSpreadsheetUsingProvider(trust, trustSummary);
            using var workbook = new XLWorkbook(new MemoryStream(result));
            var worksheet = workbook.Worksheet("Academies");

            // Assert
            worksheet.LastRowUsed().RowNumber().Should().Be(3); // Last row should be headers as there is no data for the next row
        }

        [Fact]
        public void ExportAcademiesToSpreadsheetUsingProvider_ShouldCorrectlyHandleNullValues()
        {
            // Arrange
            var academy = new Academy(
                123456,
                DateTime.Now,
                null, // EstablishmentName 
                null, // TypeOfEstablishment 
                null, // LocalAuthority
                null, // UrbanRural 
                null, // PhaseOfEducation
                null, // NumberOfPupils 
                null, // SchoolCapacity 
                null, // PercentageFreeSchoolMeals
                new AgeRange(5, 11), // Sample age range
                OfstedRating.None, // CurrentOfstedRating 
                OfstedRating.None, // PreviousOfstedRating 
                100);
            var trust = new Trust(
                "1",
                "Sample Trust",
                "1001",
                "12345",
                "Multi-academy trust",
                "Address",
                DateTime.Now,
                "123456",
                "Region",
                [academy],
                [],
                null,
                null,
                "Open");
            var trustSummary = new TrustSummaryServiceModel("1", "Sample Trust", "Multi-academy trust", 1);

            // Act
            var result = _sut.ExportAcademiesToSpreadsheetUsingProvider(trust, trustSummary);
            using var workbook = new XLWorkbook(new MemoryStream(result));
            var worksheet = workbook.Worksheet("Academies");

            // Assert
            worksheet.Cell(4, 1).Value.ToString().Should().Be(string.Empty);
            worksheet.Cell(4, 2).Value.ToString().Should().Be("123456");
            worksheet.Cell(4, 3).Value.ToString().Should().Be(string.Empty);
            worksheet.Cell(4, 4).Value.ToString().Should().Be(string.Empty);
            worksheet.Cell(4, 5).Value.ToString().Should().Be(string.Empty);
            worksheet.Cell(4, 6).Value.ToString().Should().Be(DateTime.Now.ToString("yyyy-MM-dd"));
            worksheet.Cell(4, 7).Value.ToString().Should().Be("None");
            worksheet.Cell(4, 8).Value.ToString().Should().Be(string.Empty);
            worksheet.Cell(4, 9).Value.ToString().Should().Be("After Joining");
            worksheet.Cell(4, 10).Value.ToString().Should().Be("None");
            worksheet.Cell(4, 11).Value.ToString().Should().Be(string.Empty);
            worksheet.Cell(4, 12).Value.ToString().Should().Be("After Joining");
            worksheet.Cell(4, 13).Value.ToString().Should().Be(string.Empty);
            worksheet.Cell(4, 14).Value.ToString().Should().Be(string.Empty);
            worksheet.Cell(4, 15).Value.ToString().Should().Be(string.Empty);
            worksheet.Cell(4, 16).Value.ToString().Should().Be(string.Empty);
            worksheet.Cell(4, 17).Value.ToString().Should().Be(string.Empty);
        }
    }
}
