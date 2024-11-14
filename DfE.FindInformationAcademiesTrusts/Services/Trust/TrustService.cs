using DfE.FindInformationAcademiesTrusts.Data;
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
    IMemoryCache memoryCache,
    IDateTimeProvider dateTimeProvider)
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

        var governanceTurnover = CalculateTurnoverRate(trustGovernance);

        return new TrustGovernanceServiceModel(
            trustGovernance.CurrentTrustLeadership,
            trustGovernance.CurrentMembers,
            trustGovernance.CurrentTrustees,
            trustGovernance.HistoricMembers,
            governanceTurnover);
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

        var academiesOverview = await academyRepository.GetOverviewOfAcademiesInTrustAsync(uid);

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
    public decimal CalculateTurnoverRate(TrustGovernance trustGovernance)
    {
        var today = dateTimeProvider.Today;

        // Past 12 Months 
        var past12MonthsStart = today.AddYears(-1);

        // Get current governors (Trustees and Members)
        var currentGovernors = trustGovernance.CurrentTrustees
            .Concat(trustGovernance.CurrentMembers)
            .ToList();


        // Get all governors for event calculations (including HistoricMembers), excluding specified roles
        var allGovernorsExcludingLeadership = trustGovernance.CurrentTrustees
            .Concat(trustGovernance.CurrentMembers)
            .Concat(trustGovernance.HistoricMembers)
            .Where(g => !g.HasRoleLeadership)
            .ToList();

        // Total number of current governor positions
        int totalCurrentGovernors = currentGovernors.Count;

        // Appointments in the past 12 months
        int appointmentsInPast12Months = allGovernorsExcludingLeadership
            .Count(g => g.DateOfAppointment != null &&
                        g.DateOfAppointment >= past12MonthsStart &&
                        g.DateOfAppointment <= today);

        // Resignations in the past 12 months
        int resignationsInPast12Months = allGovernorsExcludingLeadership
            .Count(g => g.DateOfTermEnd != null &&
                        g.DateOfTermEnd >= past12MonthsStart &&
                        g.DateOfTermEnd <= today);

        int totalEvents = appointmentsInPast12Months + resignationsInPast12Months;

        // Calculate turnover rate and round to 1 decimal point
        return totalCurrentGovernors > 0
            ? Math.Round((decimal)totalEvents / totalCurrentGovernors * 100m, 1)
            : 0m;
    }
}
