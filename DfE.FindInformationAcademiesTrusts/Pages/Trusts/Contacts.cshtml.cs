using DfE.FindInformationAcademiesTrusts.Data;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts;

public class ContactsModel : TrustsAreaModel
{
    public Governor? ChairOfTrustees { get; set; }

    public Governor? AccountingOfficer { get; set; }

    public Governor? ChiefFinancialOfficer { get; set; }

    public string ContactNameNotAvailableMessage = "No contact name available";

    public string ContactEmailNotAvailableMessage = "No contact email available";

    public string ContactInformationNotAvailableMessage = "No contact information available";

    public ContactsModel(ITrustProvider trustProvider) : base(trustProvider, "Contacts")
    {
    }

    public override async Task<IActionResult> OnGetAsync()
    {
        var pageResult = await base.OnGetAsync();
        ChairOfTrustees = Trust.Governors.FirstOrDefault(x =>
            x.Role == "Chair of Trustees" && (x.DateOfTermEnd == null || x.DateOfTermEnd >= DateTime.Now));
        AccountingOfficer = Trust.Governors.FirstOrDefault(x =>
            x.Role == "Accounting Officer" && (x.DateOfTermEnd == null || x.DateOfTermEnd >= DateTime.Now));
        ChiefFinancialOfficer = Trust.Governors.FirstOrDefault(x =>
            x.Role == "Chief Financial Officer" && (x.DateOfTermEnd == null || x.DateOfTermEnd >= DateTime.Now));

        return pageResult;
    }
}
