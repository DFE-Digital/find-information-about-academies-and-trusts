using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Pages;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Ofsted;
using DfE.FindInformationAcademiesTrusts.Services.Academy;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Export;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts.Governance;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts.Ofsted;

public abstract class BaseOfstedAreaModelTests<T> : BaseTrustPageTests<T>, ITestSubpages where T : OfstedAreaModel
{
    protected readonly Mock<IAcademyService> _mockAcademyService = new();
    protected readonly Mock<IExportService> _mockExportService = new();
    protected readonly Mock<IDateTimeProvider> _mockDateTimeProvider = new();

    [Fact]
    public override async Task OnGetAsync_sets_correct_data_source_list()
    {
        await _sut.OnGetAsync();

        _mockDataSourceService.Verify(e => e.GetAsync(Source.Gias), Times.Once);
        _mockDataSourceService.Verify(e => e.GetAsync(Source.Mis), Times.Once);

        _sut.DataSourcesPerPage.Should().BeEquivalentTo([
            new DataSourcePageListEntry(ViewConstants.OfstedCurrentRatingsPageName, [
                    new DataSourceListEntry(_misDataSource, "Current Ofsted rating"),
                    new DataSourceListEntry(_misDataSource, "Date of current inspection")
                ]
            ),
            new DataSourcePageListEntry(ViewConstants.OfstedPreviousRatingsPageName, [
                    new DataSourceListEntry(_misDataSource, "Previous Ofsted rating"),
                    new DataSourceListEntry(_misDataSource, "Date of previous inspection")
                ]
            ),
            new DataSourcePageListEntry(ViewConstants.OfstedImportantDatesPageName, [
                    new DataSourceListEntry(_giasDataSource, "Date joined trust"),
                    new DataSourceListEntry(_misDataSource, "Date of current inspection"),
                    new DataSourceListEntry(_misDataSource, "Date of previous inspection")
                ]
            ),
            new DataSourcePageListEntry(ViewConstants.OfstedSafeguardingAndConcernsPageName, [
                    new DataSourceListEntry(_misDataSource, "Effective safeguarding"),
                    new DataSourceListEntry(_misDataSource, "Category of concern"),
                    new DataSourceListEntry(_misDataSource, "Date of current inspection")
                ]
            )
        ]);
    }

    [Fact]
    public async Task OnGetAsync_sets_academies_from_academyService()
    {
        var academies = new[]
        {
            new AcademyOfstedServiceModel("1", "Academy 1", new DateTime(2022, 12, 1),
                new OfstedRating(2, new DateTime(2023, 1, 1)),
                new OfstedRating(3, new DateTime(2023, 2, 1))),
            new AcademyOfstedServiceModel("2", "Academy 2", new DateTime(2022, 11, 2),
                new OfstedRating(2, new DateTime(2023, 1, 2)),
                new OfstedRating(3, new DateTime(2023, 3, 1))),
            new AcademyOfstedServiceModel("3", "Academy 3", new DateTime(2022, 10, 3),
                new OfstedRating(2, new DateTime(2023, 1, 3)),
                new OfstedRating(3, new DateTime(2023, 4, 1)))
        };
        _mockAcademyService.Setup(a => a.GetAcademiesInTrustOfstedAsync(_sut.Uid))
            .ReturnsAsync(academies);

        _ = await _sut.OnGetAsync();

        _sut.Academies.Should().BeEquivalentTo(academies);
    }

    [Fact]
    public async Task OnGetExportAsync_ShouldReturnFileResult_WhenUidIsValid()
    {
        // Arrange
        byte[] expectedBytes = [1, 2, 3];
        _mockExportService.Setup(x => x.ExportOfstedDataToSpreadsheetAsync(TrustUid))
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
        _mockExportService.Setup(x => x.ExportOfstedDataToSpreadsheetAsync(uid))
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
    public override async Task OnGetAsync_should_set_active_NavigationLink_to_current_page()
    {
        _ = await _sut.OnGetAsync();

        _sut.NavigationLinks.Should().ContainSingle(l => l.LinkIsActive)
            .Which.LinkText.Should().Be("Ofsted");
    }

    [Fact]
    public abstract Task OnGetAsync_should_set_active_SubNavigationLink_to_current_subpage();

    [Fact]
    public async Task OnGetAsync_should_populate_SubNavigationLinks_to_subpages()
    {
        _ = await _sut.OnGetAsync();

        _sut.SubNavigationLinks.Should()
            .SatisfyRespectively(
                l =>
                {
                    l.LinkText.Should().Be("Current ratings");
                    l.SubPageLink.Should().Be("./CurrentRatings");
                    l.ServiceName.Should().Be("Ofsted");
                },
                l =>
                {
                    l.LinkText.Should().Be("Previous ratings");
                    l.SubPageLink.Should().Be("./PreviousRatings");
                    l.ServiceName.Should().Be("Ofsted");
                },
                l =>
                {
                    l.LinkText.Should().Be("Important dates");
                    l.SubPageLink.Should().Be("./ImportantDates");
                    l.ServiceName.Should().Be("Ofsted");
                },
                l =>
                {
                    l.LinkText.Should().Be("Safeguarding and concerns");
                    l.SubPageLink.Should().Be("./SafeguardingAndConcerns");
                    l.ServiceName.Should().Be("Ofsted");
                });
    }

    [Fact]
    public override async Task OnGetAsync_should_configure_TrustPageMetadata_PageName()
    {
        _ = await _sut.OnGetAsync();

        _sut.TrustPageMetadata.PageName.Should().Be("Ofsted");
    }

    [Fact]
    public abstract Task OnGetAsync_should_configure_TrustPageMetadata_SubPageName();
}
