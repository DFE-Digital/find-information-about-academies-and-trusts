using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Services.Academy;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Export;
using DfE.FindInformationAcademiesTrusts.Services.Trust;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.Ofsted;

public class PreviousRatingsModel(
    IDataSourceService dataSourceService,
    ITrustService trustService,
    IAcademyService academyService,
    IOfstedDataExportService ofstedDataExportService,
    IDateTimeProvider dateTimeProvider,
    ILogger<PreviousRatingsModel> logger) : OfstedAreaModel(dataSourceService, trustService,
    academyService, ofstedDataExportService, dateTimeProvider, logger)
{
    public const string SubPageName = "Previous ratings";
    public override TrustPageMetadata TrustPageMetadata => base.TrustPageMetadata with { SubPageName = SubPageName };
}
