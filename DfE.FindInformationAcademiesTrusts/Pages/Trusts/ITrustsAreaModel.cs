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
    string? PageTitle { get; set; }

    /// <summary>
    /// The name of the section the page sits under in side navigation
    /// </summary>
    string Section { get; }

    string MapDataSourceToName(DataSourceServiceModel dataSource);
}

public record DataSourceListEntry(DataSourceServiceModel DataSource, IEnumerable<string> Fields);
