using System.Diagnostics.CodeAnalysis;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Factories;
using Microsoft.EntityFrameworkCore;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb;

public class TrustProvider : ITrustProvider
{
    private readonly IAcademiesDbContext _academiesDbContext;
    private readonly ITrustFactory _trustFactory;
    private readonly IAcademiesProvider _academiesProvider;
    private readonly IGovernorProvider _governorProvider;
    private readonly IPersonProvider _personProvider;

    [ExcludeFromCodeCoverage]
    public TrustProvider(AcademiesDbContext academiesDbContext, ITrustFactory trustFactory,
        IGovernorProvider governorProvider, IAcademiesProvider academiesProvider,
        IPersonProvider personProvider) : this(
        (IAcademiesDbContext)academiesDbContext, trustFactory, governorProvider, academiesProvider, personProvider)
    {
    }

    public TrustProvider(IAcademiesDbContext academiesDbContext, ITrustFactory trustFactory,
        IGovernorProvider governorProvider, IAcademiesProvider academiesProvider, IPersonProvider personProvider)
    {
        _academiesProvider = academiesProvider;
        _personProvider = personProvider;
        _academiesDbContext = academiesDbContext;
        _trustFactory = trustFactory;
        _governorProvider = governorProvider;
    }

    public async Task<Trust?> GetTrustByUidAsync(string uid)
    {
        var giasGroup = await _academiesDbContext.Groups.SingleOrDefaultAsync(g => g.GroupUid == uid);
        if (giasGroup is null) return null;

        var academiesTask = _academiesProvider.GetAcademiesLinkedTo(uid);
        var governorsTask = _governorProvider.GetGovernorsLinkedTo(uid);
        var mstrTrustTask = _academiesDbContext.MstrTrusts.SingleOrDefaultAsync(m => m.GroupUid == uid);

        Task.WaitAll(academiesTask,
            governorsTask,
            mstrTrustTask);

        var sfsoLead =await _personProvider.GetSfsoLeadLinkedTo(uid);
        var trustRelationshipManager = await _personProvider.GetTrustRelationshipManagerLinkedTo(uid);
        
        return _trustFactory.CreateTrustFrom(giasGroup, mstrTrustTask.Result, academiesTask.Result,
            governorsTask.Result, trustRelationshipManager,
            sfsoLead);
    }
}
