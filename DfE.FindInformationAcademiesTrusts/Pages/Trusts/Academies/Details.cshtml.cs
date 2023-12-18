using DfE.FindInformationAcademiesTrusts.Data;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.Academies;

public class AcademiesDetailsModel : TrustsAreaModel, IAcademiesAreaModel
{
    public IOtherServicesLinkBuilder LinkBuilder { get; }

    public AcademiesDetailsModel(ITrustProvider trustProvider, IDataSourceProvider dataSourceProvider,
        IOtherServicesLinkBuilder linkBuilder, ILogger<AcademiesDetailsModel> logger) : base(trustProvider,
        dataSourceProvider, logger, "Academies in this trust")
    {
        PageTitle = "Academies details";
        LinkBuilder = linkBuilder;
    }

    public override async Task<IActionResult> OnGetAsync()
    {
        var pageResult = await base.OnGetAsync();

        if (pageResult.GetType() == typeof(NotFoundResult)) return pageResult;

        DataSources.Add(new DataSourceListEntry(await DataSourceProvider.GetGiasUpdated(),
            new List<string> { "Details" }));

        return pageResult;
    }

    public string TabName => "Details";
}
