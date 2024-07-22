using System.Diagnostics;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Factories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb;

public interface IAcademiesProvider
{
    Task<Academy[]> GetAcademiesLinkedTo(string uid);
}

public class AcademiesProvider : IAcademiesProvider
{
    private readonly IAcademiesDbContext _academiesDbContext;
    private readonly IAcademyFactory _academyFactory;
    private readonly ILogger<AcademiesProvider> _logger;

    // [ExcludeFromCodeCoverage]
    // public AcademiesProvider(AcademiesDbContext academiesDbContext, IAcademyFactory academyFactory): this(
    //     (IAcademiesDbContext)academiesDbContext, academyFactory)
    // {
    //     
    // }
    //
    public AcademiesProvider(IAcademiesDbContext academiesDbContext, IAcademyFactory academyFactory,
        ILogger<AcademiesProvider> logger)
    {
        _academiesDbContext = academiesDbContext;
        _academyFactory = academyFactory;
        _logger = logger;
    }

    public async Task<Academy[]> GetAcademiesLinkedTo(string uid)
    {
        _logger.LogInformation("---------------------------------------");
        _logger.LogInformation("Start get academies");

        var timer = Stopwatch.StartNew();


#if false
        var thing = await _academiesDbContext
            .GiasGroupLinks
            .Where(gl => gl.GroupUid == uid && gl.Urn != null)
            .Select(
                gl => _academyFactory.CreateFrom(
                    gl, _academiesDbContext.GiasEstablishments.First(e => e.Urn.ToString() == gl.Urn), _academiesDbContext.MisEstablishments.FirstOrDefault(me =>
                        me.Urn.ToString() == gl.Urn || me.UrnAtTimeOfLatestFullInspection.ToString() == gl.Urn), _academiesDbContext.MisEstablishments.FirstOrDefault(me =>
                        me.Urn.ToString() == gl.Urn || me.UrnAtTimeOfPreviousFullInspection.ToString() == gl.Urn), _academiesDbContext.MisFurtherEducationEstablishments.FirstOrDefault(mfe =>
                        mfe.ProviderUrn.ToString() == gl.Urn))
            )
            .ToArrayAsync();
#else
        var thing = await _academiesDbContext
            .GiasGroupLinks
            .Where(gl => gl.GroupUid == uid && gl.Urn != null)
            .Join(_academiesDbContext.GiasEstablishments, link => link.Urn,
                establishment => establishment.Urn.ToString(),
                (gl, giasEstablishment) => _academyFactory.CreateFromExplicit(
                    giasEstablishment.Urn,
                    gl.JoinedDate!,
                    giasEstablishment.EstablishmentName,
                    giasEstablishment.TypeOfEstablishmentName,
                    giasEstablishment.LaName,
                    giasEstablishment.UrbanRuralName,
                    giasEstablishment.PhaseOfEducationName,
                    giasEstablishment.NumberOfPupils,
                    giasEstablishment.SchoolCapacity,
                    giasEstablishment.PercentageFsm,
                    giasEstablishment.StatutoryLowAge!,
                    giasEstablishment.StatutoryHighAge!,
                    giasEstablishment.LaCode!,
                    _academiesDbContext.MisEstablishments.FirstOrDefault(me =>
                        me.Urn.ToString() == gl.Urn || me.UrnAtTimeOfLatestFullInspection.ToString() == gl.Urn),
                    _academiesDbContext.MisEstablishments.FirstOrDefault(me =>
                        me.Urn.ToString() == gl.Urn || me.UrnAtTimeOfPreviousFullInspection.ToString() == gl.Urn),
                    _academiesDbContext.MisFurtherEducationEstablishments.FirstOrDefault(mfe =>
                        mfe.ProviderUrn.ToString() == gl.Urn))
            )
            .ToArrayAsync();
#endif

        timer.Stop();
        _logger.LogInformation("Finish get academies. Time elapsed: {time}", timer.Elapsed);
        _logger.LogInformation("---------------------------------------");


        return thing;
    }
}
