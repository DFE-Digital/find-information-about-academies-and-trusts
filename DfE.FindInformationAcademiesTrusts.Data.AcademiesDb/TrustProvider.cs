using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Contexts;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Factories;
using Microsoft.EntityFrameworkCore;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb;

public class TrustProvider(
    IAcademiesDbContext academiesDbContext,
    ITrustFactory trustFactory,
    IAcademyFactory academyFactory,
    IGovernorFactory governorFactory,
    IPersonFactory personFactory)
    : ITrustProvider
{
    public async Task<Trust?> GetTrustByUidAsync(string uid)
    {
        var giasGroup = await academiesDbContext.Groups.SingleOrDefaultAsync(g => g.GroupUid == uid);
        if (giasGroup is null) return null;

        var mstrTrust = await academiesDbContext.MstrTrusts.SingleOrDefaultAsync(m => m.GroupUid == uid);
        var academies = await GetAcademiesLinkedTo(uid);
        var governors = await GetGovernorsLinkedTo(uid);
        var trustRelationshipManager = await GetTrustRelationshipManagerLinkedTo(uid);
        var sfsoLead = await GetSfsoLeadLinkedTo(uid);

        return trustFactory.CreateTrustFrom(giasGroup, mstrTrust, academies, governors, trustRelationshipManager,
            sfsoLead);
    }

    private async Task<Person?> GetTrustRelationshipManagerLinkedTo(string uid)
    {
        return await academiesDbContext.CdmAccounts
            .Where(a => a.SipUid == uid)
            .Join(academiesDbContext.CdmSystemusers, account => account.SipTrustrelationshipmanager,
                systemuser => systemuser.Systemuserid, (_, systemuser) => personFactory.CreateFrom(systemuser))
            .SingleOrDefaultAsync();
    }

    private async Task<Person?> GetSfsoLeadLinkedTo(string uid)
    {
        return await academiesDbContext.CdmAccounts
            .Where(a => a.SipUid == uid)
            .Join(academiesDbContext.CdmSystemusers, account => account.SipAmsdlead,
                systemuser => systemuser.Systemuserid, (_, systemuser) => personFactory.CreateFrom(systemuser))
            .SingleOrDefaultAsync();
    }

    private async Task<Governor[]> GetGovernorsLinkedTo(string uid)
    {
        return await academiesDbContext.GiasGovernances
            .Where(g => g.Uid == uid)
            .Join(academiesDbContext.TadTrustGovernances,
                gg => gg.Gid!,
                mtg => mtg.Gid,
                (gg, mtg) => governorFactory.CreateFrom(gg, mtg))
            .ToArrayAsync();
    }

    private async Task<Academy[]> GetAcademiesLinkedTo(string uid)
    {
        return await academiesDbContext
            .GiasGroupLinks.Where(gl => gl.GroupUid == uid && gl.Urn != null)
            .Select(
                gl =>
                    academyFactory.CreateFrom(
                        gl,
                        academiesDbContext.GiasEstablishments.First(e => e.Urn.ToString() == gl.Urn),
                        academiesDbContext.MisEstablishments.FirstOrDefault(me =>
                            me.Urn.ToString() == gl.Urn || me.UrnAtTimeOfLatestFullInspection.ToString() == gl.Urn),
                        academiesDbContext.MisEstablishments.FirstOrDefault(me =>
                            me.Urn.ToString() == gl.Urn || me.UrnAtTimeOfPreviousFullInspection.ToString() == gl.Urn),
                        academiesDbContext.MisFurtherEducationEstablishments.FirstOrDefault(mfe =>
                            mfe.ProviderUrn.ToString() == gl.Urn))
            )
            .ToArrayAsync();
    }
}
