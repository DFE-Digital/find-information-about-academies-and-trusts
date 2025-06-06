using DfE.FindInformationAcademiesTrusts.Pages.Shared;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.School;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.Pages.Schools.Overview;

public class FederationModel(ISchoolService schoolService, 
    ITrustService trustService,
    ISchoolOverviewFederationService schoolOverviewFederationService,
    IDataSourceService dataSourceService) : OverviewAreaModel(schoolService, trustService, dataSourceService)
{
    public override PageMetadata PageMetadata => base.PageMetadata with
    {
        SubPageName = SubPageName
    };

    public const string SubPageName = "Federation details";
    
    public SchoolOverviewFederationServiceModel SchoolOverviewFederationServiceModel { get; private set; } = null!;
    
    public string? Name { get; set; }
    public string? FederationUid { get; set; }
    public DateTime? OpenedOnDate { get; set; }
    public Dictionary<string, string>[]? Schools { get; set; }

    public override async Task<IActionResult> OnGetAsync()
    {
        var pageResult = await base.OnGetAsync();
        if (pageResult is NotFoundResult) return pageResult;

        SchoolOverviewFederationServiceModel =
            await schoolOverviewFederationService.GetSchoolOverviewFederationAsync(Urn);

        Name = SchoolOverviewFederationServiceModel.Name;
        FederationUid = SchoolOverviewFederationServiceModel.FederationUid;
        OpenedOnDate = SchoolOverviewFederationServiceModel.OpenedOnDate;
        Schools = SchoolOverviewFederationServiceModel.Schools;
        
        return pageResult;
    }
}
