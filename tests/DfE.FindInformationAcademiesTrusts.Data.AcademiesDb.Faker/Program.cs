using Bogus;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Faker.Fakers;

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


            var faker = new AcademiesDbFaker(Data.Regions, Data.LocalAuthorities, Data.Schools,
                Data.GovernorAppointingBodies, Data.GiasPhaseNames);
            var academiesDbData = faker.Generate(Data.TrustsToGenerate);

            SqlScriptGenerator.GenerateAndSaveSqlScripts(academiesDbData, "data/createScript.sql",
                "data/insertScript.sql");

            // Comment in if you regenerate the data
            // JsonGenerator.GenerateAndSaveTrustsJson(academiesDbData, "data/trusts.json");
        }
        catch (Exception e)
        {
            Console.WriteLine($"error {e}");
        }
    }
}
