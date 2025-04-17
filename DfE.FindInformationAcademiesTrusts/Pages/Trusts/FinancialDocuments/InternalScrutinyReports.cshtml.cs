using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Pages.Shared;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.FinancialDocument;
using DfE.FindInformationAcademiesTrusts.Services.Trust;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.FinancialDocuments;

public class InternalScrutinyReportsModel(
    IDataSourceService dataSourceService,
    ITrustService trustService,
    IFinancialDocumentService financialDocumentService)
    : FinancialDocumentsAreaModel(dataSourceService, trustService, financialDocumentService)
{
    public const string SubPageName = "Internal scrutiny reports";

    public override PageMetadata PageMetadata => base.PageMetadata with { SubPageName = SubPageName };

    protected override FinancialDocumentType FinancialDocumentType => FinancialDocumentType.InternalScrutinyReport;
    public override string FinancialDocumentDisplayName => "scrutiny report";
}
