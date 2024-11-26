using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Trust;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.Contacts;

public class InTrustModel(
    IDataSourceService dataSourceService,
    ITrustService trustService,
    ILogger<InTrustModel> logger)
    : ContactAreaModel(dataSourceService, trustService, logger);
