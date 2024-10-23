using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Pages;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Academies;
using DfE.FindInformationAcademiesTrusts.Services.Academy;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Export;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts.Academies
{
    public class AcademiesPageModelTests
    {
        private readonly Mock<ITrustProvider> _mockTrustProvider = new();
        private readonly Mock<ITrustService> _mockTrustService = new();
        private readonly Mock<IAcademyService> _mockAcademyService = new();
        private readonly Mock<IExportService> _mockExportService = new();
        private readonly Mock<IOtherServicesLinkBuilder> _mockOtherServicesLinkBuilder = new();
        private readonly Mock<IDataSourceService> _mockDataSourceService = new();
        private readonly Mock<IDateTimeProvider> _mockDateTimeProvider = new();
        private readonly Mock<ILogger<AcademiesDetailsModel>> _mockLogger = new();
        private readonly AcademiesPageModel _sut;

        public AcademiesPageModelTests()
        {
            _sut = new AcademiesDetailsModel(_mockTrustProvider.Object, _mockDataSourceService.Object, _mockOtherServicesLinkBuilder.Object, _mockLogger.Object, _mockTrustService.Object, _mockAcademyService.Object, _mockExportService.Object, _mockDateTimeProvider.Object);
        }

        [Fact]
        public async Task OnGetExportAsync_ShouldReturnFileResult_WhenUidIsValid()
        {
            // Arrange
            string uid = "1234";
            var trust = new Trust("1234",
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
            var trustSummary = new TrustSummaryServiceModel(uid, "Sample Trust", "Multi-academy trust", 0);
            var expectedBytes = new byte[] { 1, 2, 3 };
            var ofstedRatings = Array.Empty<AcademyOfstedServiceModel>();

            _mockTrustProvider.Setup(x => x.GetTrustByUidAsync(uid)).ReturnsAsync(trust);
            _mockTrustService.Setup(x => x.GetTrustSummaryAsync(uid)).ReturnsAsync(trustSummary);
            _mockExportService.Setup(x => x.ExportAcademiesToSpreadsheetUsingProvider(trust, trustSummary, ofstedRatings)).Returns(expectedBytes);

            // Act
            var result = await _sut.OnGetExportAsync(uid);

            // Assert
            result.Should().BeOfType<FileContentResult>();
            var fileResult = result as FileContentResult;
            fileResult?.ContentType.Should().Be("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            fileResult?.FileContents.Should().BeEquivalentTo(expectedBytes);
        }
        [Fact]
        public async Task OnGetExportAsync_ShouldReturnNotFound_WhenUidIsInvalid()
        {
            // Arrange
            string uid = "invalid-uid";

            _mockTrustProvider.Setup(x => x.GetTrustByUidAsync(uid)).ReturnsAsync((Trust?)null);
            _mockTrustService.Setup(x => x.GetTrustSummaryAsync(uid)).ReturnsAsync((TrustSummaryServiceModel?)null);

            // Act
            var result = await _sut.OnGetExportAsync(uid);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task OnGetExportAsync_ShouldSanitizeTrustName_WhenTrustNameContainsIllegalCharacters()
        {
            // Arrange
            string uid = "1234";
            var trustWithIllegalChars = new Trust("1234",
                                                  "Sample/Trust:Name?", // Trust name with illegal characters
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
            var trustSummary = new TrustSummaryServiceModel(uid, "Sample/Trust:Name?", "Multi-academy trust", 0);
            var expectedBytes = new byte[] { 1, 2, 3 };
            var ofstedRatings = Array.Empty<AcademyOfstedServiceModel>();

            _mockTrustProvider.Setup(x => x.GetTrustByUidAsync(uid)).ReturnsAsync(trustWithIllegalChars);
            _mockTrustService.Setup(x => x.GetTrustSummaryAsync(uid)).ReturnsAsync(trustSummary);
            _mockExportService.Setup(x => x.ExportAcademiesToSpreadsheetUsingProvider(trustWithIllegalChars, trustSummary, ofstedRatings)).Returns(expectedBytes);

            // Act
            var result = await _sut.OnGetExportAsync(uid);

            // Assert
            result.Should().BeOfType<FileContentResult>();
            var fileResult = result as FileContentResult;
            fileResult?.ContentType.Should().Be("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            fileResult?.FileContents.Should().BeEquivalentTo(expectedBytes);

            // Verify that the file name is sanitized (no illegal characters)
            string fileDownloadName = fileResult?.FileDownloadName ?? string.Empty;
            var invalidFileNameChars = Path.GetInvalidFileNameChars();

            // Check that the file name doesn't contain any invalid characters
            bool containsInvalidChars = fileDownloadName.Any(c => invalidFileNameChars.Contains(c));
            containsInvalidChars.Should().BeFalse("the file name should not contain any illegal characters");
        }
    }
}


