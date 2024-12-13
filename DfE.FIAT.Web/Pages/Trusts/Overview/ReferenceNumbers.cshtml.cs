using DfE.FIAT.Web.Services.DataSource;
using DfE.FIAT.Web.Services.Trust;

namespace DfE.FIAT.Web.Pages.Trusts.Overview;

public class ReferenceNumbersModel(
    IDataSourceService dataSourceService,
    ILogger<ReferenceNumbersModel> logger,
    ITrustService trustService)
    : OverviewAreaModel(dataSourceService, trustService, logger);
