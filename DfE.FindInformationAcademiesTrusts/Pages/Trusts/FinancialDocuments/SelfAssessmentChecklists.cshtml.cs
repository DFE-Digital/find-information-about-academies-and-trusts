using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Trust;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.FinancialDocuments;

public class SelfAssessmentChecklistsModel(
    IDataSourceService dataSourceService,
    ILogger<SelfAssessmentChecklistsModel> logger,
    ITrustService trustService)
    : FinancialDocumentsAreaModel(dataSourceService, trustService, logger)
{
    public override TrustPageMetadata TrustPageMetadata => base.TrustPageMetadata with
    {
        SubPageName = ViewConstants.FinancialDocumentsSelfAssessmentChecklistsSubPageName
    };
}
