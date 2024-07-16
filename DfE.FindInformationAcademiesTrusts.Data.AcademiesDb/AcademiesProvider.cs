using System.Diagnostics.CodeAnalysis;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Factories;
using Microsoft.EntityFrameworkCore;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb;

public interface IAcademiesProvider
{
    Task<Academy[]> GetAcademiesLinkedTo(string uid);
}

public class AcademiesProvider : IAcademiesProvider
{
    private readonly IAcademiesDbContext _academiesDbContext;
    private readonly IAcademyFactory _academyFactory;

    [ExcludeFromCodeCoverage]
    public AcademiesProvider(AcademiesDbContext academiesDbContext, IAcademyFactory academyFactory): this(
        (IAcademiesDbContext)academiesDbContext, academyFactory)
    {
        
    }
    
    public AcademiesProvider(IAcademiesDbContext academiesDbContext, IAcademyFactory academyFactory)
    {
        _academiesDbContext = academiesDbContext;
        _academyFactory = academyFactory;
    }

    public async Task<Academy[]> GetAcademiesLinkedTo(string uid)
    {
        return await _academiesDbContext
            .GiasGroupLinks
            .AsSplitQuery()
            .Where(gl => gl.GroupUid == uid && gl.Urn != null)
            .Select(
                gl => _academyFactory.CreateFrom(
                    gl, _academiesDbContext.GiasEstablishments.First(e => e.Urn.ToString() == gl.Urn), _academiesDbContext.MisEstablishments.FirstOrDefault(me =>
                        me.Urn.ToString() == gl.Urn || me.UrnAtTimeOfLatestFullInspection.ToString() == gl.Urn), _academiesDbContext.MisEstablishments.FirstOrDefault(me =>
                        me.Urn.ToString() == gl.Urn || me.UrnAtTimeOfPreviousFullInspection.ToString() == gl.Urn), _academiesDbContext.MisFurtherEducationEstablishments.FirstOrDefault(mfe =>
                        mfe.ProviderUrn.ToString() == gl.Urn))
            )
            .ToArrayAsync();
    }
}
