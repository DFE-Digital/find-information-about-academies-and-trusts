using DfE.FindInformationAcademiesTrusts.Data.Repositories;
using DfE.FindInformationAcademiesTrusts.ServiceModels;
using Microsoft.Extensions.Caching.Memory;

namespace DfE.FindInformationAcademiesTrusts.Services;

public interface IAcademyService
{
}

public class AcademyService(
    IAcademyRepository academyRepository,
    IMemoryCache memoryCache)
    : ITrustService
{

}
