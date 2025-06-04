using DfE.FindInformationAcademiesTrusts.Pages.Shared;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.School;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.Pages.Schools.Overview;

public class FederationModel(ISchoolService schoolService, 
    ITrustService trustService,
    IDataSourceService dataSourceService) : OverviewAreaModel(schoolService, trustService, dataSourceService)
{
    public override PageMetadata PageMetadata => base.PageMetadata with
    {
        SubPageName = SubPageName
    };

    public const string SubPageName = "Federation details";
    
    public string? Name { get; set; }
    public string? FederationUid { get; set; }
    public DateTime? OpenedOnDate { get; set; }
    public Dictionary<string, string> Schools { get; set; } = new();

    public override async Task<IActionResult> OnGetAsync()
    {
        var pageResult = await base.OnGetAsync();
        if (pageResult is NotFoundResult) return pageResult;

        Name = "My school name";
        FederationUid = "1234/5678";
        OpenedOnDate = DateTime.UtcNow;
        Schools = new Dictionary<string, string>
        {
            { "123", "This is a school" },
            { "456", "This is another school" },
            { "789", "This is also a school" },
        };
        
        return pageResult;
    }
}
