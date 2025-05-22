using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Pages.Shared;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.School;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.Pages.Schools.Overview;

public class SenModel(ISchoolService schoolService, 
    ITrustService trustService, 
    ISchoolOverviewSenService schoolOverviewSenService,
    IDataSourceService dataSourceService) : OverviewAreaModel(schoolService, trustService, dataSourceService)
{
    public override PageMetadata PageMetadata => base.PageMetadata with
    {
        SubPageName = SubPageName
    };

    public const string SubPageName = "SEN (special educational needs)";

    public SchoolOverviewSenServiceModel SchoolOverviewSenServiceModel { get; private set; } = null!;

    public string? ResourcedProvisionOnRoll { get; set; }
    public string? ResourcedProvisionCapacity { get; set; }
    public string? SenOnRoll { get; set; }
    public string? SenCapacity { get; set; }
    public string? ResourcedProvisionType { get; set; }
    public List<string> SenProvisionTypes { get; set; } = new();

    public override async Task<IActionResult> OnGetAsync()
    {
        var pageResult = await base.OnGetAsync();
        if (pageResult is NotFoundResult) return pageResult;

        SchoolOverviewSenServiceModel = await schoolOverviewSenService.GetSchoolOverviewSenAsync(Urn);
        ResourcedProvisionOnRoll = SchoolOverviewSenServiceModel.ResourcedProvisionOnRoll != null ? SchoolOverviewSenServiceModel.ResourcedProvisionOnRoll : "Not available";
        ResourcedProvisionCapacity = SchoolOverviewSenServiceModel.ResourcedProvisionCapacity != null ? SchoolOverviewSenServiceModel.ResourcedProvisionCapacity : "Not available";
        SenOnRoll = SchoolOverviewSenServiceModel.SenOnRoll != null ? SchoolOverviewSenServiceModel.SenOnRoll : "Not available";
        SenCapacity = SchoolOverviewSenServiceModel.SenCapacity != null ? SchoolOverviewSenServiceModel.SenCapacity : "Not available";
        ResourcedProvisionType = SchoolOverviewSenServiceModel.ResourcedProvisionTypes != null ? SchoolOverviewSenServiceModel.ResourcedProvisionTypes : "Not available";
        SenProvisionTypes = SchoolOverviewSenServiceModel.SenProvisionTypes.Count > 0 ? SchoolOverviewSenServiceModel.SenProvisionTypes : new List<string>{"Not available"};
        
        return pageResult;
    }
}
