using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Services;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.Academies;

public class OfstedRatingsModel : TrustsAreaModel, IAcademiesAreaModel
{
    public Trust Trust { get; set; } = default!;

    public OfstedRatingsModel(ITrustProvider trustProvider, IDataSourceProvider dataSourceProvider,
        ILogger<OfstedRatingsModel> logger, ITrustService trustService) : base(trustProvider, dataSourceProvider,
        trustService, logger, "Academies in this trust")
    {
        PageTitle = "Academies Ofsted ratings";
    }

    public override async Task<IActionResult> OnGetAsync()
    {
        var pageResult = await base.OnGetAsync();

        if (pageResult.GetType() == typeof(NotFoundResult)) return pageResult;

        Trust = (await TrustProvider.GetTrustByUidAsync(Uid))!;

        DataSources.Add(new DataSourceListEntry(await DataSourceProvider.GetGiasUpdated(),
            new[] { "Date joined trust" }));

        DataSources.Add(new DataSourceListEntry(await DataSourceProvider.GetMisEstablishmentsUpdated(),
            new[]
            {
                "Current Ofsted rating", "Date of last inspection", "Previous Ofsted rating",
                "Date of previous inspection"
            }));

        return pageResult;
    }

    public string TabName => "Ofsted ratings";
}
