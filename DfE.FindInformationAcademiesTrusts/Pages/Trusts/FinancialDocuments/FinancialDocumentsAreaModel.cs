using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.FinancialDocuments;

public class FinancialDocumentsAreaModel(
    IDataSourceService dataSourceService,
    ITrustService trustService,
    ILogger<FinancialDocumentsAreaModel> logger)
    : TrustsAreaModel(dataSourceService, trustService, logger)
{
    public override TrustPageMetadata TrustPageMetadata =>
        base.TrustPageMetadata with { PageName = ViewConstants.FinancialDocumentsPageName };

    public virtual bool InternalUseOnly => true;

    public string[] FinancialDocuments { get; set; } = ["Doc 1", "Doc 2", "Doc 3"];

    public override async Task<IActionResult> OnGetAsync()
    {
        var pageResult = await base.OnGetAsync();
        if (pageResult is NotFoundResult) return pageResult;

        SubNavigationLinks =
        [
            GetSubPageLink<FinancialStatementsModel>
                (ViewConstants.FinancialDocumentsFinancialStatementsSubPageName, "./FinancialStatements"),
            GetSubPageLink<ManagementLettersModel>
                (ViewConstants.FinancialDocumentsManagementLettersSubPageName, "./ManagementLetters"),
            GetSubPageLink<InternalScrutinyReportsModel>
                (ViewConstants.FinancialDocumentsInternalScrutinyReportsSubPageName, "./InternalScrutinyReports"),
            GetSubPageLink<SelfAssessmentChecklistsModel>
                (ViewConstants.FinancialDocumentsSelfAssessmentChecklistsSubPageName, "./SelfAssessmentChecklists")
        ];

        return Page();
    }
}
