using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Services.Academy;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Export;
using DfE.FindInformationAcademiesTrusts.Services.Trust;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.Ofsted;

public class CurrentRatingsModel(
    IDataSourceService dataSourceService,
    ITrustService trustService,
    IAcademyService academyService,
    IExportService exportService,
    IDateTimeProvider dateTimeProvider,
    ILogger<CurrentRatingsModel> logger) : OfstedAreaModel(dataSourceService, trustService,
    academyService, exportService, dateTimeProvider, logger)
{
    public override TrustPageMetadata TrustPageMetadata =>
        base.TrustPageMetadata with { SubPageName = ViewConstants.OfstedCurrentRatingsPageName };
}
