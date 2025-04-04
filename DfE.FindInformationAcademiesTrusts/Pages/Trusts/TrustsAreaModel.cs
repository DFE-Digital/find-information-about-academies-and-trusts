using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Extensions;
using DfE.FindInformationAcademiesTrusts.Pages.Shared;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Academies;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Contacts;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts.FinancialDocuments;
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
    public List<DataSourcePageListEntry> DataSourcesPerPage { get; set; } = [];
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
            case Source.Prepare:
                return "Prepare";
            case Source.Complete:
                return "Complete";
            case Source.ManageFreeSchoolProjects:
                return "Manage free school projects";
            default:
                logger.LogError("Data source {source} does not map to known type", dataSource);
                return "Unknown";
        }
    }

    public string MapDataSourceToTestId(DataSourceListEntry source)
    {
        return $"data-source-{source.DataSource.Source.ToString()}-{source.DataField}".Kebabify();
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
            new TrustNavigationLinkModel(ViewConstants.OverviewPageName, "/Trusts/Overview/TrustDetails", Uid,
                this is OverviewAreaModel, "overview-nav"),
            new TrustNavigationLinkModel(ViewConstants.ContactsPageName, "/Trusts/Contacts/InDfe", Uid,
                this is ContactsAreaModel, "contacts-nav"),
            new TrustNavigationLinkModel($"{ViewConstants.AcademiesPageName} ({TrustSummary.NumberOfAcademies})",
                "/Trusts/Academies/InTrust/Details", Uid, this is AcademiesAreaModel, "academies-nav"),
            new TrustNavigationLinkModel(ViewConstants.OfstedPageName, "/Trusts/Ofsted/SingleHeadlineGrades", Uid,
                this is OfstedAreaModel, "ofsted-nav"),
            new TrustNavigationLinkModel(ViewConstants.FinancialDocumentsPageName,
                "/Trusts/FinancialDocuments/FinancialStatements", Uid, this is FinancialDocumentsAreaModel,
                "financial-documents-nav"),
            new TrustNavigationLinkModel(ViewConstants.GovernancePageName, "/Trusts/Governance/TrustLeadership", Uid,
                this is GovernanceAreaModel, "governance-nav")
        ];
    }

    public TrustSubNavigationLinkModel[] SubNavigationLinks { get; set; } = [];

    public TrustSubNavigationLinkModel GetSubPageLink<TTo>(string linkText, string aspPage) where TTo : TrustsAreaModel
    {
        return new TrustSubNavigationLinkModel
        (
            linkText,
            aspPage,
            Uid,
            TrustPageMetadata.PageName!,
            this is TTo
        );
    }
}
