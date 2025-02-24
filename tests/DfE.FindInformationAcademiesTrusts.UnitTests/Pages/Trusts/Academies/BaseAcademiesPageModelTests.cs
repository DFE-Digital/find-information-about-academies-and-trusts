﻿using DfE.FindInformationAcademiesTrusts.Data;
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
    protected readonly Mock<IExportService> MockExportService = new();
    protected readonly Mock<IDateTimeProvider> MockDateTimeProvider = new();

    [Fact]
    public async Task OnGetExportAsync_ShouldReturnFileResult_WhenUidIsValid()
    {
        // Arrange
        byte[] expectedBytes = [1, 2, 3];

        MockExportService.Setup(x => x.ExportAcademiesToSpreadsheetAsync(TrustUid))
            .ReturnsAsync(expectedBytes);

        // Act
        var result = await Sut.OnGetExportAsync(TrustUid);

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

        MockTrustService.Setup(x => x.GetTrustSummaryAsync(uid))
            .ReturnsAsync((TrustSummaryServiceModel?)null);

        // Act
        var result = await Sut.OnGetExportAsync(uid);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task OnGetExportAsync_ShouldSanitizeTrustName_WhenTrustNameContainsIllegalCharacters()
    {
        // Arrange
        var uid = TrustUid;
        var expectedBytes = new byte[] { 1, 2, 3 };

        MockTrustService.Setup(x => x.GetTrustSummaryAsync(uid))
            .ReturnsAsync(DummyTrustSummary with { Name = "Sample/Trust:Name?" });
        MockExportService.Setup(x => x.ExportAcademiesToSpreadsheetAsync(uid))
            .ReturnsAsync(expectedBytes);

        // Act
        var result = await Sut.OnGetExportAsync(uid);

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
        MockTrustService.Setup(t => t.GetTrustSummaryAsync(fakeTrust.Uid)).ReturnsAsync(fakeTrust);
        Sut.Uid = fakeTrust.Uid;

        _ = await Sut.OnGetAsync();
        MockDataSourceService.Verify(e => e.GetAsync(Source.Gias), Times.Once);
        MockDataSourceService.Verify(e => e.GetAsync(Source.ExploreEducationStatistics), Times.Once);
        Sut.DataSourcesPerPage.Should().BeEquivalentTo([
            new DataSourcePageListEntry(ViewConstants.AcademiesDetailsPageName,
                [new DataSourceListEntry(GiasDataSource)]),
            new DataSourcePageListEntry(ViewConstants.AcademiesPupilNumbersPageName,
                [new DataSourceListEntry(GiasDataSource)]),
            new DataSourcePageListEntry(ViewConstants.AcademiesFreeSchoolMealsPageName, [
                new DataSourceListEntry(GiasDataSource, "Pupils eligible for free school meals"),
                new DataSourceListEntry(EesDataSource, "Local authority average 2023/24"),
                new DataSourceListEntry(EesDataSource, "National average 2023/24")
            ])
        ]);
    }

    [Fact]
    public override async Task OnGetAsync_should_set_active_NavigationLink_to_current_page()
    {
        _ = await Sut.OnGetAsync();

        Sut.NavigationLinks.Should().ContainSingle(l => l.LinkIsActive)
            .Which.LinkText.Should().Be("Academies (3)");
    }

    [Fact]
    public override async Task OnGetAsync_should_configure_TrustPageMetadata_PageName()
    {
        _ = await Sut.OnGetAsync();

        Sut.TrustPageMetadata.PageName.Should().Be("Academies");
    }

    [Fact]
    public async Task OnGetAsync_sets_SubNavigationLinks_toEmptyArray()
    {
        _ = await Sut.OnGetAsync();
        Sut.SubNavigationLinks.Should().Equal();
    }

    [Fact]
    public abstract Task OnGetAsync_should_configure_TrustPageMetadata_TabPageName();
}
