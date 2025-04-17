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
    IOfstedDataExportService ofstedDataExportService,
    IDateTimeProvider dateTimeProvider) : OfstedAreaModel(dataSourceService, trustService,
    academyService, ofstedDataExportService, dateTimeProvider)
{
    public const string SubPageName = "Current ratings";

    public override TrustPageMetadata TrustPageMetadata => base.TrustPageMetadata with { SubPageName = SubPageName };
}
