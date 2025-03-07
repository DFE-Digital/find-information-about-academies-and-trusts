using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.FinancialDocument;
using DfE.FindInformationAcademiesTrusts.Services.Trust;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.FinancialDocuments;

public class FinancialStatementsModel(
    IDataSourceService dataSourceService,
    ILogger<FinancialStatementsModel> logger,
    ITrustService trustService,
    IFinancialDocumentService financialDocumentService)
    : FinancialDocumentsAreaModel(dataSourceService, trustService, logger, financialDocumentService)
{
    public override TrustPageMetadata TrustPageMetadata =>
        base.TrustPageMetadata with { SubPageName = ViewConstants.FinancialDocumentsFinancialStatementsSubPageName };

    public override bool InternalUseOnly => false;
    protected override FinancialDocumentType FinancialDocumentType => FinancialDocumentType.FinancialStatement;
    public override string FinancialDocumentDisplayName => "financial statement";
}
