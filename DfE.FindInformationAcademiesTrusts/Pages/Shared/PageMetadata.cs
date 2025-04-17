namespace DfE.FindInformationAcademiesTrusts.Pages.Shared;

public record PageMetadata(
    string EntityName,
    bool ModelStateIsValid,
    string? PageName = null,
    string? SubPageName = null,
    string? TabName = null)
{
    public string BrowserTitle
    {
        get
        {
            var orderedTitleParts = new[] { TabName, SubPageName, PageName, EntityName }.Where(s => s is not null);
            var browserTitle = string.Join(" - ", orderedTitleParts);

            if (!ModelStateIsValid)
            {
                browserTitle = $"Error: {browserTitle}";
            }

            return browserTitle;
        }
    }
}
