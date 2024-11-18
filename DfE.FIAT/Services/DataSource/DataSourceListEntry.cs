using DfE.FIAT.Web.Pages;

namespace DfE.FIAT.Web.Services.DataSource;

public record DataSourceListEntry(DataSourceServiceModel DataSource, IEnumerable<string> Fields)
{
    public string LastUpdatedText => DataSource.LastUpdated is null
        ? "Unknown"
        : DataSource.LastUpdated.Value.ToString(StringFormatConstants.ViewDate);

    public string? UpdatedByText => DataSource.UpdatedBy == "TRAMs Migration"
        ? "TRAMS Migration"
        : DataSource.UpdatedBy;
}
