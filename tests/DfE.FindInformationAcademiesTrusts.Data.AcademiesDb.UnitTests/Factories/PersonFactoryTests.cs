using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Factories;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Cdm;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests.Factories;

public class PersonFactoryTests
{
    [Fact]
    public void CreateFrom_should_transform_cdmSystemuser_into_person()
    {
        var sut = new PersonFactory();

        var cdmSystemuser = new CdmSystemuser
            { Fullname = "Test person", Internalemailaddress = "test.person@education.gov.uk" };

        var result = sut.CreateFrom(cdmSystemuser);

        result.FullName.Should().Be("Test person");
        result.Email.Should().Be("test.person@education.gov.uk");
    }
}
