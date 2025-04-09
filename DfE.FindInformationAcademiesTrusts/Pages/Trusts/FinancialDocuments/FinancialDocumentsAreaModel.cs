using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.FinancialDocument;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.FinancialDocuments;

public abstract class FinancialDocumentsAreaModel(
    IDataSourceService dataSourceService,
    ITrustService trustService,
    ILogger<FinancialDocumentsAreaModel> logger,
    IFinancialDocumentService financialDocumentService)
    : TrustsAreaModel(dataSourceService, trustService, logger)
{
    public const string PageName = "Financial documents";

    public override TrustPageMetadata TrustPageMetadata => base.TrustPageMetadata with { PageName = PageName };

    public virtual bool InternalUseOnly => true;
    protected abstract FinancialDocumentType FinancialDocumentType { get; }
    public abstract string FinancialDocumentDisplayName { get; }
    private FinancialDocumentServiceModel[] _financialDocuments = null!;

    public FinancialDocumentServiceModel[] FinancialDocuments
    {
        get => _financialDocuments;
        set => _financialDocuments = value.OrderByDescending(doc => doc.YearTo).ToArray();
    }

    public override async Task<IActionResult> OnGetAsync()
    {
        var pageResult = await base.OnGetAsync();
        if (pageResult is NotFoundResult) return pageResult;

        FinancialDocuments =
            await financialDocumentService.GetFinancialDocumentsAsync(Uid, FinancialDocumentType);

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
