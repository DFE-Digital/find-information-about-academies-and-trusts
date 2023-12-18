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

    public ContactsModel(ITrustProvider trustProvider, IDataSourceProvider sourceProvider,
        ILogger<ContactsModel> logger) : base(trustProvider, sourceProvider, logger, "Contacts")
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

        DataSources.Add(new DataSourceListEntry(await DataSourceProvider.GetCdmUpdated(),
            new List<string> { "DfE contacts" }));

        DataSources.Add(new DataSourceListEntry(await DataSourceProvider.GetGiasUpdated(),
            new List<string>
                { "Accounting officer name", "Chief financial officer name", "Chair of trustees name" }));

        DataSources.Add(new DataSourceListEntry(await DataSourceProvider.GetMstrUpdated(),
            new List<string>
                { "Accounting officer email", "Chief financial officer email", "Chair of trustees email" }));

        return pageResult;
    }
}
