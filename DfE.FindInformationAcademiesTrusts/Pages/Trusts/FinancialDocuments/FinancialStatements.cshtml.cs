using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Trust;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.FinancialDocuments;

public class FinancialStatementsModel(
    IDataSourceService dataSourceService,
    ILogger<FinancialStatementsModel> logger,
    ITrustService trustService)
    : FinancialDocumentsAreaModel(dataSourceService, trustService, logger)
{
    public override TrustPageMetadata TrustPageMetadata =>
        base.TrustPageMetadata with { SubPageName = ViewConstants.FinancialDocumentsFinancialStatementsSubPageName };

    public override bool InternalUseOnly => false;
}
