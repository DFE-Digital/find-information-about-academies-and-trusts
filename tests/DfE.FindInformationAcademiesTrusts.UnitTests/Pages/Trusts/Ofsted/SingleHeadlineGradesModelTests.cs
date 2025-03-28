using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Ofsted;
using DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts.Ofsted;

public class SingleHeadlineGradesModelTests : BaseOfstedAreaModelTests<SingleHeadlineGradesModel>
{
    public SingleHeadlineGradesModelTests()
    {
        Sut = new SingleHeadlineGradesModel(MockDataSourceService,
                MockTrustService,
                MockAcademyService,
                MockExportService,
                MockDateTimeProvider,
                MockLogger.CreateLogger<SingleHeadlineGradesModel>()
            )
            { Uid = TrustUid };
    }

    [Fact]
    public override async Task OnGetAsync_should_set_active_SubNavigationLink_to_current_subpage()
    {
        _ = await Sut.OnGetAsync();

        Sut.SubNavigationLinks.Should().ContainSingle(l => l.LinkIsActive)
            .Which.SubPageLink.Should().Be("./SingleHeadlineGrades");
    }

    [Fact]
    public override async Task OnGetAsync_should_configure_TrustPageMetadata_SubPageName()
    {
        _ = await Sut.OnGetAsync();

        Sut.TrustPageMetadata.SubPageName.Should().Be("Single headline grades");
    }
}
