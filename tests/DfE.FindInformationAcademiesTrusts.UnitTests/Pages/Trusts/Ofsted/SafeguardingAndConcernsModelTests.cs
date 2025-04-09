using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Ofsted;
using DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts.Ofsted;

public class SafeguardingAndConcernsModelTests : BaseOfstedAreaModelTests<SafeguardingAndConcernsModel>
{
    public SafeguardingAndConcernsModelTests()
    {
        Sut = new SafeguardingAndConcernsModel(MockDataSourceService,
                MockTrustService,
                MockAcademyService,
                MockOfstedDataExportService,
                MockDateTimeProvider,
                MockLogger.CreateLogger<SafeguardingAndConcernsModel>()
            )
            { Uid = TrustUid };
    }

    [Fact]
    public override async Task OnGetAsync_should_configure_TrustPageMetadata_SubPageName()
    {
        _ = await Sut.OnGetAsync();

        Sut.TrustPageMetadata.SubPageName.Should().Be("Safeguarding and concerns");
    }
}
