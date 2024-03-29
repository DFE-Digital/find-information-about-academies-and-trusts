using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.UnitTests.Mocks;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts;
using DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts;

public class TrustsAreaModelTests
{
    private readonly Mock<ITrustProvider> _mockTrustProvider;
    private readonly Mock<IDataSourceProvider> _mockDataUpdatedProvider;
    private readonly TrustsAreaModel _sut;
    private readonly MockLogger<TrustsAreaModel> _logger;

    public TrustsAreaModelTests()
    {
        _logger = new MockLogger<TrustsAreaModel>();
        _mockTrustProvider = new Mock<ITrustProvider>();
        _mockDataUpdatedProvider = new Mock<IDataSourceProvider>();
        _sut = new TrustsAreaModel(_mockTrustProvider.Object, _mockDataUpdatedProvider.Object, _logger.Object,
            "Details");
    }

    [Fact]
    public async Task OnGetAsync_should_fetch_a_trust_by_uid()
    {
        var dummyTrust = DummyTrustFactory.GetDummyTrust("1234");
        _mockTrustProvider.Setup(s => s.GetTrustByUidAsync(dummyTrust.Uid))
            .ReturnsAsync(dummyTrust);
        _sut.Uid = dummyTrust.Uid;

        await _sut.OnGetAsync();
        _sut.Trust.Should().Be(dummyTrust);
    }

    [Fact]
    public async Task GroupUid_should_be_empty_string_by_default()
    {
        await _sut.OnGetAsync();
        _sut.Uid.Should().BeEquivalentTo(string.Empty);
    }

    [Fact]
    public void PageName_should_be_set_at_initialisation()
    {
        var sut = new TrustsAreaModel(_mockTrustProvider.Object, _mockDataUpdatedProvider.Object, _logger.Object,
            "Contacts");
        sut.PageName.Should().Be("Contacts");
    }

    [Fact]
    public void PageSection_should_be_AboutTheTrust()
    {
        _sut.Section.Should().Be("About the trust");
    }

    [Fact]
    public async Task OnGetAsync_should_return_not_found_result_if_trust_is_not_found()
    {
        _mockTrustProvider.Setup(s => s.GetTrustByUidAsync("1111"))
            .ReturnsAsync((Trust?)null);

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

    [Theory]
    [InlineData(Source.Gias, "Get information about schools")]
    [InlineData(Source.Mstr, "Get information about schools (internal use only, do not share outside of DfE)")]
    [InlineData(Source.Cdm, "RSD (Regional Services Division) service support team")]
    [InlineData(Source.Mis, "State-funded school inspections and outcomes: management information")]
    [InlineData(Source.ExploreEducationStatistics, "Explore education statistics")]
    public void MapDataSourceToName_should_return_the_correct_string_for_each_source(Source source, string expected)
    {
        var result = _sut.MapDataSourceToName(new DataSource(source, null, UpdateFrequency.Daily));
        result.Should().Be(expected);
    }

    [Fact]
    public void MapDataSourceToName_should_return_Unknown_when_source_is_not_recognised()
    {
        var dataSource = new DataSource((Source)10, null, UpdateFrequency.Daily);
        var result = _sut.MapDataSourceToName(dataSource);
        _logger.VerifyLogError($"Data source {dataSource} does not map to known type");
        result.Should().Be("Unknown");
    }
}
