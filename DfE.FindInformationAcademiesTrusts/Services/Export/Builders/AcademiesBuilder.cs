using DfE.FindInformationAcademiesTrusts.Data.Repositories.Academy;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.Trust;
using DfE.FindInformationAcademiesTrusts.Extensions;
using DfE.FindInformationAcademiesTrusts.Services.Academy;

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
                "Current Ofsted Rating",
                ExportHelpers.BeforeOrAfterJoiningHeader,
                "Date of Current Ofsted",
                "Previous Ofsted Rating",
                ExportHelpers.BeforeOrAfterJoiningHeader,
                "Date of Previous Ofsted",
                "Phase of Education",
                ExportHelpers.AgeRange,
                "Pupil Numbers",
                "Capacity",
                "% Full",
                "Pupils eligible for Free School Meals",
                "LA average pupils eligible for Free School Meals",
                "National average pupils eligible for Free School Meals",
                "Pupils eligible for Free School Meals"
            };

            WriteHeaders(headers);

            return this;
        }

        public AcademiesBuilder WriteRows(AcademyDetails[] academies, AcademyOfsted[] academiesOfstedRatings,
            AcademyPupilNumbers[] academiesPupilNumbers,
            AcademyFreeSchoolMealsServiceModel[] academiesFreeSchoolMeals)
        {
            foreach (var details in academies)
            {
                var ofstedData = academiesOfstedRatings.Single(x => x.Urn == details.Urn);
                var pupilData = academiesPupilNumbers.Single(x => x.Urn == details.Urn);
                var freeMealsData = academiesFreeSchoolMeals.Single(x => x.Urn == details.Urn);

                GenerateAcademyRow(details, ofstedData, pupilData, freeMealsData);
                AddRow();
            }

            return this;
        }

        private void GenerateAcademyRow(
            AcademyDetails academy,
            AcademyOfsted ofstedData,
            AcademyPupilNumbers pupilNumbersData,
            AcademyFreeSchoolMealsServiceModel freeSchoolMealsData)
        {
            var previousRating = ofstedData.PreviousOfstedRating;
            var currentRating = ofstedData.CurrentOfstedRating;
            var percentageFull = ExportHelpers.CalculatePercentageFull(pupilNumbersData.NumberOfPupils, pupilNumbersData.SchoolCapacity);

            SetTextCell(CurrentRow, 1, academy.EstablishmentName ?? string.Empty);
            SetTextCell(CurrentRow, 2, academy.Urn);
            SetTextCell(CurrentRow, 3, academy.LocalAuthority ?? string.Empty);
            SetTextCell(CurrentRow, 4, academy.TypeOfEstablishment ?? string.Empty);
            SetTextCell(CurrentRow, 5, academy.UrbanRural ?? string.Empty);

            SetDateCell(CurrentRow, 6, ofstedData.DateAcademyJoinedTrust);

            SetTextCell(CurrentRow, 7, currentRating.OverallEffectiveness.ToDisplayString(true));
            SetTextCell(CurrentRow, 8,
                ExportHelpers.IsOfstedRatingBeforeOrAfterJoining(
                    currentRating.OverallEffectiveness,
                    ofstedData.DateAcademyJoinedTrust,
                    currentRating.InspectionDate
                )
            );
            SetDateCell(CurrentRow, 9, currentRating.InspectionDate);

            SetTextCell(CurrentRow, 10, previousRating.OverallEffectiveness.ToDisplayString(false));
            SetTextCell(CurrentRow, 11,
                ExportHelpers.IsOfstedRatingBeforeOrAfterJoining(
                    previousRating.OverallEffectiveness,
                    ofstedData.DateAcademyJoinedTrust,
                    previousRating.InspectionDate
                )
            );
            SetDateCell(CurrentRow, 12, previousRating.InspectionDate);

            SetTextCell(CurrentRow, 13, pupilNumbersData.PhaseOfEducation ?? string.Empty);

            SetTextCell(CurrentRow, 14,
                $"{pupilNumbersData.AgeRange.Minimum} - {pupilNumbersData.AgeRange.Maximum}"
            );

            SetTextCell(CurrentRow, 15, pupilNumbersData.NumberOfPupils?.ToString() ?? string.Empty);
            SetTextCell(CurrentRow, 16, pupilNumbersData.SchoolCapacity?.ToString() ?? string.Empty);
            SetTextCell(CurrentRow, 17, percentageFull > 0 ? $"{percentageFull}%" : string.Empty);
            SetTextCell(CurrentRow,
                18,
                freeSchoolMealsData.PercentageFreeSchoolMeals.HasValue
                    ? $"{freeSchoolMealsData.PercentageFreeSchoolMeals}%"
                    : string.Empty
            );

            SetTextCell(CurrentRow, 19,
                freeSchoolMealsData.LaAveragePercentageFreeSchoolMeals > 0
                    ? $"{Math.Round(freeSchoolMealsData.LaAveragePercentageFreeSchoolMeals, 1)}%"
                    : string.Empty
            );
            SetTextCell(CurrentRow, 20,
                freeSchoolMealsData.NationalAveragePercentageFreeSchoolMeals > 0
                    ? $"{Math.Round(freeSchoolMealsData.NationalAveragePercentageFreeSchoolMeals, 1)}%"
                    : string.Empty
            );
        }
    }
}