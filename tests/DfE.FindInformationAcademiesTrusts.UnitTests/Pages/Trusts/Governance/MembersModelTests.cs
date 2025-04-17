using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Governance;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts.Governance;

public class MembersModelTests : BaseGovernanceAreaModelTests<MembersModel>
{
    public MembersModelTests()
    {
        Sut = new MembersModel(MockDataSourceService, MockTrustService)
            { Uid = TrustUid };
    }

    [Fact]
    public override async Task OnGetAsync_should_configure_TrustPageMetadata_SubPageName()
    {
        _ = await Sut.OnGetAsync();

        Sut.PageMetadata.SubPageName.Should().Be("Members");
    }
}
