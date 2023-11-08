namespace DfE.FindInformationAcademiesTrusts.Data;

public class PaginatedList<T> : List<T>, IPaginatedList<T>
{
    public Pagination Pagination { get; }

    public PaginatedList(IEnumerable<T> items, int count, int pageIndex, int pageSize)
    {
        var totalPages = (int)Math.Ceiling(count / (double)pageSize);
        Pagination = new Pagination(pageIndex, totalPages, count);
        AddRange(items);
    }
}
