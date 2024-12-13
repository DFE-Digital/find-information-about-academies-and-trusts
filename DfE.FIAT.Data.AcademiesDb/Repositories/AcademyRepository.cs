using System.Globalization;
using DfE.FIAT.Data.AcademiesDb.Contexts;
using DfE.FIAT.Data.AcademiesDb.Extensions;
using DfE.FIAT.Data.Repositories.Academy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DfE.FIAT.Data.AcademiesDb.Repositories;

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

    private async Task<Dictionary<string, AcademyOfstedRatings>> GetOfstedRatings(int[] urns)
    {
        // Ofsted data is held in MisEstablishments for most academies
        var ofstedRatings =
            await academiesDbContext.MisEstablishments
                .Where(me => urns.Contains(me.Urn!.Value))
                .Select(me => new AcademyOfstedRatings(
                    me.Urn!.Value,
                    new OfstedRating(
                        ConvertOverallEffectivenessToOfstedRatingScore(me.OverallEffectiveness),
                        ConvertNullableIntToOfstedRatingScore(me.QualityOfEducation),
                        ConvertNullableIntToOfstedRatingScore(me.BehaviourAndAttitudes),
                        ConvertNullableIntToOfstedRatingScore(me.PersonalDevelopment),
                        ConvertNullableIntToOfstedRatingScore(me.EffectivenessOfLeadershipAndManagement),
                        ConvertNullableIntToOfstedRatingScore(me.EarlyYearsProvisionWhereApplicable),
                        ConvertNullableIntToOfstedRatingScore(me.SixthFormProvisionWhereApplicable),
                        ConvertStringToCategoriesOfConcern(me.CategoryOfConcern),
                        ConvertStringToSafeguardingScore(me.SafeguardingIsEffective),
                        me.InspectionStartDate.ParseAsNullableDate()),
                    new OfstedRating(
                        ConvertOverallEffectivenessToOfstedRatingScore(me.PreviousFullInspectionOverallEffectiveness),
                        ConvertNullableIntToOfstedRatingScore(me.PreviousQualityOfEducation),
                        ConvertNullableIntToOfstedRatingScore(me.PreviousBehaviourAndAttitudes),
                        ConvertNullableIntToOfstedRatingScore(me.PreviousPersonalDevelopment),
                        ConvertNullableIntToOfstedRatingScore(me.PreviousEffectivenessOfLeadershipAndManagement),
                        ConvertNullableIntToOfstedRatingScore(me.PreviousEarlyYearsProvisionWhereApplicable),
                        ConvertNullableStringToOfstedRatingScore(me.PreviousSixthFormProvisionWhereApplicable),
                        ConvertStringToCategoriesOfConcern(me.PreviousCategoryOfConcern),
                        ConvertStringToSafeguardingScore(me.PreviousSafeguardingIsEffective),
                        me.PreviousInspectionStartDate.ParseAsNullableDate())))
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
                        new AcademyOfstedRatings(
                            mfe.ProviderUrn,
                            new OfstedRating(
                                ConvertNullableIntToOfstedRatingScore(mfe.OverallEffectiveness),
                                ConvertNullableIntToOfstedRatingScore(mfe.QualityOfEducation),
                                ConvertNullableIntToOfstedRatingScore(mfe.BehaviourAndAttitudes),
                                ConvertNullableIntToOfstedRatingScore(mfe.PersonalDevelopment),
                                ConvertNullableIntToOfstedRatingScore(mfe.EffectivenessOfLeadershipAndManagement),
                                OfstedRatingScore.NotInspected,
                                OfstedRatingScore.NotInspected,
                                CategoriesOfConcern.DoesNotApply,
                                ConvertStringToSafeguardingScore(mfe.IsSafeguardingEffective),
                                mfe.LastDayOfInspection.ParseAsNullableDate()),
                            new OfstedRating(
                                ConvertNullableIntToOfstedRatingScore(mfe.PreviousOverallEffectiveness),
                                ConvertNullableIntToOfstedRatingScore(mfe.PreviousQualityOfEducation),
                                ConvertNullableIntToOfstedRatingScore(mfe.PreviousBehaviourAndAttitudes),
                                ConvertNullableIntToOfstedRatingScore(mfe.PreviousPersonalDevelopment),
                                ConvertNullableIntToOfstedRatingScore(
                                    mfe.PreviousEffectivenessOfLeadershipAndManagement),
                                OfstedRatingScore.NotInspected,
                                OfstedRatingScore.NotInspected,
                                CategoriesOfConcern.DoesNotApply,
                                ConvertStringToSafeguardingScore(mfe.PreviousSafeguarding),
                                mfe.PreviousLastDayOfInspection.ParseAsNullableDate())))
                    .ToArrayAsync()
            );
        }

        // Log any URNs that couldn't be found
        foreach (var urn in urns.Except(ofstedRatings.Select(a => a.Urn)))
        {
            logger.LogError(
                "URN {Urn} was not found in Mis.Establishments or Mis.FurtherEducationEstablishments. This indicates a data integrity issue with the Ofsted data in Academies Db.",
                urn);
            ofstedRatings.Add(new AcademyOfstedRatings(urn, OfstedRating.Unknown, OfstedRating.Unknown));
        }

        //Log any errors that occured during parsing (we have to do this outside of the EF call)
        foreach (var ofstedRating in ofstedRatings.Where(rating =>
                     rating.Current.HasAnyUnknownRating || rating.Previous.HasAnyUnknownRating))
        {
            logger.LogError(
                "URN {Urn} has some unrecognised ofsted ratings. This could be a data integrity issue with the Ofsted data in Academies Db.",
                ofstedRating.Urn);
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

    public async Task<AcademyOverview[]> GetOverviewOfAcademiesInTrustAsync(string uid)
    {
        return await academiesDbContext.GiasGroupLinks
            .Where(gl => gl.GroupUid == uid)
            .Join(
                academiesDbContext.GiasEstablishments,
                gl => gl.Urn!,
                e => e.Urn.ToString(),
                (gl, e) =>
                    new AcademyOverview
                    (
                        e.Urn.ToString(),
                        e.LaName ?? string.Empty,
                        e.NumberOfPupils.ParseAsNullableInt(),
                        e.SchoolCapacity.ParseAsNullableInt()
                    ))
            .ToArrayAsync();
    }

    public static OfstedRatingScore ConvertOverallEffectivenessToOfstedRatingScore(string? rating)
    {
        // Check if it is 'Not judged' all other gradings are int based
        if (rating?.ToLower().Equals("not judged") ?? false)
        {
            return OfstedRatingScore.NoJudgement;
        }

        return ConvertNullableStringToOfstedRatingScore(rating);
    }

    private static OfstedRatingScore ConvertNullableIntToOfstedRatingScore(int? rating)
    {
        if (rating is null)
            return OfstedRatingScore.NotInspected;

        if (Enum.IsDefined(typeof(OfstedRatingScore), rating))
            return (OfstedRatingScore)rating;

        return OfstedRatingScore.Unknown;
    }

    private static OfstedRatingScore ConvertNullableStringToOfstedRatingScore(string? rating)
    {
        if (rating is null)
            return OfstedRatingScore.NotInspected;

        // Attempt to parse the string as an integer then cast to enum
        if (int.TryParse(rating, out var intRating) && Enum.IsDefined(typeof(OfstedRatingScore), intRating))
        {
            return (OfstedRatingScore)intRating;
        }

        return OfstedRatingScore.Unknown;
    }

    private static CategoriesOfConcern ConvertStringToCategoriesOfConcern(string? input)
    {
        return input switch
        {
            null => CategoriesOfConcern.NotInspected,
            "" => CategoriesOfConcern.NoConcerns,
            "SM" => CategoriesOfConcern.SpecialMeasures,
            "SWK" => CategoriesOfConcern.SeriousWeakness,
            "NTI" => CategoriesOfConcern.NoticeToImprove,
            _ => CategoriesOfConcern.Unknown
        };
    }

    private static SafeguardingScore ConvertStringToSafeguardingScore(string? input)
    {
        return input switch
        {
            null or "NULL" => SafeguardingScore.NotInspected,
            "Yes" => SafeguardingScore.Yes,
            "No" => SafeguardingScore.No,
            "9" => SafeguardingScore.NotRecorded,
            _ => SafeguardingScore.Unknown
        };
    }

    private sealed record AcademyOfstedRatings(int Urn, OfstedRating Current, OfstedRating Previous);
}
