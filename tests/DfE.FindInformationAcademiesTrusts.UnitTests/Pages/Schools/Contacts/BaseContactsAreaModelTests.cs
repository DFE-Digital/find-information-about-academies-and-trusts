using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Pages.Schools.Contacts;
using DfE.FindInformationAcademiesTrusts.Pages.Shared.DataSource;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Schools.Contacts;

public abstract class BaseContactsAreaModelTests<T> : BaseSchoolPageTests<T> where T : ContactsAreaModel
{
    [Fact]
    public override async Task OnGetAsync_should_configure_PageMetadata_PageName()
    {
        await Sut.OnGetAsync();

        Sut.PageMetadata.PageName.Should().Be("Contacts");
    }

    [Fact]
    public override async Task OnGetAsync_sets_correct_data_source_list()
    {
        await OnGetAsync_sets_correct_data_source_list_for_academy();

        MockDataSourceService.ClearReceivedCalls();

        await OnGetAsync_sets_correct_data_source_list_for_school();
    }

    private async Task OnGetAsync_sets_correct_data_source_list_for_academy()
    {
        Sut.Urn = AcademyUrn;

        _ = await Sut.OnGetAsync();
        await MockDataSourceService.Received(1).GetAsync(Source.Gias);
        await MockDataSourceService.Received(1)
            .GetSchoolContactDataSourceAsync(AcademyUrn, SchoolContactRole.RegionsGroupLocalAuthorityLead);

        Sut.DataSourcesPerPage.Should().BeEquivalentTo([
            new DataSourcePageListEntry("Contacts in DfE", [
                new DataSourceListEntry(Mocks.MockDataSourceService.Fiat, "Regions group LA lead")
            ]),
            new DataSourcePageListEntry("In this academy", [
                new DataSourceListEntry(Mocks.MockDataSourceService.Gias, "Head teacher name")
            ])
        ]);
    }

    private async Task OnGetAsync_sets_correct_data_source_list_for_school()
    {
        Sut.Urn = SchoolUrn;

        _ = await Sut.OnGetAsync();
        await MockDataSourceService.Received(1).GetAsync(Source.Gias);
        await MockDataSourceService.Received(1)
            .GetSchoolContactDataSourceAsync(SchoolUrn, SchoolContactRole.RegionsGroupLocalAuthorityLead);

        Sut.DataSourcesPerPage.Should().BeEquivalentTo([
            new DataSourcePageListEntry("Contacts in DfE", [
                new DataSourceListEntry(Mocks.MockDataSourceService.Fiat, "Regions group LA lead")
            ]),
            new DataSourcePageListEntry("In this school", [
                new DataSourceListEntry(Mocks.MockDataSourceService.Gias, DataField: "Head teacher name")
            ])
        ]);
    }
}
