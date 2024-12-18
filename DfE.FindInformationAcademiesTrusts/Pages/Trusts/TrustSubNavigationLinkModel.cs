using DfE.FindInformationAcademiesTrusts.Extensions;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts;

public record TrustSubNavigationLinkModel(
    string LinkText,
    string SubPageLink,
    string Uid,
    string ServiceName,
    bool LinkIsActive)
{
    public string TestId => $"{ServiceName}-{LinkText}-subnav".Kebabify();
}
