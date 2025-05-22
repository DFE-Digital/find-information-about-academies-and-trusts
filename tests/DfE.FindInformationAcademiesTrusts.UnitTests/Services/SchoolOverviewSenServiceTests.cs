using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.School;
using DfE.FindInformationAcademiesTrusts.Services.School;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Services;

public class SchoolOverviewSenServiceTests
{
    private readonly int _schoolUrn = 123;

    private readonly SchoolOverviewSenService _sut;
    private readonly ISchoolRepository _mockSchoolRepository = Substitute.For<ISchoolRepository>();

    private SenProvision _senProvision = new(
        "22",
        "25",
        "13",
        "25",
        "Resourced provision",
        new List<string>
        {
            "Sen1", "Sen2", "Sen3", "Sen4", "Sen5", "Sen6", "Sen7", "Sen8", "Sen9", "Sen10", "Sen11", "Sen12", "Sen13"
        }
    );

    public SchoolOverviewSenServiceTests()
    {
        _sut = new SchoolOverviewSenService(_mockSchoolRepository);
    }

    [Fact]
    public async Task should_set_values_correctly()
    {
        var expectedResult = new SchoolOverviewSenServiceModel(
            "22",
            "25",
            "13",
            "25",
            "Resourced provision",
            new List<string>
            {
                "type1", "type2", "type3", "type4", "type5", "type6", "type7", "type8", "type9", "type10", "type11",
                "type12", "type13"
            });
        
        _mockSchoolRepository.GetSchoolSenProvisionAsync(_schoolUrn).Returns(_senProvision);
        
        var result = await _sut.GetSchoolOverviewSenAsync(_schoolUrn);
        
        result.Should().BeEquivalentTo(expectedResult);
        result.SenProvisionTypes.Count().Should().Be(13);
    }

    [Fact]
    public async Task should_not_include_null_values_in_sen_provision_types_list()
    {
        _senProvision = _senProvision with { SenProvisionTypes = new List<string>
        {
            null!, "type2", "type3", "type4", "type5", "type6", "type7", "type8", "type9", "type10", "type11", "type12", "type13"
        }};
        
        var expectedResult = new SchoolOverviewSenServiceModel(
            "22",
            "25",
            "13",
            "25",
            "Resourced provision",
            new List<string>
            {
                "type2", "type3", "type4", "type5", "type6", "type7", "type8", "type9", "type10", "type11",
                "type12", "type13"
            });
        
        _mockSchoolRepository.GetSchoolSenProvisionAsync(_schoolUrn).Returns(_senProvision);
        
        var result = await _sut.GetSchoolOverviewSenAsync(_schoolUrn);
        
        result.Should().BeEquivalentTo(expectedResult);
        result.SenProvisionTypes.Count().Should().Be(12);
    }
}