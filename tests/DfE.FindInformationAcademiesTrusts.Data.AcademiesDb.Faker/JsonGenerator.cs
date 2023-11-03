using System.Text.Json;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Faker.Fakers;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Faker;

public static class JsonGenerator
{
    public static void GenerateAndSaveTrustsJson(AcademiesDbData fakeData, string outputFilePath)
    {
        var serializeOptions = new JsonSerializerOptions
            { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        var trustHelper = new TrustHelper();
        var trusts = fakeData.Groups
            .OrderBy(g => g.GroupName)
            .Select(g => trustHelper
                .CreateTrustFrom(g, fakeData.MstrTrusts.FirstOrDefault(t => t.GroupUid == g.GroupUid)!)
            );

        File.WriteAllText(outputFilePath, JsonSerializer.Serialize(trusts, serializeOptions));
    }
}
