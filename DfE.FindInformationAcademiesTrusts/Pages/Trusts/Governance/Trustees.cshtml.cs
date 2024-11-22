using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Trust;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.Governance;

public class TrusteesModel(
    IDataSourceService dataSourceService,
    ILogger<TrusteesModel> logger,
    ITrustService trustService)
    : GovernanceAreaModel(dataSourceService, trustService, logger);
