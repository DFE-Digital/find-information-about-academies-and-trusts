using System.Text.Json;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Faker;

public static class JsonGenerator
{
    public static void GenerateAndSaveTrustsJson(Group[] fakeGroups, string outputFilePath)
    {
        var serializeOptions = new JsonSerializerOptions
            { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        var trustHelper = new TrustHelper();
        var trusts = fakeGroups.OrderBy(g => g.GroupName).Select(g => trustHelper.CreateTrustFromGroup(g));

        File.WriteAllText(outputFilePath, JsonSerializer.Serialize(trusts, serializeOptions));
    }
}
