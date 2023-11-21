using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Faker.Helpers;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Gias;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Mstr;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Faker.Fakers;

public class AcademiesDbFaker
{
    private int _counter = 1233;
    private readonly string?[] _regions;
    private readonly string?[] _localAuthorities;
    private readonly GiasGroupLinkFaker _giasGroupLinkFaker = new();
    private readonly Bogus.Faker _generalFaker = new();
    private readonly GiasEstablishmentFaker _establishmentFaker;

    public AcademiesDbFaker(string?[] regions, string?[] localAuthorities, string[] fakeSchoolNames)
    {
        _regions = regions;
        _localAuthorities = localAuthorities;
        _establishmentFaker = new GiasEstablishmentFaker(fakeSchoolNames);
    }

    public AcademiesDbData Generate(TrustToGenerate[] trustsToGenerate)
    {
        var academiesDbData = new AcademiesDbData();

        foreach (var trustToGenerate in trustsToGenerate)
        {
            var group = GenerateGroup(trustToGenerate);
            academiesDbData.GiasGroups.Add(group);
            academiesDbData.MstrTrusts.Add(GenerateMstrTrust(group.GroupUid));

            var academies = GenerateAcademies(trustToGenerate, group.GroupUid!).ToArray();
            academiesDbData.GiasEstablishments.AddRange(academies);
            academiesDbData.GiasGroupLinks.AddRange(academies.Select(academy => GenerateGroupLink(academy, group)));
        }

        return academiesDbData;
    }

    private GiasGroup GenerateGroup(TrustToGenerate trustToGenerate)
    {
        var fakeGroup = new GiasGroupFaker(trustToGenerate, _counter++);
        return fakeGroup.Generate();
    }

    private MstrTrust GenerateMstrTrust(string? uid)
    {
        return new MstrTrustFaker(uid!, _regions).Generate();
    }

    private GiasGroupLink GenerateGroupLink(GiasEstablishment establishment, GiasGroup giasGroup)
    {
        return _giasGroupLinkFaker
            .SetGiasGroupOpenedDate(giasGroup.IncorporatedOnOpenDate)
            .Generate(giasGroup.GroupUid!,
                establishment.Urn.ToString());
    }

    private IEnumerable<GiasEstablishment> GenerateAcademies(TrustToGenerate trustToGenerate, string uid)
    {
        _establishmentFaker.SetUid(uid).SetLocalAuthoritiesSelection(
            _generalFaker.PickRandom(_localAuthorities, _generalFaker.Random.Int(1, 4)).ToArray());

        if (trustToGenerate.Schools.Any())
        {
            return _establishmentFaker.Generate(trustToGenerate.Schools);
        }

        return _establishmentFaker.Generate(_generalFaker.Random.Int(0, 11));
    }
}
