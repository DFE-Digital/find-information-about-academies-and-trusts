using System.Globalization;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Contexts;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Repositories;

public class OfstedRepository(AcademiesDbContext academiesDbContext, ILogger<AcademyRepository> logger)
{
    public async Task<Dictionary<string, SafeguardingAndConcernRatings>> GetSafeguardingAndConcerns(string uid)
    {
        var giasGroupLinks = await GetGiasGroupLinks(uid);
        var urns = giasGroupLinks.Select(gl => int.Parse(gl.Urn)).ToArray();

        // Ofsted data is held in MisEstablishments for most academies
        var misRatings =
            await academiesDbContext.MisEstablishments
                .Where(me => urns.Contains(me.Urn!.Value))
                .Select(me => new SafeguardingAndConcernRatings(me.Urn!.Value,
                    new SafeguardingAndConcernsRating(
                        OfstedRating.ConvertStringToCategoriesOfConcern(me.CategoryOfConcern),
                        OfstedRating.ConvertStringToSafeguardingScore(me.SafeguardingIsEffective),
                        me.InspectionStartDate.ParseAsNullableDate()),
                    new SafeguardingAndConcernsRating(
                        OfstedRating.ConvertStringToCategoriesOfConcern(me.PreviousCategoryOfConcern),
                        OfstedRating.ConvertStringToSafeguardingScore(me.PreviousSafeguardingIsEffective),
                        me.PreviousInspectionStartDate.ParseAsNullableDate())))
                .ToListAsync();
        var urnsNotInMisEstablishments = urns.Except(misRatings.Select(a => a.Urn)).ToArray();
        if (urnsNotInMisEstablishments.Length > 0)
        {
            misRatings.AddRange(await academiesDbContext.MisFurtherEducationEstablishments
                .Where(mfe => urnsNotInMisEstablishments.Contains(mfe.ProviderUrn))
                .Select(mfe => new SafeguardingAndConcernRatings(mfe.ProviderUrn,
                    new SafeguardingAndConcernsRating(
                        CategoriesOfConcern.None,
                        OfstedRating.ConvertStringToSafeguardingScore(mfe.IsSafeguardingEffective),
                        mfe.LastDayOfInspection.ParseAsNullableDate()),
                    new SafeguardingAndConcernsRating(
                        CategoriesOfConcern.None,
                        OfstedRating.ConvertStringToSafeguardingScore(mfe.PreviousSafeguarding),
                        mfe.PreviousLastDayOfInspection.ParseAsNullableDate())))
                .ToArrayAsync());
        }

        foreach (var urn in urns.Except(misRatings.Select(a => a.Urn)))
        {
            logger.LogError(
                "URN {Urn} was not found in Mis.Establishments or Mis.FurtherEducationEstablishments. This indicates a data integrity issue with the Ofsted data in Academies Db.",
                urn);
            misRatings.Add(new SafeguardingAndConcernRatings(urn, null, null));
        }

        return misRatings.ToDictionary(o => o.Urn.ToString(), o => o);
    }

    public async Task<Dictionary<string, ImportantDates>> GetImportantDates(string uid)
    {
        var giasGroupLinks = await GetGiasGroupLinks(uid);
        var urns = giasGroupLinks.Select(gl => int.Parse(gl.Urn)).ToArray();

        // Ofsted data is held in MisEstablishments for most academies
        var misRatings =
            await academiesDbContext.MisEstablishments
                .Where(me => urns.Contains(me.Urn!.Value))
                .Select(me => new ImportantDates(me.Urn!.Value,
                    DateTime.ParseExact(
                        giasGroupLinks.FirstOrDefault(item => item.Urn.ParseAsNullableInt() == me.Urn!.Value)!
                            .JoinedDate!, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    me.InspectionStartDate.ParseAsNullableDate(),
                    me.PreviousInspectionStartDate.ParseAsNullableDate()))
                .ToListAsync();
        var urnsNotInMisEstablishments = urns.Except(misRatings.Select(a => a.Urn)).ToArray();
        if (urnsNotInMisEstablishments.Length > 0)
        {
            misRatings.AddRange(await academiesDbContext.MisFurtherEducationEstablishments
                .Where(mfe => urnsNotInMisEstablishments.Contains(mfe.ProviderUrn))
                .Select(mfe =>
                    new ImportantDates(mfe.ProviderUrn,
                        DateTime.ParseExact(
                            giasGroupLinks.FirstOrDefault(item => item.Urn.ParseAsNullableInt() == mfe.ProviderUrn)!
                                .JoinedDate!, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        mfe.LastDayOfInspection.ParseAsNullableDate(),
                        mfe.PreviousLastDayOfInspection.ParseAsNullableDate()))
                .ToArrayAsync());
        }

        foreach (var urn in urns.Except(misRatings.Select(a => a.Urn)))
        {
            logger.LogError(
                "URN {Urn} was not found in Mis.Establishments or Mis.FurtherEducationEstablishments. This indicates a data integrity issue with the Ofsted data in Academies Db.",
                urn);
            misRatings.Add(new ImportantDates(urn,
                DateTime.ParseExact(
                    giasGroupLinks.FirstOrDefault(item => item.Urn.ParseAsNullableInt() == urn)!.JoinedDate!,
                    "dd/MM/yyyy", CultureInfo.InvariantCulture), null, null));
        }

        return misRatings.ToDictionary(o => o.Urn.ToString(), o => o);
    }

    public async Task<Dictionary<string, Rating>> GetCurrentRatings(string uid)
    {
        var giasGroupLinks = await GetGiasGroupLinks(uid);
        var urns = giasGroupLinks.Select(gl => int.Parse(gl.Urn)).ToArray();

        var ofstedRatings =
            await academiesDbContext.MisEstablishments
                .Where(me => urns.Contains(me.Urn!.Value))
                .Select(me => new Rating(me.Urn!.Value,
                    new OfstedRating(
                        (OfstedRatingScore?)me.OverallEffectiveness ?? OfstedRatingScore.None,
                        (OfstedRatingScore?)me.QualityOfEducation ?? OfstedRatingScore.None,
                        (OfstedRatingScore?)me.BehaviourAndAttitudes ?? OfstedRatingScore.None,
                        (OfstedRatingScore?)me.PersonalDevelopment ?? OfstedRatingScore.None,
                        (OfstedRatingScore?)me.EffectivenessOfLeadershipAndManagement ?? OfstedRatingScore.None,
                        (OfstedRatingScore?)me.EarlyYearsProvisionWhereApplicable ?? OfstedRatingScore.None,
                        (OfstedRatingScore?)me.SixthFormProvisionWhereApplicable ?? OfstedRatingScore.None,
                        OfstedRating.ConvertStringToCategoriesOfConcern(me.CategoryOfConcern),
                        OfstedRating.ConvertStringToSafeguardingScore(me.SafeguardingIsEffective),
                        me.InspectionStartDate.ParseAsNullableDate())))
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
                        new Rating(mfe.ProviderUrn, new OfstedRating(
                            (OfstedRatingScore?)mfe.OverallEffectiveness ?? OfstedRatingScore.None,
                            (OfstedRatingScore?)mfe.QualityOfEducation ?? OfstedRatingScore.None,
                            (OfstedRatingScore?)mfe.BehaviourAndAttitudes ?? OfstedRatingScore.None,
                            (OfstedRatingScore?)mfe.PersonalDevelopment ?? OfstedRatingScore.None,
                            (OfstedRatingScore?)mfe.EffectivenessOfLeadershipAndManagement ??
                            OfstedRatingScore.None,
                            OfstedRatingScore.None, OfstedRatingScore.None,
                            CategoriesOfConcern.None,
                            OfstedRating.ConvertStringToSafeguardingScore(mfe.IsSafeguardingEffective),
                            mfe.LastDayOfInspection.ParseAsNullableDate())
                        ))
                    .ToArrayAsync()
            );
        }

        foreach (var urn in urns.Except(ofstedRatings.Select(a => a.Urn)))
        {
            logger.LogError(
                "URN {Urn} was not found in Mis.Establishments or Mis.FurtherEducationEstablishments. This indicates a data integrity issue with the Ofsted data in Academies Db.",
                urn);
            ofstedRatings.Add(new Rating(urn,
                null));
        }

        return ofstedRatings.ToDictionary(o => o.Urn.ToString(), o => o);
    }

    public async Task<Dictionary<string, Rating>> GetPreviousRatings(string uid)
    {
        var giasGroupLinks = await GetGiasGroupLinks(uid);
        var urns = giasGroupLinks.Select(gl => int.Parse(gl.Urn)).ToArray();

        var ofstedRatings =
            await academiesDbContext.MisEstablishments
                .Where(me => urns.Contains(me.Urn!.Value))
                .Select(me => new Rating(me.Urn!.Value,
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
                        new Rating(mfe.ProviderUrn, new OfstedRating(
                            (OfstedRatingScore?)mfe.PreviousOverallEffectiveness ?? OfstedRatingScore.None,
                            (OfstedRatingScore?)mfe.PreviousQualityOfEducation ?? OfstedRatingScore.None,
                            (OfstedRatingScore?)mfe.PreviousBehaviourAndAttitudes ?? OfstedRatingScore.None,
                            (OfstedRatingScore?)mfe.PreviousPersonalDevelopment ?? OfstedRatingScore.None,
                            (OfstedRatingScore?)mfe.PreviousEffectivenessOfLeadershipAndManagement ??
                            OfstedRatingScore.None,
                            OfstedRatingScore.None, OfstedRatingScore.None,
                            CategoriesOfConcern.None,
                            OfstedRating.ConvertStringToSafeguardingScore(mfe.PreviousSafeguarding),
                            mfe.PreviousLastDayOfInspection.ParseAsNullableDate())
                        ))
                    .ToArrayAsync()
            );
        }

        foreach (var urn in urns.Except(ofstedRatings.Select(a => a.Urn)))
        {
            logger.LogError(
                "URN {Urn} was not found in Mis.Establishments or Mis.FurtherEducationEstablishments. This indicates a data integrity issue with the Ofsted data in Academies Db.",
                urn);
            ofstedRatings.Add(new Rating(urn,
                null));
        }

        return ofstedRatings.ToDictionary(o => o.Urn.ToString(), o => o);
    }

    private async Task<List<GiasGroupLinkData>> GetGiasGroupLinks(string uid)
    {
        return await academiesDbContext.GiasGroupLinks
            .Where(gl => gl.GroupUid == uid)
            .Select(gl => new GiasGroupLinkData(gl.Urn!,
                gl.EstablishmentName,
                gl.JoinedDate))
            .ToListAsync();
    }
}

public record Rating(int Urn, OfstedRating? TheRating);

public record ImportantDates(int Urn, DateTime DateJoined, DateTime? CurrentInspection, DateTime? PreviousInspection);

public record SafeguardingAndConcernRatings(
    int Urn,
    SafeguardingAndConcernsRating? CurrentRating,
    SafeguardingAndConcernsRating? PreviousRating);

public record SafeguardingAndConcernsRating(
    CategoriesOfConcern ConvertStringToCategoriesOfConcern,
    SafeguardingScore ConvertStringToSafeguardingScore,
    DateTime? InspectionDate);

internal record GiasGroupLinkData(string Urn, string? EstablishmentName, string? JoinedDate);
