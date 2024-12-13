using DfE.FIAT.Web.Services.DataSource;
using DfE.FIAT.Web.Services.Trust;

namespace DfE.FIAT.Web.Pages.Trusts.Governance;

public class TrusteesModel(
    IDataSourceService dataSourceService,
    ILogger<TrusteesModel> logger,
    ITrustService trustService)
    : GovernanceAreaModel(dataSourceService, trustService, logger);
