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
                        (OfstedRatingScore?)me.QualityOfEducation ?? OfstedRatingScore.NotInspected,
                        (OfstedRatingScore?)me.BehaviourAndAttitudes ?? OfstedRatingScore.NotInspected,
                        (OfstedRatingScore?)me.PersonalDevelopment ?? OfstedRatingScore.NotInspected,
                        (OfstedRatingScore?)me.EffectivenessOfLeadershipAndManagement ?? OfstedRatingScore.NotInspected,
                        (OfstedRatingScore?)me.EarlyYearsProvisionWhereApplicable ?? OfstedRatingScore.NotInspected,
                        (OfstedRatingScore?)me.SixthFormProvisionWhereApplicable ?? OfstedRatingScore.NotInspected,
                        ConvertStringToCategoriesOfConcern(me.CategoryOfConcern),
                        OfstedRating.ConvertStringToSafeguardingScore(me.SafeguardingIsEffective),
                        me.InspectionStartDate.ParseAsNullableDate()),
                    new OfstedRating(
                        ConvertOverallEffectivenessToOfstedRatingScore(me.PreviousFullInspectionOverallEffectiveness),
                        (OfstedRatingScore?)me.PreviousQualityOfEducation ?? OfstedRatingScore.NotInspected,
                        (OfstedRatingScore?)me.PreviousBehaviourAndAttitudes ?? OfstedRatingScore.NotInspected,
                        (OfstedRatingScore?)me.PreviousPersonalDevelopment ?? OfstedRatingScore.NotInspected,
                        (OfstedRatingScore?)me.PreviousEffectivenessOfLeadershipAndManagement ??
                        OfstedRatingScore.NotInspected,
                        (OfstedRatingScore?)me.PreviousEarlyYearsProvisionWhereApplicable ??
                        OfstedRatingScore.NotInspected,
                        (OfstedRatingScore?)me.PreviousSixthFormProvisionWhereApplicable.ParseAsNullableInt() ??
                        OfstedRatingScore.NotInspected,
                        ConvertStringToCategoriesOfConcern(me.PreviousCategoryOfConcern),
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
                        new AcademyOfstedRatings(
                            mfe.ProviderUrn,
                            new OfstedRating(
                                (OfstedRatingScore?)mfe.OverallEffectiveness ?? OfstedRatingScore.NotInspected,
                                (OfstedRatingScore?)mfe.QualityOfEducation ?? OfstedRatingScore.NotInspected,
                                (OfstedRatingScore?)mfe.BehaviourAndAttitudes ?? OfstedRatingScore.NotInspected,
                                (OfstedRatingScore?)mfe.PersonalDevelopment ?? OfstedRatingScore.NotInspected,
                                (OfstedRatingScore?)mfe.EffectivenessOfLeadershipAndManagement ??
                                OfstedRatingScore.NotInspected,
                                OfstedRatingScore.NotInspected,
                                OfstedRatingScore.NotInspected,
                                CategoriesOfConcern.DoesNotApply,
                                OfstedRating.ConvertStringToSafeguardingScore(mfe.IsSafeguardingEffective),
                                mfe.LastDayOfInspection.ParseAsNullableDate()),
                            new OfstedRating(
                                (OfstedRatingScore?)mfe.PreviousOverallEffectiveness ?? OfstedRatingScore.NotInspected,
                                (OfstedRatingScore?)mfe.PreviousQualityOfEducation ?? OfstedRatingScore.NotInspected,
                                (OfstedRatingScore?)mfe.PreviousBehaviourAndAttitudes ?? OfstedRatingScore.NotInspected,
                                (OfstedRatingScore?)mfe.PreviousPersonalDevelopment ?? OfstedRatingScore.NotInspected,
                                (OfstedRatingScore?)mfe.PreviousEffectivenessOfLeadershipAndManagement ??
                                OfstedRatingScore.NotInspected,
                                OfstedRatingScore.NotInspected,
                                OfstedRatingScore.NotInspected,
                                CategoriesOfConcern.DoesNotApply,
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
            ofstedRatings.Add(new AcademyOfstedRatings(urn, OfstedRating.Unknown, OfstedRating.Unknown));
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
        if (string.IsNullOrWhiteSpace(rating))
            return OfstedRatingScore.NotInspected;

        // Check if it is 'Not judged' all other gradings are int based
        if (rating.ToLower().Equals("not judged"))
        {
            return OfstedRatingScore.NoJudgement;
        }

        // Attempt to parse the string as an integer
        if (int.TryParse(rating, out var intRating) && Enum.IsDefined(typeof(OfstedRatingScore), intRating))
        {
            return (OfstedRatingScore)intRating;
        }

        // Default case if parsing fails
        return OfstedRatingScore.NotInspected;
    }

    private CategoriesOfConcern ConvertStringToCategoriesOfConcern(string? input)
    {
        switch (input)
        {
            case null:
                return CategoriesOfConcern.NotInspected;
            case "":
                return CategoriesOfConcern.NoConcerns;
            case "SM":
                return CategoriesOfConcern.SpecialMeasures;
            case "SWK":
                return CategoriesOfConcern.SeriousWeakness;
            case "NTI":
                return CategoriesOfConcern.NoticeToImprove;
            default:
                logger.LogError(
                    "Category of concern {input} was not recognised. This could be a data integrity issue with the Ofsted data in Academies Db.",
                    input);
                return CategoriesOfConcern.NotInspected;
        }
    }

    private sealed record AcademyOfstedRatings(int Urn, OfstedRating Current, OfstedRating Previous);
}
