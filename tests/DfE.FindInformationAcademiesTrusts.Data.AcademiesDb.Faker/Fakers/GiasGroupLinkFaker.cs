using System.Globalization;
using Bogus;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Gias;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Faker.Fakers;

public class GiasGroupLinkFaker
{
    private readonly Faker<GiasGroupLink> _groupLinkFaker;
    private string? _giasGroupOpenedDate;

    public GiasGroupLinkFaker(DateTime refDate)
    {
        _groupLinkFaker = new Faker<GiasGroupLink>("en_GB")
            .RuleFor(t => t.JoinedDate, f => f.Date.Between(
                    DateTime.ParseExact(_giasGroupOpenedDate!, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    refDate).ToString("dd/MM/yyyy")
            );
    }

    public GiasGroupLinkFaker SetGiasGroupOpenedDate(string? giasGroupOpenedDate)
    {
        _giasGroupOpenedDate = giasGroupOpenedDate;
        return this;
    }

    public GiasGroupLink Generate(string uid, string urn)
    {
        var fakeGroupLink = _groupLinkFaker.Generate();
        fakeGroupLink.GroupUid = uid;
        fakeGroupLink.Urn = urn;
        return fakeGroupLink;
    }
}
