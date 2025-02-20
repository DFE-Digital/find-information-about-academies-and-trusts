using DfE.FindInformationAcademiesTrusts.Pages;

namespace DfE.FindInformationAcademiesTrusts.Services.DataSource;

public record DataSourceListEntry(DataSourceServiceModel DataSource, string DataField = "All information was")
{
    public string LastUpdatedText => DataSource.LastUpdated is null
        ? "Unknown"
        : DataSource.LastUpdated.Value.ToString(StringFormatConstants.DisplayDateFormat);

    public string? UpdatedByText => DataSource.UpdatedBy == "TRAMs Migration"
        ? "TRAMS Migration"
        : DataSource.UpdatedBy;
}
