using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Faker.Helpers;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Gias;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Mstr;

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
            Array.Empty<GiasEstablishment>(),
            Array.Empty<GiasGovernance>(),
            Array.Empty<GroupLink>(),
            groups,
            mstrTrusts);
    }

    private GiasGroup GenerateGroup(TrustToGenerate trustToGenerate)
    {
        var fakeGroup = new GiasGroupFaker(trustToGenerate, _counter++);
        return fakeGroup.Generate();
    }

    private MstrTrust GenerateMstrTrust(GiasGroup giasGroup)
    {
        return new MstrTrustFaker(giasGroup.GroupUid!, _regions).Generate();
    }
}
