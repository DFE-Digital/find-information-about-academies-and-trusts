using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Pages;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Ofsted;
using DfE.FindInformationAcademiesTrusts.Services.Academy;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Export;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts.Ofsted;

public abstract class BaseOfstedAreaModelTests<T> : BaseTrustPageTests<T>, ITestSubpages, ITestExport
    where T : OfstedAreaModel
{
    protected readonly Mock<IAcademyService> MockAcademyService = new();
    protected readonly Mock<IExportService> MockExportService = new();
    protected readonly Mock<IDateTimeProvider> MockDateTimeProvider = new();

    [Fact]
    public override async Task OnGetAsync_sets_correct_data_source_list()
    {
        await Sut.OnGetAsync();

        MockDataSourceService.Verify(e => e.GetAsync(Source.Gias), Times.Once);
        MockDataSourceService.Verify(e => e.GetAsync(Source.Mis), Times.Once);

        Sut.DataSourcesPerPage.Should().BeEquivalentTo([
            new DataSourcePageListEntry(ViewConstants.OfstedSingleHeadlineGradesPageName, [
                    new DataSourceListEntry(GiasDataSource, "Date joined trust"),
                    new DataSourceListEntry(MisDataSource, "All single headline grades"),
                    new DataSourceListEntry(MisDataSource, "All inspection dates")
                ]
            ),
            new DataSourcePageListEntry(ViewConstants.OfstedCurrentRatingsPageName, [
                    new DataSourceListEntry(MisDataSource, "Current Ofsted rating"),
                    new DataSourceListEntry(MisDataSource, "Date of current inspection")
                ]
            ),
            new DataSourcePageListEntry(ViewConstants.OfstedPreviousRatingsPageName, [
                    new DataSourceListEntry(MisDataSource, "Previous Ofsted rating"),
                    new DataSourceListEntry(MisDataSource, "Date of previous inspection")
                ]
            ),
            new DataSourcePageListEntry(ViewConstants.OfstedSafeguardingAndConcernsPageName, [
                    new DataSourceListEntry(MisDataSource, "Effective safeguarding"),
                    new DataSourceListEntry(MisDataSource, "Category of concern"),
                    new DataSourceListEntry(MisDataSource, "Date of current inspection")
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
        MockAcademyService.Setup(a => a.GetAcademiesInTrustOfstedAsync(Sut.Uid))
            .ReturnsAsync(academies);

        _ = await Sut.OnGetAsync();

        Sut.Academies.Should().BeEquivalentTo(academies);
    }

    [Fact]
    public async Task OnGetExportAsync_ShouldReturnFileResult_WhenUidIsValid()
    {
        // Arrange
        byte[] expectedBytes = [1, 2, 3];
        MockExportService.Setup(x => x.ExportOfstedDataToSpreadsheetAsync(TrustUid))
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
        MockExportService.Setup(x => x.ExportOfstedDataToSpreadsheetAsync(uid))
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
    public override async Task OnGetAsync_should_set_active_NavigationLink_to_current_page()
    {
        _ = await Sut.OnGetAsync();

        Sut.NavigationLinks.Should().ContainSingle(l => l.LinkIsActive)
            .Which.LinkText.Should().Be("Ofsted");
    }

    [Fact]
    public abstract Task OnGetAsync_should_set_active_SubNavigationLink_to_current_subpage();

    [Fact]
    public async Task OnGetAsync_should_populate_SubNavigationLinks_to_subpages()
    {
        _ = await Sut.OnGetAsync();

        Sut.SubNavigationLinks.Should()
            .SatisfyRespectively(
                l =>
                {
                    l.LinkText.Should().Be("Single headline grades");
                    l.SubPageLink.Should().Be("./SingleHeadlineGrades");
                    l.ServiceName.Should().Be("Ofsted");
                },
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
                    l.LinkText.Should().Be("Safeguarding and concerns");
                    l.SubPageLink.Should().Be("./SafeguardingAndConcerns");
                    l.ServiceName.Should().Be("Ofsted");
                });
    }

    [Fact]
    public override async Task OnGetAsync_should_configure_TrustPageMetadata_PageName()
    {
        _ = await Sut.OnGetAsync();

        Sut.TrustPageMetadata.PageName.Should().Be("Ofsted");
    }

    [Fact]
    public abstract Task OnGetAsync_should_configure_TrustPageMetadata_SubPageName();
}
