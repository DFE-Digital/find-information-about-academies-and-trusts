using DfE.FindInformationAcademiesTrusts.Pages.Schools.Overview;
using DfE.FindInformationAcademiesTrusts.Services.School;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Schools.Overview;

public class FederationModelTests : BaseOverviewAreaModelTests<FederationModel>
{
    private readonly ISchoolOverviewFederationService _mockSchoolOverviewFederationService =
        Substitute.For<ISchoolOverviewFederationService>();

    private SchoolOverviewFederationServiceModel _dummyFederationDetails = new(
        "My School",
        "12345",
        DateTime.Now,
        new Dictionary<string, string>
        {
            { "6789", "Another school" },
            { "44567", "A third school" }
        });
    public FederationModelTests()
    {
        _mockSchoolOverviewFederationService.GetSchoolOverviewFederationAsync(Arg.Any<int>())
            .Returns(_dummyFederationDetails);
        
        Sut = new FederationModel(MockSchoolService, MockTrustService, _mockSchoolOverviewFederationService, MockDataSourceService);
        Sut.Urn = SchoolUrn;
    }

    [Fact]
    public override async Task OnGetAsync_should_configure_PageMetadata_SubPageName()
    {
        await Sut.OnGetAsync();
        
        Sut.PageMetadata.SubPageName.Should().Be("Federation details");
    }
    [Fact]
    public async Task OnGetAsync_should_set_SchoolOverviewFederationServiceModel()
    {
        await Sut.OnGetAsync();
        
        Sut.SchoolOverviewFederationServiceModel.Should().Be(_dummyFederationDetails);
    }

    [Theory]
    [InlineData(null, null, null, null)]
    public async Task OnGetAsync_should_set_null_values_correctly(
        string? federationName,
        string? federationUid,
        DateTime? openedOnDate,
        Dictionary<string, string>? schools)
    {
        _dummyFederationDetails = _dummyFederationDetails with
        {
            FederationName = federationName,
            FederationUid = federationUid,
            OpenedOnDate = openedOnDate,
            Schools = schools
        };
        
        var federationNameValue = federationName ?? "Not available";
        var federationUidValue = federationUid ?? "Not available";
        
        _mockSchoolOverviewFederationService.GetSchoolOverviewFederationAsync(Arg.Any<int>())
            .Returns(_dummyFederationDetails);
        await Sut.OnGetAsync();
        
        Sut.FederationName.Should().Be(federationNameValue);
        Sut.FederationUid.Should().Be(federationUidValue);
        Sut.OpenedOnDate.Should().Be(openedOnDate);
        Sut.Schools.Should().BeEquivalentTo(new Dictionary<string, string>());
    }
}
