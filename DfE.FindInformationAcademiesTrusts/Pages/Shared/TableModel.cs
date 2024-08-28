namespace DfE.FindInformationAcademiesTrusts.Pages.Shared;

public class TableModel(string tableName, RowValue[][] rows, ColumnValue[] columns)
{
    public string TableName { get; set; } = tableName;
    public RowValue[][] Rows { get; set; } = rows;
    public ColumnValue[] Columns { get; set; } = columns;
}

public class RowValue(string data, string? sortValue = null)
{
    public string data { get; set; } = data;
    public string sortValue { get; set; } = sortValue ?? data;
}

public class ColumnValue(string columnText, string testId, bool isDate = false)
{
    public string ColumnText { get; set; } = columnText;
    public string TestId { get; set; } = testId;
    public bool IsDate { get; set; } = isDate;
}
