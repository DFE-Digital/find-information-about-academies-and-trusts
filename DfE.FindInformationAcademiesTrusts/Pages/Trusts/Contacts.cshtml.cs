using DfE.FindInformationAcademiesTrusts.Data;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts;

public class ContactsModel : TrustsAreaModel
{
    public Governor? ChairOfTrustees { get; set; }

    public Governor? AccountingOfficer { get; set; }

    public Governor? ChiefFinancialOfficer { get; set; }

    public const string ContactNameNotAvailableMessage = "No contact name available";

    public const string ContactEmailNotAvailableMessage = "No contact email available";

    public const string ContactInformationNotAvailableMessage = "No contact information available";

    public ContactsModel(ITrustProvider trustProvider, IDataSourceProvider sourceProvider) : base(trustProvider,
        sourceProvider, "Contacts")
    {
    }

    public override async Task<IActionResult> OnGetAsync()
    {
        var pageResult = await base.OnGetAsync();

        if (pageResult.GetType() == typeof(NotFoundResult)) return pageResult;

        ChairOfTrustees = Array.Find(Trust.Governors, x =>
            x is { Role: "Chair of Trustees", IsCurrentGovernor: true });
        AccountingOfficer = Array.Find(Trust.Governors, x =>
            x is { Role: "Accounting Officer", IsCurrentGovernor: true });
        ChiefFinancialOfficer = Array.Find(Trust.Governors, x =>
            x is { Role: "Chief Financial Officer", IsCurrentGovernor: true });

        var giasSource = await GetGiasDataUpdated();
        var cdmSource = await GetCdmDateUpdated();
        var mstrSource = await GetMstrDataUpdated();
        DataSources = new[] { new DataSourceListEntry(giasSource!, "Accounting Officer Name, Chief Financial Officer Name, Chair of trustees Name"), 
            new DataSourceListEntry(cdmSource!, "DfE Contacts"), 
            new DataSourceListEntry(mstrSource!, "Accounting Officer Email, Chief Financial Officer Email, Chair of trustees Email")
        };
        return pageResult;
    }
}
