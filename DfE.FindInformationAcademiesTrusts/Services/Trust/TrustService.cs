using DfE.FindInformationAcademiesTrusts.Data.Repositories.Academy;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.Trust;
using Microsoft.Extensions.Caching.Memory;

namespace DfE.FindInformationAcademiesTrusts.Services.Trust;

public interface ITrustService
{
    Task<TrustSummaryServiceModel?> GetTrustSummaryAsync(string uid);
    Task<TrustDetailsServiceModel> GetTrustDetailsAsync(string uid);
    Task<TrustGovernanceServiceModel> GetTrustGovernanceAsync(string uid);
    Task<TrustContactsServiceModel> GetTrustContactsAsync(string uid);
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
    public async Task<TrustGovernanceServiceModel> GetTrustGovernanceAsync(string uid)
    {
        var urn = await academyRepository.GetSingleAcademyTrustAcademyUrnAsync(uid);

        var trustGovernance = await trustRepository.GetTrustGovernanceAsync(uid, urn);

        return new TrustGovernanceServiceModel(
            trustGovernance.TrustLeadership,
            trustGovernance.Members,
            trustGovernance.Trustees,
            trustGovernance.HistoricMembers);
    }

    public async Task<TrustContactsServiceModel> GetTrustContactsAsync(string uid)
    {
        var urn = await academyRepository.GetSingleAcademyTrustAcademyUrnAsync(uid);

        var (trustRelationshipManager, sfsoLead, accountingOfficer, chairOfTrustees, chiefFinancialOfficer) =
            await trustRepository.GetTrustContactsAsync(uid, urn);

        return new TrustContactsServiceModel(trustRelationshipManager, sfsoLead, accountingOfficer, chairOfTrustees,
            chiefFinancialOfficer);
    }
}
