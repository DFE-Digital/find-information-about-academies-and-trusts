namespace DfE.FindInformationAcademiesTrusts.Data.Repositories;

public interface IAcademyRepository
{
    Task<string?> GetUrnForSingleAcademyTrustAsync(string uid);
    Task<int> GetNumberOfAcademiesInTrustAsync(string uid);
}
