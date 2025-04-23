using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Pages.Schools.Overview;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Schools.Overview;

public class DetailsModelTests : BaseOverviewAreaModelTests<DetailsModel>
{
    public DetailsModelTests()
    {
        Sut = new DetailsModel(MockSchoolService, MockTrustService);
    }

    [Fact]
    public override async Task OnGetAsync_should_configure_PageMetadata_SubPageName()
    {
        await OnGetAsync_should_configure_PageMetadata_SubPageName_for_school();
        await OnGetAsync_should_configure_PageMetadata_SubPageName_for_academy();
    }

    private async Task OnGetAsync_should_configure_PageMetadata_SubPageName_for_school()
    {
        Sut.Urn = SchoolUrn;

        MockSchoolService.GetSchoolSummaryAsync(SchoolUrn)
            .Returns(DummySchoolSummary with { Category = SchoolCategory.LaMaintainedSchool });

        await Sut.OnGetAsync();

        Sut.PageMetadata.SubPageName.Should().Be("School details");
    }

    private async Task OnGetAsync_should_configure_PageMetadata_SubPageName_for_academy()
    {
        Sut.Urn = AcademyUrn;

        MockSchoolService.GetSchoolSummaryAsync(AcademyUrn)
            .Returns(DummySchoolSummary with { Category = SchoolCategory.Academy });

        await Sut.OnGetAsync();

        Sut.PageMetadata.SubPageName.Should().Be("Academy details");
    }

    [Theory]
    [InlineData(SchoolCategory.LaMaintainedSchool, "School details")]
    [InlineData(SchoolCategory.Academy, "Academy details")]
    public void SubPageName_should_include_school_type(SchoolCategory schoolCategory, string expectedSubPageName)
    {
        DetailsModel.SubPageName(schoolCategory).Should().Be(expectedSubPageName);
    }

    [Fact]
    public void SubPageName_should_throw_for_unrecognised_school_type()
    {
        var act = () => DetailsModel.SubPageName((SchoolCategory)999);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }
}
