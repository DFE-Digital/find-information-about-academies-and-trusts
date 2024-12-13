using System.ComponentModel.DataAnnotations;
using DfE.FIAT.Data;
using DfE.FIAT.Data.Enums;
using DfE.FIAT.Web.Extensions;
using DfE.FIAT.Web.Services.DataSource;
using DfE.FIAT.Web.Services.Trust;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DfE.FIAT.Web.Pages.Trusts.Contacts;

public abstract class EditContactModel(
    IDataSourceService dataSourceService,
    ITrustService trustService,
    ILogger<EditContactModel> logger,
    ContactRole role)
    : TrustsAreaModel(dataSourceService, trustService,
        logger, $"Edit {role.MapRoleToViewString()} details")
{
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

    public string GetErrorClass(string key)
    {
        return ModelState.ContainsKey(key) && ModelState[key]!.Errors.Any() ? "govuk-form-group--error" : string.Empty;
    }

    public string GenerateErrorAriaDescribedBy(string key)
    {
        return string.Join(" ", GetErrorList(key).Select(value => $"error-{key}-{value.index}"));
    }

    public (int index, string errorMessage)[] GetErrorList(string key)
    {
        if (ModelState.ContainsKey(key))
        {
            return ModelState[key]!.Errors.Select((error, index) => (index, error.ErrorMessage)).ToArray();
        }

        return [];
    }

    public string GeneratePageTitle()
    {
        return $"{(ModelState.IsValid ? string.Empty : "Error: ")}{PageTitle ?? PageName} - {TrustSummary.Name}";
    }
}
