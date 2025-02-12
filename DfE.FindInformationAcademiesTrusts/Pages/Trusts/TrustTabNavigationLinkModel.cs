using DfE.FindInformationAcademiesTrusts.Extensions;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts;

public record TrustTabNavigationLinkModel(
    string LinkText,
    string TabPageLink,
    string TabNavName,
    bool LinkIsActive)
{
    public string TestId => $"{TabNavName}-{LinkText}-tab".Kebabify();
}
