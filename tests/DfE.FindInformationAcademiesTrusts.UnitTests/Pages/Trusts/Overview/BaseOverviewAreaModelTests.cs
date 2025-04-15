using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Pages;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Overview;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using NSubstitute;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts.Overview;

public abstract class BaseOverviewAreaModelTests<T> : BaseTrustPageTests<T>, ITestSubpages where T : OverviewAreaModel
{
    protected readonly TrustOverviewServiceModel BaseTrustOverviewServiceModel =
        new(TrustUid, "", "", "", TrustType.MultiAcademyTrust, "", "", null, null, 0, new Dictionary<string, int>(), 0,
            0);

    protected BaseOverviewAreaModelTests()
    {
        MockTrustService.GetTrustOverviewAsync(Arg.Any<string>())
            .Returns(callinfo => BaseTrustOverviewServiceModel with { Uid = callinfo.Arg<string>() });
    }

    protected void SetupTrustOverview(TrustOverviewServiceModel trustOverviewServiceModel)
    {
        MockTrustService.GetTrustOverviewAsync(trustOverviewServiceModel.Uid)
            .Returns(Task.FromResult(trustOverviewServiceModel));
    }

    [Fact]
    public override async Task OnGetAsync_sets_correct_data_source_list()
    {
        _ = await Sut.OnGetAsync();
        await MockDataSourceService.Received(1).GetAsync(Source.Gias);

        Sut.DataSourcesPerPage.Should().BeEquivalentTo([
            new DataSourcePageListEntry("Trust details", [new DataSourceListEntry(GiasDataSource)]),
            new DataSourcePageListEntry("Trust summary", [new DataSourceListEntry(GiasDataSource)]),
            new DataSourcePageListEntry("Reference numbers", [new DataSourceListEntry(GiasDataSource)])
        ]);
    }

    [Fact]
    public override async Task OnGetAsync_should_set_active_NavigationLink_to_current_page()
    {
        _ = await Sut.OnGetAsync();

        Sut.NavigationLinks.Should().ContainSingle(l => l.LinkIsActive)
            .Which.LinkText.Should().Be(ViewConstants.OverviewPageName);
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
        _ = await Sut.OnGetAsync();

        Sut.TrustPageMetadata.PageName.Should().Be("Overview");
    }

    [Fact]
    public abstract Task OnGetAsync_should_configure_TrustPageMetadata_SubPageName();
}
