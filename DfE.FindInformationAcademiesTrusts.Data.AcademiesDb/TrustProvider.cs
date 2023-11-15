using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb;

public class TrustProvider : ITrustProvider
{
    private readonly IAcademiesDbContext _academiesDbContext;
    private readonly ITrustHelper _trustHelper;
    private readonly IAcademyHelper _academyHelper;

    [ExcludeFromCodeCoverage]
    public TrustProvider(AcademiesDbContext academiesDbContext, ITrustHelper trustHelper,
        IAcademyHelper academyHelper) : this(
        (IAcademiesDbContext)academiesDbContext, trustHelper, academyHelper)
    {
    }

    public TrustProvider(IAcademiesDbContext academiesDbContext, ITrustHelper trustHelper, IAcademyHelper academyHelper)
    {
        _academyHelper = academyHelper;
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
            var academies = await GetAcademiesLinkedTo(uid);
            trust = _trustHelper.CreateTrustFrom(group, mstrTrust, academies);
        }

        return trust;
    }

    private async Task<Academy[]> GetAcademiesLinkedTo(string uid)
    {
        return await _academiesDbContext
            .GroupLinks.Where(gl => gl.GroupUid == uid && gl.Urn != null)
            .Join(_academiesDbContext.Establishments,
                gl => gl.Urn!,
                e => e.Urn.ToString(),
                (gl, e) => _academyHelper.CreateAcademyFrom(gl, e))
            .ToArrayAsync();
    }
}
