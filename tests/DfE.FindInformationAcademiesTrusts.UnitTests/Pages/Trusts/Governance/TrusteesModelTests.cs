using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Governance;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts.Governance;

public class TrusteesModelTests : BaseGovernanceAreaModelTests<TrusteesModel>
{
    public TrusteesModelTests()
    {
        Sut = new TrusteesModel(MockDataSourceService, MockTrustService)
            { Uid = TrustUid };
    }

    [Fact]
    public override async Task OnGetAsync_should_configure_TrustPageMetadata_SubPageName()
    {
        _ = await Sut.OnGetAsync();

        Sut.TrustPageMetadata.SubPageName.Should().Be("Trustees");
    }
}
