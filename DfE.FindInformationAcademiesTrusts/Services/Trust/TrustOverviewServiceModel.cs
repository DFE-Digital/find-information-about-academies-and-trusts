using DfE.FindInformationAcademiesTrusts.Data;

namespace DfE.FindInformationAcademiesTrusts.Services.Trust;

public record TrustOverviewServiceModel(
    string Uid,
    int TotalAcademies,
    IReadOnlyDictionary<string, int> AcademiesByLocalAuthority,
    int TotalPupilNumbers,
    int TotalCapacity,
    IReadOnlyDictionary<OfstedRatingScore, int> OfstedRatings)
{
    public double PercentageFull => TotalCapacity > 0 ? (double)TotalPupilNumbers / TotalCapacity * 100 : 0;
}
