using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Services.Academy;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Export;
using DfE.FindInformationAcademiesTrusts.Services.Trust;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.Ofsted;

public class SingleHeadlineGradesModel(
    IDataSourceService dataSourceService,
    ITrustService trustService,
    IAcademyService academyService,
    IOfstedDataExportService ofstedDataExportService,
    IDateTimeProvider dateTimeProvider,
    ILogger<SingleHeadlineGradesModel> logger) : OfstedAreaModel(dataSourceService, trustService,
    academyService, ofstedDataExportService, dateTimeProvider, logger)
{
    public const string SubPageName = "Single headline grades";

    public override TrustPageMetadata TrustPageMetadata => base.TrustPageMetadata with { SubPageName = SubPageName };
}
