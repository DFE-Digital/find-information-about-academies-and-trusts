using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Services.DataSource;

public class DataSourceListEntryTest
{
    [Fact]
    public void LastUpdatedText_returns_unknown_when_LastUpdated_is_null()
    {
        var sut = new DataSourceListEntry(new DataSourceServiceModel(Source.Cdm, null, null), []);
        sut.LastUpdatedText.Should().Be("Unknown");
    }

    [Fact]
    public void LastUpdatedText_returns_correctString_when_LastUpdated_is_set()
    {
        var date = DateTime.Today;
        var sut = new DataSourceListEntry(new DataSourceServiceModel(Source.Cdm, date, null), []);
        sut.LastUpdatedText.Should().Be(date.ToString("d MMM yyyy"));
    }

    [Fact]
    public void UpdatedByText_returns_null_when_UpdatedBy_is_null()
    {
        var sut = new DataSourceListEntry(new DataSourceServiceModel(Source.Cdm, null, null), []);
        sut.UpdatedByText.Should().Be(null);
    }

    [Fact]
    public void UpdatedByText_returns_correctString_when_UpdatedBy_is_set()
    {
        var sut = new DataSourceListEntry(new DataSourceServiceModel(Source.Cdm, null, null, "Updated by"), []);
        sut.UpdatedByText.Should().Be("Updated by");
    }

    [Fact]
    public void UpdatedByText_returns_correctString_when_UpdatedBy_is_set_to_TRAMs_Migration()
    {
        var sut = new DataSourceListEntry(new DataSourceServiceModel(Source.Cdm, null, null, "TRAMs Migration"), []);
        sut.UpdatedByText.Should().Be("TRAMS Migration");
    }
}
