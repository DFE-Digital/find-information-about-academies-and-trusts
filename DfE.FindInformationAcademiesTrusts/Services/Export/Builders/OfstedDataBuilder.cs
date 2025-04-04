using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.Academy;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.Trust;
using DfE.FindInformationAcademiesTrusts.Extensions;

namespace DfE.FindInformationAcademiesTrusts.Services.Export.Builders
{
    public class OfstedDataBuilder(string sheetName = "Ofsted") : ExportBuilder(sheetName)
    {
        public OfstedDataBuilder WriteTrustInformation(TrustSummary? trustSummary)
        {
            WriteTrustInformation<OfstedDataBuilder>(trustSummary);
            return this;
        }

        public OfstedDataBuilder WriteHeaders()
        {
            var headers = new List<string>
            {
                ExportHelpers.SchoolName,
                ExportHelpers.DateJoined,
                "Current single headline grade",
                ExportHelpers.BeforeOrAfterJoiningHeader,
                "Date of Current Inspection",
                "Previous single headline grade",
                ExportHelpers.BeforeOrAfterJoiningHeader,
                "Date of previous inspection",
                "Quality of Education",
                "Behaviour and Attitudes",
                "Personal Development",
                "Leadership and Management",
                "Early Years Provision",
                "Sixth Form Provision",
                "Previous Quality of Education",
                "Previous Behaviour and Attitudes",
                "Previous Personal Development",
                "Previous Leadership and Management",
                "Previous Early Years Provision",
                "Previous Sixth Form Provision",
                "Effective Safeguarding",
                "Category of Concern"
            };

            WriteHeaders(headers);

            return this;
        }

        public OfstedDataBuilder WriteRows(AcademyDetails[] academies, AcademyOfsted[] academiesOfstedRatings)
        {
            foreach (var details in academies)
            {
                var ofstedData = academiesOfstedRatings.SingleOrDefault(x => x.Urn == details.Urn);

                GenerateOfstedRow(details, ofstedData);
                AddRow();
            }

            return this;
        }

        private void GenerateOfstedRow(AcademyDetails academy, AcademyOfsted? ofstedData)
        {
            var previousRating = ofstedData?.PreviousOfstedRating ?? OfstedRating.NotInspected;
            var currentRating = ofstedData?.CurrentOfstedRating ?? OfstedRating.NotInspected;

            // School Name
            SetTextCell(CurrentRow, 1, academy.EstablishmentName ?? string.Empty);

            // Date Joined Trust
            SetDateCell(CurrentRow, 2, ofstedData?.DateAcademyJoinedTrust);

            // Current Single Headline Grade
            SetTextCell(CurrentRow, 3, currentRating.OverallEffectiveness.ToDisplayString(true));

            // Before/After Joining (Current)
            SetTextCell(CurrentRow, 4,
                ExportHelpers.IsOfstedRatingBeforeOrAfterJoining(
                    currentRating.OverallEffectiveness,
                    ofstedData?.DateAcademyJoinedTrust,
                    currentRating.InspectionDate
                )
            );

            // Current Inspection Date
            SetDateCell(CurrentRow, 5, currentRating.InspectionDate);

            // Previous Single Headline Grade
            SetTextCell(CurrentRow, 6, previousRating.OverallEffectiveness.ToDisplayString(false));

            // Before/After Joining (Previous)
            SetTextCell(CurrentRow, 7,
                ExportHelpers.IsOfstedRatingBeforeOrAfterJoining(
                    previousRating.OverallEffectiveness,
                    ofstedData?.DateAcademyJoinedTrust,
                    previousRating.InspectionDate
                )
            );

            // Previous Inspection Date
            SetDateCell(CurrentRow, 8, previousRating.InspectionDate);

            // Current Ratings
            SetTextCell(CurrentRow, 9, currentRating.QualityOfEducation.ToDisplayString(true));
            SetTextCell(CurrentRow, 10, currentRating.BehaviourAndAttitudes.ToDisplayString(true));
            SetTextCell(CurrentRow, 11, currentRating.PersonalDevelopment.ToDisplayString(true));
            SetTextCell(CurrentRow, 12,
                currentRating.EffectivenessOfLeadershipAndManagement.ToDisplayString(true));
            SetTextCell(CurrentRow, 13, currentRating.EarlyYearsProvision.ToDisplayString(true));
            SetTextCell(CurrentRow, 14, currentRating.SixthFormProvision.ToDisplayString(true));

            // Previous Ratings
            SetTextCell(CurrentRow, 15, previousRating.QualityOfEducation.ToDisplayString(false));
            SetTextCell(CurrentRow, 16, previousRating.BehaviourAndAttitudes.ToDisplayString(false));
            SetTextCell(CurrentRow, 17, previousRating.PersonalDevelopment.ToDisplayString(false));
            SetTextCell(CurrentRow, 18,
                previousRating.EffectivenessOfLeadershipAndManagement.ToDisplayString(false));
            SetTextCell(CurrentRow, 19, previousRating.EarlyYearsProvision.ToDisplayString(false));
            SetTextCell(CurrentRow, 20, previousRating.SixthFormProvision.ToDisplayString(false));

            // Safeguarding Effective
            SetTextCell(CurrentRow, 21, currentRating.SafeguardingIsEffective.ToDisplayString());

            // Category of Concern
            SetTextCell(CurrentRow, 22, currentRating.CategoryOfConcern.ToDisplayString());
        }

    }
}