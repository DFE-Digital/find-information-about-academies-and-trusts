using System.Reflection;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models;
using Microsoft.EntityFrameworkCore;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Faker;

public static class SqlScriptGenerator
{
    public static void GenerateAndSaveSqlScripts(Group[] fakeGroups, string createScriptOutputFilePath,
        string insertScriptOutputFilePath)
    {
        var dbContextOptions = new DbContextOptionsBuilder<AcademiesDbContext>().UseSqlServer();
        using var context = new AcademiesDbContext(dbContextOptions.Options);

        GenerateSqlCreateScript(context, createScriptOutputFilePath);
        GenerateSqlInsertScript(fakeGroups, context, insertScriptOutputFilePath);
    }

    private static void GenerateSqlCreateScript(AcademiesDbContext context, string outputFilePath)
    {
        var createScript = context.Database.GenerateCreateScript();

        Directory.CreateDirectory("data");
        File.WriteAllText(outputFilePath, createScript);
    }

    private static void GenerateSqlInsertScript(Group[] fakeGroups, AcademiesDbContext context, string ouputFilePath)
    {
        var groupProperties = typeof(Group).GetProperties();
        var valuesStrings = fakeGroups.Select(group => $"({GetEntityValues(group, groupProperties)})");

        var insertScript =
            $"INSERT INTO [gias].[Group] ({GetEntityProperties(groupProperties, context)}) VALUES {string.Join(',', valuesStrings)}";
        File.WriteAllText(ouputFilePath, insertScript);
    }

    private static string GetEntityValues<T>(T obj, PropertyInfo[] entityProperties)
    {
        var list = entityProperties
            .Select(pi => GetValueAsString(pi.GetValue(obj), pi.PropertyType));

        return string.Join(", ", list);
    }

    private static string GetEntityProperties(PropertyInfo[] propertyInfos, AcademiesDbContext context)
    {
        var entityProperties = context.Model.FindEntityTypes(typeof(Group)).FirstOrDefault()!.GetProperties();
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

        throw new Exception("unknown type");
    }

    private static string TransformIntoSqlSafeString(string? unsantisedString)
    {
        if (string.IsNullOrEmpty(unsantisedString))
            return "''";
        return $"'{unsantisedString.Replace("'", @"''")}'";
    }
}
