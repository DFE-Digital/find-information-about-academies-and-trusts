namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Faker.Helpers;

public class TrustToGenerate
{
    public TrustToGenerate(string name, string trustType = "Multi-academy trust", params string[] schools)
    {
        if (trustType == "Single-academy trust" && schools.Length != 1)
            throw new ArgumentException("Single academy trusts must have one school specified");

        Name = name;
        TrustType = trustType;
        Schools = schools;
    }

    public string Name { get; }
    public string TrustType { get; }
    public string[] Schools { get; }
}
