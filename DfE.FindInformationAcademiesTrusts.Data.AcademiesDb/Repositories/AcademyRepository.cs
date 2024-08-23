using System.Globalization;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Contexts;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Extensions;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.Academy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Repositories;

public class AcademyRepository(IAcademiesDbContext academiesDbContext, ILogger<AcademyRepository> logger)
    : IAcademyRepository
{
    public async Task<AcademyDetails[]> GetAcademiesInTrustDetailsAsync(string uid)
    {
        return await academiesDbContext.GiasGroupLinks
            .Where(gl => gl.GroupUid == uid)
            .Join(academiesDbContext.GiasEstablishments,
                gl => gl.Urn!, e => e.Urn.ToString(),
                (gl, e) =>
                    new AcademyDetails(e.Urn.ToString(),
                        e.EstablishmentName,
                        e.TypeOfEstablishmentName,
                        e.LaName,
                        e.UrbanRuralName))
            .ToArrayAsync();
    }

    public async Task<AcademyOfsted[]> GetAcademiesInTrustOfstedAsync(string uid)
    {
        var giasGroupLinkData = await academiesDbContext.GiasGroupLinks
            .Where(gl => gl.GroupUid == uid)
            .Select(gl => new
            {
                Urn = gl.Urn!,
                gl.EstablishmentName,
                gl.JoinedDate
            })
            .ToListAsync();

        var ofstedRatings = await GetOfstedRatings(giasGroupLinkData.Select(gl => int.Parse(gl.Urn)).ToArray());

        var academyOfsteds = giasGroupLinkData.Select(gl =>
                new AcademyOfsted(gl.Urn,
                    gl.EstablishmentName,
                    DateTime.ParseExact(gl.JoinedDate!, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    ofstedRatings[gl.Urn].Previous,
                    ofstedRatings[gl.Urn].Current
                ))
            .ToArray();

        return academyOfsteds;
    }

    private async Task<Dictionary<string, (OfstedRating Current, OfstedRating Previous)>> GetOfstedRatings(int[] urns)
    {
        // Ofsted data is held in MisEstablishments for most academies
        var ofstedRatings =
            await academiesDbContext.MisEstablishments
                .Where(me => urns.Contains(me.Urn!.Value))
                .Select(me => new
                {
                    Urn = me.Urn!.Value,
                    Current = me.OverallEffectiveness == null
                        ? OfstedRating.None
                        : new OfstedRating(
                            (OfstedRatingScore)me.OverallEffectiveness.Value,
                            me.InspectionEndDate.ParseAsNullableDate()
                        ),
                    Previous = me.PreviousFullInspectionOverallEffectiveness == null
                        ? OfstedRating.None
                        : new OfstedRating(
                            (OfstedRatingScore)int.Parse(me.PreviousFullInspectionOverallEffectiveness),
                            me.PreviousInspectionEndDate.ParseAsNullableDate()
                        )
                })
                .ToListAsync();

        // Look in MisFurtherEducationEstablishments for academies not found in MisEstablishments
        // Note: if an entry is in MisEstablishments then it will not be in MisFurtherEducationEstablishments, even if it has no ofsted data
        var urnsNotInMisEstablishments = urns.Except(ofstedRatings.Select(a => a.Urn)).ToArray();
        if (urnsNotInMisEstablishments.Length > 0)
        {
            ofstedRatings.AddRange(
                await academiesDbContext.MisFurtherEducationEstablishments
                    .Where(mfe => urnsNotInMisEstablishments.Contains(mfe.ProviderUrn))
                    .Select(mfe =>
                        new
                        {
                            Urn = mfe.ProviderUrn,
                            Current = mfe.OverallEffectiveness == null
                                ? OfstedRating.None
                                : new OfstedRating(
                                    (OfstedRatingScore)mfe.OverallEffectiveness.Value,
                                    mfe.LastDayOfInspection.ParseAsNullableDate()
                                ),
                            Previous = mfe.PreviousOverallEffectiveness == null
                                ? OfstedRating.None
                                : new OfstedRating(
                                    (OfstedRatingScore)mfe.PreviousOverallEffectiveness.Value,
                                    mfe.PreviousLastDayOfInspection.ParseAsNullableDate()
                                )
                        }
                    ).ToArrayAsync()
            );
        }

        foreach (var urn in urns.Except(ofstedRatings.Select(a => a.Urn)))
        {
            logger.LogError(
                "URN {Urn} was not found in Mis.Establishments or Mis.FurtherEducationEstablishments. This indicates a data integrity issue with the Ofsted data in Academies Db.",
                urn);
            ofstedRatings.Add(new { Urn = urn, Current = OfstedRating.None, Previous = OfstedRating.None });
        }

        return ofstedRatings.ToDictionary(o => o.Urn.ToString(), o => (o.Current, o.Previous));
    }

    public async Task<int> GetNumberOfAcademiesInTrustAsync(string uid)
    {
        return await academiesDbContext.GiasGroupLinks.CountAsync(gl => gl.GroupUid == uid && gl.Urn != null);
    }

    public async Task<string?> GetSingleAcademyTrustAcademyUrnAsync(string uid)
    {
        return await academiesDbContext.GiasGroupLinks
            .Where(gl => gl.GroupUid == uid
                         && gl.GroupType == "Single-academy trust")
            .Select(gl => gl.Urn)
            .FirstOrDefaultAsync();
    }
}
