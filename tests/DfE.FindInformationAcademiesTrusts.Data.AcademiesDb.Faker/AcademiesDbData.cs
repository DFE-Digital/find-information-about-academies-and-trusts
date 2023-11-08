using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Faker.Fakers;

public class AcademiesDbData
{
    public readonly Group[] Groups;
    public readonly MstrTrust[] MstrTrusts;

    public AcademiesDbData(Group[] groups, MstrTrust[] mstrTrusts)
    {
        Groups = groups;
        MstrTrusts = mstrTrusts;
    }
}
