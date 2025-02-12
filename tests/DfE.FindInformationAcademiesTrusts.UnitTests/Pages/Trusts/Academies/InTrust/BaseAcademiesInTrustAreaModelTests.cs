using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Pages;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Academies.InTrust;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts.Academies.InTrust;

public abstract class AcademiesInTrustAreaModelTests<T> : BaseAcademiesAreaModelTests<T>, ITestTabPages
    where T : AcademiesInTrustAreaModel
{
    protected readonly Mock<IDateTimeProvider> MockDateTimeProvider = new();

    [Fact]
    public override async Task OnGetExportAsync_ShouldReturnFileResult_WhenUidIsValid()
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
    public override async Task OnGetExportAsync_ShouldReturnNotFound_WhenUidIsInvalid()
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
    public override async Task OnGetExportAsync_ShouldSanitizeTrustName_WhenTrustNameContainsIllegalCharacters()
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
        await Sut.OnGetAsync();
        MockDataSourceService.Verify(e => e.GetAsync(Source.Gias), Times.Once);

        Sut.DataSourcesPerPage.Should().BeEquivalentTo([
            new DataSourcePageListEntry(ViewConstants.AcademiesInTrustDetailsPageName,
                [new DataSourceListEntry(GiasDataSource)]),
            new DataSourcePageListEntry(ViewConstants.AcademiesInTrustPupilNumbersPageName,
                [new DataSourceListEntry(GiasDataSource)]),
            new DataSourcePageListEntry(ViewConstants.AcademiesInTrustFreeSchoolMealsPageName,
            [
                new DataSourceListEntry(GiasDataSource, "Pupils eligible for free school meals"),
                new DataSourceListEntry(EesDataSource, "Local authority average 2023/24"),
                new DataSourceListEntry(EesDataSource, "National average 2023/24")
            ])
        ]);
    }

    [Fact]
    public override async Task OnGetAsync_should_set_active_SubNavigationLink_to_current_subpage()
    {
        _ = await Sut.OnGetAsync();

        Sut.SubNavigationLinks.Should().ContainSingle(l => l.LinkIsActive)
            .Which.SubPageLink.Should().Be("/Trusts/Academies/InTrust/Details");
    }

    [Fact]
    public override async Task OnGetAsync_should_configure_TrustPageMetadata_SubPageName()
    {
        _ = await Sut.OnGetAsync();

        Sut.TrustPageMetadata.SubPageName.Should().Be("In this trust");
    }

    [Fact]
    public override async Task OnGetAsync_should_populate_TabList_to_tabs()
    {
        _ = await Sut.OnGetAsync();

        Sut.TabList.Should()
            .SatisfyRespectively(
                l =>
                {
                    l.LinkText.Should().Be("Details");
                    l.TabPageLink.Should().Be("./Details");
                    l.TabNavName.Should().Be("In this trust");
                },
                l =>
                {
                    l.LinkText.Should().Be("Pupil numbers");
                    l.TabPageLink.Should().Be("./PupilNumbers");
                    l.TabNavName.Should().Be("In this trust");
                },
                l =>
                {
                    l.LinkText.Should().Be("Free school meals");
                    l.TabPageLink.Should().Be("./FreeSchoolMeals");
                    l.TabNavName.Should().Be("In this trust");
                });
    }
}
