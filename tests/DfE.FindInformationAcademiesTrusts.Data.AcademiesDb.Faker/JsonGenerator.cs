using System.Text.Json;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Factories;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Faker;

public static class JsonGenerator
{
    public static void GenerateAndSaveTrustsJson(AcademiesDbData fakeData, string outputFilePath)
    {
        var serializeOptions = new JsonSerializerOptions
            { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        var trustProvider = new TrustProvider(fakeData.AsAcademiesDbContext(), new TrustFactory(), new AcademyFactory(),
            new GovernorFactory(),
            new PersonFactory());
        var trusts = fakeData.GiasGroups
            .OrderBy(g => g.GroupName)
            .Select(g => trustProvider.GetTrustByUidAsync(g.GroupUid!).Result);

        File.WriteAllText(outputFilePath, JsonSerializer.Serialize(trusts, serializeOptions));
    }
}
