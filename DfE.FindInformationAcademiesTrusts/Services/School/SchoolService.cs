using DfE.FindInformationAcademiesTrusts.Data.Repositories.School;
using Microsoft.Extensions.Caching.Memory;

namespace DfE.FindInformationAcademiesTrusts.Services.School;

public interface ISchoolService
{
    Task<SchoolSummaryServiceModel?> GetSchoolSummaryAsync(string urn);
}

public class SchoolService(IMemoryCache memoryCache, ISchoolRepository schoolRepository) : ISchoolService
{
    public async Task<SchoolSummaryServiceModel?> GetSchoolSummaryAsync(string urn)
    {
        var cacheKey = $"{nameof(GetSchoolSummaryAsync)}:{urn}";

        if (memoryCache.TryGetValue(cacheKey, out SchoolSummaryServiceModel? cachedTrustSummary))
        {
            return cachedTrustSummary;
        }

        var summary = await schoolRepository.GetSchoolSummaryAsync(urn);

        if (summary is null)
        {
            return null;
        }

        var schoolSummaryServiceModel =
            new SchoolSummaryServiceModel(urn, summary.Name, summary.Type, summary.Category);

        memoryCache.Set(cacheKey, schoolSummaryServiceModel,
            new MemoryCacheEntryOptions { SlidingExpiration = TimeSpan.FromMinutes(10) });

        return schoolSummaryServiceModel;
    }
}
