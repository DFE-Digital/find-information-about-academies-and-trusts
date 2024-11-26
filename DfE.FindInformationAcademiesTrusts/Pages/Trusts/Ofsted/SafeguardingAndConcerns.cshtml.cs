using DfE.FindInformationAcademiesTrusts.Services.Academy;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Trust;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.Ofsted;

public class SafeguardingAndConcernsModel(
    IDataSourceService dataSourceService,
    ITrustService trustService,
    IAcademyService academyService,
    ILogger<SafeguardingAndConcernsModel> logger) : OfstedAreaModel(dataSourceService, trustService,
    academyService, logger, model => model.Page == "./SafeguardingAndConcerns");
