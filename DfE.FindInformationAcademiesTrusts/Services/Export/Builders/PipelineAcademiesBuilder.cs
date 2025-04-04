using DfE.FindInformationAcademiesTrusts.Data.Repositories.Trust;
using DfE.FindInformationAcademiesTrusts.Services.Academy;

namespace DfE.FindInformationAcademiesTrusts.Services.Export.Builders
{
    public class PipelineAcademiesBuilder(string sheetName = "Pipeline Academies") : ExportBuilder(sheetName)
    {
        public PipelineAcademiesBuilder WriteTrustInformation(TrustSummary? trustSummary)
        {
            WriteTrustInformation<PipelineAcademiesBuilder>(trustSummary);
            return this;
        }

        public PipelineAcademiesBuilder WriteHeadersForPreAdvisory()
        {
            AddRow();
            WriteHeaders(["Pre-advisory academies"]);
            WriteHeaders([ExportHelpers.SchoolName, ExportHelpers.Urn, ExportHelpers.AgeRange, ExportHelpers.LocalAuthority, "Project type", "Proposed conversion or transfer date"]);

            return this;
        }

        public PipelineAcademiesBuilder WriteHeadersForPostAdvisory()
        {
            AddRow();
            WriteHeaders(["Post-advisory academies"]);
            WriteHeaders([ExportHelpers.SchoolName, ExportHelpers.Urn, ExportHelpers.AgeRange, ExportHelpers.LocalAuthority, "Project type", "Proposed conversion or transfer date"]);
          
            return this;
        }

        public PipelineAcademiesBuilder WriteHeadersForFreeSchools()
        {
            AddRow();
            WriteHeaders(["Free schools"]);
            WriteHeaders([ExportHelpers.SchoolName, ExportHelpers.Urn, ExportHelpers.AgeRange, ExportHelpers.LocalAuthority, "Project type", "Provisional opening date"]);
            
            return this;
        }

        public PipelineAcademiesBuilder WriteRows(AcademyPipelineServiceModel[] pipelineAcademies)
        {
            var orderedPipelineAcademies = pipelineAcademies.OrderBy(a => a.EstablishmentName).ToArray();

            foreach (var details in orderedPipelineAcademies)
            {
                GeneratePipelineAcademyRow(details);
                AddRow();
            }

            return this;
        }

        private void GeneratePipelineAcademyRow(AcademyPipelineServiceModel pipelineAcademy)
        {
            SetTextCell(CurrentRow, 1, pipelineAcademy.EstablishmentName ?? string.Empty);
            SetTextCell(CurrentRow, 2, pipelineAcademy.Urn ?? string.Empty);
            SetTextCell(CurrentRow, 3, pipelineAcademy.AgeRange != null
                ? $"{pipelineAcademy.AgeRange.Minimum} - {pipelineAcademy.AgeRange.Maximum}"
                : "Unconfirmed"
            );
            SetTextCell(CurrentRow, 4, pipelineAcademy.LocalAuthority ?? string.Empty);
            SetTextCell(CurrentRow, 5, pipelineAcademy.ProjectType ?? string.Empty);

            if (pipelineAcademy.ChangeDate != null)
            {
                SetDateCell(CurrentRow, 6, pipelineAcademy.ChangeDate);
            }
            else
            {
                SetTextCell(CurrentRow, 6, "Unconfirmed");
            }
        }
    }
}