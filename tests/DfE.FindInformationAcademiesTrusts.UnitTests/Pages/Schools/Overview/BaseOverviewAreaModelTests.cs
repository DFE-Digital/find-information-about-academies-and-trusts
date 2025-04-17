using DfE.FindInformationAcademiesTrusts.Pages.Schools.Overview;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Schools.Overview;

public abstract class BaseOverviewAreaModelTests<T> : BaseSchoolPageTests<T> where T : OverviewAreaModel
{
    [Fact]
    public override Task OnGetAsync_should_configure_PageMetadata_PageName()
    {
        Sut.PageMetadata.PageName.Should().Be("Overview");
        return Task.CompletedTask;
    }

    [Fact]
    public override Task OnGetAsync_sets_correct_data_source_list()
    {
        Sut.DataSourcesPerPage.Should().BeEmpty();
        return Task.CompletedTask;
    }
}
