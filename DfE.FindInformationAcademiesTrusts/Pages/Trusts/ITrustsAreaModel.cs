using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Trust;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts;

public interface ITrustsAreaModel
{
    TrustSummaryServiceModel TrustSummary { get; }

    List<DataSourceListEntry> DataSources { get; }

    /// <summary>
    /// The name of the page as displayed in the page h1
    /// </summary>
    string PageName { get; }

    /// <summary>
    /// The name of the page as displayed in the browser title
    /// </summary>
    TrustPageMetadata TrustPageMetadata { get; }

    string? PageTitle { get; set; }

    string MapDataSourceToName(DataSourceServiceModel dataSource);
    string MapDataSourceToTestId(DataSourceListEntry source);

    TrustNavigationLinkModel[] NavigationLinks { get; }
    TrustSubNavigationLinkModel[] SubNavigationLinks { get; }
}
