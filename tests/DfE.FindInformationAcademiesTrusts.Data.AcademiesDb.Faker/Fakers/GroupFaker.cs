using Bogus;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Faker.Helpers;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Faker.Fakers;

public class GroupFaker
{
    private readonly Faker<Group> _groupFaker;

    public GroupFaker(TrustToGenerate trustToGenerate, int uid)
    {
        _groupFaker = new Faker<Group>("en_GB")
            .RuleFor(g => g.GroupName, trustToGenerate.Name)
            .RuleFor(g => g.GroupUid, f => $"{uid}")
            .RuleFor(g => g.GroupId, f => $"TR{f.Random.Int(0, 9999)}")
            .RuleFor(g => g.Ukprn, f => $"100{f.Random.Int(0, 99999):D5}")
            .RuleFor(g => g.GroupType, trustToGenerate.TrustType)
            .RuleFor(g => g.GroupContactStreet, f => $"{f.Address.BuildingNumber()} {f.Address.StreetName()}")
            .RuleFor(g => g.GroupContactLocality, f => f.PickRandom(f.Address.StreetName(), string.Empty))
            .RuleFor(g => g.GroupContactTown, f => f.PickRandom(f.Address.City(), string.Empty))
            .RuleFor(g => g.GroupContactPostcode, f => f.Address.ZipCode())
            .RuleFor(g => g.IncorporatedOnOpenDate,
                f => f.Date.Past(10, new DateTime(2023, 11, 9))
                    .ToString("dd/MM/yyyy")) //Need a ref date for `Date.Past` so the data generated doesn't change every day
            .RuleFor(g => g.CompaniesHouseNumber, f => f.Random.Int(1100000, 09999999).ToString("D8"));
    }

    public Group Generate()
    {
        return _groupFaker.Generate();
    }
}
