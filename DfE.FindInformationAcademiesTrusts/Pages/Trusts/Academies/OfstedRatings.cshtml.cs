using DfE.FindInformationAcademiesTrusts.Data;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.Academies;

public class OfstedRatingsModel : TrustsAreaModel, IAcademiesAreaModel
{
    public OfstedRatingsModel(ITrustProvider trustProvider, IDataSourceProvider dataSourceProvider) : base(
        trustProvider, dataSourceProvider, "Academies in this trust")
    {
        PageTitle = "Academies Ofsted ratings";
    }

    public override async Task<IActionResult> OnGetAsync()
    {
        var pageResult = await base.OnGetAsync();

        if (pageResult.GetType() == typeof(NotFoundResult)) return pageResult;

        var giasSource = await GetGiasDataUpdated();
        if (giasSource is not null)
        {
            DataSources.Add(new DataSourceListEntry(giasSource,
                new List<string> { "Date joined trust", "Current Ofsted rating", "Date of last inspection" }));
        }

        var misSource = await GetMisEstablishmentsDataUpdated();
        if (misSource is not null)
        {
            DataSources.Add(new DataSourceListEntry(misSource,
                new List<string> { "Previous Ofsted rating", "Date of previous inspection" }));
        }

        return pageResult;
    }

    public string TabName => "Ofsted ratings";
}
