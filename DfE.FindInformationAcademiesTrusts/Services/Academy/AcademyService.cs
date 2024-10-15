using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.Academy;

namespace DfE.FindInformationAcademiesTrusts.Services.Academy;

public interface IAcademyService
{
    Task<AcademyDetailsServiceModel?> GetAcademyDetailsAsync(string urn);
    Task<IPaginatedList<AcademyDetailsServiceModel>> SearchAcademiesAsync(string searchTerm, int page = 1);
    Task<AcademyDetailsServiceModel[]> GetAcademiesInTrustDetailsAsync(string uid);
    Task<AcademyOfstedServiceModel[]> GetAcademiesInTrustOfstedAsync(string uid);
    Task<AcademyPupilNumbersServiceModel[]> GetAcademiesInTrustPupilNumbersAsync(string uid);
    Task<AcademyFreeSchoolMealsServiceModel[]> GetAcademiesInTrustFreeSchoolMealsAsync(string uid);
}

public class AcademyService(
    IAcademyRepository academyRepository,
    IFreeSchoolMealsAverageProvider freeSchoolMealsAverageProvider) : IAcademyService
{
    public async Task<AcademyDetailsServiceModel?> GetAcademyDetailsAsync(string urn)
    {
        var academy = await academyRepository.GetAcademyDetailsAsync(urn);
        if (academy == null)
        {
            return null;
        }

        return new AcademyDetailsServiceModel(
            academy.Urn,
            academy.EstablishmentName,
            academy.LocalAuthority,
            academy.TypeOfEstablishment,
            academy.UrbanRural
        );
    }
    public async Task<IPaginatedList<AcademyDetailsServiceModel>> SearchAcademiesAsync(string searchTerm, int page = 1)
    {
        var academiesPaginatedList = await academyRepository.SearchAcademiesAsync(searchTerm, page);

        var academyServiceModels = academiesPaginatedList
            .Select(a => new AcademyDetailsServiceModel(
                a.Urn,
                a.EstablishmentName,
                a.LocalAuthority,
                a.TypeOfEstablishment,
                a.UrbanRural))
            .ToArray();

        return new PaginatedList<AcademyDetailsServiceModel>(
            academyServiceModels,
            academiesPaginatedList.PageStatus.TotalResults,
            academiesPaginatedList.PageStatus.PageIndex,
            10);
    }


    public async Task<AcademyDetailsServiceModel[]> GetAcademiesInTrustDetailsAsync(string uid)
    {
        var academies = await academyRepository.GetAcademiesInTrustDetailsAsync(uid);

        return academies.Select(a =>
            new AcademyDetailsServiceModel(a.Urn, a.EstablishmentName, a.LocalAuthority, a.TypeOfEstablishment,
                a.UrbanRural)).ToArray();
    }

    public async Task<AcademyOfstedServiceModel[]> GetAcademiesInTrustOfstedAsync(string uid)
    {
        var academies = await academyRepository.GetAcademiesInTrustOfstedAsync(uid);

        return academies.Select(a =>
            new AcademyOfstedServiceModel(a.Urn, a.EstablishmentName, a.DateAcademyJoinedTrust, a.PreviousOfstedRating,
                a.CurrentOfstedRating)).ToArray();
    }

    public async Task<AcademyPupilNumbersServiceModel[]> GetAcademiesInTrustPupilNumbersAsync(string uid)
    {
        var academies = await academyRepository.GetAcademiesInTrustPupilNumbersAsync(uid);

        return academies.Select(a =>
            new AcademyPupilNumbersServiceModel(a.Urn, a.EstablishmentName, a.PhaseOfEducation,
                a.AgeRange, a.NumberOfPupils,
                a.SchoolCapacity)).ToArray();
    }

    public async Task<AcademyFreeSchoolMealsServiceModel[]> GetAcademiesInTrustFreeSchoolMealsAsync(string uid)
    {
        var academies = await academyRepository.GetAcademiesInTrustFreeSchoolMealsAsync(uid);

        return academies.Select(a =>
                new AcademyFreeSchoolMealsServiceModel(
                    a.Urn,
                    a.EstablishmentName,
                    a.PercentageFreeSchoolMeals,
                    freeSchoolMealsAverageProvider.GetLaAverage(a.LocalAuthorityCode, a.PhaseOfEducation,
                        a.TypeOfEstablishment),
                    freeSchoolMealsAverageProvider.GetNationalAverage(a.PhaseOfEducation, a.TypeOfEstablishment)))
            .ToArray();
    }
}
