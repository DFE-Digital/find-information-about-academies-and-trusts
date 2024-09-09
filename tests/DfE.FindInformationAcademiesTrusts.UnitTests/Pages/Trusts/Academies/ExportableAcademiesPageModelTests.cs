using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Pages;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Academies;
using DfE.FindInformationAcademiesTrusts.Services;
using DfE.FindInformationAcademiesTrusts.Services.Academy;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts.Academies
{
    public class ExportableAcademiesPageModelTests
    {
        private readonly Mock<ITrustProvider> _mockTrustProvider = new();
        private readonly Mock<ITrustService> _mockTrustService = new();
        private readonly Mock<IAcademyService> _mockAcademyService = new();
        private readonly Mock<IExportService> _mockExportService = new();
        private readonly Mock<IOtherServicesLinkBuilder> _mockOtherServicesLinkBuilder = new();
        private readonly Mock<IDataSourceService> _mockDataSourceService = new();
        private readonly Mock<ILogger<AcademiesDetailsModel>> _mockLogger = new();
        private readonly ExportableAcademiesPageModel _sut;

        public ExportableAcademiesPageModelTests()
        {
            _sut = new AcademiesDetailsModel(_mockTrustProvider.Object, _mockDataSourceService.Object, _mockOtherServicesLinkBuilder.Object, _mockLogger.Object, _mockTrustService.Object, _mockAcademyService.Object, _mockExportService.Object);
        }

        [Fact]
        public async Task OnPostExportAsync_ShouldReturnFileResult_WhenUidIsValid()
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

            _mockTrustProvider.Setup(x => x.GetTrustByUidAsync(uid)).ReturnsAsync(trust);
            _mockTrustService.Setup(x => x.GetTrustSummaryAsync(uid)).ReturnsAsync(trustSummary);
            _mockExportService.Setup(x => x.ExportAcademiesToSpreadsheetUsingProvider(trust, trustSummary)).Returns([]);

            // Act
            var result = await _sut.OnGetExportAsync(uid);

            // Assert
            result.Should().BeOfType<FileContentResult>();
            var fileResult = result as FileContentResult;
            fileResult?.ContentType.Should().Be("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

    }

}