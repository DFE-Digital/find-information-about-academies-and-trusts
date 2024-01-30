using System.Diagnostics.CodeAnalysis;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Factories;
using Microsoft.EntityFrameworkCore;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb;

public class TrustProvider : ITrustProvider
{
    private readonly IAcademiesDbContext _academiesDbContext;
    private readonly ITrustFactory _trustFactory;
    private readonly IAcademyFactory _academyFactory;
    private readonly IGovernorFactory _governorFactory;
    private readonly IPersonFactory _personFactory;

    [ExcludeFromCodeCoverage]
    public TrustProvider(AcademiesDbContext academiesDbContext, ITrustFactory trustFactory,
        IAcademyFactory academyFactory, IGovernorFactory governorFactory, IPersonFactory personFactory) : this(
        (IAcademiesDbContext)academiesDbContext, trustFactory, academyFactory, governorFactory, personFactory)
    {
    }

    public TrustProvider(IAcademiesDbContext academiesDbContext, ITrustFactory trustFactory,
        IAcademyFactory academyFactory, IGovernorFactory governorFactory, IPersonFactory personFactory)
    {
        _academyFactory = academyFactory;
        _governorFactory = governorFactory;
        _personFactory = personFactory;
        _academiesDbContext = academiesDbContext;
        _trustFactory = trustFactory;
    }

    public async Task<Trust?> GetTrustByUidAsync(string uid)
    {
        var giasGroup = await _academiesDbContext.Groups.SingleOrDefaultAsync(g => g.GroupUid == uid);
        if (giasGroup is null) return null;

        var mstrTrust = await _academiesDbContext.MstrTrusts.SingleOrDefaultAsync(m => m.GroupUid == uid);
        var academies = await GetAcademiesLinkedTo(uid);
        var governors = await GetGovernorsLinkedTo(uid);
        var trustRelationshipManager = await GetTrustRelationshipManagerLinkedTo(uid);
        var sfsoLead = await GetSfsoLeadLinkedTo(uid);

        return _trustFactory.CreateTrustFrom(giasGroup, mstrTrust, academies, governors, trustRelationshipManager,
            sfsoLead);
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

    private async Task<Governor[]> GetGovernorsLinkedTo(string uid)
    {
        return await _academiesDbContext.GiasGovernances
            .Where(g => g.Uid == uid)
            .Join(_academiesDbContext.MstrTrustGovernances,
                gg => gg.Gid!,
                mtg => mtg.Gid,
                (gg, mtg) => _governorFactory.CreateFrom(gg, mtg))
            .ToArrayAsync();
    }

    private async Task<Academy[]> GetAcademiesLinkedTo(string uid)
    {
        return await _academiesDbContext
            .GiasGroupLinks.Where(gl => gl.GroupUid == uid && gl.Urn != null)
            .Select(
                gl =>
                    _academyFactory.CreateFrom(
                        gl,
                        _academiesDbContext.GiasEstablishments.First(e => e.Urn.ToString() == gl.Urn),
                        _academiesDbContext.MisEstablishments.FirstOrDefault(me =>
                            me.Urn.ToString() == gl.Urn || me.UrnAtTimeOfLatestFullInspection.ToString() == gl.Urn),
                        _academiesDbContext.MisEstablishments.FirstOrDefault(me =>
                            me.Urn.ToString() == gl.Urn || me.UrnAtTimeOfPreviousFullInspection.ToString() == gl.Urn),
                        _academiesDbContext.MisFurtherEducationEstablishments.FirstOrDefault(mfe =>
                            mfe.ProviderUrn.ToString() == gl.Urn))
            )
            .ToArrayAsync();
    }
}
