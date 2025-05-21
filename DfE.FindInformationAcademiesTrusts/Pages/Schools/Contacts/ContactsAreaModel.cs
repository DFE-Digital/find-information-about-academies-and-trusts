using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Pages.Shared;
using DfE.FindInformationAcademiesTrusts.Pages.Shared.Contact;
using DfE.FindInformationAcademiesTrusts.Services.School;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.Pages.Schools.Contacts;

public class ContactsAreaModel(ISchoolService schoolService, ITrustService trustService)
    : SchoolAreaModel(schoolService, trustService)
{
    public const string PageName = "Contacts";

    public override PageMetadata PageMetadata => base.PageMetadata with { PageName = PageName };

    public ContactModel HeadTeacher { get; private set; } = null!;

    public override async Task<IActionResult> OnGetAsync()
    {
        var pageResult = await base.OnGetAsync();

        if (pageResult.GetType() == typeof(NotFoundResult)) return pageResult;

        HeadTeacher = new ContactModel("Head teacher", "head-teacher",
            new Person("Aaron Aaronson", "aa@someschool.com"));

        return pageResult;
    }
}
