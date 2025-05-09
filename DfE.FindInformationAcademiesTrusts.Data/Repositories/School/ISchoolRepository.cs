namespace DfE.FindInformationAcademiesTrusts.Data.Repositories.School;

public interface ISchoolRepository
{
    Task<SchoolSummary?> GetSchoolSummaryAsync(int urn);

    Task<SchoolDetails?> GetSchoolDetailsAsync(int urn);

    Task<DateOnly> GetDateJoinedTrustAsync(int urn);
}
