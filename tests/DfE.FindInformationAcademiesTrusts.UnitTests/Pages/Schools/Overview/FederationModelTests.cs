using DfE.FindInformationAcademiesTrusts.Pages.Schools.Overview;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Schools.Overview;

public class FederationModelTests : BaseOverviewAreaModelTests<FederationModel>
{
    public FederationModelTests()
    {
        Sut = new FederationModel(MockSchoolService, MockTrustService, MockDataSourceService);
        Sut.Urn = SchoolUrn;
    }

    [Fact]
    public override async Task OnGetAsync_should_configure_PageMetadata_SubPageName()
    {
        await Sut.OnGetAsync();
        
        Sut.PageMetadata.SubPageName.Should().Be("Federation details");
    }
}
