using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Faker;

public static class SqlScriptGenerator
{
    public static void GenerateAndSaveSqlScripts(AcademiesDbData fakeData, string createScriptOutputFilePath,
        string insertScriptOutputFilePath)
    {
        var dbContextOptions = new DbContextOptionsBuilder<AcademiesDbContext>().UseSqlServer();
        using var context = new AcademiesDbContext(dbContextOptions.Options);

        GenerateSqlCreateScript(context, createScriptOutputFilePath);
        GenerateSqlInsertScript(context, insertScriptOutputFilePath, fakeData);
    }

    private static void GenerateSqlCreateScript(AcademiesDbContext context, string outputFilePath)
    {
        var createScript = context.Database.GenerateCreateScript();

        Directory.CreateDirectory("data");
        File.WriteAllText(outputFilePath, createScript);
    }

    private static void GenerateSqlInsertScript(AcademiesDbContext context, string outputFilePath,
        AcademiesDbData fakeData)
    {
        var insertScripts = new List<string>();
        insertScripts.AddRange(GenerateSqlInsertScriptSegmentsFor(fakeData.GiasGroups, context));
        insertScripts.AddRange(GenerateSqlInsertScriptSegmentsFor(fakeData.MstrTrusts, context));
        insertScripts.AddRange(GenerateSqlInsertScriptSegmentsFor(fakeData.MstrTrustGovernances, context));
        insertScripts.AddRange(GenerateSqlInsertScriptSegmentsFor(fakeData.GiasGovernances, context));
        insertScripts.AddRange(GenerateSqlInsertScriptSegmentsFor(fakeData.GiasGroupLinks, context));
        insertScripts.AddRange(GenerateSqlInsertScriptSegmentsFor(fakeData.GiasEstablishments, context));
        insertScripts.AddRange(GenerateSqlInsertScriptSegmentsFor(fakeData.CdmAccounts, context));
        insertScripts.AddRange(GenerateSqlInsertScriptSegmentsFor(fakeData.CdmSystemusers, context));
        insertScripts.AddRange(GenerateSqlInsertScriptSegmentsFor(fakeData.MisEstablishments, context));

        File.WriteAllLines(outputFilePath, insertScripts);
    }

    private static List<string> GenerateSqlInsertScriptSegmentsFor<T>(List<T> rowObjects, AcademiesDbContext context)
    {
        var entityType = context.Model.FindEntityTypes(typeof(T)).FirstOrDefault()!;
        var entityProperties = typeof(T).GetProperties();
        var entityColumnNames = GetEntityColumnNames(entityProperties, entityType);
        var tableName = $"[{entityType.GetSchema()}].[{entityType.GetTableName()}]";

        var insertScriptSegments = new List<string>();

        // SQL Server Insert statements will only work for 1000 rows at a time. Batch the objects into a value less than that
        const int insertRowBatch = 500;
        for (var i = 0; i < rowObjects.Count; i += insertRowBatch)
        {
            var rowObjectBatch = rowObjects.Skip(i);
            if (i + insertRowBatch < rowObjects.Count)
                rowObjectBatch = rowObjectBatch.Take(insertRowBatch);

            var rows = rowObjectBatch.Select(obj => $"({GetEntityValues(obj, entityProperties)})");
            insertScriptSegments.Add($"INSERT INTO {tableName} ({entityColumnNames}) VALUES {string.Join(',', rows)};");
        }

        return insertScriptSegments;
    }

    private static string GetEntityValues<T>(T obj, PropertyInfo[] entityProperties)
    {
        var list = entityProperties
            .Select(pi => GetValueAsString(pi.GetValue(obj), pi.PropertyType));

        return string.Join(", ", list);
    }

    private static string GetEntityColumnNames(PropertyInfo[] propertyInfos, IEntityType entityType)
    {
        var entityProperties = entityType.GetProperties();
        var columnNames =
            propertyInfos.Select(pi =>
            {
                var columnName = entityProperties.FirstOrDefault(ep => ep.Name == pi.Name)!.GetColumnName();
                return $"[{columnName}]";
            });
        return string.Join(',', columnNames);
    }

    private static string GetValueAsString(object? value, Type propertyType)
    {
        if (value is null)
            return "NULL";

        if (propertyType == typeof(string))
        {
            return TransformIntoSqlSafeString(value.ToString());
        }

        if (propertyType == typeof(int) || propertyType == typeof(long) || propertyType == typeof(int?))
        {
            return value.ToString()!;
        }

        if (propertyType == typeof(Guid) || propertyType == typeof(Guid?))
        {
            return $"'{value}'";
        }

        throw new ApplicationException($"Can't get value as string for unknown type '{propertyType}'");
    }

    private static string TransformIntoSqlSafeString(string? unsantisedString)
    {
        if (string.IsNullOrEmpty(unsantisedString))
            return "''";
        return $"'{unsantisedString.Replace("'", @"''")}'";
    }
}
