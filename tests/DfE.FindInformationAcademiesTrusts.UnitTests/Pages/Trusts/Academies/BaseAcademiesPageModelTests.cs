using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Pages;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Academies;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Export;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts.Academies;

public abstract class BaseAcademiesPageModelTests<T> : BaseTrustPageTests<T>, ITestTabPages where T : AcademiesPageModel
{
    protected readonly Mock<IExportService> _mockExportService = new();
    protected readonly Mock<IDateTimeProvider> _mockDateTimeProvider = new();

    [Fact]
    public async Task OnGetExportAsync_ShouldReturnFileResult_WhenUidIsValid()
    {
        // Arrange
        byte[] expectedBytes = [1, 2, 3];

        _mockExportService.Setup(x => x.ExportAcademiesToSpreadsheetAsync(TrustUid))
            .ReturnsAsync(expectedBytes);

        // Act
        var result = await _sut.OnGetExportAsync(TrustUid);

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

        _mockTrustService.Setup(x => x.GetTrustSummaryAsync(uid))
            .ReturnsAsync((TrustSummaryServiceModel?)null);

        // Act
        var result = await _sut.OnGetExportAsync(uid);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task OnGetExportAsync_ShouldSanitizeTrustName_WhenTrustNameContainsIllegalCharacters()
    {
        // Arrange
        var uid = TrustUid;
        var expectedBytes = new byte[] { 1, 2, 3 };

        _mockTrustService.Setup(x => x.GetTrustSummaryAsync(uid))
            .ReturnsAsync(dummyTrustSummary with { Name = "Sample/Trust:Name?" });
        _mockExportService.Setup(x => x.ExportAcademiesToSpreadsheetAsync(uid))
            .ReturnsAsync(expectedBytes);

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
    public override async Task OnGetAsync_sets_correct_data_source_list()
    {
        TrustSummaryServiceModel fakeTrust = new("1234", "My Trust", "Multi-academy trust", 3);
        _mockTrustService.Setup(t => t.GetTrustSummaryAsync(fakeTrust.Uid)).ReturnsAsync(fakeTrust);
        _sut.Uid = fakeTrust.Uid;

        _ = await _sut.OnGetAsync();
        _mockDataSourceService.Verify(e => e.GetAsync(Source.Gias), Times.Once);
        _mockDataSourceService.Verify(e => e.GetAsync(Source.ExploreEducationStatistics), Times.Once);
        _sut.DataSourcesPerPage.Should().BeEquivalentTo([
            new DataSourcePageListEntry(ViewConstants.AcademiesDetailsPageName,
                [new DataSourceListEntry(_giasDataSource)]),
            new DataSourcePageListEntry(ViewConstants.AcademiesPupilNumbersPageName,
                [new DataSourceListEntry(_giasDataSource)]),
            new DataSourcePageListEntry(ViewConstants.AcademiesFreeSchoolMealsPageName, [
                new DataSourceListEntry(_giasDataSource, "Pupils eligible for free school meals"),
                new DataSourceListEntry(_eesDataSource, "Local authority average 2023/24"),
                new DataSourceListEntry(_eesDataSource, "National average 2023/24")
            ])
        ]);
    }

    [Fact]
    public override async Task OnGetAsync_should_set_active_NavigationLink_to_current_page()
    {
        _ = await _sut.OnGetAsync();

        _sut.NavigationLinks.Should().ContainSingle(l => l.LinkIsActive)
            .Which.LinkText.Should().Be("Academies (3)");
    }

    [Fact]
    public override async Task OnGetAsync_should_configure_TrustPageMetadata_PageName()
    {
        _ = await _sut.OnGetAsync();

        _sut.TrustPageMetadata.PageName.Should().Be("Academies");
    }

    [Fact]
    public async Task OnGetAsync_sets_SubNavigationLinks_toEmptyArray()
    {
        _ = await _sut.OnGetAsync();
        _sut.SubNavigationLinks.Should().Equal();
    }

    [Fact]
    public abstract Task OnGetAsync_should_configure_TrustPageMetadata_TabPageName();
}
