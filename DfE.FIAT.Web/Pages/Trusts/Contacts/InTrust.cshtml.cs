using DfE.FIAT.Web.Services.DataSource;
using DfE.FIAT.Web.Services.Trust;

namespace DfE.FIAT.Web.Pages.Trusts.Contacts;

public class InTrustModel(
    IDataSourceService dataSourceService,
    ITrustService trustService,
    ILogger<InTrustModel> logger)
    : ContactAreaModel(dataSourceService, trustService, logger);
