using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Governance;
using DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts.Governance;

public class TrustLeadershipModelTests : BaseGovernanceAreaModelTests<TrustLeadershipModel>
{
    public TrustLeadershipModelTests()
    {
        Sut = new TrustLeadershipModel(MockDataSourceService,
                MockLogger.CreateLogger<TrustLeadershipModel>(), MockTrustService)
            { Uid = TrustUid };
    }

    [Fact]
    public override async Task OnGetAsync_should_configure_TrustPageMetadata_SubPageName()
    {
        _ = await Sut.OnGetAsync();

        Sut.TrustPageMetadata.SubPageName.Should().Be("Trust leadership");
    }
}
