using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Trust;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.FinancialDocuments;

public class InternalScrutinyReportsModel(
    IDataSourceService dataSourceService,
    ILogger<InternalScrutinyReportsModel> logger,
    ITrustService trustService)
    : FinancialDocumentsAreaModel(dataSourceService, trustService, logger)
{
    public override TrustPageMetadata TrustPageMetadata => base.TrustPageMetadata with
    {
        SubPageName = ViewConstants.FinancialDocumentsInternalScrutinyReportsSubPageName
    };
}
