using DfE.FindInformationAcademiesTrusts.Data;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts;

public class OverviewModel : TrustsAreaModel
{
    public IEnumerable<(string? Authority, int Total)> AcademiesInEachLocalAuthority =>
        Trust.Academies
            .GroupBy(x => x.LocalAuthority)
            .Select(c => (Authority: c.Key, Total: c.Count()))
            .OrderByDescending(t => t.Total)
            .ThenBy(t => t.Authority);

    public IEnumerable<(OfstedRatingScore Rating, int Total)> OfstedRatings
        => Trust.Academies
            .GroupBy(x => x.CurrentOfstedRating.OfstedRatingScore)
            .Select(c => (Rating: c.Key, Total: c.Count()));

    public int NumberOfAcademiesInTrust => Trust.Academies.Length;
    public int TotalPupilNumbersInTrust => Trust.Academies.Select(x => x.NumberOfPupils ?? 0).Sum();
    public int TotalPupilCapacityInTrust => Trust.Academies.Select(x => x.SchoolCapacity ?? 0).Sum();

    public int? TotalPercentageCapacityInTrust =>
        TotalPupilCapacityInTrust > 0
            ? (int)Math.Round(TotalPupilNumbersInTrust / (double)TotalPupilCapacityInTrust * 100)
            : null;

    public OverviewModel(ITrustProvider trustProvider) : base(trustProvider, "Overview")
    {
    }

    public int GetNumberOfAcademiesWithOfstedRating(OfstedRatingScore score)
    {
        return OfstedRatings.Any(x => x.Rating == score)
            ? OfstedRatings.Single(x => x.Rating == score).Total
            : 0;
    }
}
