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
            "type1", "type2", "type3", "type4", "type5", "type6", "type7", "type8", "type9", "type10", "type11", "type12", "type13"
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
        result.SenProvisionTypes.Count.Should().Be(13);
    }
}