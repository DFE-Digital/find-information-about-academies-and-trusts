using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Gias;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Repositories;
using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.School;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests.Repositories;

public class SchoolRepositoryTests
{
    private readonly SchoolRepository _sut;
    private readonly MockAcademiesDbContext _mockAcademiesDbContext = new();

    private readonly IStringFormattingUtilities _stringFormattingUtilities = new StringFormattingUtilities();

    public SchoolRepositoryTests()
    {
        _sut = new SchoolRepository(_mockAcademiesDbContext.Object, _stringFormattingUtilities);
    }

    [Fact]
    public async Task GetSchoolSummaryAsync_should_return_null_if_not_found()
    {
        var result = await _sut.GetSchoolSummaryAsync(999999);
        result.Should().BeNull();
    }

    [Theory]
    [InlineData(123456, "Academy converter", "Academies", SchoolCategory.Academy)]
    [InlineData(234567, "Sixth form centres", "Colleges", SchoolCategory.LaMaintainedSchool)]
    [InlineData(345678, "Free schools special", "Free Schools", SchoolCategory.LaMaintainedSchool)]
    [InlineData(456789, "Foundation school", "Local authority maintained schools", SchoolCategory.LaMaintainedSchool)]
    [InlineData(456789, "Non-maintained special school", "Special schools", SchoolCategory.LaMaintainedSchool)]
    public async Task GetSchoolSummaryAsync_should_return_schoolSummary_if_found(int urn, string type, string typeGroup,
        SchoolCategory expectedCategory)
    {
        var name = $"School {urn}";
        _mockAcademiesDbContext.GiasEstablishments.AddRange(
        [
            new GiasEstablishment
            {
                Urn = urn,
                EstablishmentName = name,
                TypeOfEstablishmentName = type,
                EstablishmentTypeGroupName = typeGroup
            },
            new GiasEstablishment
            {
                Urn = urn + 1,
                EstablishmentName = "Different school",
                TypeOfEstablishmentName = type,
                EstablishmentTypeGroupName = typeGroup
            }
        ]);

        var result = await _sut.GetSchoolSummaryAsync(urn);
        result.Should().BeEquivalentTo(new SchoolSummary(name, type, expectedCategory));
    }

    [Theory]
    [InlineData("City technology college", "Independent schools")]
    [InlineData("Online provider", "Online provider")]
    [InlineData("Miscellaneous", "Other types")]
    [InlineData("Higher education institutions", "Universities")]
    public async Task GetSchoolSummaryAsync_should_not_return_schoolSummarys_for_unsupported_establishment_types(
        string type,
        string typeGroup)
    {
        _mockAcademiesDbContext.GiasEstablishments.Add(new GiasEstablishment
        {
            Urn = 123456,
            EstablishmentName = "Unsupported Establishment",
            TypeOfEstablishmentName = type,
            EstablishmentTypeGroupName = typeGroup
        });

        var result = await _sut.GetSchoolSummaryAsync(123456);
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetSchoolDetailsAsync_should_return_school_details()
    {
        var urn = 123456;

        _mockAcademiesDbContext.GiasEstablishments.AddRange(
        [
            new GiasEstablishment
            {
                Urn = urn,
                TypeOfEstablishmentName = "Foundation school",
                EstablishmentTypeGroupName = "Local authority maintained schools",
                EstablishmentName = "cool school",
                Street = "1st line",
                Town = "Funky Town",
                Postcode = "BBL 123",
                GorName = "Yorkshire",
                LaName = "Leeds",
                PhaseOfEducationName = "Secondary",
                StatutoryLowAge = "5",
                StatutoryHighAge = "16",
                NurseryProvisionName = "None"
            }
        ]);

        var result = await _sut.GetSchoolDetailsAsync(urn);

        result.Should().BeEquivalentTo(new SchoolDetails("cool school", "1st line, Funky Town, BBL 123", "Yorkshire",
            "Leeds", "Secondary", new AgeRange(5, 16), "None"));
    }

    [Fact]
    public async Task GetDateJoinedTrust_should_return_correct_date()
    {
        var urn = 45678;
        var joinedDate = "24/05/2024";
        var expectedJoinedDate = new DateOnly(2024, 05, 24);

        _mockAcademiesDbContext.GiasGroupLinks.AddRange(
        [
            new GiasGroupLink
            {
                Urn = urn.ToString(),
                GroupUid = "TR123",
                GroupStatusCode = "OPEN",
                JoinedDate = joinedDate
            }
        ]);

        var result = await _sut.GetDateJoinedTrustAsync(urn);

        result.Should().Be(expectedJoinedDate);
    }
}
