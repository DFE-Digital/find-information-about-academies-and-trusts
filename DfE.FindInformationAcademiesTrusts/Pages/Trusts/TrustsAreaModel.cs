using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.ServiceModels;
using DfE.FindInformationAcademiesTrusts.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts;

public class TrustsAreaModel(
    ITrustProvider trustProvider,
    IDataSourceService dataSourceService,
    ITrustService trustService,
    ILogger<TrustsAreaModel> logger,
    string pageName)
    : PageModel, ITrustsAreaModel
{
    protected readonly IDataSourceService DataSourceService = dataSourceService;
    protected readonly ITrustProvider TrustProvider = trustProvider;
    protected readonly ITrustService TrustService = trustService;

    [BindProperty(SupportsGet = true)] public string Uid { get; set; } = "";
    public TrustSummaryServiceModel TrustSummary { get; set; } = default!;
    public List<DataSourceListEntry> DataSources { get; set; } = [];
    public string PageName { get; init; } = pageName;
    public string? PageTitle { get; init; }
    public string Section => ViewConstants.AboutTheTrustSectionName;

    public string MapDataSourceToName(DataSourceServiceModel dataSource)
    {
        switch (dataSource.Source)
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
                logger.LogError("Data source {source} does not map to known type", dataSource);
                return "Unknown";
        }
    }

    public virtual async Task<IActionResult> OnGetAsync()
    {
        var trustSummary = await TrustService.GetTrustSummaryAsync(Uid);

        if (trustSummary == null)
        {
            return new NotFoundResult();
        }

        TrustSummary = trustSummary;

        return Page();
    }
}
