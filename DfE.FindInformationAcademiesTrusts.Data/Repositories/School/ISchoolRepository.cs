namespace DfE.FindInformationAcademiesTrusts.Data.Repositories.School;

public interface ISchoolRepository
{
    Task<SchoolSummary?> GetSchoolSummaryAsync(string urn);
}
