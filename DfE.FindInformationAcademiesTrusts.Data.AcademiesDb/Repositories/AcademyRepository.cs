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
                .Select(me => new AcademyOfstedRatings(me.Urn!.Value,
                    new OfstedRating(
                        ConvertOverallEffectivenessToOfstedRatingScore(me.OverallEffectiveness),
                        (OfstedRatingScore?)me.QualityOfEducation ?? OfstedRatingScore.None,
                        (OfstedRatingScore?)me.BehaviourAndAttitudes ?? OfstedRatingScore.None,
                        (OfstedRatingScore?)me.PersonalDevelopment ?? OfstedRatingScore.None,
                        (OfstedRatingScore?)me.EffectivenessOfLeadershipAndManagement ?? OfstedRatingScore.None,
                        (OfstedRatingScore?)me.EarlyYearsProvisionWhereApplicable ?? OfstedRatingScore.None,
                        (OfstedRatingScore?)me.SixthFormProvisionWhereApplicable ?? OfstedRatingScore.None,
                        OfstedRating.ConvertStringToCategoriesOfConcern(me.CategoryOfConcern),
                        OfstedRating.ConvertStringToSafeguardingScore(me.SafeguardingIsEffective),
                        me.InspectionStartDate.ParseAsNullableDate()),
                    new OfstedRating(
                        (OfstedRatingScore?)me.PreviousFullInspectionOverallEffectiveness.ParseAsNullableInt() ??
                        OfstedRatingScore.None,
                        (OfstedRatingScore?)me.PreviousQualityOfEducation ?? OfstedRatingScore.None,
                        (OfstedRatingScore?)me.PreviousBehaviourAndAttitudes ?? OfstedRatingScore.None,
                        (OfstedRatingScore?)me.PreviousPersonalDevelopment ?? OfstedRatingScore.None,
                        (OfstedRatingScore?)me.PreviousEffectivenessOfLeadershipAndManagement ?? OfstedRatingScore.None,
                        (OfstedRatingScore?)me.PreviousEarlyYearsProvisionWhereApplicable ?? OfstedRatingScore.None,
                        (OfstedRatingScore?)me.PreviousSixthFormProvisionWhereApplicable.ParseAsNullableInt() ??
                        OfstedRatingScore.None,
                        OfstedRating.ConvertStringToCategoriesOfConcern(me.PreviousCategoryOfConcern),
                        OfstedRating.ConvertStringToSafeguardingScore(me.PreviousSafeguardingIsEffective),
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
                        new AcademyOfstedRatings(mfe.ProviderUrn, new OfstedRating(
                                (OfstedRatingScore?)mfe.OverallEffectiveness ?? OfstedRatingScore.None,
                                (OfstedRatingScore?)mfe.QualityOfEducation ?? OfstedRatingScore.None,
                                (OfstedRatingScore?)mfe.BehaviourAndAttitudes ?? OfstedRatingScore.None,
                                (OfstedRatingScore?)mfe.PersonalDevelopment ?? OfstedRatingScore.None,
                                (OfstedRatingScore?)mfe.EffectivenessOfLeadershipAndManagement ??
                                OfstedRatingScore.None,
                                OfstedRatingScore.None, OfstedRatingScore.None,
                                CategoriesOfConcern.None,
                                OfstedRating.ConvertStringToSafeguardingScore(mfe.IsSafeguardingEffective),
                                mfe.LastDayOfInspection.ParseAsNullableDate()),
                            new OfstedRating(
                                (OfstedRatingScore?)mfe.PreviousOverallEffectiveness ?? OfstedRatingScore.None,
                                (OfstedRatingScore?)mfe.PreviousQualityOfEducation ?? OfstedRatingScore.None,
                                (OfstedRatingScore?)mfe.PreviousBehaviourAndAttitudes ?? OfstedRatingScore.None,
                                (OfstedRatingScore?)mfe.PreviousPersonalDevelopment ?? OfstedRatingScore.None,
                                (OfstedRatingScore?)mfe.PreviousEffectivenessOfLeadershipAndManagement ??
                                OfstedRatingScore.None,
                                OfstedRatingScore.None, OfstedRatingScore.None,
                                CategoriesOfConcern.None,
                                OfstedRating.ConvertStringToSafeguardingScore(mfe.PreviousSafeguarding),
                                mfe.PreviousLastDayOfInspection.ParseAsNullableDate())))
                    .ToArrayAsync()
            );
        }

        foreach (var urn in urns.Except(ofstedRatings.Select(a => a.Urn)))
        {
            logger.LogError(
                "URN {Urn} was not found in Mis.Establishments or Mis.FurtherEducationEstablishments. This indicates a data integrity issue with the Ofsted data in Academies Db.",
                urn);
            ofstedRatings.Add(new AcademyOfstedRatings(urn, OfstedRating.None, OfstedRating.None));
        }

        return ofstedRatings.ToDictionary(o => o.Urn.ToString(), o => o);
    }
    public static OfstedRatingScore ConvertOverallEffectivenessToOfstedRatingScore(string? rating)
    {
        if (string.IsNullOrWhiteSpace(rating))
            return OfstedRatingScore.None;

        // Check if it is 'Not judged' all other gradings are int based
        if (rating.ToLower().Equals("not judged"))
        {
            return OfstedRatingScore.NoJudgement;
        }

        // Attempt to parse the string as an integer
        if (int.TryParse(rating, out int intRating) && Enum.IsDefined(typeof(OfstedRatingScore), intRating))
        {
            return (OfstedRatingScore)intRating;
        }

        // Default case if parsing fails
        return OfstedRatingScore.None;
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

    private sealed record AcademyOfstedRatings(int Urn, OfstedRating Current, OfstedRating Previous);
}
