using DfE.FIAT.Web.Services.DataSource;
using DfE.FIAT.Web.Services.Trust;

namespace DfE.FIAT.Web.Pages.Trusts.Contacts;

public class InDfeModel(
    IDataSourceService dataSourceService,
    ITrustService trustService,
    ILogger<InDfeModel> logger)
    : ContactAreaModel(dataSourceService, trustService, logger);
