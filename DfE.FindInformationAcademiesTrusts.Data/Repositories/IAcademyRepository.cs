namespace DfE.FindInformationAcademiesTrusts.Data.Repositories;

public interface IAcademyRepository
{
    Task<string?> GetSingleAcademyTrustAcademyUrnAsync(string uid);
    Task<int> GetNumberOfAcademiesInTrustAsync(string uid);
}
