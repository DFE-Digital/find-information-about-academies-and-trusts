using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Contacts;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts.Contacts;

public class InDfeModelTests : BaseContactsAreaModelTests<InDfeModel>
{
    public InDfeModelTests()
    {
        Sut = new InDfeModel(MockDataSourceService,
                MockTrustService)
            { Uid = TrustUid };
    }

    [Fact]
    public override async Task OnGetAsync_should_configure_TrustPageMetadata_SubPageName()
    {
        _ = await Sut.OnGetAsync();

        Sut.PageMetadata.SubPageName.Should().Be("In DfE");
    }
}
