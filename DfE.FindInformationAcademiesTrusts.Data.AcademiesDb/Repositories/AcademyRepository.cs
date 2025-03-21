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
    private static readonly DateTime
        SingleHeadlineGradesPolicyChangeDate = new(2024, 09, 02, 0, 0, 0, DateTimeKind.Utc);

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
            await academiesDbContext.MisMstrEstablishmentsFiat
                .Where(me => urns.Contains(me.Urn))
                .Select(me => new AcademyOfstedRatings(
                    me.Urn,
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
        // Check to see if all ratings have been found in MisEstablishments, if not search in MisFurtherEducationEstablishments
        // Note: if an entry is in MisEstablishments then it will not be in MisFurtherEducationEstablishments, even if it has no ofsted data
        if (urns.Length != ofstedRatings.Count)
        {
            var urnsNotInMisEstablishments = urns.Except(ofstedRatings.Select(a => a.Urn)).ToArray();
            ofstedRatings.AddRange(
                await academiesDbContext.MisMstrFurtherEducationEstablishmentsFiat
                    .Where(mfe => urnsNotInMisEstablishments.Contains(mfe.ProviderUrn))
                    .Select(mfe =>
                        new AcademyOfstedRatings(
                            mfe.ProviderUrn,
                            new OfstedRating(
                                ConvertOverallEffectivenessToOfstedRatingScore(mfe.OverallEffectiveness),
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
                                ConvertOverallEffectivenessToOfstedRatingScore(mfe.PreviousOverallEffectiveness),
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

        // Ensure that there are no single headline grades after policy change on 2nd September 2024
        if (ofstedRatings.Any(rating =>
                HasSingleHeadlineGradeIssuedAfterPolicyChange(rating.Current) ||
                HasSingleHeadlineGradeIssuedAfterPolicyChange(rating.Previous)))
        {
            LogErrorForUrnsWithSingleHeadlineGradesAfterPolicyChange(ofstedRatings);

            ofstedRatings = RemoveSingleHeadlineGradesIssuedAfterPolicyChange(ofstedRatings);
        }

        return ofstedRatings.ToDictionary(o => o.Urn.ToString(), o => o);
    }

    private void LogErrorForUrnsWithSingleHeadlineGradesAfterPolicyChange(List<AcademyOfstedRatings> ofstedRatings)
    {
        foreach (var ofstedRating in ofstedRatings)
        {
            if (HasSingleHeadlineGradeIssuedAfterPolicyChange(ofstedRating.Current))
            {
                logger.LogError(
                    "URN {Urn} has a current Ofsted single headline grade of {score} issued on {InspectionDate} which was after single headline grades stopped being issued on 2nd September. This could be a data integrity issue with the Ofsted data in Academies Db.",
                    ofstedRating.Urn, ofstedRating.Current.OverallEffectiveness, ofstedRating.Current.InspectionDate);
            }

            if (HasSingleHeadlineGradeIssuedAfterPolicyChange(ofstedRating.Previous))
            {
                logger.LogError(
                    "URN {Urn} has a previous Ofsted single headline grade of {score} issued on {InspectionDate} which was after single headline grades stopped being issued on 2nd September. This could be a data integrity issue with the Ofsted data in Academies Db.",
                    ofstedRating.Urn, ofstedRating.Previous.OverallEffectiveness, ofstedRating.Previous.InspectionDate);
            }
        }
    }

    private static bool HasSingleHeadlineGradeIssuedAfterPolicyChange(OfstedRating rating)
    {
        return rating.InspectionDate >= SingleHeadlineGradesPolicyChangeDate &&
               rating.OverallEffectiveness != OfstedRatingScore.SingleHeadlineGradeNotAvailable;
    }

    private static List<AcademyOfstedRatings> RemoveSingleHeadlineGradesIssuedAfterPolicyChange(
        List<AcademyOfstedRatings> ratings)
    {
        return ratings.Select(rating => rating with
        {
            Current = HasSingleHeadlineGradeIssuedAfterPolicyChange(rating.Current)
                ? rating.Current with { OverallEffectiveness = OfstedRatingScore.SingleHeadlineGradeNotAvailable }
                : rating.Current,
            Previous = HasSingleHeadlineGradeIssuedAfterPolicyChange(rating.Previous)
                ? rating.Previous with { OverallEffectiveness = OfstedRatingScore.SingleHeadlineGradeNotAvailable }
                : rating.Previous
        }).ToList();
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
            return OfstedRatingScore.SingleHeadlineGradeNotAvailable;
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
