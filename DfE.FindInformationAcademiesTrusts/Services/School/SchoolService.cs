using DfE.FindInformationAcademiesTrusts.Data.Enums;

namespace DfE.FindInformationAcademiesTrusts.Services.School;

public interface ISchoolService
{
    Task<SchoolSummaryServiceModel?> GetSchoolSummaryAsync(string urn);
}

public class SchoolService : ISchoolService
{
    public Task<SchoolSummaryServiceModel?> GetSchoolSummaryAsync(string urn)
    {
        var schoolName = urn.EndsWith('2') ? $"Super Cool School {urn}" : $"Super Chill Academy {urn}";
        var schoolType = urn.EndsWith('2') ? "Community school" : "Academy sponsor led";
        var schoolCategory = urn.EndsWith('2') ? SchoolCategory.LaMaintainedSchool : SchoolCategory.Academy;

        return Task.FromResult(new SchoolSummaryServiceModel(urn, schoolName, schoolType, schoolCategory))!;
    }
}
