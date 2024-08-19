namespace DfE.FindInformationAcademiesTrusts.Data.Repositories.Academy;

public interface IAcademyRepository
{
    Task<string?> GetSingleAcademyTrustAcademyUrnAsync(string uid);
    Task<int> GetNumberOfAcademiesInTrustAsync(string uid);
    Task<AcademyDetails[]> GetAcademiesInTrustDetailsAsync(string uid);
    Task<AcademyOfsted[]> GetAcademiesInTrustOfstedAsync(string uid);
}
