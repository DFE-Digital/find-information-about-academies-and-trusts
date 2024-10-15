using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Contexts;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Extensions;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.Academy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Globalization;

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
    public async Task<AcademyDetails?> GetAcademyDetailsAsync(string urn)
    {
        return await academiesDbContext.GiasEstablishments
            .Where(e => e.Urn.ToString() == urn)
            .Select(e => new AcademyDetails(
                e.Urn.ToString(),
                e.EstablishmentName,
                e.TypeOfEstablishmentName,
                e.LaName,
                e.UrbanRuralName))
            .FirstOrDefaultAsync();
    }
    public async Task<IPaginatedList<AcademyDetails>> SearchAcademiesAsync(string searchTerm, int page = 1)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return PaginatedList<AcademyDetails>.Empty();
        }

        const int PageSize = 10;
        var lowerSearchTerm = searchTerm.ToLower();

        var query = academiesDbContext.GiasEstablishments
            .Where(e =>
                e.EstablishmentName != null &&
                (
                    e.Urn.ToString().Contains(lowerSearchTerm) ||
                    e.EstablishmentName.ToLower().Contains(lowerSearchTerm)
                )
            );

        var count = await query.CountAsync();

        var academies = await query
            .OrderBy(e => e.EstablishmentName)
            .Skip((page - 1) * PageSize)
            .Take(PageSize)
            .Select(e => new AcademyDetails(
                e.Urn.ToString(),
                e.EstablishmentName,
                e.LaName,
                e.TypeOfEstablishmentName,
                e.UrbanRuralName))
            .ToArrayAsync();

        return new PaginatedList<AcademyDetails>(academies, count, page, PageSize);
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

    private async Task<Dictionary<string, OfstedRatings>> GetOfstedRatings(int[] urns)
    {
        // Ofsted data is held in MisEstablishments for most academies
        var ofstedRatings =
            await academiesDbContext.MisEstablishments
                .Where(me => urns.Contains(me.Urn!.Value))
                .Select(me => new OfstedRatings(me.Urn!.Value, me.OverallEffectiveness, me.InspectionStartDate,
                    me.PreviousFullInspectionOverallEffectiveness, me.PreviousInspectionStartDate))
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
                        new OfstedRatings(mfe.ProviderUrn, mfe.OverallEffectiveness, mfe.LastDayOfInspection,
                            mfe.PreviousOverallEffectiveness, mfe.PreviousLastDayOfInspection))
                    .ToArrayAsync()
            );
        }

        foreach (var urn in urns.Except(ofstedRatings.Select(a => a.Urn)))
        {
            logger.LogError(
                "URN {Urn} was not found in Mis.Establishments or Mis.FurtherEducationEstablishments. This indicates a data integrity issue with the Ofsted data in Academies Db.",
                urn);
            ofstedRatings.Add(new OfstedRatings(urn, OfstedRating.None, OfstedRating.None));
        }

        return ofstedRatings.ToDictionary(o => o.Urn.ToString(), o => o);
    }

    public async Task<AcademyPupilNumbers[]> GetAcademiesInTrustPupilNumbersAsync(string uid)
    {
        return await academiesDbContext.GiasGroupLinks
            .Where(gl => gl.GroupUid == uid)
            .Join(academiesDbContext.GiasEstablishments,
                gl => gl.Urn!, e => e.Urn.ToString(),
                (_, e) =>
                    new AcademyPupilNumbers(e.Urn.ToString(),
                        e.EstablishmentName,
                        e.PhaseOfEducationName,
                        new AgeRange(e.StatutoryLowAge!, e.StatutoryHighAge!),
                        e.NumberOfPupils.ParseAsNullableInt(),
                        e.SchoolCapacity.ParseAsNullableInt()))
            .ToArrayAsync();
    }

    public async Task<AcademyFreeSchoolMeals[]> GetAcademiesInTrustFreeSchoolMealsAsync(string uid)
    {
        return await academiesDbContext.GiasGroupLinks
            .Where(gl => gl.GroupUid == uid)
            .Join(academiesDbContext.GiasEstablishments,
                gl => gl.Urn!, e => e.Urn.ToString(),
                (gl, e) =>
                    new AcademyFreeSchoolMeals(e.Urn.ToString(),
                        e.EstablishmentName,
                        e.PercentageFsm.ParseAsNullableDouble(),
                        int.Parse(e.LaCode!),
                        e.TypeOfEstablishmentName,
                        e.PhaseOfEducationName))
            .ToArrayAsync();
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

    public async Task<AcademyOverview[]> GetAcademiesInTrustOverviewAsync(string uid)
    {
        var academiesData = await academiesDbContext.GiasGroupLinks
            .Where(gl => gl.GroupUid == uid)
            .Join(
                academiesDbContext.GiasEstablishments,
                gl => gl.Urn!,
                e => e.Urn.ToString(),
                (gl, e) => new
                {
                    Urn = e.Urn.ToString(),
                    LocalAuthority = e.LaName,
                    NumberOfPupils = e.NumberOfPupils.ParseAsNullableInt(),
                    SchoolCapacity = e.SchoolCapacity.ParseAsNullableInt()
                })
            .ToListAsync();

        // Get URNs as ints
        var urns = academiesData.Select(a => int.Parse(a.Urn)).ToArray();

        // Fetch Ofsted ratings
        var ofstedRatingsDict = await GetOfstedRatings(urns);

        var academiesOverview = academiesData.Select(a =>
        {
            var currentOfstedRating = OfstedRatingScore.None;
            if (ofstedRatingsDict.TryGetValue(a.Urn, out var ofstedRatings))
            {
                currentOfstedRating = ofstedRatings.Current?.OfstedRatingScore ?? OfstedRatingScore.None;
            }

            return new AcademyOverview(
                a.Urn,
                a.LocalAuthority ?? string.Empty,
                a.NumberOfPupils,
                a.SchoolCapacity,
                currentOfstedRating
            );
        }).ToArray();

        return academiesOverview;
    }

    private sealed record OfstedRatings(int Urn, OfstedRating Current, OfstedRating Previous)
    {
        public OfstedRatings(int urn, int? currentOverallEffectiveness, string? currentInspectionDate,
            string? previousOverallEffectiveness, string? previousInspectionDate) :
            this(urn,
                currentOverallEffectiveness,
                currentInspectionDate,
                previousOverallEffectiveness is null ? null : int.Parse(previousOverallEffectiveness),
                previousInspectionDate)
        {
        }

        public OfstedRatings(int urn, int? currentOverallEffectiveness, string? currentInspectionDate,
            int? previousOverallEffectiveness, string? previousInspectionDate)
            : this(urn,
                currentOverallEffectiveness == null
                    ? OfstedRating.None
                    : new OfstedRating(
                        (OfstedRatingScore)currentOverallEffectiveness.Value,
                        currentInspectionDate.ParseAsNullableDate()
                    ),
                previousOverallEffectiveness == null
                    ? OfstedRating.None
                    : new OfstedRating(
                        (OfstedRatingScore)previousOverallEffectiveness,
                        previousInspectionDate.ParseAsNullableDate()
                    )
            )
        {
        }
    }
}
