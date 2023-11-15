using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Faker.Helpers;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Faker.Fakers;

public class AcademiesDbFaker
{
    private int _counter = 1233;
    private readonly string?[] _regions;

    public AcademiesDbFaker(string?[] regions)
    {
        _regions = regions;
    }

    public AcademiesDbData Generate(TrustToGenerate[] trustsToGenerate)
    {
        var groups = trustsToGenerate
            .Select(GenerateGroup).ToArray();
        var mstrTrusts = groups.Select(GenerateMstrTrust).ToArray();

        return new AcademiesDbData(
            Array.Empty<Establishment>(),
            Array.Empty<Governance>(),
            Array.Empty<GroupLink>(),
            groups,
            mstrTrusts);
    }

    private Group GenerateGroup(TrustToGenerate trustToGenerate)
    {
        var fakeGroup = new GroupFaker(trustToGenerate, _counter++);
        return fakeGroup.Generate();
    }

    private MstrTrust GenerateMstrTrust(Group group)
    {
        return new MstrTrustFaker(group.GroupUid!, _regions).Generate();
    }
}
