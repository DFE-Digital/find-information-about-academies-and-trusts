using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Mstr;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Repositories;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests.Mocks;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests.Repositories;

public class PipelineEstablishmentRepositoryTests
{
    private readonly PipelineEstablishmentRepository _sut;
    private readonly MockAcademiesDbContext _mockAcademiesDbContext = new();

    public PipelineEstablishmentRepositoryTests()
    {
        _sut = new PipelineEstablishmentRepository(_mockAcademiesDbContext.Object);
    }

    [Fact]
    public async Task GetPipelineFreeSchoolProjects_should_return_project_from_repository()
    {
        _mockAcademiesDbContext.AddMstrFreeSchoolProject(new MstrFreeSchoolProject
        {
            SK = 1,
            ProjectID = 12345,
            ProjectName = "My Test Free School Project",
            ProjectApplicationType = "New",
            LocalAuthority = "Some Local Authority",
            Region = "East Midlands",
            SchoolPhase = "Primary",
            SchoolType = "Free School",
            ProjectStatus = "In Progress",
            Stage = "Stage 1",
            RouteOfProject = "Route A",
            StatutoryLowestAge = 5,
            StatutoryHighestAge = 11,
            NewURN = 999999,
            EstablishmentName = "Test Establishment",
            ActualDateOpened = DateTime.UtcNow.AddDays(-10),
            TrustID = "2806",
            TrustName = "My Test Trust",
            TrustType = "Single Academy",
            CompaniesHouseNumber = "12345678",
            DateSource = DateTime.UtcNow
        });


        var result = await _sut.GetPipelineFreeSchoolProjects("2806");

        result.Should().NotBeEmpty();

    }
}
