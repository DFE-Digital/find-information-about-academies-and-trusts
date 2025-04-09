using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Academies.Pipeline;
using DfE.FindInformationAcademiesTrusts.Services.Academy;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using Microsoft.AspNetCore.Mvc;
using NSubstitute.ReturnsExtensions;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts.Academies.Pipeline;

public abstract class BasePipelineAcademiesAreaModelTests<T> : BaseAcademiesAreaModelTests<T>
    where T : PipelineAcademiesAreaModel
{
    protected readonly IDateTimeProvider MockDateTimeProvider = Substitute.For<IDateTimeProvider>();

    protected BasePipelineAcademiesAreaModelTests()
    {
        MockAcademyService.GetAcademiesPipelineSummaryAsync(TrustUid)
            .Returns(new AcademyPipelineSummaryServiceModel(1, 2, 3));
    }

    [Fact]
    public override async Task OnGetExportAsync_ShouldReturnFileResult_WhenUidIsValid()
    {
        // Arrange
        byte[] expectedBytes = [1, 2, 3];

        MockPipelineAcademiesExportService.BuildAsync(TrustUid).Returns(expectedBytes);

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

        MockTrustService.GetTrustSummaryAsync(uid).ReturnsNull();

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

        MockTrustService.GetTrustSummaryAsync(TrustUid).Returns(trustSummary);
        MockPipelineAcademiesExportService.BuildAsync(TrustUid).Returns(expectedBytes);

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
    public override async Task OnGetAsync_sets_correct_data_source_list()
    {
        _ = await Sut.OnGetAsync();

        await MockDataSourceService.Received(1).GetAsync(Source.Prepare);
        await MockDataSourceService.Received(1).GetAsync(Source.ManageFreeSchoolProjects);
        await MockDataSourceService.Received(1).GetAsync(Source.Complete);

        Sut.DataSourcesPerPage.Should().BeEquivalentTo([
            new DataSourcePageListEntry("Pre advisory board",
                [new DataSourceListEntry(Mocks.MockDataSourceService.Prepare)]),
            new DataSourcePageListEntry("Post advisory board",
                [new DataSourceListEntry(Mocks.MockDataSourceService.Complete)]),
            new DataSourcePageListEntry("Free schools",
                [new DataSourceListEntry(Mocks.MockDataSourceService.ManageFreeSchool)])
        ]);
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
                    l.LinkDisplayText.Should().Be("Pre advisory board (1)");
                    l.AspPage.Should().Be("./PreAdvisoryBoard");
                    l.TestId.Should().Be("pipeline-pre-advisory-board-tab");
                },
                l =>
                {
                    l.LinkDisplayText.Should().Be("Post advisory board (2)");
                    l.AspPage.Should().Be("./PostAdvisoryBoard");
                    l.TestId.Should().Be("pipeline-post-advisory-board-tab");
                },
                l =>
                {
                    l.LinkDisplayText.Should().Be("Free schools (3)");
                    l.AspPage.Should().Be("./FreeSchools");
                    l.TestId.Should().Be("pipeline-free-schools-tab");
                }
            );
    }
}
