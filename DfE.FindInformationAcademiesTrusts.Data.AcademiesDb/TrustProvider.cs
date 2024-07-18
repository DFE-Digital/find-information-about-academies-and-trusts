using System.Diagnostics.CodeAnalysis;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Factories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb;

public class TrustProvider : ITrustProvider
{
    private readonly IAcademiesDbContext _academiesDbContext;
    private readonly ITrustFactory _trustFactory;
    private readonly IAcademiesProvider _academiesProvider;
    private readonly IGovernorProvider _governorProvider;
    private readonly IPersonProvider _personProvider;
    private readonly IMemoryCache _memoryCache;
    //
    // [ExcludeFromCodeCoverage]
    // public TrustProvider(AcademiesDbContext academiesDbContext, ITrustFactory trustFactory,
    //     IGovernorProvider governorProvider, IAcademiesProvider academiesProvider,
    //     IPersonProvider personProvider, IMemoryCache memoryCache) : this(
    //     (IAcademiesDbContext)academiesDbContext, trustFactory, governorProvider, academiesProvider, personProvider, memoryCache)
    // {
    // }

    public TrustProvider(IAcademiesDbContext academiesDbContext, ITrustFactory trustFactory,
        IGovernorProvider governorProvider, IAcademiesProvider academiesProvider, IPersonProvider personProvider, IMemoryCache memoryCache)
    {
        _academiesProvider = academiesProvider;
        _personProvider = personProvider;
        _memoryCache = memoryCache;
        _academiesDbContext = academiesDbContext;
        _trustFactory = trustFactory;
        _governorProvider = governorProvider;
    }

    public async Task<Trust?> GetTrustByUidAsync(string uid)
    {
        var cacheKey = $"Trust UID: {uid}";
        if (_memoryCache.TryGetValue(cacheKey, out Trust? trust))
        {
            return trust;
        }
        
        trust = await GetTrustFromDb(uid);

        _memoryCache.Set(cacheKey, trust, new MemoryCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1), SlidingExpiration = TimeSpan.FromMinutes(10) });
        
        return trust;
    }

    private async Task<Trust?> GetTrustFromDb(string uid)
    {
        var giasGroup = await _academiesDbContext.Groups.SingleOrDefaultAsync(g => g.GroupUid == uid);
        if (giasGroup is null) return null;

        var academies = await _academiesProvider.GetAcademiesLinkedTo(uid);
        var governors = await _governorProvider.GetGovernorsLinkedTo(uid);
        var mstrTrust = await _academiesDbContext.MstrTrusts.SingleOrDefaultAsync(m => m.GroupUid == uid);
        var sfsoLead = await _personProvider.GetSfsoLeadLinkedTo(uid);
        var trustRelationshipManager = await _personProvider.GetTrustRelationshipManagerLinkedTo(uid);

        var myTrust = _trustFactory.CreateTrustFrom(giasGroup, mstrTrust, academies,
            governors, trustRelationshipManager,
            sfsoLead);
        return myTrust;
    }
}
