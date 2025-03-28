using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.FinancialDocument;
using DfE.FindInformationAcademiesTrusts.Services.Trust;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.FinancialDocuments;

public class ManagementLettersModel(
    IDataSourceService dataSourceService,
    ILogger<ManagementLettersModel> logger,
    ITrustService trustService,
    IFinancialDocumentService financialDocumentService)
    : FinancialDocumentsAreaModel(dataSourceService, trustService, logger, financialDocumentService)
{
    public override TrustPageMetadata TrustPageMetadata =>
        base.TrustPageMetadata with { SubPageName = ViewConstants.FinancialDocumentsManagementLettersSubPageName };

    protected override FinancialDocumentType FinancialDocumentType => FinancialDocumentType.ManagementLetter;
    public override string FinancialDocumentDisplayName => "management letter";
}
