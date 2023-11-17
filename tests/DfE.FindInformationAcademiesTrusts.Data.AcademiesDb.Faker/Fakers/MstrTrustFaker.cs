using Bogus;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Mstr;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Faker.Fakers;

public class MstrTrustFaker
{
    private readonly Faker<MstrTrust> _mstrTrustFaker;

    public MstrTrustFaker(string uid, string?[] region)
    {
        _mstrTrustFaker = new Faker<MstrTrust>()
            .RuleFor(t => t.GroupUid, uid)
            .RuleFor(t => t.GORregion, f => f.PickRandom(region));
    }

    public MstrTrust Generate()
    {
        return _mstrTrustFaker.Generate();
    }
}
