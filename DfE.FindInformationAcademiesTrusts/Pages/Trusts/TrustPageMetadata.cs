namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts;

public record TrustPageMetadata(
    string TrustName,
    bool ModelStateIsValid,
    string? PageName = null,
    string? SubPageName = null,
    string? TabName = null)
{
    public string BrowserTitle
    {
        get
        {
            var orderedTitleParts = new[] { TabName, SubPageName, PageName, TrustName }.Where(s => s is not null);
            var browserTitle = string.Join(" - ", orderedTitleParts);

            if (!ModelStateIsValid)
            {
                browserTitle = $"Error: {browserTitle}";
            }

            return browserTitle;
        }
    }
}
