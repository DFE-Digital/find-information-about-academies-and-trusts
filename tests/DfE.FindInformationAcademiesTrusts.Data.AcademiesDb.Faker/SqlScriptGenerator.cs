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
        var insertScripts = new[]
        {
            GenerateSqlInsertScriptSegmentFor(fakeData.GiasGroups, context),
            GenerateSqlInsertScriptSegmentFor(fakeData.MstrTrusts, context),
            GenerateSqlInsertScriptSegmentFor(fakeData.GiasGroupLinks, context),
            GenerateSqlInsertScriptSegmentFor(fakeData.GiasEstablishments, context),
            GenerateSqlInsertScriptSegmentFor(fakeData.CdmAccounts, context),
            GenerateSqlInsertScriptSegmentFor(fakeData.CdmSystemusers, context)
        };

        File.WriteAllLines(outputFilePath, insertScripts);
    }

    private static string GenerateSqlInsertScriptSegmentFor<T>(List<T> fakeObjects, AcademiesDbContext context)
    {
        var objProperties = typeof(T).GetProperties();
        var entityType = context.Model.FindEntityTypes(typeof(T)).FirstOrDefault()!;
        var tableName = $"[{entityType.GetSchema()}].[{entityType.GetTableName()}]";
        var valuesStrings = fakeObjects.Select(obj => $"({GetEntityValues(obj, objProperties)})");
        var insertString =
            $"INSERT INTO {tableName} ({GetEntityProperties(objProperties, entityType)}) VALUES {string.Join(',', valuesStrings)};";
        return insertString;
    }

    private static string GetEntityValues<T>(T obj, PropertyInfo[] entityProperties)
    {
        var list = entityProperties
            .Select(pi => GetValueAsString(pi.GetValue(obj), pi.PropertyType));

        return string.Join(", ", list);
    }

    private static string GetEntityProperties(PropertyInfo[] propertyInfos, IEntityType entityType)
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

        if (propertyType == typeof(int))
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
