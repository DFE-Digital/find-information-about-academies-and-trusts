using Bogus;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Faker.Helpers;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Gias;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Faker.Fakers;

public class GiasGroupFaker
{
    private readonly Faker<GiasGroup> _giasGroupFaker;

    public GiasGroupFaker(DateTime refDate)
    {
        _giasGroupFaker = new Faker<GiasGroup>("en_GB")
            .RuleFor(g => g.GroupId, f => $"TR{f.Random.Int(0, 9999)}")
            .RuleFor(g => g.Ukprn, f => $"100{f.Random.Int(0, 99999):D5}")
            .RuleFor(g => g.GroupContactStreet, f => $"{f.Address.BuildingNumber()} {f.Address.StreetName()}")
            .RuleFor(g => g.GroupContactLocality, f => f.PickRandom(f.Address.StreetName(), string.Empty))
            .RuleFor(g => g.GroupContactTown, f => f.PickRandom(f.Address.City(), string.Empty))
            .RuleFor(g => g.GroupContactPostcode, f => f.Address.ZipCode())
            .RuleFor(g => g.IncorporatedOnOpenDate,
                f => f.Date.Past(10, refDate)
                    .ToString("dd/MM/yyyy"))
            .RuleFor(g => g.CompaniesHouseNumber, f => f.Random.Int(1100000, 09999999).ToString("D8"))
            // Temporary measure to ensure tests continue to provide value
            // Defaulting all to open as that is all our system handles now
            .RuleFor(g => g.GroupStatus, f => "Open")
            .RuleFor(g => g.GroupStatusCode, f => "OPEN");
    }

    public GiasGroup Generate(TrustToGenerate trustToGenerate, int uid)
    {
        var fakeGiasGroup = _giasGroupFaker.Generate();
        fakeGiasGroup.GroupName = trustToGenerate.Name;
        fakeGiasGroup.GroupType = trustToGenerate.TrustType;
        fakeGiasGroup.GroupUid = uid.ToString();

        if (trustToGenerate.Status is not null)
        {
            fakeGiasGroup.GroupStatus = trustToGenerate.Status;
        }

        return fakeGiasGroup;
    }
}
