using DfE.FindInformationAcademiesTrusts.Pages.Schools.Overview;
using DfE.FindInformationAcademiesTrusts.Services.School;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Schools.Overview;

public class SenModelTests : BaseOverviewAreaModelTests<SenModel>
{
    private readonly ISchoolOverviewSenService _mockSchoolOverviewSenService =
        Substitute.For<ISchoolOverviewSenService>();

    private SchoolOverviewSenServiceModel _dummySenDetails = new(
        "20",
        "25",
        "16",
        "22",
        "Resourced provision",
        ["ASD - Autistic Spectrum Disorder", "SpLD - Specific Learning Difficulty"]);

    public SenModelTests()
    {
        _mockSchoolOverviewSenService.GetSchoolOverviewSenAsync(Arg.Any<int>())
            .Returns(_dummySenDetails);
        
        Sut = new SenModel(MockSchoolService, MockTrustService, _mockSchoolOverviewSenService, MockDataSourceService);
        Sut.Urn = SchoolUrn;
    }

    [Fact]
    public override async Task OnGetAsync_should_configure_PageMetadata_SubPageName()
    {
        await Sut.OnGetAsync();
        
        Sut.PageMetadata.SubPageName.Should().Be("SEN (special educational needs)");
    }

    [Fact]
    public async Task OnGetAsync_should_set_SchoolOverviewSenServiceModel()
    {
        await Sut.OnGetAsync();
        
        Sut.SchoolOverviewSenServiceModel.Should().Be(_dummySenDetails);
    }
    
    // [Theory]
    // [InlineData(null, null, null, null, null, new List<string>())]
    // public async Task OnGetAsync_should_set_null_values_correctly(
    //     string? resourcedProvisionOnRoll, 
    //     string? resourcedProvisionCapacity,
    //     string? senOnRoll,
    //     string? senCapacity,
    //     string? resourcedProvisionTypes,
    //     List<string> senProvisionTypes)
    // {
    //     _dummySenDetails = _dummySenDetails with
    //     {
    //         ResourcedProvisionOnRoll = resourcedProvisionOnRoll,
    //         ResourcedProvisionCapacity = resourcedProvisionCapacity,
    //         SenOnRoll = senOnRoll,
    //         SenCapacity = senCapacity,
    //         ResourcedProvisionTypes = resourcedProvisionTypes,
    //         SenProvisionTypes = senProvisionTypes
    //     };
    //     
    //     _mockSchoolOverviewSenService.GetSchoolOverviewSenAsync(Arg.Any<int>())
    //         .Returns(_dummySenDetails);
    //     await Sut.OnGetAsync();
    //     
    //     Sut.SchoolOverviewSenServiceModel.Should().Be(_dummySenDetails);
    // }
}
