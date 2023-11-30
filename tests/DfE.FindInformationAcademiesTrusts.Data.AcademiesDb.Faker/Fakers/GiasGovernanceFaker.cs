using System.Globalization;
using Bogus;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Gias;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Faker.Fakers;

public class GiasGovernanceFaker
{
    private readonly Dictionary<string, string[]> _governorAppointingBodies;
    private const string DateFormat = "dd/MM/yyyy";
    private readonly Faker<GiasGovernance> _governanceFaker = new();
    private readonly Bogus.Faker _generalFaker = new();

    public GiasGovernanceFaker(DateTime refDate, Dictionary<string, string[]> governorAppointingBodies)
    {
        _governorAppointingBodies = governorAppointingBodies;
        _governanceFaker
            .RuleFor(g => g.Gid, f => f.Random.Int(1000000, 1500000).ToString())
            .RuleFor(g => g.Forename1, f => f.Person.FirstName)
            .RuleFor(g => g.Forename2, "")
            .RuleFor(g => g.Surname, f => f.Person.LastName)
            .RuleSet("current", set =>
            {
                set.RuleFor(g => g.DateOfAppointment, f => f.Date.Past(2, refDate).ToString(DateFormat))
                    .RuleFor(g => g.DateTermOfOfficeEndsEnded, f => f.Date.Future(4, refDate).ToString(DateFormat));
            })
            .RuleSet("historic", set =>
            {
                set.RuleFor(g => g.DateOfAppointment, f => f.Date.Past(10, refDate).ToString(DateFormat))
                    .RuleFor(g => g.DateTermOfOfficeEndsEnded,
                        (f, g) => f.Date
                            .Between(
                                DateTime.ParseExact(g.DateOfAppointment!, DateFormat, CultureInfo.InvariantCulture),
                                refDate).ToString(DateFormat));
            });
    }

    private GiasGovernance Generate(string uid, string role, bool isCurrent)
    {
        var giasGovernance = _governanceFaker.Generate(isCurrent ? "default,current" : "default,historic");

        giasGovernance.Uid = uid;
        giasGovernance.Role = role;
        giasGovernance.AppointingBody = _generalFaker.PickRandom(_governorAppointingBodies[role]);

        return giasGovernance;
    }

    private IEnumerable<GiasGovernance> Generate(int numberToGenerate, string uid, string role)
    {
        for (var i = 0; i < numberToGenerate; i++)
        {
            yield return Generate(uid, role, _generalFaker.Random.Bool());
        }
    }

    public List<GiasGovernance> Generate(string uid)
    {
        var governors = new List<GiasGovernance>
        {
            Generate(uid, "Chair of Trustees", true),
            Generate(uid, "Chief Financial Officer", true),
            Generate(uid, "Accounting Officer", true)
        };

        governors.AddRange(Generate(_generalFaker.Random.Int(0, 20), uid, "Trustee"));
        governors.AddRange(Generate(_generalFaker.Random.Int(0, 10), uid, "Member"));

        return governors;
    }
}
