namespace DfE.FindInformationAcademiesTrusts.Pages.ManageMyCasework
{
    public class CaseInfo(IEnumerable<CaseInfoItem> cases, string title, string titleLink, string system, string projectType)
    {
        public string Title { get; init; } = title;
        public string TitleLink { get; init; } = titleLink;
        public string System { get; init; } = system;
        public string ProjectType { get; init; } = projectType;

        public IEnumerable<CaseInfoItem> Cases { get; init; } = cases;
    }

    public record CaseInfoItem(string Label, string Value, string? Link);
}
