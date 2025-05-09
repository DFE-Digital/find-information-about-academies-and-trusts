using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Pages.Schools.Overview;
using DfE.FindInformationAcademiesTrusts.Pages.Shared.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Schools.Overview;

public abstract class BaseOverviewAreaModelTests<T> : BaseSchoolPageTests<T> where T : OverviewAreaModel
{
    protected readonly DataSourceServiceModel GiasDataSource =
        new(Source.Gias, new DateTime(2023, 11, 9), UpdateFrequency.Daily);

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

    [Fact]
    public async Task OnGetAsync_sets_correct_data_source_list_for_academy()
    {
        Sut.Urn = DummyAcademySummary.Urn;

        _ = await Sut.OnGetAsync();
        await MockDataSourceService.Received(1).GetAsync(Source.Gias);

        Sut.DataSourcesPerPage.Should().BeEquivalentTo([
            new DataSourcePageListEntry("Academy details", [new DataSourceListEntry(GiasDataSource)])
        ]);
    }

    [Fact]
    public async Task OnGetAsync_sets_correct_data_source_list_for_school()
    {
        Sut.Urn = DummySchoolSummary.Urn;

        _ = await Sut.OnGetAsync();
        await MockDataSourceService.Received(1).GetAsync(Source.Gias);

        Sut.DataSourcesPerPage.Should().BeEquivalentTo([
            new DataSourcePageListEntry("School details", [new DataSourceListEntry(GiasDataSource)])
        ]);
    }
}
