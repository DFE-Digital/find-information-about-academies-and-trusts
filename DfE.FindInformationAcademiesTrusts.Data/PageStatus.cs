namespace DfE.FindInformationAcademiesTrusts.Data;

public class PageStatus : IPageStatus
{
    public PageStatus(int pageIndex, int totalPages, int totalResults)
    {
        PageIndex = pageIndex;
        TotalPages = totalPages;
        TotalResults = totalResults;
    }

    public int PageIndex { get; }
    public int TotalPages { get; }
    public int TotalResults { get; }
    public bool HasPreviousPage => PageIndex > 1;
    public bool HasNextPage => PageIndex < TotalPages;
}
