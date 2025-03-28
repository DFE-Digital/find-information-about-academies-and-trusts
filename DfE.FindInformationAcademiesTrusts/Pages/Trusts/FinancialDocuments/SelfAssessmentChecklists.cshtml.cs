using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.FinancialDocument;
using DfE.FindInformationAcademiesTrusts.Services.Trust;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.FinancialDocuments;

public class SelfAssessmentChecklistsModel(
    IDataSourceService dataSourceService,
    ILogger<SelfAssessmentChecklistsModel> logger,
    ITrustService trustService,
    IFinancialDocumentService financialDocumentService)
    : FinancialDocumentsAreaModel(dataSourceService, trustService, logger, financialDocumentService)
{
    public override TrustPageMetadata TrustPageMetadata => base.TrustPageMetadata with
    {
        SubPageName = ViewConstants.FinancialDocumentsSelfAssessmentChecklistsSubPageName
    };

    protected override FinancialDocumentType FinancialDocumentType => FinancialDocumentType.SelfAssessmentChecklist;
    public override string FinancialDocumentDisplayName => "self-assessment checklist";
}
