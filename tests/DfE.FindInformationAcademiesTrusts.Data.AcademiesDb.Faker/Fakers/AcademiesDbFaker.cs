using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Faker.Helpers;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Gias;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Faker.Fakers;

public class AcademiesDbFaker
{
    private int _counter = 1233;
    private readonly Bogus.Faker _generalFaker = new();

    private readonly GiasGroupLinkFaker _giasGroupLinkFaker;
    private readonly GiasEstablishmentFaker _giasEstablishmentFaker;
    private readonly GiasGroupFaker _giasGroupFaker;
    private readonly GiasGovernanceFaker _giasGovernanceFaker;
    private readonly MstrTrustFaker _mstrTrustFaker;
    private readonly MstrTrustGovernanceFaker _mstrTrustGovernanceFaker;
    private readonly CdmAccountFaker _cdmAccountFaker = new();
    private readonly CdmSystemuserFaker _cdmSystemuserFaker = new();
    private readonly MisEstablishmentFaker _misEstablishmentFaker;
    private AcademiesDbData _academiesDbData;
    private readonly IEnumerable<int> _laCodes;
    private readonly ApplicationEventFaker _applicationEventFaker;
    private readonly ApplicationSettingsFaker _applicationSettingsFaker;

    public AcademiesDbFaker(string?[] regions, Dictionary<int, string> localAuthorities, string[] fakeSchoolNames,
        Dictionary<string, string[]> governorAppointingBodies, string[] giasPhaseNames)
    {
        // Need a ref date for any use of `faker.Date` so the data generated doesn't change every day
        var refDate = new DateTime(2023, 11, 9);

        _laCodes = localAuthorities.Select(la => la.Key);
        _giasGroupFaker = new GiasGroupFaker(refDate);
        _giasGovernanceFaker = new GiasGovernanceFaker(refDate, governorAppointingBodies);
        _giasEstablishmentFaker =
            new GiasEstablishmentFaker(fakeSchoolNames, giasPhaseNames, new EstablishmentTypePicker(),
                localAuthorities);
        _giasGroupLinkFaker = new GiasGroupLinkFaker(refDate);
        _mstrTrustFaker = new MstrTrustFaker(regions);
        _mstrTrustGovernanceFaker = new MstrTrustGovernanceFaker();
        _misEstablishmentFaker = new MisEstablishmentFaker(refDate);
        _applicationEventFaker = new ApplicationEventFaker(refDate);
        _applicationSettingsFaker = new ApplicationSettingsFaker(refDate);
    }

    public AcademiesDbData Generate(TrustToGenerate[] trustsToGenerate)
    {
        _academiesDbData = new AcademiesDbData();

        ConfigureDfeContacts();

        foreach (var trustToGenerate in trustsToGenerate)
        {
            var group = _giasGroupFaker.Generate(trustToGenerate, _counter++);
            _academiesDbData.GiasGroups.Add(group);
            var groupUid = group.GroupUid!;
            _academiesDbData.MstrTrusts.Add(_mstrTrustFaker.Generate(groupUid));
            _academiesDbData.CdmAccounts.Add(_cdmAccountFaker.Generate(groupUid));

            GenerateAcademies(trustToGenerate, groupUid, group.IncorporatedOnOpenDate);

            var giasGovernances = _giasGovernanceFaker.Generate(groupUid);
            _academiesDbData.GiasGovernances.AddRange(giasGovernances);
            _academiesDbData.MstrTrustGovernances.AddRange(_mstrTrustGovernanceFaker.Generate(giasGovernances));
        }

        _academiesDbData.ApplicationEvents.AddRange(_applicationEventFaker.Generate());
        _academiesDbData.ApplicationSettings.AddRange(_applicationSettingsFaker.Generate());

        _academiesDbData.ApplicationEvents.AddRange(_applicationEventFaker.Generate());
        _academiesDbData.ApplicationSettings.AddRange(_applicationSettingsFaker.Generate());

        return _academiesDbData;
    }

    private void GenerateAcademies(TrustToGenerate trustToGenerate, string groupUid, string? groupOpenDate)
    {
        var giasEstablishments = GenerateGiasEstablishments(trustToGenerate, groupUid).ToArray();
        _academiesDbData.GiasEstablishments.AddRange(giasEstablishments);

        foreach (var giasEstablishment in giasEstablishments)
        {
            _academiesDbData.GiasGroupLinks.Add(GenerateGroupLink(giasEstablishment.Urn, groupUid, groupOpenDate));
            _academiesDbData.MisEstablishments.Add(_misEstablishmentFaker.Generate(giasEstablishment.Urn));
        }
    }

    private void ConfigureDfeContacts()
    {
        var dfeContacts = _cdmSystemuserFaker.Generate(50);
        _academiesDbData.CdmSystemusers.AddRange(dfeContacts);
        _cdmAccountFaker.SetSfsoLeads(dfeContacts.Take(25).Select(c => c.Systemuserid));
        _cdmAccountFaker.SetTrustRelationshipManagers(dfeContacts.Skip(25).Select(c => c.Systemuserid));
    }

    private GiasGroupLink GenerateGroupLink(int establishmentUrn, string groupUid, string? openDate)
    {
        return _giasGroupLinkFaker
            .SetGiasGroupOpenedDate(openDate)
            .Generate(groupUid,
                establishmentUrn.ToString());
    }

    private IEnumerable<GiasEstablishment> GenerateGiasEstablishments(TrustToGenerate trustToGenerate, string uid)
    {
        _giasEstablishmentFaker.SetUid(uid).SetLocalAuthoritiesSelection(
            _generalFaker.PickRandom(_laCodes, _generalFaker.Random.Int(1, 4)).ToArray());

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
