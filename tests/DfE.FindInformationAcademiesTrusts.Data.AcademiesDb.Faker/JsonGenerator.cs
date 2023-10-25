using System.Text.Json;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Faker;

public static class JsonGenerator
{
    public static void GenerateAndSaveTrustsJson(Group[] fakeGroups, string outputFilePath)
    {
        var serializeOptions = new JsonSerializerOptions
            { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        var jsonisisedTrusts = JsonSerializer.Serialize(fakeGroups, serializeOptions);
        File.WriteAllText(outputFilePath, jsonisisedTrusts);
    }
}
