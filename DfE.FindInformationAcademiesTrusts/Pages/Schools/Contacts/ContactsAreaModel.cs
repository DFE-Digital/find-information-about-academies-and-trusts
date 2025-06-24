using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Pages.Shared;
using DfE.FindInformationAcademiesTrusts.Pages.Shared.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.School;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.Pages.Schools.Contacts;

public class ContactsAreaModel(
    ISchoolService schoolService,
    ITrustService trustService,
    IDataSourceService dataSourceService)
    : SchoolAreaModel(schoolService, trustService)
{
    public const string PageName = "Contacts";

    public override PageMetadata PageMetadata => base.PageMetadata with { PageName = PageName };

    public override async Task<IActionResult> OnGetAsync()
    {
        var pageResult = await base.OnGetAsync();

        if (pageResult.GetType() == typeof(NotFoundResult)) return pageResult;

        var giasDataSource = await dataSourceService.GetAsync(Source.Gias);

        DataSourcesPerPage =
        [
            new DataSourcePageListEntry(InSchoolModel.SubPageName(SchoolCategory),
                [new DataSourceListEntry(giasDataSource, "Head teacher name")])
        ];

        return Page();
    }
}
