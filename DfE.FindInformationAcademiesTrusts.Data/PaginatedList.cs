﻿namespace DfE.FindInformationAcademiesTrusts.Data;

public class PaginatedList<T> : List<T>, IPaginatedList<T>
{
    public PageStatus PageStatus { get; }

    public PaginatedList(IEnumerable<T> items, int count, int pageIndex, int pageSize)
    {
        var totalPages = (int)Math.Ceiling(count / (double)pageSize);
        PageStatus = new PageStatus(pageIndex, totalPages, count);
        AddRange(items);
    }

    public static PaginatedList<T> Empty()
    {
        return new PaginatedList<T>(Array.Empty<T>(), 0, 0, 1);
    }
}
