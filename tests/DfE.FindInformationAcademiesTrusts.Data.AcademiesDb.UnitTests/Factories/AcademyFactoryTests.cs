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
        StatutoryHighAge = "11",
        LaCode = "334"
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
            StatutoryHighAge = "11",
            LaCode = "334"
        };

        var result = _sut.CreateFrom(_giasGroupLink, giasEstablishment);

        result.Urn.Should().Be(1234);
        result.DateAcademyJoinedTrust.Should().Be(new DateTime(2023, 11, 16));
        result.EstablishmentName.Should().Be("trust 1");
        result.TypeOfEstablishment.Should().Be("Academy sponsor led");
        result.LocalAuthority.Should().Be("my la");
        result.UrbanRural.Should().Be("urban");
        result.PhaseOfEducation.Should().Be("Primary");
        result.NumberOfPupils.Should().Be(999);
        result.SchoolCapacity.Should().Be(1000);
        result.PercentageFreeSchoolMeals.Should().BeApproximately(30.40, 0.01);
        result.AgeRange.Should().Be(new AgeRange(5, 11));
        result.OldLaCode.Should().Be(334);
    }

    [Fact]
    public void CreateAcademyFrom_should_set_DateAcademyJoinedTrust_from_giasGroupLink()
    {
        var groupLink = new GiasGroupLink { JoinedDate = "12/01/2022" };
        var result = _sut.CreateFrom(groupLink, _giasEstablishment);

        result.DateAcademyJoinedTrust.Should().Be(new DateTime(2022, 1, 12));
    }

    [Fact]
    public void
        CreateAcademyFrom_should_set_current_ofsted_to_none_if_no_misEstablishment_and_no_misFurtherEducationEstablishment()
    {
        var result = _sut.CreateFrom(_giasGroupLink, _giasEstablishment);

        result.CurrentOfstedRating.OfstedRatingScore.Should().Be(OfstedRatingScore.None);
        result.CurrentOfstedRating.InspectionDate.Should().BeNull();
    }

    [Theory]
    [InlineData(1, OfstedRatingScore.Outstanding)]
    [InlineData(2, OfstedRatingScore.Good)]
    [InlineData(3, OfstedRatingScore.RequiresImprovement)]
    [InlineData(4, OfstedRatingScore.Inadequate)]
    public void CreateAcademyFrom_should_create_current_ofsted_rating_score_from_misEstablishment_when_provided(
        int score,
        OfstedRatingScore expectedScore)
    {
        var misEstablishment = new MisEstablishment
        {
            UrnAtTimeOfLatestFullInspection = _giasEstablishment.Urn,
            InspectionEndDate = "11/05/2022",
            OverallEffectiveness = score
        };

        var result = _sut.CreateFrom(_giasGroupLink, _giasEstablishment, misEstablishment);

        result.CurrentOfstedRating.Should().NotBeNull();
        result.CurrentOfstedRating.OfstedRatingScore.Should().Be(expectedScore);
    }

    [Theory]
    [InlineData(2022, 12, 1)]
    [InlineData(2022, 1, 12)]
    [InlineData(2023, 6, 6)]
    [InlineData(2020, 2, 29)]
    public void CreateAcademyFrom_should_create_current_ofsted_rating_date_from_misEstablishment_when_provided(int year,
        int month,
        int day)
    {
        var misEstablishment = new MisEstablishment
        {
            UrnAtTimeOfLatestFullInspection = _giasEstablishment.Urn,
            InspectionEndDate = $"{day:00}/{month:00}/{year}",
            OverallEffectiveness = 1
        };

        var result = _sut.CreateFrom(_giasGroupLink, _giasEstablishment, misEstablishment);

        result.CurrentOfstedRating.Should().NotBeNull();
        result.CurrentOfstedRating.InspectionDate.Should().Be(new DateTime(year, month, day));
    }

    [Theory]
    [InlineData(1, OfstedRatingScore.Outstanding)]
    [InlineData(2, OfstedRatingScore.Good)]
    [InlineData(3, OfstedRatingScore.RequiresImprovement)]
    [InlineData(4, OfstedRatingScore.Inadequate)]
    public void
        CreateAcademyFrom_should_create_current_ofsted_rating_score_from_MisFurtherEducationEstablishment_when_provided(
            int score,
            OfstedRatingScore expectedScore)
    {
        var misFurtherEducationEstablishment = new MisFurtherEducationEstablishment
        {
            ProviderUrn = _giasEstablishment.Urn,
            LastDayOfInspection = "11/05/2022",
            OverallEffectiveness = score
        };

        var result = _sut.CreateFrom(_giasGroupLink, _giasEstablishment,
            misFurtherEducationEstablishment: misFurtherEducationEstablishment);

        result.CurrentOfstedRating.Should().NotBeNull();
        result.CurrentOfstedRating.OfstedRatingScore.Should().Be(expectedScore);
    }

    [Theory]
    [InlineData(2022, 12, 1)]
    [InlineData(2022, 1, 12)]
    [InlineData(2023, 6, 6)]
    [InlineData(2020, 2, 29)]
    public void
        CreateAcademyFrom_should_create_current_ofsted_rating_date_from_MisFurtherEducationEstablishment_when_provided(
            int year, int month,
            int day)
    {
        var misFurtherEducationEstablishment = new MisFurtherEducationEstablishment
        {
            ProviderUrn = _giasEstablishment.Urn,
            LastDayOfInspection = $"{day:00}/{month:00}/{year}",
            OverallEffectiveness = 1
        };

        var result = _sut.CreateFrom(_giasGroupLink, _giasEstablishment,
            misFurtherEducationEstablishment: misFurtherEducationEstablishment);

        result.CurrentOfstedRating.Should().NotBeNull();
        result.CurrentOfstedRating.InspectionDate.Should().Be(new DateTime(year, month, day));
    }

    [Fact]
    public void
        CreateAcademyFrom_should_set_previous_ofsted_to_none_if_no_misEstablishment_and_no_misFurtherEducationEstablishment()
    {
        var result = _sut.CreateFrom(_giasGroupLink, _giasEstablishment);

        result.PreviousOfstedRating.OfstedRatingScore.Should().Be(OfstedRatingScore.None);
        result.PreviousOfstedRating.InspectionDate.Should().BeNull();
    }

    [Theory]
    [InlineData(1, OfstedRatingScore.Outstanding)]
    [InlineData(2, OfstedRatingScore.Good)]
    [InlineData(3, OfstedRatingScore.RequiresImprovement)]
    [InlineData(4, OfstedRatingScore.Inadequate)]
    public void CreateAcademyFrom_should_create_previous_ofsted_rating_score_from_misEstablishment_when_provided(
        int score,
        OfstedRatingScore expectedScore)
    {
        var misEstablishment = new MisEstablishment
        {
            UrnAtTimeOfPreviousFullInspection = _giasEstablishment.Urn,
            PreviousInspectionEndDate = "11/05/2022",
            PreviousFullInspectionOverallEffectiveness = score.ToString()
        };

        var result = _sut.CreateFrom(_giasGroupLink, _giasEstablishment,
            misEstablishmentPreviousOfsted: misEstablishment);

        result.PreviousOfstedRating.Should().NotBeNull();
        result.PreviousOfstedRating.OfstedRatingScore.Should().Be(expectedScore);
    }

    [Theory]
    [InlineData(2022, 12, 1)]
    [InlineData(2022, 1, 12)]
    [InlineData(2023, 6, 6)]
    [InlineData(2020, 2, 29)]
    public void CreateAcademyFrom_should_create_previous_ofsted_rating_date_from_misEstablishment_when_provided(
        int year,
        int month,
        int day)
    {
        var misEstablishment = new MisEstablishment
        {
            UrnAtTimeOfPreviousFullInspection = _giasEstablishment.Urn,
            PreviousInspectionEndDate = $"{day:00}/{month:00}/{year}",
            PreviousFullInspectionOverallEffectiveness = "1"
        };

        var result = _sut.CreateFrom(_giasGroupLink, _giasEstablishment,
            misEstablishmentPreviousOfsted: misEstablishment);

        result.PreviousOfstedRating.Should().NotBeNull();
        result.PreviousOfstedRating.InspectionDate.Should().Be(new DateTime(year, month, day));
    }

    [Theory]
    [InlineData(1, OfstedRatingScore.Outstanding)]
    [InlineData(2, OfstedRatingScore.Good)]
    [InlineData(3, OfstedRatingScore.RequiresImprovement)]
    [InlineData(4, OfstedRatingScore.Inadequate)]
    public void
        CreateAcademyFrom_should_create_previous_ofsted_rating_score_from_MisFurtherEducationEstablishment_when_provided(
            int score,
            OfstedRatingScore expectedScore)
    {
        var misFurtherEducationEstablishment = new MisFurtherEducationEstablishment
        {
            ProviderUrn = _giasEstablishment.Urn,
            PreviousLastDayOfInspection = "11/05/2022",
            PreviousOverallEffectiveness = score
        };

        var result = _sut.CreateFrom(_giasGroupLink, _giasEstablishment,
            misFurtherEducationEstablishment: misFurtherEducationEstablishment);

        result.PreviousOfstedRating.Should().NotBeNull();
        result.PreviousOfstedRating.OfstedRatingScore.Should().Be(expectedScore);
    }

    [Theory]
    [InlineData(2022, 12, 1)]
    [InlineData(2022, 1, 12)]
    [InlineData(2023, 6, 6)]
    [InlineData(2020, 2, 29)]
    public void
        CreateAcademyFrom_should_create_previous_ofsted_rating_date_from_MisFurtherEducationEstablishment_when_provided(
            int year, int month,
            int day)
    {
        var misFurtherEducationEstablishment = new MisFurtherEducationEstablishment
        {
            ProviderUrn = _giasEstablishment.Urn,
            PreviousLastDayOfInspection = $"{day:00}/{month:00}/{year}",
            PreviousOverallEffectiveness = 1
        };

        var result = _sut.CreateFrom(_giasGroupLink, _giasEstablishment,
            misFurtherEducationEstablishment: misFurtherEducationEstablishment);

        result.PreviousOfstedRating.Should().NotBeNull();
        result.PreviousOfstedRating.InspectionDate.Should().Be(new DateTime(year, month, day));
    }
    [Fact]
    public void CreateAcademyFrom_should_set_current_ofsted_inspection_date_to_null_if_InspectionEndDate_is_null_or_empty()
    {
        var misEstablishmentWithNullDate = new MisEstablishment
        {
            UrnAtTimeOfLatestFullInspection = _giasEstablishment.Urn,
            InspectionEndDate = null,
            OverallEffectiveness = 1
        };

        var misEstablishmentWithEmptyDate = new MisEstablishment
        {
            UrnAtTimeOfLatestFullInspection = _giasEstablishment.Urn,
            InspectionEndDate = "",
            OverallEffectiveness = 1
        };

        // Test when InspectionEndDate is null
        var resultWithNullDate = _sut.CreateFrom(_giasGroupLink, _giasEstablishment, misEstablishmentWithNullDate);
        resultWithNullDate.CurrentOfstedRating.InspectionDate.Should().BeNull(); // The date should be null

        // Test when InspectionEndDate is empty
        var resultWithEmptyDate = _sut.CreateFrom(_giasGroupLink, _giasEstablishment, misEstablishmentWithEmptyDate);
        resultWithEmptyDate.CurrentOfstedRating.InspectionDate.Should().BeNull(); // The date should be null
    }

    [Fact]
    public void CreateAcademyFrom_should_set_previous_ofsted_inspection_date_to_null_if_PreviousInspectionEndDate_is_null_or_empty()
    {
        var misEstablishmentWithNullDate = new MisEstablishment
        {
            UrnAtTimeOfPreviousFullInspection = _giasEstablishment.Urn,
            PreviousInspectionEndDate = null,
            PreviousFullInspectionOverallEffectiveness = "1"
        };

        var misEstablishmentWithEmptyDate = new MisEstablishment
        {
            UrnAtTimeOfPreviousFullInspection = _giasEstablishment.Urn,
            PreviousInspectionEndDate = "",
            PreviousFullInspectionOverallEffectiveness = "1"
        };

        // Test when PreviousInspectionEndDate is null
        var resultWithNullDate = _sut.CreateFrom(_giasGroupLink, _giasEstablishment, misEstablishmentWithNullDate);
        resultWithNullDate.PreviousOfstedRating.InspectionDate.Should().BeNull(); // The date should be null

        // Test when PreviousInspectionEndDate is empty
        var resultWithEmptyDate = _sut.CreateFrom(_giasGroupLink, _giasEstablishment, misEstablishmentWithEmptyDate);
        resultWithEmptyDate.PreviousOfstedRating.InspectionDate.Should().BeNull(); // The date should be null
    }

}
