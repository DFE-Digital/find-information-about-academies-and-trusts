namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts;

public record TrustSubNavigationLinkModel(
    string LinkText,
    string Page,
    string Uid,
    string ServiceName)
{
    public bool LinkIsActive { get; set; }

    public string TestId =>
        $"{ServiceName.ToLowerInvariant().Trim().Replace(" ", "-")}-{LinkText.ToLowerInvariant().Trim().Replace(" ", "-")}-subnav";
}
