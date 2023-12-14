using Bogus;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Extensions;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Mis;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Faker.Fakers;

public class MisEstablishmentFaker
{
    private readonly Faker<MisEstablishment> _misEstablishmentFaker;

    public MisEstablishmentFaker(DateTime refDate)
    {
        _misEstablishmentFaker = new Faker<MisEstablishment>()
            .RuleFor(m => m.OverallEffectiveness, f => f.Random.Int(1, 4).OrNull(f, .2F))
            .RuleFor(m => m.InspectionEndDate, (f, m) =>
                m.OverallEffectiveness is null
                    ? null
                    : f.Date.Past(4, refDate).ToString("dd/MM/yyyy"))
            .RuleFor(m => m.PreviousFullInspectionOverallEffectiveness,
                (f, m) => m.OverallEffectiveness is null
                    ? null
                    : f.Random.Int(1, 4).OrNull(f, .2F)?.ToString())
            .RuleFor(m => m.PreviousInspectionEndDate, (f, m) =>
                m.PreviousFullInspectionOverallEffectiveness is null
                    ? null
                    : f.Date.Past(9, m.InspectionEndDate!.ParseAsNullableDate()).ToString("dd/MM/yyyy"));
    }

    public MisEstablishment Generate(int urn)
    {
        var misEstablishment = _misEstablishmentFaker.Generate();
        misEstablishment.Urn = urn;
        misEstablishment.UrnAtTimeOfLatestFullInspection =
            misEstablishment.OverallEffectiveness is null ? null : urn;
        misEstablishment.UrnAtTimeOfPreviousFullInspection =
            misEstablishment.PreviousFullInspectionOverallEffectiveness is null ? null : urn;
        return misEstablishment;
    }
}
