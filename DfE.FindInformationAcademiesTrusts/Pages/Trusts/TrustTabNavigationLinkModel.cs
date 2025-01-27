using DfE.FindInformationAcademiesTrusts.Extensions;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts;

public record TrustTabNavigationLinkModel(
    string LinkText,
    string SubPageLink,
    string Uid,
    string SubNavName,
    bool LinkIsActive)
{
    public string TestId => $"{SubNavName}-{LinkText}-tab".Kebabify();
}