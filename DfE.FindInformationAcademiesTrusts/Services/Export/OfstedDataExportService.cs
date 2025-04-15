using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Exceptions;
using DfE.FindInformationAcademiesTrusts.Extensions;
using DfE.FindInformationAcademiesTrusts.Services.Academy;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using static DfE.FindInformationAcademiesTrusts.Services.Export.ExportColumns;

namespace DfE.FindInformationAcademiesTrusts.Services.Export
{
    public interface IOfstedDataExportService
    {
        Task<byte[]> BuildAsync(string uid);
    }

    public class OfstedDataExportService(IAcademyService academyService, ITrustService trustService) : ExportBuilder("Ofsted"), IOfstedDataExportService
    {
        private readonly List<string> headers =
        [
            CommonColumnNames.SchoolName,
            CommonColumnNames.DateJoined,
            "Current single headline grade",
            CommonColumnNames.BeforeOrAfterJoiningHeader,
            "Date of Current Inspection",
            "Previous single headline grade",
            CommonColumnNames.BeforeOrAfterJoiningHeader,
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
        ];
        
        public async Task<byte[]> BuildAsync(string uid)
        {
            var trustSummary = await trustService.GetTrustSummaryAsync(uid);
 
            if (trustSummary is null)
            {
                throw new DataIntegrityException($"Trust summary not found for UID {uid}");
            }

            var academiesDetails = await academyService.GetAcademiesInTrustDetailsAsync(uid);
            var academiesOfstedRatings = await academyService.GetAcademiesInTrustOfstedAsync(uid);

            return WriteTrustInformation(trustSummary)
                .WriteHeaders(headers)
                .WriteRows(() =>  WriteRows(academiesDetails, academiesOfstedRatings))
                .Build();
        }

        private void WriteRows(AcademyDetailsServiceModel[] academies, AcademyOfstedServiceModel[] academiesOfstedRatings)
        {
            foreach (var details in academies)
            {
                var ofstedData = academiesOfstedRatings.SingleOrDefault(x => x.Urn == details.Urn);

                GenerateOfstedRow(details, ofstedData);
            }
        }

        private void GenerateOfstedRow(AcademyDetailsServiceModel academy, AcademyOfstedServiceModel? ofstedData)
        {
            var previousRating = ofstedData?.PreviousOfstedRating ?? OfstedRating.NotInspected;
            var currentRating = ofstedData?.CurrentOfstedRating ?? OfstedRating.NotInspected;

            SetTextCell(OfstedColumns.SchoolName, academy.EstablishmentName ?? string.Empty);
            SetDateCell(OfstedColumns.DateJoined, ofstedData?.DateAcademyJoinedTrust);
            SetTextCell(OfstedColumns.CurrentSingleHeadlineGrade, currentRating.OverallEffectiveness.ToDisplayString(true));
            SetTextCell(OfstedColumns.CurrentBeforeAfterJoining, ofstedData?.WhenDidCurrentInspectionHappen.ToDisplayString() ?? string.Empty);
            SetDateCell(OfstedColumns.DateOfCurrentInspection, currentRating.InspectionDate);
            SetTextCell(OfstedColumns.PreviousSingleHeadlineGrade, previousRating.OverallEffectiveness.ToDisplayString(false));
            SetTextCell(OfstedColumns.PreviousBeforeAfterJoining, ofstedData?.WhenDidPreviousInspectionHappen.ToDisplayString() ?? string.Empty);
            SetDateCell(OfstedColumns.DateOfPreviousInspection, previousRating.InspectionDate);
            SetTextCell(OfstedColumns.CurrentQualityOfEducation, currentRating.QualityOfEducation.ToDisplayString(true));
            SetTextCell(OfstedColumns.CurrentBehaviourAndAttitudes, currentRating.BehaviourAndAttitudes.ToDisplayString(true)); 
            SetTextCell(OfstedColumns.CurrentPersonalDevelopment, currentRating.PersonalDevelopment.ToDisplayString(true)); 
            SetTextCell(OfstedColumns.CurrentLeadershipAndManagement, currentRating.EffectivenessOfLeadershipAndManagement.ToDisplayString(true));
            SetTextCell(OfstedColumns.CurrentEarlyYearsProvision, currentRating.EarlyYearsProvision.ToDisplayString(true)); 
            SetTextCell(OfstedColumns.CurrentSixthFormProvision, currentRating.SixthFormProvision.ToDisplayString(true));

 
            SetTextCell(OfstedColumns.PreviousQualityOfEducation, previousRating.QualityOfEducation.ToDisplayString(false));
            SetTextCell(OfstedColumns.PreviousBehaviourAndAttitudes, previousRating.BehaviourAndAttitudes.ToDisplayString(false));
            SetTextCell(OfstedColumns.PreviousPersonalDevelopment, previousRating.PersonalDevelopment.ToDisplayString(false));
            SetTextCell(OfstedColumns.PreviousLeadershipAndManagement, previousRating.EffectivenessOfLeadershipAndManagement.ToDisplayString(false));
            SetTextCell(OfstedColumns.PreviousEarlyYearsProvision, previousRating.EarlyYearsProvision.ToDisplayString(false));
            SetTextCell(OfstedColumns.PreviousSixthFormProvision, previousRating.SixthFormProvision.ToDisplayString(false));

            SetTextCell(OfstedColumns.EffectiveSafeguarding, currentRating.SafeguardingIsEffective.ToDisplayString());

            SetTextCell(OfstedColumns.CategoryOfConcern, currentRating.CategoryOfConcern.ToDisplayString());

            CurrentRow++;
        }
    }
}
