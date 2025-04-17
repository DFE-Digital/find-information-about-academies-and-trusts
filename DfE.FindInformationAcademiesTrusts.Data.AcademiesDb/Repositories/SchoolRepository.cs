using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.School;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Repositories;

public class SchoolRepository : ISchoolRepository
{
    public Task<SchoolSummary?> GetSchoolSummaryAsync(string urn)
    {
        if (urn.StartsWith('3'))
            return Task.FromResult<SchoolSummary?>(null);

        var schoolName = urn.EndsWith('2') ? $"Cool School {urn}" : $"Chill Academy {urn}";
        var schoolType = urn.EndsWith('2') ? "Community school" : "Academy sponsor led";
        var schoolCategory = urn.EndsWith('2') ? SchoolCategory.LaMaintainedSchool : SchoolCategory.Academy;

        return Task.FromResult(new SchoolSummary(schoolName, schoolType, schoolCategory))!;
    }
}
