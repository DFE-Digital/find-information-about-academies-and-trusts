using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.Academy;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.PipelineAcademy;

namespace DfE.FindInformationAcademiesTrusts.Services.Academy;

public interface IAcademyService
{
    Task<AcademyDetailsServiceModel[]> GetAcademiesInTrustDetailsAsync(string uid);
    Task<AcademyOfstedServiceModel[]> GetAcademiesInTrustOfstedAsync(string uid);
    Task<AcademyPupilNumbersServiceModel[]> GetAcademiesInTrustPupilNumbersAsync(string uid);
    Task<AcademyFreeSchoolMealsServiceModel[]> GetAcademiesInTrustFreeSchoolMealsAsync(string uid);
    Task<AcademyPipelineSummaryServiceModel> GetAcademiesPipelineSummaryAsync(string trustReferenceNumber);
    Task<AcademyPipelineServiceModel[]> GetAcademiesPipelinePreAdvisoryAsync(string trustReferenceNumber);
    Task<AcademyPipelineServiceModel[]> GetAcademiesPipelinePostAdvisoryAsync(string trustReferenceNumber);
    Task<AcademyPipelineServiceModel[]> GetAcademiesPipelineFreeSchoolsAsync(string trustReferenceNumber);
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

    public async Task<AcademyPipelineSummaryServiceModel> GetAcademiesPipelineSummaryAsync(string trustReferenceNumber)
    {
        var repoSummary = await pipelineEstablishmentRepository.GetAcademiesPipelineSummaryAsync(trustReferenceNumber);

        return new AcademyPipelineSummaryServiceModel(
            repoSummary.PreAdvisoryCount,
            repoSummary.PostAdvisoryCount,
            repoSummary.FreeSchoolsCount
        );
    }



    public async Task<AcademyPipelineServiceModel[]> GetAcademiesPipelinePreAdvisoryAsync(string trustReferenceNumber)
    {
        var advisoryConversions =
            await pipelineEstablishmentRepository.GetAdvisoryConversionEstablishmentsAsync(trustReferenceNumber, AdvisoryType.PreAdvisory)
            ?? Array.Empty<PipelineEstablishment>();

        var advisoryTransfers =
            await pipelineEstablishmentRepository.GetAdvisoryTransferEstablishmentsAsync(trustReferenceNumber, AdvisoryType.PreAdvisory)
            ?? Array.Empty<PipelineEstablishment>();

        var preAdvisoryEstablishments = advisoryConversions
            .Concat(advisoryTransfers)
            .ToArray();

        if (preAdvisoryEstablishments.Length == 0)
        {
            return Array.Empty<AcademyPipelineServiceModel>();
        }

        return preAdvisoryEstablishments.Select(fs => new AcademyPipelineServiceModel(
            fs.Urn,
            fs.EstablishmentName,
            fs.AgeRange,
            fs.LocalAuthority,
            fs.ProjectType,
            fs.ChangeDate
        )).ToArray();
    }

    public async Task<AcademyPipelineServiceModel[]> GetAcademiesPipelinePostAdvisoryAsync(string trustReferenceNumber)
    {
        var advisoryConversions =
            await pipelineEstablishmentRepository.GetAdvisoryConversionEstablishmentsAsync(trustReferenceNumber, AdvisoryType.PostAdvisory)
            ?? Array.Empty<PipelineEstablishment>();

        var advisoryTransfers =
            await pipelineEstablishmentRepository.GetAdvisoryTransferEstablishmentsAsync(trustReferenceNumber, AdvisoryType.PostAdvisory)
            ?? Array.Empty<PipelineEstablishment>();

        var postAdvisoryEstablishments = advisoryConversions
            .Concat(advisoryTransfers)
            .ToArray();

        if (postAdvisoryEstablishments.Length == 0)
        {
            return Array.Empty<AcademyPipelineServiceModel>();
        }

        return postAdvisoryEstablishments.Select(fs => new AcademyPipelineServiceModel(
            fs.Urn,
            fs.EstablishmentName,
            fs.AgeRange,
            fs.LocalAuthority,
            fs.ProjectType,
            fs.ChangeDate
        )).ToArray();
    }

    public async Task<AcademyPipelineServiceModel[]> GetAcademiesPipelineFreeSchoolsAsync(string trustReferenceNumber)
    {
        PipelineEstablishment[]? freeSchools = await pipelineEstablishmentRepository.GetPipelineFreeSchoolProjectsAsync(trustReferenceNumber);

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
