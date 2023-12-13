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

        var cdmSource = await GetCdmDateUpdated();
        if (cdmSource is not null)
        {
            DataSources.Add(new DataSourceListEntry(cdmSource,
                new List<string> { "DfE Contacts" }));
        }

        var giasSource = await GetGiasDataUpdated();
        if (giasSource is not null)
        {
            DataSources.Add(new DataSourceListEntry(giasSource,
                new List<string>
                    { "Accounting Officer name", "Chief Financial Officer name", "Chair of trustees name" }));
        }

        var mstrSource = await GetMstrDataUpdated();
        if (mstrSource is not null)
        {
            DataSources.Add(new DataSourceListEntry(mstrSource,
                new List<string>
                    { "Accounting Officer email", "Chief Financial Officer email", "Chair of trustees email" }));
        }

        return pageResult;
    }
}
