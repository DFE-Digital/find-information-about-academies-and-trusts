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

    [ExcludeFromCodeCoverage]
    public TrustProvider(AcademiesDbContext academiesDbContext, ITrustFactory trustFactory,
        IAcademyFactory academyFactory, IGovernorFactory governorFactory) : this(
        (IAcademiesDbContext)academiesDbContext, trustFactory, academyFactory, governorFactory)
    {
    }

    public TrustProvider(IAcademiesDbContext academiesDbContext, ITrustFactory trustFactory,
        IAcademyFactory academyFactory, IGovernorFactory governorFactory)
    {
        _academyFactory = academyFactory;
        _governorFactory = governorFactory;
        _academiesDbContext = academiesDbContext;
        _trustFactory = trustFactory;
    }

    public async Task<Trust?> GetTrustByUidAsync(string uid)
    {
        var group = await _academiesDbContext.Groups.SingleOrDefaultAsync(g => g.GroupUid == uid);
        if (group is null) return null;

        var mstrTrust = await _academiesDbContext.MstrTrusts.SingleOrDefaultAsync(m => m.GroupUid == uid);
        var academies = await GetAcademiesLinkedTo(uid);
        var governors = await GetGovernorsLinkedTo(uid);

        return _trustFactory.CreateTrustFrom(group, mstrTrust, academies, governors);
    }

    private async Task<Governor[]> GetGovernorsLinkedTo(string uid)
    {
        return await _academiesDbContext.Governances
            .Where(g => g.Uid == uid)
            .Select(g => _governorFactory.CreateFrom(g))
            .ToArrayAsync();
    }

    private async Task<Academy[]> GetAcademiesLinkedTo(string uid)
    {
        return await _academiesDbContext
            .GroupLinks.Where(gl => gl.GroupUid == uid && gl.Urn != null)
            .Join(_academiesDbContext.GiasEstablishments,
                gl => gl.Urn!,
                e => e.Urn.ToString(),
                (gl, e) => _academyFactory.CreateAcademyFrom(gl, e))
            .ToArrayAsync();
    }
}
