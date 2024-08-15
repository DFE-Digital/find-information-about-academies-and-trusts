using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.Academies;

public class OfstedRatingsModel : TrustsAreaModel, IAcademiesAreaModel
{
    public Trust Trust { get; set; } = default!;

    public OfstedRatingsModel(ITrustProvider trustProvider, IDataSourceService dataSourceService,
        ILogger<OfstedRatingsModel> logger, ITrustService trustService) : base(trustProvider, dataSourceService,
        trustService, logger, "Academies in this trust")
    {
        PageTitle = "Academies Ofsted ratings";
    }

    public override async Task<IActionResult> OnGetAsync()
    {
        var pageResult = await base.OnGetAsync();

        if (pageResult.GetType() == typeof(NotFoundResult)) return pageResult;

        Trust = (await TrustProvider.GetTrustByUidAsync(Uid))!;

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

    public string TabName => "Ofsted ratings";
}
