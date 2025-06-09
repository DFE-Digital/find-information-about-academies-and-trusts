using Dfe.CaseAggregationService.Client.Contracts;
using DfE.FindInformationAcademiesTrusts.Data;

namespace DfE.FindInformationAcademiesTrusts.Services.ManageMyCasework
{
    public interface IGetCasesService
    {
        Task<IPaginatedList<UserCaseInfo>> GetCasesAsync(string? userName, string? userEmail, bool includeSignificantChange, bool includePrepare, bool includeComplete, bool includeManageFreeSchools, bool includeConcerns, bool includeWarningNotices, string? searchTerm, int page, int? recordCount, IEnumerable<string> filters, SortCriteria sorted);
    }
    public class GetCasesService: IGetCasesService
    {
        private readonly ICasesClient _caseAggregationServiceClient;
        public GetCasesService(ICasesClient caseAggregationServiceClient)
        {
            _caseAggregationServiceClient = caseAggregationServiceClient;
        }
        public async Task<IPaginatedList<UserCaseInfo>> GetCasesAsync(string? userName, string? userEmail, bool includeSignificantChange, bool includePrepare, bool includeComplete, bool includeManageFreeSchools, bool includeConcerns, bool includeWarningNotices, string? searchTerm, int page, int? recordCount, IEnumerable<string> filters, SortCriteria sorted)
        {
            var cases = await _caseAggregationServiceClient.GetCasesByUserAsync(userEmail, userName, includeSignificantChange, includePrepare, includeComplete, includeManageFreeSchools, includeConcerns, includeWarningNotices, searchTerm, filters, sorted, page, recordCount, "1");
            
            return new PaginatedList<UserCaseInfo>(cases.CaseInfos, cases.TotalRecordCount, page, 25);
        }
    }
}
