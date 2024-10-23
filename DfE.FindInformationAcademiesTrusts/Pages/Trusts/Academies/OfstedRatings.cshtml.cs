using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Services.Academy;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Export;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.Academies;

public class OfstedRatingsModel : AcademiesPageModel
{
    public AcademyOfstedServiceModel[] Academies { get; set; } = default!;
    private IAcademyService AcademyService { get; }

    public OfstedRatingsModel(ITrustProvider trustProvider, IDataSourceService dataSourceService,
        ILogger<OfstedRatingsModel> logger, ITrustService trustService, IAcademyService academyService, IExportService exportService, IDateTimeProvider dateTimeProvider) : base(dataSourceService, trustService, exportService, logger, dateTimeProvider)
    {
        PageTitle = "Academies Ofsted ratings";
        TabName = "Ofsted ratings";
        AcademyService = academyService;
    }

    public override async Task<IActionResult> OnGetAsync()
    {
        var pageResult = await base.OnGetAsync();

        if (pageResult.GetType() == typeof(NotFoundResult)) return pageResult;

        Academies = await AcademyService.GetAcademiesInTrustOfstedAsync(Uid);

        DataSources.Add(new DataSourceListEntry(await DataSourceService.GetAsync(Source.Gias),
            new[] { "Date joined trust" }));

        DataSources.Add(new DataSourceListEntry(await DataSourceService.GetAsync(Source.Mis),
            new[]
            {
                "Current Ofsted rating", "Date of last inspection", "Previous Ofsted rating",
                "Date of previous inspection"
            }));

        return pageResult;
    }
}
