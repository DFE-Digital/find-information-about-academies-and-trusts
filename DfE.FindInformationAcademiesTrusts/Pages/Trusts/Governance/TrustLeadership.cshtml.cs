using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Trust;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.Governance;

public class TrustLeadershipModel(
    IDataSourceService dataSourceService,
    ILogger<TrustLeadershipModel> logger,
    ITrustService trustService)
    : GovernanceAreaModel(dataSourceService, trustService, logger, item => item.Page == "./TrustLeadership");
