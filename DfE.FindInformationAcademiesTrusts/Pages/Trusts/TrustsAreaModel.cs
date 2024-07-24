using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts;

public class TrustsAreaModel(
    ITrustProvider trustProvider,
    IDataSourceProvider dataSourceProvider,
    ILogger<TrustsAreaModel> logger,
    string pageName)
    : PageModel, ITrustsAreaModel
{
    protected readonly IDataSourceProvider DataSourceProvider = dataSourceProvider;

    protected readonly ITrustProvider TrustProvider = trustProvider;

    [BindProperty(SupportsGet = true)] public string Uid { get; set; } = "";
    public TrustSummaryDto TrustSummaryDto { get; set; } = default!;
    public List<DataSourceListEntry> DataSources { get; set; } = new();
    public string PageName { get; init; } = pageName;
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
                logger.LogError("Data source {source} does not map to known type", source);
                return "Unknown";
        }
    }

    public virtual async Task<IActionResult> OnGetAsync()
    {
        var trustSummaryDto = await TrustProvider.GetTrustSummaryAsync(Uid);

        if (trustSummaryDto == null)
        {
            return new NotFoundResult();
        }

        TrustSummaryDto = trustSummaryDto;

        return Page();
    }
}
