using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.Academies;

public class AcademiesDetailsModel : TrustsAreaModel, IAcademiesAreaModel
{
    public Trust Trust { get; set; } = default!;
    public IOtherServicesLinkBuilder LinkBuilder { get; }

    public AcademiesDetailsModel(ITrustProvider trustProvider, IDataSourceProvider dataSourceProvider,
        IOtherServicesLinkBuilder linkBuilder, ILogger<AcademiesDetailsModel> logger,
        ITrustService trustService) : base(trustProvider,
        dataSourceProvider, trustService, logger, "Academies in this trust")
    {
        PageTitle = "Academies details";
        LinkBuilder = linkBuilder;
    }

    public override async Task<IActionResult> OnGetAsync()
    {
        var pageResult = await base.OnGetAsync();

        if (pageResult.GetType() == typeof(NotFoundResult)) return pageResult;

        Trust = (await TrustProvider.GetTrustByUidAsync(Uid))!;

        DataSources.Add(new DataSourceListEntry(await DataSourceProvider.GetGiasUpdated(),
            new List<string> { "Details" }));

        return pageResult;
    }

    public string TabName => "Details";
}
