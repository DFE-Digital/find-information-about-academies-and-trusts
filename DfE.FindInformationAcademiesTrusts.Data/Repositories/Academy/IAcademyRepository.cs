namespace DfE.FindInformationAcademiesTrusts.Data.Repositories.Academy;

public interface IAcademyRepository
{
    Task<string?> GetSingleAcademyTrustAcademyUrnAsync(string uid);
    Task<string?> GetAcademyTrustTrustReferenceNumberAsync(string uid);
    Task<int> GetNumberOfAcademiesInTrustAsync(string uid);
    Task<AcademyDetails[]> GetAcademiesInTrustDetailsAsync(string uid);
    Task<AcademyOfsted[]> GetAcademiesInTrustOfstedAsync(string uid);
    Task<AcademyPupilNumbers[]> GetAcademiesInTrustPupilNumbersAsync(string uid);
    Task<AcademyFreeSchoolMeals[]> GetAcademiesInTrustFreeSchoolMealsAsync(string uid);
    Task<AcademyOverview[]> GetOverviewOfAcademiesInTrustAsync(string uid);
}
