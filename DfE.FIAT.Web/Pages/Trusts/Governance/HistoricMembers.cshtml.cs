using DfE.FIAT.Web.Services.DataSource;
using DfE.FIAT.Web.Services.Trust;

namespace DfE.FIAT.Web.Pages.Trusts.Governance;

public class HistoricMembersModel(
    IDataSourceService dataSourceService,
    ILogger<HistoricMembersModel> logger,
    ITrustService trustService)
    : GovernanceAreaModel(dataSourceService, trustService, logger);
