using System.Text.RegularExpressions;
using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Pages.Shared;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts;

public class TrustsAreaModel(
    ITrustProvider trustProvider,
    IDataSourceService dataSourceService,
    ITrustService trustService,
    ILogger<TrustsAreaModel> logger,
    string pageName)
    : BasePageModel, ITrustsAreaModel
{
    protected readonly IDataSourceService DataSourceService = dataSourceService;
    protected readonly ITrustProvider TrustProvider = trustProvider;
    protected readonly ITrustService TrustService = trustService;

    [BindProperty(SupportsGet = true)] public string Uid { get; set; } = "";
    public TrustSummaryServiceModel TrustSummary { get; set; } = default!;
    public List<DataSourceListEntry> DataSources { get; set; } = [];
    public string PageName { get; init; } = pageName;
    public string? PageTitle { get; set; }
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
            case Source.FiatDb:
                return "Find information about academies and trusts";
            default:
                logger.LogError("Data source {source} does not map to known type", dataSource);
                return "Unknown";
        }
    }

    public string MapDataSourceToTestId(DataSourceListEntry source)
    {
        return
            $@"data-source-{source.DataSource.Source.ToString().ToLowerInvariant()}-{string.Join("-", source.Fields.Select(s => Regex.Replace(s.ToLowerInvariant().Trim(), @"\s+", "-", RegexOptions.Compiled, TimeSpan.FromMilliseconds(500))))}";
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
