using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;
using Microsoft.Extensions.Logging;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts;

public class TrustsAreaModelTests : BaseTrustPageTests<TrustsAreaModel>
{
    private readonly MockLogger<TrustsAreaModel> _logger = new();

    private class TrustsAreaModelImpl(
        IDataSourceService dataSourceService,
        ITrustService trustService,
        ILogger<TrustsAreaModel> logger) : TrustsAreaModel(dataSourceService, trustService, logger);

    public TrustsAreaModelTests()
    {
        _sut = new TrustsAreaModelImpl(_mockDataSourceService.Object, _mockTrustService.Object, _logger.Object)
            { Uid = TrustUid };
    }

    [Fact]
    public void GroupUid_should_be_empty_string_by_default()
    {
        _sut = new TrustsAreaModelImpl(_mockDataSourceService.Object, _mockTrustService.Object, _logger.Object);
        _sut.Uid.Should().BeEquivalentTo(string.Empty);
    }

    [Fact]
    public async Task OnGetAsync_sets_SubNavigationLinks_toEmptyArray()
    {
        _ = await _sut.OnGetAsync();
        _sut.SubNavigationLinks.Should().BeEmpty();
    }

    [Theory]
    [InlineData(Source.Gias, "Get information about schools")]
    [InlineData(Source.Mstr, "Get information about schools (internal use only, do not share outside of DfE)")]
    [InlineData(Source.Cdm, "RSD (Regional Services Division) service support team")]
    [InlineData(Source.Mis, "State-funded school inspections and outcomes: management information")]
    [InlineData(Source.ExploreEducationStatistics, "Explore education statistics")]
    [InlineData(Source.FiatDb, "Find information about academies and trusts")]
    public void MapDataSourceToName_should_return_the_correct_string_for_each_source(Source source, string expected)
    {
        var result = _sut.MapDataSourceToName(new DataSourceServiceModel(source, null, UpdateFrequency.Daily));
        result.Should().Be(expected);
    }

    [Fact]
    public void MapDataSourceToName_should_return_Unknown_when_source_is_not_recognised()
    {
        var dataSource = new DataSourceServiceModel((Source)10, null, UpdateFrequency.Daily);
        var result = _sut.MapDataSourceToName(dataSource);
        _logger.VerifyLogError($"Data source {dataSource} does not map to known type");
        result.Should().Be("Unknown");
    }

    [Fact]
    public void MapDataSourceToTestId_ShouldMapSourceAndDataFieldCorrectly()
    {
        // Arrange
        var source = new DataSourceServiceModel(Source.Cdm, null, null);

        // Act
        var result = _sut.MapDataSourceToTestId(new DataSourceListEntry(source));

        // Assert
        Assert.Equal("data-source-cdm-all-information", result);
    }

    [Fact]
    public void MapDataSourceToTestId_ShouldHandleEmptyFields()
    {
        // Arrange
        var source = new DataSourceServiceModel(Source.Gias, null, null);

        // Act
        var result = _sut.MapDataSourceToTestId(new DataSourceListEntry(source, ""));

        // Assert
        Assert.Equal("data-source-gias", result); // Field is empty, but source should still be present
    }

    [Fact]
    public void MapDataSourceToTestId_ShouldHandleFieldsWithSpaces()
    {
        // Arrange
        var source = new DataSourceServiceModel(Source.Mis, null, null);

        // Act
        var result = _sut.MapDataSourceToTestId(new DataSourceListEntry(source, "fields"));

        // Assert
        Assert.Equal("data-source-mis-fields", result);
    }

    [Fact]
    public void MapDataSourceToTestId_ShouldHandleMixedCaseFields()
    {
        // Arrange
        var source = new DataSourceServiceModel(Source.FiatDb, null, null);

        // Act
        var result = _sut.MapDataSourceToTestId(new DataSourceListEntry(source, "FiElDOne FiELDTwo"));

        // Assert
        Assert.Equal("data-source-fiatdb-fieldone-fieldtwo", result);
    }

    [Fact]
    public void MapDataSourceToTestId_ShouldRemovePunctuation_and_PreseveWordsAndNumbers()
    {
        // Arrange
        var source = new DataSourceServiceModel(Source.FiatDb, null, null);

        // Act
        var result = _sut.MapDataSourceToTestId(new DataSourceListEntry(source, "#FiElDOne @FiELDTwo!"));

        // Assert
        Assert.Equal("data-source-fiatdb-fieldone-fieldtwo", result);
    }

    [Fact]
    public override async Task OnGetAsync_should_set_active_NavigationLink_to_current_page()
    {
        _ = await _sut.OnGetAsync();

        // Default is no current page
        _sut.NavigationLinks.Should().AllSatisfy(l => l.LinkIsActive.Should().BeFalse());
    }

    [Fact]
    public override async Task OnGetAsync_should_configure_TrustPageMetadata_PageName()
    {
        await _sut.OnGetAsync();
        _sut.TrustPageMetadata.PageName.Should().BeNull();
    }

    public override async Task OnGetAsync_sets_correct_data_source_list()
    {
        await _sut.OnGetAsync();

        //Default to empty
        _sut.DataSourcesPerPage.Should().BeEmpty();
    }
}
