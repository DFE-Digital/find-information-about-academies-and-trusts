using DfE.FindInformationAcademiesTrusts.Data;

namespace DfE.FindInformationAcademiesTrusts.Services.Trust
{
    public class TrustOverviewServiceModel(
        string uid,
        int totalAcademies,
        IReadOnlyDictionary<string, int> academiesByLocalAuthority,
        int totalPupilNumbers,
        int totalCapacity,
        IReadOnlyDictionary<OfstedRatingScore, int> ofstedRatings)
    {
        public string Uid { get; } = uid;
        public int TotalAcademies { get; } = totalAcademies;
        public IReadOnlyDictionary<string, int> AcademiesByLocalAuthority { get; } = academiesByLocalAuthority;
        public int TotalPupilNumbers { get; } = totalPupilNumbers;
        public int TotalCapacity { get; } = totalCapacity;
        public double PercentageFull => TotalCapacity > 0 ? (double)TotalPupilNumbers / TotalCapacity * 100 : 0;
        public IReadOnlyDictionary<OfstedRatingScore, int> OfstedRatings { get; } = ofstedRatings;
    }
}
