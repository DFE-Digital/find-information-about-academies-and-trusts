namespace DfE.FindInformationAcademiesTrusts.Data;

public class PaginatedList<T> : List<T>, IPaginatedList<T>
{
    public int PageIndex { get; }
    public int TotalPages { get; }
    public int TotalResults { get; }

    public PaginatedList(IEnumerable<T> items, int count, int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        TotalResults = count;
        AddRange(items);
    }

    public PaginatedList()
    {
    }

    public bool HasPreviousPage => PageIndex > 1;

    public bool HasNextPage => PageIndex < TotalPages;
}
