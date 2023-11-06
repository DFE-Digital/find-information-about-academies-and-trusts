namespace DfE.FindInformationAcademiesTrusts.Data;

public interface IPaginatedList<T> : IList<T>
{
    int PageIndex { get; }
    int TotalPages { get; }
    int TotalResults { get; }
    bool HasPreviousPage { get; }
    bool HasNextPage { get; }
}
