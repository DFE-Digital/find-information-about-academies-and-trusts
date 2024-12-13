using DfE.FIAT.Data;
using DfE.FIAT.Web.Services.Academy;
using DfE.FIAT.Web.Services.DataSource;
using DfE.FIAT.Web.Services.Export;
using DfE.FIAT.Web.Services.Trust;

namespace DfE.FIAT.Web.Pages.Trusts.Ofsted;

public class CurrentRatingsModel(
    IDataSourceService dataSourceService,
    ITrustService trustService,
    IAcademyService academyService,
    IExportService exportService,
    IDateTimeProvider dateTimeProvider,
    ILogger<CurrentRatingsModel> logger) : OfstedAreaModel(dataSourceService, trustService,
    academyService, exportService, dateTimeProvider, logger);
