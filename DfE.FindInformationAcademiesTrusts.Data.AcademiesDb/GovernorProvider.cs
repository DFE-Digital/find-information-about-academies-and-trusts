using System.Diagnostics.CodeAnalysis;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Factories;
using Microsoft.EntityFrameworkCore;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb;

public interface IGovernorProvider
{
    Task<Governor[]> GetGovernorsLinkedTo(string uid);
}

public class GovernorProvider : IGovernorProvider
{
    private readonly IAcademiesDbContext _academiesDbContext;
    private readonly IGovernorFactory _governorFactory;
    //
    // [ExcludeFromCodeCoverage]
    // public GovernorProvider(AcademiesDbContext academiesDbContext, IGovernorFactory governorFactory): this((IAcademiesDbContext)academiesDbContext, governorFactory)
    // {}

    public GovernorProvider(IAcademiesDbContext academiesDbContext, IGovernorFactory governorFactory)
    {
        _academiesDbContext = academiesDbContext;
        _governorFactory = governorFactory;
    }

    public async Task<Governor[]> GetGovernorsLinkedTo(string uid)
    {
        return await _academiesDbContext.GiasGovernances
            .Where(g => g.Uid == uid)
            .Join(_academiesDbContext.MstrTrustGovernances,
                gg => gg.Gid!,
                mtg => mtg.Gid,
                (gg, mtg) => _governorFactory.CreateFrom(gg, mtg))
            .ToArrayAsync();
    }
}
