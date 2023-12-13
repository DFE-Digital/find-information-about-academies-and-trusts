using System.Runtime.CompilerServices;
using DfE.FindInformationAcademiesTrusts.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

[assembly: InternalsVisibleTo("DfE.FindInformationAcademiesTrusts.UnitTests")]

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts;

public class TrustsAreaModel : PageModel, ITrustsAreaModel
{
    private readonly ITrustProvider _trustProvider;
    private readonly IDataSourceProvider _dataSourceProvider;

    public TrustsAreaModel(ITrustProvider trustProvider, IDataSourceProvider dataSourceProvider, string pageName)
    {
        _trustProvider = trustProvider;
        _dataSourceProvider = dataSourceProvider;
        PageName = pageName;
    }

    [BindProperty(SupportsGet = true)] public string Uid { get; set; } = "";
    public Trust Trust { get; set; } = default!;
    public List<DataSourceListEntry> DataSources { get; set; } = new();
    public string PageName { get; init; }
    public string? PageTitle { get; init; }
    public string Section => ViewConstants.AboutTheTrustSectionName;

    public string MapDataSourceToName(DataSource source)
    {
        switch (source.Source)
        {
            case Source.Gias:
            case Source.Mstr:
                return "Get information about schools";
            case Source.Cdm:
                return "RSD Service Support team";
            case Source.Mis:
                return "State-funded school inspections and outcomes: management information";
            default:
                return "";
        }
    }

    internal async Task<DataSource?> GetGiasDataUpdated()
    {
        return await _dataSourceProvider.GetGiasUpdated();
    }

    internal async Task<DataSource?> GetMstrDataUpdated()
    {
        return await _dataSourceProvider.GetMstrUpdated();
    }

    internal async Task<DataSource?> GetCdmDateUpdated()
    {
        return await _dataSourceProvider.GetCdmUpdated();
    }


    internal async Task<DataSource?> GetMisEstablishmentsDataUpdated()
    {
        return await _dataSourceProvider.GetMisEstablishmentsUpdated();
    }


    internal async Task<DataSource?> GetMisFurtherEducationEstablishmentsDataUpdated()
    {
        return await _dataSourceProvider.GetMisFurtherEducationEstablishmentsUpdated();
    }


    public virtual async Task<IActionResult> OnGetAsync()
    {
        var trust = await _trustProvider.GetTrustByUidAsync(Uid);

        if (trust == null)
        {
            return new NotFoundResult();
        }

        Trust = trust;
        return Page();
    }
}
