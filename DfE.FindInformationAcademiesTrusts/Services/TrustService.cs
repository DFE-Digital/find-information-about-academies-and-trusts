using DfE.FindInformationAcademiesTrusts.Data.Repositories;
using DfE.FindInformationAcademiesTrusts.ServiceModels;
using Microsoft.Extensions.Caching.Memory;

namespace DfE.FindInformationAcademiesTrusts.Services;

public interface ITrustService
{
    Task<TrustSummaryServiceModel?> GetTrustSummaryAsync(string uid);
    Task<TrustDetailsServiceModel> GetTrustDetailsAsync(string uid);
}

public class TrustService(
    IAcademyRepository academyRepository,
    ITrustRepository trustRepository,
    IMemoryCache memoryCache)
    : ITrustService
{
    public async Task<TrustSummaryServiceModel?> GetTrustSummaryAsync(string uid)
    {
        var cacheKey = $"{nameof(TrustService)}:{uid}";

        if (memoryCache.TryGetValue(cacheKey, out TrustSummaryServiceModel? cachedTrustSummary))
        {
            return cachedTrustSummary!;
        }

        var summary = await trustRepository.GetTrustSummaryAsync(uid);

        if (summary is null)
        {
            return null;
        }

        var count = await academyRepository.GetNumberOfAcademiesInTrustAsync(uid);

        var trustSummaryServiceModel = new TrustSummaryServiceModel(uid, summary.Name, summary.Type, count);

        memoryCache.Set(cacheKey, trustSummaryServiceModel,
            new MemoryCacheEntryOptions { SlidingExpiration = TimeSpan.FromMinutes(10) });

        return trustSummaryServiceModel;
    }

    public async Task<TrustDetailsServiceModel> GetTrustDetailsAsync(string uid)
    {
        var singleAcademyTrustAcademyUrn = await academyRepository.GetSingleAcademyTrustAcademyUrnAsync(uid);

        var trustDetails = await trustRepository.GetTrustDetailsAsync(uid);

        var trustDetailsDto = new TrustDetailsServiceModel(
            trustDetails.Uid,
            trustDetails.GroupId,
            trustDetails.Ukprn,
            trustDetails.CompaniesHouseNumber,
            trustDetails.Type,
            trustDetails.Address,
            trustDetails.RegionAndTerritory,
            singleAcademyTrustAcademyUrn,
            trustDetails.OpenedDate
        );

        return trustDetailsDto;
    }
}
