using DfE.FindInformationAcademiesTrusts.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts;

public class TrustsAreaModel : PageModel, ITrustsAreaModel
{
    private readonly ITrustProvider _trustProvider;
    protected readonly IDataSourceProvider DataSourceProvider;
    private readonly ILogger<TrustsAreaModel> _logger;

    public TrustsAreaModel(ITrustProvider trustProvider, IDataSourceProvider dataSourceProvider,
        ILogger<TrustsAreaModel> logger, string pageName)
    {
        _trustProvider = trustProvider;
        DataSourceProvider = dataSourceProvider;
        _logger = logger;
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
                return "Get information about schools";
            case Source.Mstr:
                return "Get information about schools (internal use only, do not share outside of DfE)";
            case Source.Cdm:
                return "RSD (Regional Services Division) service support team";
            case Source.Mis:
                return "State-funded school inspections and outcomes: management information";
            case Source.ExploreEducationStatistics:
                return "Explore education statistics";
            default:
                _logger.LogError("Data source {source} does not map to known type", source);
                return "Unknown";
        }
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
