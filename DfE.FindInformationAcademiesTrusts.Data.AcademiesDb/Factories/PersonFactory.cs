using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Cdm;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Factories;

public interface IPersonFactory
{
    Person CreateFrom(CdmSystemuser cdmSystemuser);
}

public class PersonFactory : IPersonFactory
{
    public Person CreateFrom(CdmSystemuser cdmSystemuser)
    {
        return new Person(cdmSystemuser.Fullname!, cdmSystemuser.Internalemailaddress!);
    }
}
