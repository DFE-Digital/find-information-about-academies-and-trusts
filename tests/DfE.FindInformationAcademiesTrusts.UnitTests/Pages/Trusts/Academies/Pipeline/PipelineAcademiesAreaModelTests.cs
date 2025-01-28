using DfE.FindInformationAcademiesTrusts.Configuration;
using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Pages;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Academies.Pipeline;
using DfE.FindInformationAcademiesTrusts.Services.Academy;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Export;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts.Academies.Pipeline;

public class PipelineAcademiesAreaModelTests
{
    //TODO: make this inline with other tests
    private readonly Mock<ITrustService> _mockTrustService = new();
    private readonly Mock<IAcademyService> _mockAcademyService = new();
    private readonly Mock<IExportService> _mockExportService = new();
    private readonly Mock<IDataSourceService> _mockDataSourceService = new();
    private readonly Mock<IDateTimeProvider> _mockDateTimeProvider = new();
    private readonly Mock<ILogger<PipelineAcademiesAreaModel>> _mockLogger = new();
    private readonly Mock<IFeatureManager> _mockFeatureManager = new();

    private readonly PipelineAcademiesAreaModel _sut;

    private readonly DataSourceServiceModel _prepareDataSource =
        new(Source.Prepare, new DateTime(2025, 1, 1), UpdateFrequency.Daily);

    private readonly DataSourceServiceModel _completeDataSource =
        new(Source.Complete, new DateTime(2025, 1, 1), UpdateFrequency.Daily);

    private readonly DataSourceServiceModel _manageFreeSchoolDataSource =
        new(Source.ManageFreeSchoolProjects, new DateTime(2025, 1, 1), UpdateFrequency.Daily);

    private class PipelineAcademiesAreaModelImpl(
        IDataSourceService dataSourceService,
        ITrustService trustService,
        IAcademyService academyService,
        IExportService exportService,
        ILogger<PipelineAcademiesAreaModel> logger,
        IDateTimeProvider dateTimeProvider,
        IFeatureManager featureManager)
        : PipelineAcademiesAreaModel(dataSourceService, trustService, academyService, exportService, logger,
            dateTimeProvider, featureManager);

    public PipelineAcademiesAreaModelTests()
    {
        _mockDataSourceService.Setup(s => s.GetAsync(Source.Prepare)).ReturnsAsync(_prepareDataSource);
        _mockDataSourceService.Setup(s => s.GetAsync(Source.Complete)).ReturnsAsync(_completeDataSource);
        _mockDataSourceService.Setup(s => s.GetAsync(Source.ManageFreeSchoolProjects))
            .ReturnsAsync(_manageFreeSchoolDataSource);
        _mockAcademyService.Setup(t => t.GetAcademiesPipelineSummary())
            .Returns(new AcademyPipelineSummaryServiceModel(1, 2, 3));
        _mockFeatureManager.Setup(s => s.IsEnabledAsync(FeatureFlags.PipelineAcademies)).ReturnsAsync(true);
        _sut = new PipelineAcademiesAreaModelImpl(_mockDataSourceService.Object, _mockTrustService.Object,
            _mockAcademyService.Object,
            _mockExportService.Object, _mockLogger.Object, _mockDateTimeProvider.Object, _mockFeatureManager.Object);
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

        _sut.TrustPageMetadata.PageName.Should().Be(ViewConstants.AcademiesPageName);
        _sut.TrustPageMetadata.SubPageName.Should().Be(ViewConstants.PipelineAcademiesSubNavName);
        _sut.TrustPageMetadata.TrustName.Should().Be("My Trust");
    }

    [Fact]
    public async Task OnGetAsync_sets_correct_data_source_list()
    {
        TrustSummaryServiceModel fakeTrust = new("1234", "My Trust", "Multi-academy trust", 3);
        _mockTrustService.Setup(t => t.GetTrustSummaryAsync(fakeTrust.Uid)).ReturnsAsync(fakeTrust);
        _sut.Uid = fakeTrust.Uid;

        _ = await _sut.OnGetAsync();
        _sut.DataSourcesPerPage.Should().BeEquivalentTo([
            new DataSourcePageListEntry(ViewConstants.PipelineAcademiesPreAdvisoryBoardPageName,
                [new DataSourceListEntry(_prepareDataSource)]),
            new DataSourcePageListEntry(ViewConstants.PipelineAcademiesPostAdvisoryBoardPageName,
                [new DataSourceListEntry(_completeDataSource)]),
            new DataSourcePageListEntry(ViewConstants.PipelineAcademiesFreeSchoolsPageName, [
                new DataSourceListEntry(_manageFreeSchoolDataSource)
            ])
        ]);
    }
}