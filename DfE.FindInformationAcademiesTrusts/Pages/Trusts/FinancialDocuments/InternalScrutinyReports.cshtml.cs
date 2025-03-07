using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.FinancialDocument;
using DfE.FindInformationAcademiesTrusts.Services.Trust;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.FinancialDocuments;

public class InternalScrutinyReportsModel(
    IDataSourceService dataSourceService,
    ILogger<InternalScrutinyReportsModel> logger,
    ITrustService trustService,
    IFinancialDocumentService financialDocumentService)
    : FinancialDocumentsAreaModel(dataSourceService, trustService, logger, financialDocumentService)
{
    public override TrustPageMetadata TrustPageMetadata => base.TrustPageMetadata with
    {
        SubPageName = ViewConstants.FinancialDocumentsInternalScrutinyReportsSubPageName
    };

    protected override FinancialDocumentType FinancialDocumentType => FinancialDocumentType.InternalScrutinyReport;
    public override string FinancialDocumentDisplayName => "scrutiny report";
}
