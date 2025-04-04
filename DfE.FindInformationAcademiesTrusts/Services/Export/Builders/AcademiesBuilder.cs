using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.Academy;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.Trust;
using DfE.FindInformationAcademiesTrusts.Extensions;

namespace DfE.FindInformationAcademiesTrusts.Services.Export.Builders
{
    public class AcademiesBuilder(string sheetName = "Academies") : ExportBuilder(sheetName)
    {
        public AcademiesBuilder WriteTrustInformation(TrustSummary? trustSummary)
        {
            WriteTrustInformation<AcademiesBuilder>(trustSummary);
            return this;
        }

        public AcademiesBuilder WriteHeaders()
        {
            var headers = new List<string>
            {
                ExportHelpers.SchoolName,
                ExportHelpers.Urn,
                ExportHelpers.LocalAuthority,
                "Type",
                "Rural or Urban",
                ExportHelpers.DateJoined,
                "Previous Ofsted Rating",
                ExportHelpers.BeforeOrAfterJoiningHeader,
                "Date of Previous Ofsted",
                "Current Ofsted Rating",
                ExportHelpers.BeforeOrAfterJoiningHeader,
                "Date of Current Ofsted",
                "Phase of Education",
                ExportHelpers.AgeRange,
                "Pupil Numbers",
                "Capacity",
                "% Full",
                "Pupils eligible for Free School Meals"
            };

            WriteHeaders(headers);

            return this;
        }

        public AcademiesBuilder WriteRows(AcademyDetails[] academies, AcademyOfsted[] academiesOfstedRatings,
            AcademyPupilNumbers[] academiesPupilNumbers,
            AcademyFreeSchoolMeals[] academiesFreeSchoolMeals)
        {
            foreach (var details in academies)
            {
                var ofstedData = academiesOfstedRatings.SingleOrDefault(x => x.Urn == details.Urn);
                var pupilData = academiesPupilNumbers.SingleOrDefault(x => x.Urn == details.Urn);
                var freeMealsData = academiesFreeSchoolMeals.SingleOrDefault(x => x.Urn == details.Urn);

                GenerateAcademyRow(details, ofstedData, pupilData, freeMealsData);
                AddRow();
            }

            return this;
        }
        
        private void GenerateAcademyRow(
            AcademyDetails academy,
            AcademyOfsted? ofstedData,
            AcademyPupilNumbers? pupilNumbersData,
            AcademyFreeSchoolMeals? freeSchoolMealsData)
        {
            var previousRating = ofstedData?.PreviousOfstedRating ?? OfstedRating.NotInspected;
            var currentRating = ofstedData?.CurrentOfstedRating ?? OfstedRating.NotInspected;
            var percentageFull = ExportHelpers.CalculatePercentageFull(pupilNumbersData?.NumberOfPupils, pupilNumbersData?.SchoolCapacity);

            SetTextCell(CurrentRow, 1, academy.EstablishmentName ?? string.Empty);
            SetTextCell(CurrentRow, 2, academy.Urn);
            SetTextCell(CurrentRow, 3, academy.LocalAuthority ?? string.Empty);
            SetTextCell(CurrentRow, 4, academy.TypeOfEstablishment ?? string.Empty);
            SetTextCell(CurrentRow, 5, academy.UrbanRural ?? string.Empty);

            SetDateCell(CurrentRow, 6, ofstedData?.DateAcademyJoinedTrust);

            SetTextCell(CurrentRow, 7, previousRating.OverallEffectiveness.ToDisplayString(false));
            SetTextCell(CurrentRow, 8,
                ExportHelpers.IsOfstedRatingBeforeOrAfterJoining(
                    previousRating.OverallEffectiveness,
                    ofstedData?.DateAcademyJoinedTrust ?? DateTime.MinValue,
                    previousRating.InspectionDate
                )
            );

            SetDateCell(CurrentRow, 9, previousRating.InspectionDate);

            SetTextCell(CurrentRow, 10, currentRating.OverallEffectiveness.ToDisplayString(true));
            SetTextCell(CurrentRow, 11,
                ExportHelpers.IsOfstedRatingBeforeOrAfterJoining(
                    currentRating.OverallEffectiveness,
                    ofstedData?.DateAcademyJoinedTrust ?? DateTime.MinValue,
                    currentRating.InspectionDate
                )
            );

            SetDateCell(CurrentRow, 12, currentRating.InspectionDate);

            SetTextCell(CurrentRow, 13, pupilNumbersData?.PhaseOfEducation ?? string.Empty);

            SetTextCell(CurrentRow, 14,
                pupilNumbersData != null
                    ? $"{pupilNumbersData.AgeRange.Minimum} - {pupilNumbersData.AgeRange.Maximum}"
                    : string.Empty
            );

            SetTextCell(CurrentRow, 15, pupilNumbersData?.NumberOfPupils?.ToString() ?? string.Empty);
            SetTextCell(CurrentRow, 16, pupilNumbersData?.SchoolCapacity?.ToString() ?? string.Empty);
            SetTextCell(CurrentRow, 17, percentageFull > 0 ? $"{percentageFull}%" : string.Empty);
            SetTextCell(
                CurrentRow,
                18,
                freeSchoolMealsData?.PercentageFreeSchoolMeals.HasValue == true
                    ? $"{freeSchoolMealsData.PercentageFreeSchoolMeals}%"
                    : string.Empty
            );
        }
    }
}