using System.Text.Json;
using Bogus;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Faker.Fakers;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Faker.Helpers;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Faker;

internal static class Program
{
    public static void Main(string[] args)
    {
        //The randomizer seed enables us to generate slightly repeatable data sets
        Randomizer.Seed = new Random(28698);

        var fakeGroups = Data.TrustsToGenerate
            .Select(GenerateGroup)
            .ToArray();

        var serializeOptions = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        File.WriteAllText("output.json", JsonSerializer.Serialize(fakeGroups, serializeOptions));
    }

    private static Group GenerateGroup(TrustToGenerate trustToGenerate)
    {
        var fakeGroup = new GroupFaker(trustToGenerate);
        return fakeGroup.Generate();
    }
}
