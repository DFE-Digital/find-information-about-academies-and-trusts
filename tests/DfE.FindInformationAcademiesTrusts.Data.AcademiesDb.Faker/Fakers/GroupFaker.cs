using Bogus;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Faker.Helpers;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Faker.Fakers;

public class GroupFaker
{
    private readonly Faker<Group> _groupFaker;

    public GroupFaker(TrustToGenerate trustToGenerate)
    {
        _groupFaker = new Faker<Group>("en_GB")
            .RuleFor(t => t.GroupName, trustToGenerate.Name)
            .RuleFor(t => t.GroupUid, f => $"{f.Random.Int(1000, 9999)}")
            .RuleFor(td => td.GroupId, f => $"TR{f.Random.Int(0, 9999)}")
            .RuleFor(td => td.GroupType, trustToGenerate.TrustType)
            .RuleFor(td => td.GroupContactStreet, f => $"{f.Address.BuildingNumber()} {f.Address.StreetName()}")
            .RuleFor(td => td.GroupContactLocality, f => f.PickRandom(f.Address.StreetName(), string.Empty))
            .RuleFor(td => td.GroupContactTown, f => f.PickRandom(f.Address.City(), string.Empty))
            .RuleFor(td => td.GroupContactPostcode, f => f.Address.ZipCode());
    }

    public Group Generate()
    {
        return _groupFaker.Generate();
    }
}
