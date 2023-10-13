using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb;

public class TrustProvider : ITrustProvider
{
    private readonly IAcademiesDbContext _academiesDbContext;

    [ExcludeFromCodeCoverage]
    public TrustProvider(AcademiesDbContext academiesDbContext) : this((IAcademiesDbContext)academiesDbContext)
    {
    }

    public TrustProvider(IAcademiesDbContext academiesDbContext)
    {
        _academiesDbContext = academiesDbContext;
    }

    public async Task<Trust?> GetTrustByGroupUidAsync(string groupUid)
    {
        Trust? trust = null;

        var group = await _academiesDbContext.Groups.SingleOrDefaultAsync(g => g.GroupUid == groupUid);
        if (group is not null)
        {
            trust = new Trust(
                group.GroupUid!,
                group.GroupName ?? string.Empty,
                group.Ukprn,
                group.GroupType ?? string.Empty
            );
        }

        return trust;
    }
}
