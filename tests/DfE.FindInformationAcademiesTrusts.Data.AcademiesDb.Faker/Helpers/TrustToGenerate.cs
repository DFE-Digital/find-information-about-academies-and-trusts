namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Faker.Helpers;

public class TrustToGenerate
{
    public TrustToGenerate(string name, string trustType = "Multi-academy trust", bool hasNoAcademies = false,
        string? status = null, params string[] schools
    )
    {
        Name = name;
        TrustType = trustType;
        Schools = schools;
        HasNoAcademies = hasNoAcademies;
        Status = status;
    }

    public string Name { get; }
    public string TrustType { get; }
    public string[] Schools { get; }

    public bool HasNoAcademies { get; }

    public string? Status { get; }
}
