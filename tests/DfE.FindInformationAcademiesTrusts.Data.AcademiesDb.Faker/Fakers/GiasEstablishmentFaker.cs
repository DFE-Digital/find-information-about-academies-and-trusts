using Bogus;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Faker.Helpers;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Gias;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Faker.Fakers;

public class GiasEstablishmentFaker
{
    private IEnumerable<int> _laCodes = Array.Empty<int>();
    private readonly Faker<GiasEstablishment> _establishmentFaker;
    private readonly string[] _fakeSchoolNames;
    private string _uid = "";
    private readonly IEstablishmentTypePicker _giasEstablishmentTypePicker;

    public GiasEstablishmentFaker(string[] fakeSchoolNames, string[] giasPhaseNames,
        IEstablishmentTypePicker giasEstablishmentTypePicker, Dictionary<int, string> localAuthorities)
    {
        _giasEstablishmentTypePicker = giasEstablishmentTypePicker;
        _fakeSchoolNames = fakeSchoolNames;
        _establishmentFaker = new Faker<GiasEstablishment>("en_GB")
                // will this Urn sometimes be a duplicate?
                .RuleFor(a => a.Urn, f => int.Parse($"{_uid}{f.Random.Int(1000, 9999)}"))
                .RuleFor(e => e.LaCode, f => f.PickRandom(_laCodes).ToString())
                .RuleFor(e => e.LaName, (f, e) => localAuthorities[int.Parse(e.LaCode!)])
                .RuleFor(e => e.UrbanRuralName, f => f.PickRandom(
                    "Urban city and town",
                    "Rural town and fringe",
                    "Rural village in a sparse setting",
                    "Urban major conurbation",
                    "Urban minor conurbation"))
                .RuleFor(e => e.SchoolCapacity, f => f.Random.Int(100, 3000).ToString())
                .RuleFor(e => e.NumberOfPupils, (f, e) =>
                {
                    var schoolCapacity = int.Parse(e.SchoolCapacity!);
                    var noOfPupils = (int)Math.Round(schoolCapacity * f.Random.Double(0.4, 1.3));
                    return noOfPupils.ToString();
                })
                .RuleFor(e => e.PercentageFsm, f => f.Random.Int(1, 30).ToString())
                .RuleSet("randomSchoolName", set =>
                {
                    set.RuleFor(e => e.PhaseOfEducationName, f => f.PickRandom(giasPhaseNames))
                        .RuleFor(e => e.TypeOfEstablishmentName, RandomEstablishmentTypeOnPhase)
                        .RuleFor(e => e.EstablishmentName, GenerateSchoolName)
                        .RuleFor(e => e.StatutoryLowAge, (f, e) =>
                            GetMinAge(e.PhaseOfEducationName, f)
                        )
                        .RuleFor(e => e.StatutoryHighAge,
                            (f, e) => GetMaxAge(e.PhaseOfEducationName, f));
                })
                .RuleSet("definedSchoolName",
                    set =>
                    {
                        set.RuleFor(e => e.PhaseOfEducationName,
                                (f, e) => e.EstablishmentName!.Contains("Primary",
                                    StringComparison.CurrentCultureIgnoreCase)
                                    ? "Primary"
                                    : e.EstablishmentName.Contains("Secondary",
                                        StringComparison.CurrentCultureIgnoreCase)
                                        ? "Secondary"
                                        : f.PickRandom(giasPhaseNames))
                            .RuleFor(e => e.TypeOfEstablishmentName, RandomEstablishmentTypeOnPhase)
                            .RuleFor(e => e.StatutoryLowAge,
                                (f, e) => GetMinAge(e.PhaseOfEducationName, f))
                            .RuleFor(e => e.StatutoryHighAge,
                                (f, e) => GetMaxAge(e.PhaseOfEducationName, f));
                    })
            ;
    }

    private string RandomEstablishmentTypeOnPhase(Bogus.Faker f, GiasEstablishment e)
    {
        return f.PickRandom(_giasEstablishmentTypePicker.GetEstablishmentTypes(e));
    }

    private string GenerateSchoolName(Bogus.Faker faker, GiasEstablishment establishment)
    {
        var name = faker.PickRandom(
            _fakeSchoolNames.Concat(new[] { establishment.LaName!, faker.Address.StreetName() }));
        if (name.StartsWith("st", StringComparison.InvariantCultureIgnoreCase) || faker.Random.Bool())
            name = $"{name} {faker.PickRandom("Church of England", "Cofe", "CE", "Catholic", "C of E", "R.C.")}";

        if (faker.Random.Bool() && !establishment.PhaseOfEducationName!.Contains("Not applicable"))
            name = $"{name} {establishment.PhaseOfEducationName}";
        name = $"{name} {faker.PickRandom("School", "Academy")}";

        return name;
    }

    private string GetMinAge(string? phaseOfEducationName, Bogus.Faker faker)
    {
        return phaseOfEducationName == "Primary" ? faker.PickRandom("4", "5") : "11";
    }

    private string GetMaxAge(string? phaseOfEducationName, Bogus.Faker faker)
    {
        return phaseOfEducationName == "Primary" ? "11" : faker.PickRandom("16", "18");
    }

    public GiasEstablishmentFaker SetLocalAuthoritiesSelection(IEnumerable<int> laCodes)
    {
        _laCodes = laCodes;
        return this;
    }

    public GiasEstablishmentFaker SetUid(string uid)
    {
        _uid = uid;
        return this;
    }

    public IEnumerable<GiasEstablishment> Generate(int num)
    {
        return _establishmentFaker.Generate(num, "default,randomSchoolName");
    }

    public IEnumerable<GiasEstablishment> Generate(IEnumerable<string> schools)
    {
        return schools.Select(n =>
        {
            _establishmentFaker.RuleFor(e => e.EstablishmentName, n);
            return _establishmentFaker.Generate("default,definedSchoolName");
        });
    }
}
