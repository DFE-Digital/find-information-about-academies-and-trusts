using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.FinancialDocument;
using DfE.FindInformationAcademiesTrusts.Services.Trust;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.FinancialDocuments;

public class SelfAssessmentChecklistsModel(
    IDataSourceService dataSourceService,
    ITrustService trustService,
    IFinancialDocumentService financialDocumentService)
    : FinancialDocumentsAreaModel(dataSourceService, trustService, financialDocumentService)
{
    public const string SubPageName = "Self-assessment checklists";

    public override TrustPageMetadata TrustPageMetadata => base.TrustPageMetadata with { SubPageName = SubPageName };

    protected override FinancialDocumentType FinancialDocumentType => FinancialDocumentType.SelfAssessmentChecklist;
    public override string FinancialDocumentDisplayName => "self-assessment checklist";
}
