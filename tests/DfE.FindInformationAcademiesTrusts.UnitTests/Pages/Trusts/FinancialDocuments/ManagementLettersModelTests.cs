using DfE.FindInformationAcademiesTrusts.Pages.Trusts.FinancialDocuments;
using DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts.FinancialDocuments;

public class ManagementLettersModelTests : BaseFinancialDocumentsAreaModelTests<ManagementLettersModel>
{
    public ManagementLettersModelTests()
    {
        Sut = new ManagementLettersModel(MockDataSourceService,
                new MockLogger<ManagementLettersModel>().Object,
                MockTrustService.Object
            )
            { Uid = TrustUid };
    }

    [Fact]
    public override async Task OnGetAsync_should_set_active_SubNavigationLink_to_current_subpage()
    {
        _ = await Sut.OnGetAsync();

        Sut.SubNavigationLinks.Should().ContainSingle(l => l.LinkIsActive)
            .Which.SubPageLink.Should().Be("./ManagementLetters");
    }

    [Fact]
    public override async Task OnGetAsync_should_configure_TrustPageMetadata_SubPageName()
    {
        _ = await Sut.OnGetAsync();

        Sut.TrustPageMetadata.SubPageName.Should().Be("Management letters");
    }

    [Fact]
    public void InternalUseOnly_should_be_true()
    {
        Sut.InternalUseOnly.Should().BeTrue();
    }
}
