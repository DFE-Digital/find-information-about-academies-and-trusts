﻿using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Academies;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Export;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts.Academies;

public class AcademiesPageModelTests
{
    private readonly Mock<ITrustService> _mockTrustService = new();
    private readonly Mock<IExportService> _mockExportService = new();
    private readonly Mock<IDataSourceService> _mockDataSourceService = new();
    private readonly Mock<IDateTimeProvider> _mockDateTimeProvider = new();
    private readonly Mock<ILogger<AcademiesPageModel>> _mockLogger = new();
    private readonly AcademiesPageModel _sut;

    private class AcademiesPageModelImpl(
        IDataSourceService dataSourceService,
        ITrustService trustService,
        IExportService exportService,
        ILogger<AcademiesPageModel> logger,
        IDateTimeProvider dateTimeProvider)
        : AcademiesPageModel(dataSourceService, trustService, exportService, logger, dateTimeProvider);

    public AcademiesPageModelTests()
    {
        _sut = new AcademiesPageModelImpl(_mockDataSourceService.Object, _mockTrustService.Object,
            _mockExportService.Object, _mockLogger.Object, _mockDateTimeProvider.Object);
    }

    [Fact]
    public async Task OnGetExportAsync_ShouldReturnFileResult_WhenUidIsValid()
    {
        // Arrange
        var uid = "1234";

        var trustSummary = new TrustSummaryServiceModel(uid, "Sample Trust", "Multi-academy trust", 0);
        byte[] expectedBytes = [1, 2, 3];

        _mockTrustService.Setup(x => x.GetTrustSummaryAsync(uid)).ReturnsAsync(trustSummary);
        _mockExportService.Setup(x => x.ExportAcademiesToSpreadsheetAsync(uid)).ReturnsAsync(expectedBytes);


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
        var uid = "invalid-uid";

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
        var uid = "1234";
        var trustSummary = new TrustSummaryServiceModel(uid, "Sample/Trust:Name?", "Multi-academy trust", 0);
        var expectedBytes = new byte[] { 1, 2, 3 };

        _mockTrustService.Setup(x => x.GetTrustSummaryAsync(uid)).ReturnsAsync(trustSummary);
        _mockExportService.Setup(x => x.ExportAcademiesToSpreadsheetAsync(uid)).ReturnsAsync(expectedBytes);

        // Act
        var result = await _sut.OnGetExportAsync(uid);

        // Assert
        result.Should().BeOfType<FileContentResult>();
        var fileResult = result as FileContentResult;
        fileResult?.ContentType.Should().Be("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        fileResult?.FileContents.Should().BeEquivalentTo(expectedBytes);
        fileResult?.FileDownloadName.Should().NotBeEmpty();

        // Verify that the file name is sanitized (no illegal characters)
        var fileDownloadName = fileResult?.FileDownloadName ?? string.Empty;
        var invalidFileNameChars = Path.GetInvalidFileNameChars();

        // Check that the file name doesn't contain any invalid characters
        var containsInvalidChars = fileDownloadName.Any(c => invalidFileNameChars.Contains(c));
        containsInvalidChars.Should().BeFalse("the file name should not contain any illegal characters");
    }

    [Fact]
    public async Task OnGetAsync_should_configure_TrustPageMetadata()
    {
        TrustSummaryServiceModel fakeTrust = new("1234", "My Trust", "Multi-academy trust", 3);

        _mockTrustService.Setup(t => t.GetTrustSummaryAsync(fakeTrust.Uid)).ReturnsAsync(fakeTrust);
        _sut.Uid = fakeTrust.Uid;

        _ = await _sut.OnGetAsync();

        _sut.TrustPageMetadata.PageName.Should().Be("Academies");
        _sut.TrustPageMetadata.TrustName.Should().Be("My Trust");
    }
}
