using DfE.FindInformationAcademiesTrusts.Pages;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Overview;
using DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts.Overview;

public class ReferenceNumbersModelTests : BaseOverviewAreaModelTests<ReferenceNumbersModel>
{
    public ReferenceNumbersModelTests()
    {
        Sut = new ReferenceNumbersModel(
                MockDataSourceService,
                new MockLogger<ReferenceNumbersModel>().Object,
                MockTrustService.Object)
            { Uid = TrustUid };
    }

    [Fact]
    public override async Task OnGetAsync_should_set_active_SubNavigationLink_to_current_subpage()
    {
        _ = await Sut.OnGetAsync();

        Sut.SubNavigationLinks.Should().ContainSingle(l => l.LinkIsActive)
            .Which.SubPageLink.Should().Be("./ReferenceNumbers");
    }

    [Fact]
    public override async Task OnGetAsync_should_configure_TrustPageMetadata_SubPageName()
    {
        _ = await Sut.OnGetAsync();

        Sut.TrustPageMetadata.SubPageName.Should().Be(ViewConstants.OverviewReferenceNumbersPageName);
    }
}
