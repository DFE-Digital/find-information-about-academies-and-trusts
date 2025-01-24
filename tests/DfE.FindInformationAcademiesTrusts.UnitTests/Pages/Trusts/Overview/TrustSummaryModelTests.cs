using DfE.FindInformationAcademiesTrusts.Pages;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Overview;
using DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts.Overview;

public class TrustSummaryModelTests : BaseOverviewAreaModelTests<TrustSummaryModel>
{
    public TrustSummaryModelTests()
    {
        Sut = new TrustSummaryModel(
                MockDataSourceService.Object,
                new MockLogger<TrustSummaryModel>().Object,
                MockTrustService.Object)
            { Uid = TrustUid };
    }

    [Fact]
    public override async Task OnGetAsync_should_set_active_SubNavigationLink_to_current_subpage()
    {
        _ = await Sut.OnGetAsync();

        Sut.SubNavigationLinks.Should().ContainSingle(l => l.LinkIsActive)
            .Which.SubPageLink.Should().Be("./TrustSummary");
    }

    [Fact]
    public async Task OnGetAsync_sets_list_of_local_authorities()
    {
        var overviewWithLocalAuthorities = BaseTrustOverviewServiceModel with
        {
            AcademiesByLocalAuthority = new Dictionary<string, int>
            {
                { "localAuth1", 6 },
                { "localAuth2", 1 },
                { "localAuth3", 10 },
                { "localAuth10", 4 },
                { "localAuth11", 8 },
                { "localAuth12", 6 }
            }
        };
        SetupTrustOverview(overviewWithLocalAuthorities);

        await Sut.OnGetAsync();

        Sut.AcademiesInEachLocalAuthority
            .Should()
            .BeEquivalentTo(new (string Authority, int Total)[]
            {
                ("localAuth3", 10),
                ("localAuth11", 8),
                ("localAuth1", 6),
                ("localAuth12", 6),
                ("localAuth10", 4),
                ("localAuth2", 1)
            }, options => options.WithStrictOrdering());
    }

    [Fact]
    public override async Task OnGetAsync_should_configure_TrustPageMetadata_SubPageName()
    {
        _ = await Sut.OnGetAsync();

        Sut.TrustPageMetadata.SubPageName.Should().Be(ViewConstants.OverviewTrustSummaryPageName);
    }
}
