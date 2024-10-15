namespace DfE.FindInformationAcademiesTrusts.Data.Repositories.Academy;

public interface IAcademyRepository
{
    Task<string?> GetSingleAcademyTrustAcademyUrnAsync(string uid);
    Task<int> GetNumberOfAcademiesInTrustAsync(string uid);
    Task<AcademyDetails[]> GetAcademiesInTrustDetailsAsync(string uid);
    Task<AcademyOfsted[]> GetAcademiesInTrustOfstedAsync(string uid);
    Task<AcademyPupilNumbers[]> GetAcademiesInTrustPupilNumbersAsync(string uid);
    Task<AcademyFreeSchoolMeals[]> GetAcademiesInTrustFreeSchoolMealsAsync(string uid);
    Task<AcademyOverview[]> GetAcademiesInTrustOverviewAsync(string uid);
    Task<AcademyDetails?> GetAcademyDetailsAsync(string urn);
    Task<IPaginatedList<AcademyDetails>> SearchAcademiesAsync(string searchTerm, int page = 1);
}
