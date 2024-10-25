using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Services.Academy;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Export;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.Academies;

public class AcademiesDetailsModel : AcademiesPageModel
{
    public AcademyDetailsServiceModel[] Academies { get; set; } = default!;
    public IOtherServicesLinkBuilder LinkBuilder { get; }
    private IAcademyService AcademyService { get; }

    public AcademiesDetailsModel(IDataSourceService dataSourceService,
        IOtherServicesLinkBuilder linkBuilder, ILogger<AcademiesDetailsModel> logger,
        ITrustService trustService, IAcademyService academyService, IExportService exportService, IDateTimeProvider dateTimeProvider) : base(dataSourceService, trustService, exportService, logger, dateTimeProvider)
    {
        PageTitle = "Academies details";
        TabName = "Details";
        LinkBuilder = linkBuilder;
        AcademyService = academyService;
    }

    public override async Task<IActionResult> OnGetAsync()
    {
        var pageResult = await base.OnGetAsync();

        if (pageResult.GetType() == typeof(NotFoundResult)) return pageResult;

        Academies = await AcademyService.GetAcademiesInTrustDetailsAsync(Uid);

        DataSources.Add(new DataSourceListEntry(await DataSourceService.GetAsync(Source.Gias),
            new List<string> { "Details" }));

        return pageResult;
    }

}
