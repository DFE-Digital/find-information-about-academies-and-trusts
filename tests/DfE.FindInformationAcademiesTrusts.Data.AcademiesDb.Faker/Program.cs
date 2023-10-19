using System.Reflection;
using System.Text.Json;
using Bogus;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Faker.Fakers;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Faker.Helpers;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models;
using Microsoft.EntityFrameworkCore;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Faker;

public static class Program
{
    public static void Main(params string[] args)
    {
        try
        {
            var dbContextOptions = new DbContextOptionsBuilder<AcademiesDbContext>().UseSqlServer();
            using var context = new AcademiesDbContext(dbContextOptions.Options);
            var createScript = context.Database.GenerateCreateScript();

            Directory.CreateDirectory("data");
            File.WriteAllText("data/createScript.sql", createScript);

            //The randomizer seed enables us to generate slightly repeatable data sets
            Randomizer.Seed = new Random(28698);

            var fakeGroups = Data.TrustsToGenerate
                .Select(GenerateGroup).ToArray();

            var serializeOptions = new JsonSerializerOptions { WriteIndented = true };
            var jsonisisedTrusts = JsonSerializer.Serialize(fakeGroups, serializeOptions);
            File.WriteAllText("data/output.json", jsonisisedTrusts);

            var groupProperties = typeof(Group).GetProperties();
            var valuesStrings = fakeGroups.Select(group => $"({GetEntityValues(group, groupProperties)})");

            var insertScript =
                $"INSERT INTO [gias].[Group] ({GetEntityProperties(groupProperties, context)}) VALUES {string.Join(',', valuesStrings)}";
            File.WriteAllText("data/insertScript.sql", insertScript);
        }
        catch (Exception e)
        {
            Console.WriteLine($"error {e}");
        }
    }

    private static Group GenerateGroup(TrustToGenerate trustToGenerate)
    {
        var fakeGroup = new GroupFaker(trustToGenerate);
        return fakeGroup.Generate();
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
