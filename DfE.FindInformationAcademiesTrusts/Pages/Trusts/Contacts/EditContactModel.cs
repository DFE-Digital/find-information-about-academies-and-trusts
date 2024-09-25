﻿using System.ComponentModel.DataAnnotations;
using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.Contacts;

public abstract class EditContactModel(
    ITrustProvider trustProvider,
    IDataSourceService dataSourceService,
    ITrustService trustService,
    ILogger<EditContactModel> logger,
    string roleText)
    : TrustsAreaModel(trustProvider, dataSourceService, trustService,
        logger, $"Edit {roleText}")
{
    public const string NameField = "Name";
    public const string EmailField = "Email";

    [BindProperty] [BindRequired] public string? Name { get; set; }

    [BindProperty]
    [BindRequired]
    [EmailAddress(ErrorMessage = "Enter an email address in the correct format, like name@education.gov.uk")]
    [RegularExpression(".*@education.gov.uk$", ErrorMessage = "Enter a DfE email address ending in @education.gov.uk")]
    public string? Email { get; set; }

    [TempData] public string ContactUpdatedMessage { get; set; } = string.Empty;

    private string RoleText { get; init; } = roleText;

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return await base.OnGetAsync();
        }

        ContactUpdatedMessage = $"Changes made to the {RoleText} were successfully updated.";
        return RedirectToPage("/Trusts/Contacts", new { Uid });
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