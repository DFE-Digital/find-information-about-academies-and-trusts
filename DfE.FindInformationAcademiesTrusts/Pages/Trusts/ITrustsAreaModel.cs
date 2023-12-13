using DfE.FindInformationAcademiesTrusts.Data;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts;

public interface ITrustsAreaModel
{
    Trust Trust { get; set; }

    List<DataSourceListEntry> DataSources { get; set; }

    /// <summary>
    /// The name of the page as displayed in the page h1
    /// </summary>
    string PageName { get; }

    /// <summary>
    /// The name of the page as displayed in the browser title
    /// </summary>
    string? PageTitle { get; init; }

    /// <summary>
    /// The name of the section the page sits under in side navigation
    /// </summary>
    string Section { get; }

    string MapDataSourceToName(DataSource source);
}

public record DataSourceListEntry(DataSource DataSource, IEnumerable<string> Fields);
