using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Factories;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Gias;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Mis;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests.Factories;

public class AcademyFactoryTests
{
    private readonly AcademyFactory _sut = new();

    private readonly GiasEstablishment _giasEstablishment = new()
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
        StatutoryHighAge = "11"
    };

    private readonly GiasGroupLink _giasGroupLink = new() { JoinedDate = "16/11/2023" };

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
            StatutoryHighAge = "11"
        };

        var result = _sut.CreateAcademyFrom(_giasGroupLink, giasEstablishment);

        result.Urn.Should().Be(1234);
        result.DateAcademyJoinedTrust.Should().Be(new DateTime(2023, 11, 16));
        result.EstablishmentName.Should().Be("trust 1");
        result.TypeOfEstablishment.Should().Be("Academy sponsor led");
        result.LocalAuthority.Should().Be("my la");
        result.UrbanRural.Should().Be("urban");
        result.PhaseOfEducation.Should().Be("Primary");
        result.NumberOfPupils.Should().Be(999);
        result.SchoolCapacity.Should().Be(1000);
        result.PercentageFreeSchoolMeals.Should().Be("30.40");
        result.AgeRange.Should().Be(new AgeRange(5, 11));
    }

    [Fact]
    public void CreateAcademyFrom_should_set_DateAcademyJoinedTrust_from_giasGroupLink()
    {
        var groupLink = new GiasGroupLink { JoinedDate = "12/01/2022" };
        var result = _sut.CreateAcademyFrom(groupLink, _giasEstablishment);

        result.DateAcademyJoinedTrust.Should().Be(new DateTime(2022, 1, 12));
    }

    [Fact]
    public void CreateAcademyFrom_should_set_current_ofsted_to_none_if_no_misEstablishment()
    {
        var result = _sut.CreateAcademyFrom(_giasGroupLink, _giasEstablishment);

        result.CurrentOfstedRating.OfstedRatingScore.Should().Be(OfstedRatingScore.None);
        result.CurrentOfstedRating.InspectionEndDate.Should().BeNull();
    }

    [Theory]
    [InlineData(1, OfstedRatingScore.Outstanding)]
    [InlineData(2, OfstedRatingScore.Good)]
    [InlineData(3, OfstedRatingScore.RequiresImprovement)]
    [InlineData(4, OfstedRatingScore.Inadequate)]
    public void CreateAcademyFrom_should_create_current_ofsted_rating_score_from_misEstablishment(int score,
        OfstedRatingScore expectedScore)
    {
        var misEstablishment = new MisEstablishment
        {
            UrnAtTimeOfLatestFullInspection = _giasEstablishment.Urn,
            InspectionEndDate = "11/05/2022",
            OverallEffectiveness = score
        };

        var result = _sut.CreateAcademyFrom(_giasGroupLink, _giasEstablishment, misEstablishment);

        result.CurrentOfstedRating.Should().NotBeNull();
        result.CurrentOfstedRating.OfstedRatingScore.Should().Be(expectedScore);
    }

    [Theory]
    [InlineData(2022, 12, 1)]
    [InlineData(2022, 1, 12)]
    [InlineData(2023, 6, 6)]
    [InlineData(2020, 2, 29)]
    public void CreateAcademyFrom_should_create_current_ofsted_rating_date_from_misEstablishment(int year, int month,
        int day)
    {
        var misEstablishment = new MisEstablishment
        {
            UrnAtTimeOfLatestFullInspection = _giasEstablishment.Urn,
            InspectionEndDate = $"{day:00}/{month:00}/{year}",
            OverallEffectiveness = 1
        };

        var result = _sut.CreateAcademyFrom(_giasGroupLink, _giasEstablishment, misEstablishment);

        result.CurrentOfstedRating.Should().NotBeNull();
        result.CurrentOfstedRating.InspectionEndDate.Should().Be(new DateTime(year, month, day));
    }
}
