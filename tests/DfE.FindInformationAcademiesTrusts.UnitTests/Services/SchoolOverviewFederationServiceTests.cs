using DfE.FindInformationAcademiesTrusts.Data.Repositories.School;
using DfE.FindInformationAcademiesTrusts.Services.School;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Services;

public class SchoolOverviewFederationServiceTests
{
    private readonly int _schoolUrn = 123;

    private readonly SchoolOverviewFederationService _sut;
    private readonly ISchoolRepository _mockSchoolRepository = Substitute.For<ISchoolRepository>();

    private FederationDetails _federationDetails = new(
        "Groovy federation",
        "12345",
        DateTime.Today,
        new Dictionary<string, string>
        {
            { "6789", "Another school" },
            { "44567", "A third school" }
        });

    public SchoolOverviewFederationServiceTests()
    {
        _sut = new SchoolOverviewFederationService(_mockSchoolRepository);
    }

    [Fact]
    public async Task should_set_values_correctly()
    {
        var expectedResult = new SchoolOverviewFederationServiceModel(
            "Groovy federation",
            "12345",
            DateTime.Today,
            new Dictionary<string, string>
            {
                { "6789", "Another school" },
                { "44567", "A third school" }
            });
        
        _mockSchoolRepository.GetSchoolFederationDetailsAsync(_schoolUrn).Returns(_federationDetails);
        
        var result = await _sut.GetSchoolOverviewFederationAsync(_schoolUrn);
        
        result.Should().BeEquivalentTo(expectedResult);
    }
}
