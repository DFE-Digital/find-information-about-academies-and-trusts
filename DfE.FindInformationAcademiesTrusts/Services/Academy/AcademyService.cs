using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.Academy;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.PipelineAcademy;
using System.Diagnostics.CodeAnalysis;

namespace DfE.FindInformationAcademiesTrusts.Services.Academy;

public interface IAcademyService
{
    Task<AcademyDetailsServiceModel[]> GetAcademiesInTrustDetailsAsync(string uid);
    Task<AcademyOfstedServiceModel[]> GetAcademiesInTrustOfstedAsync(string uid);
    Task<AcademyPupilNumbersServiceModel[]> GetAcademiesInTrustPupilNumbersAsync(string uid);
    Task<AcademyFreeSchoolMealsServiceModel[]> GetAcademiesInTrustFreeSchoolMealsAsync(string uid);
    AcademyPipelineSummaryServiceModel GetAcademiesPipelineSummary();
    AcademyPipelineServiceModel[] GetAcademiesPipelinePreAdvisory();
    AcademyPipelineServiceModel[] GetAcademiesPipelinePostAdvisory();
    Task<AcademyPipelineServiceModel[]> GetAcademiesPipelineFreeSchoolsAsync(string uid);
    Task<string> GetAcademyTrustTrustReferenceNumberAsync(string uid);
}

public class AcademyService(
    IAcademyRepository academyRepository,
    IPipelineEstablishmentRepository pipelineEstablishmentRepository,
    IFreeSchoolMealsAverageProvider freeSchoolMealsAverageProvider) : IAcademyService
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

    public async Task<string> GetAcademyTrustTrustReferenceNumberAsync(string uid)
    {
        return await academyRepository.GetAcademyTrustTrustReferenceNumberAsync(uid) ?? string.Empty;
    }

    // MOCK METHOD
    // Replace with real code later
    [ExcludeFromCodeCoverage]
    public AcademyPipelineSummaryServiceModel GetAcademiesPipelineSummary()
    {
        return new AcademyPipelineSummaryServiceModel(4, 4, 4);
    }

    // MOCK METHOD
    // Replace with real code later
    [ExcludeFromCodeCoverage]
    public AcademyPipelineServiceModel[] GetAcademiesPipelinePreAdvisory()
    {
        return
        [
            new AcademyPipelineServiceModel("1234", "Baking academy", new AgeRange(4, 16), "Bristol", "Conversion",
                new DateTime(2025, 3, 3)),
            new AcademyPipelineServiceModel("1234", "Chocolate academy", new AgeRange(11, 18), "Birmingham",
                "Conversion",
                new DateTime(2025, 5, 3)),
            new AcademyPipelineServiceModel("1234", "Fruity academy", new AgeRange(9, 16), "Sheffield", "Transfer",
                new DateTime(2025, 9, 3)),
            new AcademyPipelineServiceModel(null, null, null, null, null, null)
        ];
    }

    // MOCK METHOD
    // Replace with real code later
    [ExcludeFromCodeCoverage]
    public AcademyPipelineServiceModel[] GetAcademiesPipelinePostAdvisory()
    {
        return
        [
            new AcademyPipelineServiceModel("1234", "Baking academy", new AgeRange(4, 16), "Bristol", "Conversion",
                new DateTime(2025, 3, 3)),
            new AcademyPipelineServiceModel("1234", "Chocolate academy", new AgeRange(11, 18), "Birmingham",
                "Conversion",
                new DateTime(2025, 5, 3)),
            new AcademyPipelineServiceModel("1234", "Fruity academy", new AgeRange(9, 16), "Sheffield", "Transfer",
                new DateTime(2025, 9, 3)),
            new AcademyPipelineServiceModel(null, null, null, null, null, null)
        ];
    }

    [ExcludeFromCodeCoverage]
    public async Task<AcademyPipelineServiceModel[]> GetAcademiesPipelineFreeSchoolsAsync(string trustReferenceNumber)
    {
        PipelineEstablishment[]? freeSchools = await pipelineEstablishmentRepository.GetPipelineFreeSchoolProjects(trustReferenceNumber);

        if (freeSchools is null || freeSchools.Length == 0)
        {
            return Array.Empty<AcademyPipelineServiceModel>();
        }

        return freeSchools.Select(fs => new AcademyPipelineServiceModel(
            fs.Urn,
            fs.EstablishmentName,
            fs.AgeRange,
            fs.LocalAuthority,
            fs.ProjectType,
            fs.ChangeDate
        )).ToArray();
    }
}
