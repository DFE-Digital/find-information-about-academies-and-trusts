using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Services.Trust;

namespace test_harness;

public class StubTrustService : ITrustService
{
    public Task<TrustSummaryServiceModel?> GetTrustSummaryAsync(string uid)
    {
        return DataMakerator.CreateTaskOfTypeFromId<TrustSummaryServiceModel?>(uid);
    }

    public Task<TrustGovernanceServiceModel> GetTrustGovernanceAsync(string uid)
    {
        return DataMakerator.CreateTaskOfTypeFromId<TrustGovernanceServiceModel>(uid);
    }

    public Task<TrustContactsServiceModel> GetTrustContactsAsync(string uid)
    {
        return DataMakerator.CreateTaskOfTypeFromId<TrustContactsServiceModel>(uid);
    }

    public Task<TrustOverviewServiceModel> GetTrustOverviewAsync(string uid)
    {
        return DataMakerator.CreateTaskOfTypeFromId<TrustOverviewServiceModel>(uid);
    }

    public Task<TrustContactUpdatedServiceModel> UpdateContactAsync(int uid, string? name, string? email,
        ContactRole role)
    {
        return DataMakerator.CreateTaskOfTypeFromId<TrustContactUpdatedServiceModel>(uid);
    }

    public Task<string> GetTrustReferenceNumberAsync(string uid)
    {
        return Task.FromResult($"TRN000{uid}");
    }
}
