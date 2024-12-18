using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Services.Academy;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Export;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.Academies;

public class AcademiesDetailsModel(
    IDataSourceService dataSourceService,
    IOtherServicesLinkBuilder linkBuilder,
    ILogger<AcademiesDetailsModel> logger,
    ITrustService trustService,
    IAcademyService academyService,
    IExportService exportService,
    IDateTimeProvider dateTimeProvider)
    : AcademiesPageModel(dataSourceService, trustService, exportService, logger,
        dateTimeProvider)
{
    public override TrustPageMetadata TrustPageMetadata => base.TrustPageMetadata with { TabName = "Details" };
    public AcademyDetailsServiceModel[] Academies { get; set; } = default!;
    public IOtherServicesLinkBuilder LinkBuilder { get; } = linkBuilder;

    public override async Task<IActionResult> OnGetAsync()
    {
        var pageResult = await base.OnGetAsync();

        if (pageResult.GetType() == typeof(NotFoundResult)) return pageResult;

        Academies = await academyService.GetAcademiesInTrustDetailsAsync(Uid);

        DataSources.Add(new DataSourceListEntry(await DataSourceService.GetAsync(Source.Gias), ["Details"]));

        return pageResult;
    }
}
