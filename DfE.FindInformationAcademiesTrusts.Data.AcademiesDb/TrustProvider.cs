using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb;

public class TrustProvider : ITrustProvider
{
    private readonly IAcademiesDbContext _academiesDbContext;
    private readonly ITrustHelper _trustHelper;

    [ExcludeFromCodeCoverage]
    public TrustProvider(AcademiesDbContext academiesDbContext, ITrustHelper trustHelper) : this(
        (IAcademiesDbContext)academiesDbContext, trustHelper)
    {
    }

    public TrustProvider(IAcademiesDbContext academiesDbContext, ITrustHelper trustHelper)
    {
        _academiesDbContext = academiesDbContext;
        _trustHelper = trustHelper;
    }

    public async Task<Trust?> GetTrustByUidAsync(string uid)
    {
        Trust? trust = null;

        var group = await _academiesDbContext.Groups.SingleOrDefaultAsync(g => g.GroupUid == uid);
        var mstrTrust = await _academiesDbContext.MstrTrusts.SingleOrDefaultAsync(m => m.GroupUid == uid);
        if (group is not null && mstrTrust is not null)
        {
            trust = _trustHelper.CreateTrustFrom(group, mstrTrust);
        }

        return trust;
    }
}
