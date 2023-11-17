using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Factories;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Gias;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests.Factories;

public class AcademyFactoryTests
{
    private readonly AcademyFactory _sut = new();

    [Fact]
    public void CreateAcademyFrom_should_transform_a_giasEstablishment_into_an_academy()
    {
        var giasEstablishment = new GiasEstablishment
        {
            Urn = 1234,
            EstablishmentName = "trust 1",
            TypeOfEstablishmentName = "Academy sponsor led",
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

        var result = _sut.CreateAcademyFrom(new GroupLink { JoinedDate = "16/11/2023" }, giasEstablishment);

        result.Should().BeEquivalentTo(new Academy(
                1234,
                new DateTime(2023, 11, 16),
                "trust 1",
                "Academy sponsor led",
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
