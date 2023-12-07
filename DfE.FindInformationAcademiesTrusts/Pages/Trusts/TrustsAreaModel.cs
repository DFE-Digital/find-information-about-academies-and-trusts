using DfE.FindInformationAcademiesTrusts.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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
    public IEnumerable<DataSourceListEntry> DataSources { get; set; } = default!;
    public string PageName { get; init; }
    public string? PageTitle { get; init; }
    public string Section => ViewConstants.AboutTheTrustSectionName;

    internal async Task<DataSource?> GetGiasDataUpdated()
    {
        return await _dataSourceProvider.GetGIASUpdated();
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
