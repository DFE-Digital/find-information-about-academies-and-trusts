using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Ofsted;
using DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts.Ofsted;

public class CurrentRatingsModelTests : BaseOfstedAreaModelTests<CurrentRatingsModel>
{
    public CurrentRatingsModelTests()
    {
        Sut = new CurrentRatingsModel(MockDataSourceService,
                MockTrustService,
                MockAcademyService,
                MockOfstedDataExportService,
                MockDateTimeProvider,
                MockLogger.CreateLogger<CurrentRatingsModel>()
            )
            { Uid = TrustUid };
    }

    [Fact]
    public override async Task OnGetAsync_should_configure_TrustPageMetadata_SubPageName()
    {
        _ = await Sut.OnGetAsync();

        Sut.TrustPageMetadata.SubPageName.Should().Be("Current ratings");
    }
}
