using System.Diagnostics.CodeAnalysis;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Factories;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Mstr;
using Microsoft.EntityFrameworkCore;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb;

public class TrustProvider : ITrustProvider
{
    private readonly IAcademiesDbContext _academiesDbContext;
    private readonly ITrustFactory _trustFactory;
    private readonly IPersonFactory _personFactory;
    private readonly IAcademiesProvider _academiesProvider;
    private readonly IGovernorProvider _governorProvider;

    [ExcludeFromCodeCoverage]
    public TrustProvider(AcademiesDbContext academiesDbContext, ITrustFactory trustFactory,
        IGovernorProvider governorProvider, IPersonFactory personFactory, IAcademiesProvider academiesProvider) : this(
        (IAcademiesDbContext)academiesDbContext, trustFactory, governorProvider, personFactory, academiesProvider)
    {
    }

    public TrustProvider(IAcademiesDbContext academiesDbContext, ITrustFactory trustFactory,
        IGovernorProvider governorProvider, IPersonFactory personFactory, IAcademiesProvider academiesProvider)
    {
        _personFactory = personFactory;
        _academiesProvider = academiesProvider;
        _academiesDbContext = academiesDbContext;
        _trustFactory = trustFactory;
        _governorProvider = governorProvider;
    }

    public async Task<Trust?> GetTrustByUidAsync(string uid)
    {
        var giasGroup = await _academiesDbContext.Groups.SingleOrDefaultAsync(g => g.GroupUid == uid);
        if (giasGroup is null) return null;

        Task<Academy[]> academiesTask;
        Task<(MstrTrust? mstrTrust, Person? trustRelationshipManager, Person? sfsoLead)> thingsTask;
        Task<Governor[]> governorsTask;

        Task.WaitAll(
            academiesTask = _academiesProvider.GetAcademiesLinkedTo(uid),
            governorsTask = _governorProvider.GetGovernorsLinkedTo(uid),
            thingsTask = GetThings(uid)
        );

        var academies = academiesTask.Result;
        var governors = governorsTask.Result;

        var (mstrTrust, trustRelationshipManager, sfsoLead) = thingsTask.Result;

        return _trustFactory.CreateTrustFrom(giasGroup, mstrTrust, academies, governors, trustRelationshipManager,
            sfsoLead);
    }

    private async Task<(MstrTrust? mstrTrust, Person? trustRelationshipManager, Person? sfsoLead)> GetThings(string uid)
    {
        var mstrTrust = await _academiesDbContext.MstrTrusts.SingleOrDefaultAsync(m => m.GroupUid == uid);
        var trustRelationshipManager = await GetTrustRelationshipManagerLinkedTo(uid);
        var sfsoLead = await GetSfsoLeadLinkedTo(uid);
        return (mstrTrust, trustRelationshipManager, sfsoLead);
    }

    private async Task<Person?> GetTrustRelationshipManagerLinkedTo(string uid)
    {
        return await _academiesDbContext.CdmAccounts
            .Where(a => a.SipUid == uid)
            .Join(_academiesDbContext.CdmSystemusers, account => account.SipTrustrelationshipmanager,
                systemuser => systemuser.Systemuserid, (_, systemuser) => _personFactory.CreateFrom(systemuser))
            .SingleOrDefaultAsync();
    }

    private async Task<Person?> GetSfsoLeadLinkedTo(string uid)
    {
        return await _academiesDbContext.CdmAccounts
            .Where(a => a.SipUid == uid)
            .Join(_academiesDbContext.CdmSystemusers, account => account.SipAmsdlead,
                systemuser => systemuser.Systemuserid, (_, systemuser) => _personFactory.CreateFrom(systemuser))
            .SingleOrDefaultAsync();
    }
}
