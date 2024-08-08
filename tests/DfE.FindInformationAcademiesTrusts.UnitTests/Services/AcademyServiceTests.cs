using DfE.FindInformationAcademiesTrusts.Data.Repositories;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.Models;
using DfE.FindInformationAcademiesTrusts.ServiceModels;
using DfE.FindInformationAcademiesTrusts.Services;
using DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Services;

public class AcademyServiceTests
{
    private readonly AcademyService _sut;
    private readonly Mock<IAcademyRepository> _mockAcademyRepository = new();
    private readonly MockMemoryCache _mockMemoryCache = new();

    public AcademyServiceTests()
    {
        _sut = new AcademyService(_mockAcademyRepository.Object, _mockMemoryCache.Object);
    }
}
