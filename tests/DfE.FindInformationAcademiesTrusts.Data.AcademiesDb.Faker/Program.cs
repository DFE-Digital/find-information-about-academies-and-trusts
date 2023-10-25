using Bogus;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Faker.Fakers;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Faker.Helpers;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Faker;

public static class Program
{
    public static void Main(params string[] args)
    {
        try
        {
            //The randomizer seed enables us to generate slightly repeatable data sets
            //The data sets will only change when changes are made to the generator
            Randomizer.Seed = new Random(28698);

            var fakeGroups = Data.TrustsToGenerate
                .Select(GenerateGroup).ToArray();

            SqlScriptGenerator.GenerateAndSaveSqlScripts(fakeGroups, "data/createScript.sql", "data/insertScript.sql");
            JsonGenerator.GenerateAndSaveTrustsJson(fakeGroups, "data/output.json");
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
}
