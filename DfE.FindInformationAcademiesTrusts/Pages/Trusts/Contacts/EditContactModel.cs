using System.ComponentModel.DataAnnotations;
using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Extensions;
using DfE.FindInformationAcademiesTrusts.Pages.Shared;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.Contacts;

public abstract class EditContactModel(
    IDataSourceService dataSourceService,
    ITrustService trustService,
    TrustContactRole role)
    : TrustsAreaModel(dataSourceService, trustService)
{
    public override PageMetadata PageMetadata => base.PageMetadata with
    {
        SubPageName = $"Edit {role.MapRoleToViewString()} details",
        PageName = ContactsAreaModel.PageName
    };

    public const string NameField = "Name";
    public const string EmailField = "Email";

    [BindProperty]
    [BindRequired]
    [MaxLength(500)]
    public string? Name { get; set; }

    [BindProperty]
    [BindRequired]
    [EmailAddress(ErrorMessage = "Enter an email address in the correct format, like name@education.gov.uk")]
    [RegularExpression(@"(?i)^\S*@education\.gov\.uk$",
        ErrorMessage = "Enter a DfE email address without any spaces")]
    [MaxLength(320)]
    public string? Email { get; set; }

    [TempData] public string ContactUpdatedMessage { get; set; } = string.Empty;

    protected abstract InternalContact? GetContactFromServiceModel(TrustContactsServiceModel contacts);

    public override async Task<IActionResult> OnGetAsync()
    {
        var pageResult = await base.OnGetAsync();

        if (pageResult.GetType() == typeof(NotFoundResult)) return pageResult;

        var contacts = await TrustService.GetTrustContactsAsync(Uid);

        Email = GetContactFromServiceModel(contacts)?.Email;
        Name = GetContactFromServiceModel(contacts)?.FullName;

        return pageResult;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return await base.OnGetAsync();
        }

        var result = await TrustService.UpdateContactAsync(int.Parse(Uid), Name, Email, role);

        ContactUpdatedMessage = result switch
        {
            { NameUpdated: true, EmailUpdated: true } =>
                $"Changes made to the {role.MapRoleToViewString()} name and email were updated.",
            { NameUpdated: true, EmailUpdated: false } =>
                $"Changes made to the {role.MapRoleToViewString()} name were updated.",
            { NameUpdated: false, EmailUpdated: true } =>
                $"Changes made to the {role.MapRoleToViewString()} email were updated.",
            { NameUpdated: false, EmailUpdated: false } => string.Empty,
            _ => throw new InvalidOperationException(nameof(result))
        };

        return RedirectToPage("/Trusts/Contacts/InDfe", new { Uid });
    }
}
