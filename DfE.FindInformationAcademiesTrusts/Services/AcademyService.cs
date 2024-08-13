using DfE.FindInformationAcademiesTrusts.ServiceModels;

namespace DfE.FindInformationAcademiesTrusts.Services;

public interface IAcademyService
{
    Task<AcademyDetailsServiceModel[]> GetAcademiesInTrustDetailsAsync(string uid);
}

public class AcademyService : IAcademyService
{
    public Task<AcademyDetailsServiceModel[]> GetAcademiesInTrustDetailsAsync(string uid)
    {
        throw new NotImplementedException();
    }
}
