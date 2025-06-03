namespace DfE.FindInformationAcademiesTrusts.TestDataMigrator.Queries;

public static class DeleteAllQueries
{
    public static string Delete = @"DELETE FROM [gias].[GroupLink]
                                        DELETE FROM [gias].[Establishment]
                                        DELETE FROM [gias].[Group]";
}
