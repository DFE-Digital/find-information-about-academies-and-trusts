using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Gias;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Mstr;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Faker.Fakers;

public class MstrTrustGovernanceFaker
{
    private readonly Bogus.Faker _generalFaker = new();

    public IEnumerable<MstrTrustGovernance> Generate(IEnumerable<GiasGovernance> giasGovernances)
    {
        return giasGovernances.Select(gg => new MstrTrustGovernance
        {
            Gid = gg.Gid!,
            Email = gg.Role is "Member" or "Trustee"
                ? null // Members and trustees don't have email addresses
                : _generalFaker.Random.WeightedRandom(new[] { $"{gg.Forename1}.{gg.Surname}@thetrust.com", null },
                    new[] { 0.8f, 0.2f }) // Other governors usually but not always have an email address
        });
    }
}
