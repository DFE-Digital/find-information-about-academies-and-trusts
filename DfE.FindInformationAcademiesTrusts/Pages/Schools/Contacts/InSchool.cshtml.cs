using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Pages.Shared;
using DfE.FindInformationAcademiesTrusts.Pages.Shared.Contact;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.School;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.Pages.Schools.Contacts;

public class InSchoolModel(
    ISchoolService schoolService,
    ITrustService trustService,
    ISchoolContactsService schoolContactsService,
    IDataSourceService dataSourceService)
    : ContactsAreaModel(schoolService, trustService, dataSourceService)
{
    public override PageMetadata PageMetadata => base.PageMetadata with
    {
        SubPageName = FullSubPageName(SchoolCategory)
    };

    public static string FullSubPageName(SchoolCategory schoolCategory)
    {
        return schoolCategory switch
        {
            SchoolCategory.LaMaintainedSchool => "Contacts in this school",
            SchoolCategory.Academy => "Contacts in this academy",
            _ => throw new ArgumentOutOfRangeException(nameof(schoolCategory))
        };
    }

    public static string SubPageName(SchoolCategory schoolCategory)
    {
        return schoolCategory switch
        {
            SchoolCategory.LaMaintainedSchool => "In this school",
            SchoolCategory.Academy => "In this academy",
            _ => throw new ArgumentOutOfRangeException(nameof(schoolCategory))
        };
    }

    public ContactModel HeadTeacher { get; private set; } = null!;

    public override async Task<IActionResult> OnGetAsync()
    {
        var pageResult = await base.OnGetAsync();
        if (pageResult is NotFoundResult) return pageResult;

        HeadTeacher = await schoolContactsService.GetInSchoolContactsAsync(Urn);

        return pageResult;
    }
}
