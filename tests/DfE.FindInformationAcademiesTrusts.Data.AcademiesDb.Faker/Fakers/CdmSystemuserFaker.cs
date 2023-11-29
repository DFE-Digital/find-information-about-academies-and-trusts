using Bogus;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Cdm;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Faker.Fakers;

public class CdmSystemuserFaker
{
    private readonly Faker<CdmSystemuser> _cdmSystemuserFaker;

    public CdmSystemuserFaker()
    {
        _cdmSystemuserFaker = new Faker<CdmSystemuser>()
            .RuleFor(u => u.Systemuserid, f => f.Random.Guid())
            .RuleFor(u => u.Fullname, f => f.Person.FullName)
            .RuleFor(u => u.Internalemailaddress, f => $"{f.Person.FirstName}.{f.Person.LastName}@education.gov.uk");
    }

    public CdmSystemuser[] Generate(int numToGenerate)
    {
        return _cdmSystemuserFaker.Generate(numToGenerate).ToArray();
    }
}
