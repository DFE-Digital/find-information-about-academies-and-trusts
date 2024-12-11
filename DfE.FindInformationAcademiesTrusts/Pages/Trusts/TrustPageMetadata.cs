namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts;

public record TrustPageMetadata(
    string TrustName,
    string? PageName = null,
    string? SubPageName = null,
    string? TabName = null)
{
    public string BrowserTitle
    {
        get
        {
            var orderedTitleParts = new[] { TabName, SubPageName, PageName, TrustName }.Where(s => s is not null);
            return string.Join(" - ", orderedTitleParts);
        }
    }
}
