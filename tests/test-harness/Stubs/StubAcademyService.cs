using DfE.FindInformationAcademiesTrusts.Services.Academy;

namespace test_harness;

public class StubAcademyService : IAcademyService
{
    private static Task<T[]> CreateAcademiesFromUid<T>(string uid)
    {
        return CreateAcademiesFromUid<T>(int.Parse(uid));
    }

    private static Task<T[]> CreateAcademiesFromUid<T>(int uid)
    {
        var numberOfAcademiesForUid = DataMakerator.GetNumberOfAcademiesForUid(uid);

        return Task.FromResult(
            Enumerable.Range(123000, numberOfAcademiesForUid)
                .Select(DataMakerator.CreateInstanceOfTypeFromId<T>)
                .ToArray()
        );
    }

    public Task<AcademyDetailsServiceModel[]> GetAcademiesInTrustDetailsAsync(string uid)
    {
        return CreateAcademiesFromUid<AcademyDetailsServiceModel>(uid);
    }

    public Task<AcademyOfstedServiceModel[]> GetAcademiesInTrustOfstedAsync(string uid)
    {
        return CreateAcademiesFromUid<AcademyOfstedServiceModel>(uid);
    }

    public Task<AcademyPupilNumbersServiceModel[]> GetAcademiesInTrustPupilNumbersAsync(string uid)
    {
        return CreateAcademiesFromUid<AcademyPupilNumbersServiceModel>(uid);
    }

    public Task<AcademyFreeSchoolMealsServiceModel[]> GetAcademiesInTrustFreeSchoolMealsAsync(string uid)
    {
        return CreateAcademiesFromUid<AcademyFreeSchoolMealsServiceModel>(uid);
    }

    public Task<AcademyPipelineSummaryServiceModel> GetAcademiesPipelineSummaryAsync(string trustReferenceNumber)
    {
        var uid = GetUidFromTrn(trustReferenceNumber);
//        return DataMakerator.CreateTaskOfTypeFromId<AcademyPipelineSummaryServiceModel>(uid);
        return Task.FromResult(new AcademyPipelineSummaryServiceModel(uid % 2, uid % 3, uid % 4));
    }

    private static int GetUidFromTrn(string trustReferenceNumber)
    {
        //   $"TRN{uid:d5}"
        return int.Parse(trustReferenceNumber.Replace("TRN", ""));
    }

    public Task<AcademyPipelineServiceModel[]> GetAcademiesPipelinePreAdvisoryAsync(string trustReferenceNumber)
    {
        var uid = GetUidFromTrn(trustReferenceNumber);
        return CreateAcademiesFromUid<AcademyPipelineServiceModel>(uid);
    }

    public Task<AcademyPipelineServiceModel[]> GetAcademiesPipelinePostAdvisoryAsync(string trustReferenceNumber)
    {
        var uid = GetUidFromTrn(trustReferenceNumber);
        return CreateAcademiesFromUid<AcademyPipelineServiceModel>(uid);
    }

    public Task<AcademyPipelineServiceModel[]> GetAcademiesPipelineFreeSchoolsAsync(string trustReferenceNumber)
    {
        var uid = GetUidFromTrn(trustReferenceNumber);
        return CreateAcademiesFromUid<AcademyPipelineServiceModel>(uid);
    }
}
