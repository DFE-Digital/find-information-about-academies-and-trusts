// Index.cshtml.cs
using ClosedXML.Excel;
using DfE.FindInformationAcademiesTrusts.Pages.Shared;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.Pages;

public class IndexModel : BasePageModel, IPageSearchFormModel
{
    private readonly ITrustService _trustService;

    public IndexModel(ITrustService trustService)
    {
        _trustService = trustService;
        ShowHeaderSearch = false;
    }

    public string PageSearchFormInputId => "home-search";

    [BindProperty]
    public string Region { get; set; } = "North West"; // Default value

    public async Task<IActionResult> OnPostExportCsvAsync()
    {
        var trusts = await _trustService.GetTrustsInRegionAsync(Region);

        using (var workbook = new XLWorkbook())
        {
            var worksheet = workbook.Worksheets.Add("Trusts");

            // Add headers
            worksheet.Cell(1, 1).Value = "UID";
            worksheet.Cell(1, 2).Value = "Name";
            worksheet.Cell(1, 3).Value = "Trust Relationship Manager Name";
            worksheet.Cell(1, 4).Value = "Trust Relationship Manager Email";

            int row = 2;

            foreach (var trust in trusts)
            {
                worksheet.Cell(row, 1).Value = trust.Uid;
                worksheet.Cell(row, 2).Value = trust.Name;
                worksheet.Cell(row, 3).Value = trust.TrustRelationshipManager?.FullName ?? string.Empty;
                worksheet.Cell(row, 4).Value = trust.TrustRelationshipManager?.Email ?? string.Empty;
                row++;
            }

            using (var stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                var content = stream.ToArray();

                // Return the Excel file
                return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Trusts.xlsx");
            }
        }
    }
}
