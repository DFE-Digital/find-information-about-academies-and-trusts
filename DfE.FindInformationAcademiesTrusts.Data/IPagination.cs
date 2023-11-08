namespace DfE.FindInformationAcademiesTrusts.Data;

public interface IPagination
{
    int PageIndex { get; }
    int TotalPages { get; }
    int TotalResults { get; }
    bool HasPreviousPage { get; }
    bool HasNextPage { get; }
}
