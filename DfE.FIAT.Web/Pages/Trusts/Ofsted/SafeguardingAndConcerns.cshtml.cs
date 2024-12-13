using DfE.FIAT.Data;
using DfE.FIAT.Web.Services.Academy;
using DfE.FIAT.Web.Services.DataSource;
using DfE.FIAT.Web.Services.Export;
using DfE.FIAT.Web.Services.Trust;

namespace DfE.FIAT.Web.Pages.Trusts.Ofsted;

public class SafeguardingAndConcernsModel(
    IDataSourceService dataSourceService,
    ITrustService trustService,
    IAcademyService academyService,
    IExportService exportService,
    IDateTimeProvider dateTimeProvider,
    ILogger<SafeguardingAndConcernsModel> logger) : OfstedAreaModel(dataSourceService, trustService,
    academyService, exportService, dateTimeProvider, logger);
