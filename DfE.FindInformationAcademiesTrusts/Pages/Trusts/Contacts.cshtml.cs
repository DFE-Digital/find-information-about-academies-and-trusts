using DfE.FindInformationAcademiesTrusts.Data;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts;

public class ContactsModel : TrustsAreaModel
{
    public ContactsModel(ITrustProvider trustProvider) : base(trustProvider, "Contacts")
    {
    }
}
