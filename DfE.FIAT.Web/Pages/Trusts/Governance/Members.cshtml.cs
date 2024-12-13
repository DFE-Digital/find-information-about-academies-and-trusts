using DfE.FIAT.Web.Services.DataSource;
using DfE.FIAT.Web.Services.Trust;

namespace DfE.FIAT.Web.Pages.Trusts.Governance;

public class MembersModel(
    IDataSourceService dataSourceService,
    ILogger<MembersModel> logger,
    ITrustService trustService)
    : GovernanceAreaModel(dataSourceService, trustService, logger);
