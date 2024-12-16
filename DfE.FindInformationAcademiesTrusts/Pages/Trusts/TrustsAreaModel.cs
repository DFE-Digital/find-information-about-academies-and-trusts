using System.Text.RegularExpressions;
using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Pages.Shared;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Academies;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Contacts;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Governance;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Ofsted;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Overview;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts;

public abstract class TrustsAreaModel(
    IDataSourceService dataSourceService,
    ITrustService trustService,
    ILogger<TrustsAreaModel> logger)
    : BasePageModel, ITrustsAreaModel
{
    protected readonly IDataSourceService DataSourceService = dataSourceService;
    protected readonly ITrustService TrustService = trustService;

    [BindProperty(SupportsGet = true)] public string Uid { get; set; } = "";
    public TrustSummaryServiceModel TrustSummary { get; set; } = default!;
    public List<DataSourceListEntry> DataSources { get; set; } = [];
    public virtual TrustPageMetadata TrustPageMetadata => new(TrustSummary.Name, ModelState.IsValid);

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

    public TrustNavigationLinkModel[] NavigationLinks { get; set; } = [];

    public virtual async Task<IActionResult> OnGetAsync()
    {
        var trustSummary = await TrustService.GetTrustSummaryAsync(Uid);

        if (trustSummary == null)
        {
            return new NotFoundResult();
        }

        TrustSummary = trustSummary;

        InitializeNavigationLinks();

        return Page();
    }

    private void InitializeNavigationLinks()
    {
        NavigationLinks =
        [
            new TrustNavigationLinkModel("Overview", "/Trusts/Overview/TrustDetails", Uid, this is OverviewAreaModel,
                "overview-nav"),
            new TrustNavigationLinkModel("Contacts", "/Trusts/Contacts/InDfe", Uid, this is ContactsAreaModel,
                "contacts-nav"),
            new TrustNavigationLinkModel($"Academies ({TrustSummary.NumberOfAcademies})", "/Trusts/Academies/Details",
                Uid, this is AcademiesPageModel, "academies-nav"),
            new TrustNavigationLinkModel("Ofsted", "/Trusts/Ofsted/CurrentRatings", Uid, this is OfstedAreaModel,
                "ofsted-nav"),
            new TrustNavigationLinkModel("Governance", "/Trusts/Governance/TrustLeadership", Uid,
                this is GovernanceAreaModel, "governance-nav")
        ];
    }

    public TrustSubNavigationLinkModel[] SubNavigationLinks { get; set; } = [];
}
