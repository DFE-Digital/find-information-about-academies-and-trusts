using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Services;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.Academies
{
    public abstract class AcademiesPageModel(
        ITrustProvider trustProvider,
        IDataSourceService dataSourceService,
        ITrustService trustService,
        IExportService exportService,
        ILogger<AcademiesPageModel> logger
    ) : TrustsAreaModel(trustProvider, dataSourceService, trustService, logger, "Academies in this trust")
    {
        protected IExportService ExportService { get; } = exportService;

        public string? TabName { get; init; }
        public virtual async Task<IActionResult> OnGetExportAsync(string uid)
        {
            TrustSummaryServiceModel? trustSummary = await TrustService.GetTrustSummaryAsync(uid);
            Trust? allAcademiesDetails = await TrustProvider.GetTrustByUidAsync(uid);

            if (trustSummary == null || allAcademiesDetails == null)
            {
                return new NotFoundResult();
            }

            var fileContents = ExportService.ExportAcademiesToSpreadsheetUsingProvider(allAcademiesDetails, trustSummary);
            string fileName = $"{allAcademiesDetails?.Name}-{DateTime.Now.Day}-{DateTime.Now.Month}-{DateTime.Now.Year}.xlsx";
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            return File(fileContents, contentType, fileName);
        }

    }
}
