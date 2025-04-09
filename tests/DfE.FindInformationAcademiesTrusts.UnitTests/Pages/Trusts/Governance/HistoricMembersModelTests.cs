using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Governance;
using DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts.Governance;

public class HistoricMembersModelTests : BaseGovernanceAreaModelTests<HistoricMembersModel>
{
    public HistoricMembersModelTests()
    {
        Sut = new HistoricMembersModel(MockDataSourceService,
                MockLogger.CreateLogger<HistoricMembersModel>(), MockTrustService)
            { Uid = TrustUid };
    }

    [Fact]
    public override async Task OnGetAsync_should_configure_TrustPageMetadata_SubPageName()
    {
        _ = await Sut.OnGetAsync();

        Sut.TrustPageMetadata.SubPageName.Should().Be("Historic members");
    }
}
