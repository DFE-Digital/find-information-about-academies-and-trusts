using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Pages;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Academies.Pipeline;
using DfE.FindInformationAcademiesTrusts.Services.Academy;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts.Academies.Pipeline;

public abstract class BasePipelineAcademiesAreaModelTests<T> : BaseAcademiesAreaModelTests<T>
    where T : PipelineAcademiesAreaModel
{
    protected readonly Mock<IDateTimeProvider> MockDateTimeProvider = new();

    protected BasePipelineAcademiesAreaModelTests()
    {
        MockAcademyService.Setup(t => t.GetAcademiesPipelineSummaryAsync(TrustUid))
            .ReturnsAsync(new AcademyPipelineSummaryServiceModel(1, 2, 3));
    }

    [Fact]
    public override async Task OnGetExportAsync_ShouldReturnFileResult_WhenUidIsValid()
    {
        // Arrange
        byte[] expectedBytes = [1, 2, 3];

        MockExportService.Setup(x => x.ExportAcademiesToSpreadsheetAsync(TrustUid)).ReturnsAsync(expectedBytes);

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

        MockTrustService.Setup(x => x.GetTrustSummaryAsync(uid)).ReturnsAsync((TrustSummaryServiceModel?)null);

        // Act
        var result = await Sut.OnGetExportAsync(uid);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public override async Task OnGetExportAsync_ShouldSanitizeTrustName_WhenTrustNameContainsIllegalCharacters()
    {
        // Arrange
        var trustSummary = new TrustSummaryServiceModel(TrustUid, "Sample/Trust:Name?", "Multi-academy trust", 0);
        var expectedBytes = new byte[] { 1, 2, 3 };

        MockTrustService.Setup(x => x.GetTrustSummaryAsync(TrustUid)).ReturnsAsync(trustSummary);
        MockExportService.Setup(x => x.ExportAcademiesToSpreadsheetAsync(TrustUid)).ReturnsAsync(expectedBytes);

        // Act
        var result = await Sut.OnGetExportAsync(TrustUid);

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
        // Arrange
        MockAcademyService
            .Setup(a => a.GetAcademyTrustTrustReferenceNumberAsync("1234"))
            .ReturnsAsync("1234");
        TrustSummaryServiceModel fakeTrust = new(TrustUid, "My Trust", "Multi-academy trust", 3);

        MockTrustService.Setup(t => t.GetTrustSummaryAsync(fakeTrust.Uid)).ReturnsAsync(fakeTrust);
        Sut.Uid = fakeTrust.Uid;

        // Act
        _ = await Sut.OnGetAsync();

        // Assert
        Sut.TrustPageMetadata.PageName.Should().Be(ViewConstants.AcademiesPageName);
        Sut.TrustPageMetadata.SubPageName.Should().Be(ViewConstants.PipelineAcademiesSubNavName);
        Sut.TrustPageMetadata.TrustName.Should().Be("My Trust");
    }

    [Fact]
    public override async Task OnGetAsync_sets_correct_data_source_list()
    {
        _ = await Sut.OnGetAsync();
        MockDataSourceService.Verify(e => e.GetAsync(Source.Prepare), Times.Once);
        MockDataSourceService.Verify(e => e.GetAsync(Source.ManageFreeSchoolProjects), Times.Once);
        MockDataSourceService.Verify(e => e.GetAsync(Source.Complete), Times.Once);

        Sut.DataSourcesPerPage.Should().BeEquivalentTo([
            new DataSourcePageListEntry(ViewConstants.PipelineAcademiesPreAdvisoryBoardPageName,
                [new DataSourceListEntry(MockDataSourceService.Prepare)]),
            new DataSourcePageListEntry(ViewConstants.PipelineAcademiesPostAdvisoryBoardPageName,
                [new DataSourceListEntry(MockDataSourceService.Complete)]),
            new DataSourcePageListEntry(ViewConstants.PipelineAcademiesFreeSchoolsPageName,
                [new DataSourceListEntry(MockDataSourceService.ManageFreeSchool)])
        ]);
    }

    [Fact]
    public override async Task OnGetAsync_should_set_active_SubNavigationLink_to_current_subpage()
    {
        _ = await Sut.OnGetAsync();

        Sut.SubNavigationLinks.Should().ContainSingle(l => l.LinkIsActive)
            .Which.SubPageLink.Should().Be("/Trusts/Academies/Pipeline/PreAdvisoryBoard");
    }

    [Fact]
    public override async Task OnGetAsync_should_configure_TrustPageMetadata_SubPageName()
    {
        _ = await Sut.OnGetAsync();

        Sut.TrustPageMetadata.SubPageName.Should().Be("Pipeline academies");
    }

    [Fact]
    public override async Task OnGetAsync_should_populate_TabList_to_tabs()
    {
        _ = await Sut.OnGetAsync();

        Sut.TabList.Should()
            .SatisfyRespectively(
                l =>
                {
                    l.LinkText.Should().Be("Pre advisory board (1)");
                    l.TabPageLink.Should().Be("./PreAdvisoryBoard");
                    l.TabNavName.Should().Be("Pipeline");
                },
                l =>
                {
                    l.LinkText.Should().Be("Post advisory board (2)");
                    l.TabPageLink.Should().Be("./PostAdvisoryBoard");
                    l.TabNavName.Should().Be("Pipeline");
                },
                l =>
                {
                    l.LinkText.Should().Be("Free schools (3)");
                    l.TabPageLink.Should().Be("./FreeSchools");
                    l.TabNavName.Should().Be("Pipeline");
                }
            );
    }
}
