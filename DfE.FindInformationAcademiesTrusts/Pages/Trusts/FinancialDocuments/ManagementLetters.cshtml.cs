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
    public const string SubPageName = "Management letters";

    public override TrustPageMetadata TrustPageMetadata => base.TrustPageMetadata with { SubPageName = SubPageName };

    protected override FinancialDocumentType FinancialDocumentType => FinancialDocumentType.ManagementLetter;
    public override string FinancialDocumentDisplayName => "management letter";
}
