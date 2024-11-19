namespace DfE.FIAT.Data;

public interface IPageStatus
{
    int PageIndex { get; }
    int TotalPages { get; }
    int TotalResults { get; }
    bool HasPreviousPage { get; }
    bool HasNextPage { get; }
}
