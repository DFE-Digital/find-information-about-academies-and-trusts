using DfE.FindInformationAcademiesTrusts.Data.Repositories.Academy;

namespace DfE.FindInformationAcademiesTrusts.Services.Academy;

public interface IAcademyService
{
    Task<AcademyDetailsServiceModel[]> GetAcademiesInTrustDetailsAsync(string uid);
    Task<AcademyOfstedServiceModel[]> GetAcademiesInTrustOfstedAsync(string uid);
    Task<AcademyPupilNumbersServiceModel[]> GetAcademiesInTrustPupilNumbersAsync(string uid);
    Task<AcademyFreeSchoolMealsServiceModel[]> GetAcademiesInTrustFreeSchoolMealsAsync(string uid);
}

public class AcademyService(IAcademyRepository academyRepository) : IAcademyService
{
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

    public Task<AcademyFreeSchoolMealsServiceModel[]> GetAcademiesInTrustFreeSchoolMealsAsync(string uid)
    {
        throw new NotImplementedException();
    }
}
