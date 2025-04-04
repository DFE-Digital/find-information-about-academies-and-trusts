using DfE.FindInformationAcademiesTrusts.Data;

namespace DfE.FindInformationAcademiesTrusts.Services.Export.Builders
{
    public static class ExportHelpers
    {
        internal const string AgeRange = "Age range";
        internal const string BeforeOrAfterJoiningHeader = "Before/After Joining";
        internal const string DateJoined = "Date joined";
        internal const string LocalAuthority = "Local authority";
        internal const string SchoolName = "School Name";
        internal const string Urn = "URN";

        public static string IsOfstedRatingBeforeOrAfterJoining(OfstedRatingScore ofstedRatingScore,
            DateTime? dateAcademyJoinedTrust, DateTime? inspectionEndDate)
        {
            if (ofstedRatingScore == OfstedRatingScore.NotInspected || !inspectionEndDate.HasValue)
            {
                return string.Empty;
            }

            return inspectionEndDate < dateAcademyJoinedTrust ? "Before Joining" : "After Joining";
        }

        public static float CalculatePercentageFull(int? numberOfPupils, int? schoolCapacity)
        {
            if (numberOfPupils.HasValue && schoolCapacity.HasValue && schoolCapacity.Value != 0)
            {
                return (float)Math.Round((double)numberOfPupils.Value / schoolCapacity.Value * 100);
            }

            return 0;
        }
    }
}
