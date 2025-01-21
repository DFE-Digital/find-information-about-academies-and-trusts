using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Pages;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Overview;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts.Governance;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts.Overview;

public abstract class BaseOverviewAreaModelTests<T> : BaseTrustPageTests<T>, ITestSubpages where T : OverviewAreaModel
{
    protected readonly TrustOverviewServiceModel BaseTrustOverviewServiceModel =
        new(TrustUid, "", "", "", TrustType.MultiAcademyTrust, "", "", null, null, 0, new Dictionary<string, int>(), 0,
            0);

    protected BaseOverviewAreaModelTests()
    {
        _mockTrustService.Setup(t => t.GetTrustOverviewAsync(It.IsAny<string>()))
            .ReturnsAsync((string uid) => BaseTrustOverviewServiceModel with { Uid = uid });
    }

    protected void SetupTrustOverview(TrustOverviewServiceModel trustOverviewServiceModel)
    {
        _mockTrustService.Setup(t => t.GetTrustOverviewAsync(trustOverviewServiceModel.Uid))
            .ReturnsAsync(trustOverviewServiceModel);
    }

    [Fact]
    public override async Task OnGetAsync_sets_correct_data_source_list()
    {
        _ = await _sut.OnGetAsync();
        _mockDataSourceService.Verify(e => e.GetAsync(Source.Gias), Times.Once);

        _sut.DataSourcesPerPage.Should().BeEquivalentTo([
            new DataSourcePageListEntry(ViewConstants.OverviewTrustDetailsPageName,
                [new DataSourceListEntry(_giasDataSource)]),
            new DataSourcePageListEntry(ViewConstants.OverviewTrustSummaryPageName,
                [new DataSourceListEntry(_giasDataSource)]),
            new DataSourcePageListEntry(ViewConstants.OverviewReferenceNumbersPageName,
                [new DataSourceListEntry(_giasDataSource)])
        ]);
    }

    [Fact]
    public override async Task OnGetAsync_should_set_active_NavigationLink_to_current_page()
    {
        _ = await _sut.OnGetAsync();

        _sut.NavigationLinks.Should().ContainSingle(l => l.LinkIsActive)
            .Which.LinkText.Should().Be(ViewConstants.OverviewPageName);
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
                    l.LinkText.Should().Be(ViewConstants.OverviewTrustDetailsPageName);
                    l.SubPageLink.Should().Be("./TrustDetails");
                    l.ServiceName.Should().Be(ViewConstants.OverviewPageName);
                },
                l =>
                {
                    l.LinkText.Should().Be(ViewConstants.OverviewTrustSummaryPageName);
                    l.SubPageLink.Should().Be("./TrustSummary");
                    l.ServiceName.Should().Be(ViewConstants.OverviewPageName);
                },
                l =>
                {
                    l.LinkText.Should().Be(ViewConstants.OverviewReferenceNumbersPageName);
                    l.SubPageLink.Should().Be("./ReferenceNumbers");
                    l.ServiceName.Should().Be(ViewConstants.OverviewPageName);
                });
    }

    [Fact]
    public override async Task OnGetAsync_should_configure_TrustPageMetadata_PageName()
    {
        _ = await _sut.OnGetAsync();

        _sut.TrustPageMetadata.PageName.Should().Be(ViewConstants.OverviewPageName);
    }

    [Fact]
    public abstract Task OnGetAsync_should_configure_TrustPageMetadata_SubPageName();
}
