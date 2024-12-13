using DfE.FIAT.Web.Extensions;

namespace DfE.FIAT.Web.Pages.Trusts;

public record TrustSubNavigationLinkModel(
    string LinkText,
    string Page,
    string Uid,
    string ServiceName,
    bool LinkIsActive)
{
    public string TestId => $"{ServiceName}-{LinkText}-subnav".Kebabify();
}
