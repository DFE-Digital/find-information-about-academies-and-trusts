using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Export;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.Academies
{
    public abstract class ExportableAcademiesPageModel : TrustsAreaModel
    {
        protected IExportService ExportService { get; }

        protected ExportableAcademiesPageModel(ITrustProvider trustProvider, IDataSourceService dataSourceService,
            ITrustService trustService, IExportService exportService, ILogger<ExportableAcademiesPageModel> logger)
            : base(trustProvider, dataSourceService, trustService, logger, "Academies in this trust")
        {
            ExportService = exportService;
        }

        public virtual async Task<IActionResult> OnPostExportAsync(string uid)
        {
            TrustSummaryServiceModel? trustSummary = await TrustService.GetTrustSummaryAsync(uid);
            Trust? allAcademiesDetails = await TrustProvider.GetTrustByUidAsync(uid);
            var fileContents = ExportService.ExportAcademiesToSpreadsheetUsingProvider(allAcademiesDetails, trustSummary);
            string fileName = $"{allAcademiesDetails?.Name}-{DateTime.Now.Day}-{DateTime.Now.Month}-{DateTime.Now.Year}.xlsx";
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            return File(fileContents, contentType, fileName);
        }
    }
}
