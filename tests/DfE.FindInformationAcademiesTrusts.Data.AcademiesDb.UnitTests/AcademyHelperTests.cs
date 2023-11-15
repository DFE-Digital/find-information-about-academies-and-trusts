using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests;

public class AcademyHelperTests
{
    private readonly AcademyHelper _sut = new();

    [Fact]
    public void CreateAcademyFrom_should_transform_an_establishment_into_an_academy()
    {
        var establishment = new Establishment
        {
            Urn = 1234,
            EstablishmentName = "trust 1",
            LaName = "my la",
            UrbanRuralName = "urban",
            PhaseOfEducationName = "Primary",
            NumberOfPupils = "999",
            SchoolCapacity = "1000",
            PercentageFsm = "30.40",
            StatutoryLowAge = "5",
            StatutoryHighAge = "11",
            OfstedLastInsp = "16-11-2023",
            OfstedRatingName = "Good"
        };

        var result = _sut.CreateAcademyFrom(new GroupLink(), establishment);

        result.Should().BeEquivalentTo(new Academy(
                1234,
                "trust 1",
                "my la",
                "urban",
                "Primary",
                "999",
                "1000",
                "30.40",
                new AgeRange(5, 11),
                new OfstedRating("Good", new DateTime(2023, 11, 16)),
                null
            )
        );
    }
}
