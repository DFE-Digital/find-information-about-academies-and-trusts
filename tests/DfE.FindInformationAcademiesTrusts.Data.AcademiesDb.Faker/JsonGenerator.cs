using System.Text.Json;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Factories;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Faker;

public static class JsonGenerator
{
    public static void GenerateAndSaveTrustsJson(AcademiesDbData fakeData, string outputFilePath)
    {
        var serializeOptions = new JsonSerializerOptions
            { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        var trustHelper = new TrustFactory();
        var academyFactory = new AcademyFactory();
        var trusts = fakeData.GiasGroups
            .OrderBy(g => g.GroupName)
            .Select(g =>
            {
                var academies = fakeData.GiasGroupLinks.Where(gl => gl.GroupUid == g.GroupUid && gl.Urn != null)
                    .Join(fakeData.GiasEstablishments, e => e.Urn, gl => gl.Urn.ToString(),
                        (gl, e) => academyFactory.CreateAcademyFrom(gl, e)).ToArray();

                return trustHelper
                    .CreateTrustFrom(g, fakeData.MstrTrusts.FirstOrDefault(t => t.GroupUid == g.GroupUid)!,
                        academies, Array.Empty<Governor>(), null, null);
            });

        File.WriteAllText(outputFilePath, JsonSerializer.Serialize(trusts, serializeOptions));
    }
}
