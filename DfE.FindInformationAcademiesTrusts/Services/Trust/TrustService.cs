using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Data.FiatDb.Repositories;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.Academy;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.Trust;
using Microsoft.Extensions.Caching.Memory;

namespace DfE.FindInformationAcademiesTrusts.Services.Trust;

public interface ITrustService
{
    Task<TrustSummaryServiceModel?> GetTrustSummaryAsync(string uid);
    Task<TrustGovernanceServiceModel> GetTrustGovernanceAsync(string uid);
    Task<TrustContactsServiceModel> GetTrustContactsAsync(string uid);
    Task<TrustOverviewServiceModel> GetTrustOverviewAsync(string uid);

    Task<TrustContactUpdatedServiceModel> UpdateContactAsync(int uid, string? name, string? email,
        ContactRole role);
}

public class TrustService(
    IAcademyRepository academyRepository,
    ITrustRepository trustRepository,
    IContactRepository contactRepository,
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

        var trustContacts =
            await trustRepository.GetTrustContactsAsync(uid, urn);
        var internalContacts = await contactRepository.GetInternalContactsAsync(uid);

        return new TrustContactsServiceModel(
            internalContacts.TrustRelationshipManager,
            internalContacts.SfsoLead,
            ChairOfTrustees: trustContacts.ChairOfTrustees,
            AccountingOfficer: trustContacts.AccountingOfficer,
            ChiefFinancialOfficer: trustContacts.ChiefFinancialOfficer
        );
    }

    public async Task<TrustContactUpdatedServiceModel> UpdateContactAsync(int uid, string? name, string? email,
        ContactRole role)
    {
        var (emailChanged, nameChanged) = await contactRepository.UpdateInternalContactsAsync(uid, name, email, role);

        return new TrustContactUpdatedServiceModel(emailChanged, nameChanged);
    }

    public async Task<TrustOverviewServiceModel> GetTrustOverviewAsync(string uid)
    {
        var trustOverview = await trustRepository.GetTrustOverviewAsync(uid);
        var trustType = trustOverview.Type switch
        {
            "Single-academy trust" => TrustType.SingleAcademyTrust,
            "Multi-academy trust" => TrustType.MultiAcademyTrust,
            _ => throw new InvalidOperationException($"Unknown trust type: {trustOverview.Type}")
        };

        var singleAcademyTrustAcademyUrn = trustType is TrustType.SingleAcademyTrust
            ? await academyRepository.GetSingleAcademyTrustAcademyUrnAsync(uid)
            : null;

        var academiesOverview = await academyRepository.GetAcademiesInTrustOverviewAsync(uid);

        var totalAcademies = academiesOverview.Length;

        var academiesByLocalAuthority = academiesOverview
            .GroupBy(a => a.LocalAuthority)
            .ToDictionary(g => g.Key, g => g.Count());

        var totalPupilNumbers = academiesOverview.Sum(a => a.NumberOfPupils ?? 0);
        var totalCapacity = academiesOverview.Sum(a => a.SchoolCapacity ?? 0);

        var overviewModel = new TrustOverviewServiceModel(
            trustOverview.Uid,
            trustOverview.GroupId,
            trustOverview.Ukprn,
            trustOverview.CompaniesHouseNumber,
            trustType,
            trustOverview.Address,
            trustOverview.RegionAndTerritory,
            singleAcademyTrustAcademyUrn,
            trustOverview.OpenedDate,
            totalAcademies,
            academiesByLocalAuthority,
            totalPupilNumbers,
            totalCapacity
        );

        return overviewModel;
    }
}
