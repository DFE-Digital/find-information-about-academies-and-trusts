using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Trust;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts;

public interface ITrustsAreaModel
{
    string Uid { get; }

    TrustSummaryServiceModel TrustSummary { get; }

    List<DataSourcePageListEntry> DataSourcesPerPage { get; }

    TrustPageMetadata TrustPageMetadata { get; }

    string MapDataSourceToName(DataSourceServiceModel dataSource);
    string MapDataSourceToTestId(DataSourceListEntry source);
}
