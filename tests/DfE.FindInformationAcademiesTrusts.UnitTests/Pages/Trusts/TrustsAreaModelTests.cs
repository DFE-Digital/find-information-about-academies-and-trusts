using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts;

public class TrustsAreaModelTests
{
    private readonly Mock<IDataSourceService> _mockDataSourceProvider = new();
    private readonly TrustsAreaModel _sut;
    private readonly MockLogger<TrustsAreaModel> _logger = new();
    private readonly Mock<ITrustService> _mockTrustService = new();

    private class TrustsAreaModelImpl(
        IDataSourceService dataSourceService,
        ITrustService trustService,
        ILogger<TrustsAreaModel> logger) : TrustsAreaModel(dataSourceService, trustService, logger);

    public TrustsAreaModelTests()
    {
        _sut = new TrustsAreaModelImpl(_mockDataSourceProvider.Object, _mockTrustService.Object, _logger.Object);
    }

    [Fact]
    public async Task OnGetAsync_should_fetch_a_trustsummary_by_uid()
    {
        var dummyTrustSummary = SetupSutToUseDummyTrustSummary();

        await _sut.OnGetAsync();
        _sut.TrustSummary.Should().Be(dummyTrustSummary);
    }

    [Fact]
    public async Task GroupUid_should_be_empty_string_by_default()
    {
        await _sut.OnGetAsync();
        _sut.Uid.Should().BeEquivalentTo(string.Empty);
    }

    [Fact]
    public async Task OnGetAsync_should_return_not_found_result_if_trust_is_not_found()
    {
        _mockTrustService.Setup(t => t.GetTrustSummaryAsync("1111"))
            .ReturnsAsync((TrustSummaryServiceModel?)null);

        _sut.Uid = "1111";
        var result = await _sut.OnGetAsync();
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task OnGetAsync_should_return_not_found_result_if_Uid_is_not_provided()
    {
        var result = await _sut.OnGetAsync();
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task OnGetAsync_should_populate_NavigationLinks()
    {
        _ = SetupSutToUseDummyTrustSummary();

        await _sut.OnGetAsync();
        _sut.NavigationLinks.Should().BeEquivalentTo([
            new TrustNavigationLinkModel("Overview", "/Trusts/Overview/TrustDetails", "1234", false, "overview-nav"),
            new TrustNavigationLinkModel("Contacts", "/Trusts/Contacts/InDfe", "1234", false, "contacts-nav"),
            new TrustNavigationLinkModel("Academies (3)", "/Trusts/Academies/Details",
                "1234", false, "academies-nav"),
            new TrustNavigationLinkModel("Ofsted", "/Trusts/Ofsted/CurrentRatings", "1234", false, "ofsted-nav"),
            new TrustNavigationLinkModel("Governance", "/Trusts/Governance/TrustLeadership", "1234", false,
                "governance-nav")
        ]);
    }

    [Fact]
    public async Task OnGetAsync_should_set_TrustPageTitle_to_trust_name()
    {
        _ = SetupSutToUseDummyTrustSummary(trustName: "My Trust");

        _ = await _sut.OnGetAsync();

        _sut.TrustPageMetadata.TrustName.Should().Be("My Trust");
    }

    private TrustSummaryServiceModel SetupSutToUseDummyTrustSummary(string uid = "1234", string trustName = "My Trust",
        string trustType = "Multi-academy trust", int numberOfAcademies = 3)
    {
        var dummyTrustSummary = new TrustSummaryServiceModel(uid, trustName, trustType, numberOfAcademies);
        _mockTrustService.Setup(t => t.GetTrustSummaryAsync(dummyTrustSummary.Uid))
            .ReturnsAsync(dummyTrustSummary);
        _sut.Uid = dummyTrustSummary.Uid;

        return dummyTrustSummary;
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
    public void MapDataSourceToTestId_ShouldMapSourceAndFieldsCorrectly()
    {
        // Arrange
        var source = new DataSourceServiceModel(Source.Cdm, null, null);
        var fields = new List<string> { "Field One", "FieldTwo" };

        // Act
        var result = _sut.MapDataSourceToTestId(new DataSourceListEntry(source, fields));

        // Assert
        Assert.Equal("data-source-cdm-field-one-fieldtwo", result);
    }

    [Fact]
    public void MapDataSourceToTestId_ShouldHandleEmptyFields()
    {
        // Arrange
        var source = new DataSourceServiceModel(Source.Gias, null, null);

        // Act
        var result = _sut.MapDataSourceToTestId(new DataSourceListEntry(source, new List<string>()));

        // Assert
        Assert.Equal("data-source-gias-", result); // Fields are empty, but source should still be present
    }

    [Fact]
    public void MapDataSourceToTestId_ShouldHandleFieldsWithSpaces()
    {
        // Arrange
        var source = new DataSourceServiceModel(Source.Mis, null, null);
        var fields = new List<string> { "  Field  Three", " FieldFour " };

        // Act
        var result = _sut.MapDataSourceToTestId(new DataSourceListEntry(source, fields));

        // Assert
        Assert.Equal("data-source-mis-field-three-fieldfour", result);
    }

    [Fact]
    public void MapDataSourceToTestId_ShouldHandleSingleFieldCorrectly()
    {
        // Arrange
        var source = new DataSourceServiceModel(Source.Mstr, null, null);
        var fields = new List<string> { "FieldOne" };

        // Act
        var result = _sut.MapDataSourceToTestId(new DataSourceListEntry(source, fields));

        // Assert
        Assert.Equal("data-source-mstr-fieldone", result);
    }

    [Fact]
    public void MapDataSourceToTestId_ShouldHandleMixedCaseFields()
    {
        // Arrange
        var source = new DataSourceServiceModel(Source.FiatDb, null, null);
        var fields = new List<string> { "FiElDOne", "FiELDTwo" };

        // Act
        var result = _sut.MapDataSourceToTestId(new DataSourceListEntry(source, fields));

        // Assert
        Assert.Equal("data-source-fiatdb-fieldone-fieldtwo", result);
    }
}
