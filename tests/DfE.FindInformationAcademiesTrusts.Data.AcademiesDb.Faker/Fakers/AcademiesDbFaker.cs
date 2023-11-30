using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Faker.Helpers;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Gias;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Faker.Fakers;

public class AcademiesDbFaker
{
    private int _counter = 1233;
    private readonly Bogus.Faker _generalFaker = new();

    private readonly string?[] _localAuthorities;
    private readonly GiasGroupLinkFaker _giasGroupLinkFaker;
    private readonly GiasEstablishmentFaker _giasEstablishmentFaker;
    private readonly GiasGroupFaker _giasGroupFaker;
    private readonly GiasGovernanceFaker _giasGovernanceFaker;
    private readonly MstrTrustFaker _mstrTrustFaker;
    private readonly MstrTrustGovernanceFaker _mstrTrustGovernanceFaker;
    private readonly CdmAccountFaker _cdmAccountFaker = new();
    private readonly CdmSystemuserFaker _cdmSystemuserFaker = new();

    public AcademiesDbFaker(string?[] regions, string?[] localAuthorities, string[] fakeSchoolNames,
        Dictionary<string, string[]> governorAppointingBodies)
    {
        // Need a ref date for any use of `faker.Date` so the data generated doesn't change every day
        var refDate = new DateTime(2023, 11, 9);

        _localAuthorities = localAuthorities;
        _giasGroupFaker = new GiasGroupFaker(refDate);
        _giasGovernanceFaker = new GiasGovernanceFaker(refDate, governorAppointingBodies);
        _giasEstablishmentFaker = new GiasEstablishmentFaker(fakeSchoolNames);
        _giasGroupLinkFaker = new GiasGroupLinkFaker(refDate);
        _mstrTrustFaker = new MstrTrustFaker(regions);
        _mstrTrustGovernanceFaker = new MstrTrustGovernanceFaker();
    }

    public AcademiesDbData Generate(TrustToGenerate[] trustsToGenerate)
    {
        var academiesDbData = new AcademiesDbData();

        ConfigureDfeContacts(academiesDbData);

        foreach (var trustToGenerate in trustsToGenerate)
        {
            var group = _giasGroupFaker.Generate(trustToGenerate, _counter++);
            academiesDbData.GiasGroups.Add(group);
            var groupUid = group.GroupUid!;
            academiesDbData.MstrTrusts.Add(_mstrTrustFaker.Generate(groupUid));
            academiesDbData.CdmAccounts.Add(_cdmAccountFaker.Generate(groupUid));

            var academies = GenerateAcademies(trustToGenerate, groupUid).ToArray();
            academiesDbData.GiasEstablishments.AddRange(academies);
            academiesDbData.GiasGroupLinks.AddRange(academies.Select(academy => GenerateGroupLink(academy, group)));

            var giasGovernances = _giasGovernanceFaker.Generate(groupUid);
            academiesDbData.GiasGovernances.AddRange(giasGovernances);
            academiesDbData.MstrTrustGovernances.AddRange(_mstrTrustGovernanceFaker.Generate(giasGovernances));
        }

        return academiesDbData;
    }

    private void ConfigureDfeContacts(AcademiesDbData academiesDbData)
    {
        var dfeContacts = _cdmSystemuserFaker.Generate(50);
        academiesDbData.CdmSystemusers.AddRange(dfeContacts);
        _cdmAccountFaker.SetSfsoLeads(dfeContacts.Take(25).Select(c => c.Systemuserid));
        _cdmAccountFaker.SetTrustRelationshipManagers(dfeContacts.Skip(25).Select(c => c.Systemuserid));
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
        _giasEstablishmentFaker.SetUid(uid).SetLocalAuthoritiesSelection(
            _generalFaker.PickRandom(_localAuthorities, _generalFaker.Random.Int(1, 4)).ToArray());

        if (trustToGenerate.HasNoAcademies)
        {
            return Array.Empty<GiasEstablishment>();
        }

        if (trustToGenerate.Schools.Any())
        {
            return _giasEstablishmentFaker.Generate(trustToGenerate.Schools);
        }

        return _giasEstablishmentFaker.Generate(_generalFaker.Random.Int(0, 11));
    }
}
