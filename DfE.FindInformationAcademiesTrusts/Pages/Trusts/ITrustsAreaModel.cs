using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Trust;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts;

public interface ITrustsAreaModel
{
    TrustSummaryServiceModel TrustSummary { get; }

    List<DataSourceListEntry> DataSources { get; }

    TrustPageMetadata TrustPageMetadata { get; }

    string MapDataSourceToName(DataSourceServiceModel dataSource);
    string MapDataSourceToTestId(DataSourceListEntry source);

    TrustNavigationLinkModel[] NavigationLinks { get; }
    TrustSubNavigationLinkModel[] SubNavigationLinks { get; }
}
