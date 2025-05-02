using DfE.FindInformationAcademiesTrusts.Pages.Schools.Overview;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Schools.Overview;

public abstract class BaseOverviewAreaModelTests<T> : BaseSchoolPageTests<T> where T : OverviewAreaModel
{
    [Fact]
    public override async Task OnGetAsync_should_configure_PageMetadata_PageName()
    {
        Sut.Urn = DummySchoolSummary.Urn;

        await Sut.OnGetAsync();

        Sut.PageMetadata.PageName.Should().Be("Overview");
    }

    [Fact]
    public override async Task OnGetAsync_sets_correct_data_source_list()
    {
        await Sut.OnGetAsync();

        Sut.DataSourcesPerPage.Should().BeEmpty();
    }
}
