using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Gias;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Tad;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Repositories;
using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.School;
using Microsoft.Extensions.Logging;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests.Repositories;

public class SchoolRepositoryTests
{
    private readonly SchoolRepository _sut;
    private readonly MockAcademiesDbContext _mockAcademiesDbContext = new();

    private readonly IStringFormattingUtilities _stringFormattingUtilities = new StringFormattingUtilities();
    private readonly ILogger<SchoolRepository> _mockLogger = MockLogger.CreateLogger<SchoolRepository>();

    public SchoolRepositoryTests()
    {
        _sut = new SchoolRepository(_mockAcademiesDbContext.Object, _stringFormattingUtilities, _mockLogger);
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
                EstablishmentTypeGroupName = typeGroup,
                EstablishmentStatusName = "Open"
            },
            new GiasEstablishment
            {
                Urn = urn + 1,
                EstablishmentName = "Different school",
                TypeOfEstablishmentName = type,
                EstablishmentTypeGroupName = typeGroup,
                EstablishmentStatusName = "Open"
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
            EstablishmentTypeGroupName = typeGroup,
            EstablishmentStatusName = "Open"
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
                NurseryProvisionName = "None",
                EstablishmentStatusName = "Open"
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

    [Fact]
    public async Task GetSchoolContactsAsync_should_return_headteacher_from_tad()
    {
        var urn = 45678;

        _mockAcademiesDbContext.TadHeadTeacherContacts.Add(new TadHeadTeacherContact
        {
            Urn = urn, HeadFirstName = "Teacher", HeadLastName = "McTeacherson", HeadEmail = "a.teacher@school.com"
        });

        var result = await _sut.GetSchoolContactsAsync(urn);

        result.Should().NotBeNull();
        result!.Name.Should().Be("Teacher McTeacherson");
        result.Email.Should().Be("a.teacher@school.com");
    }

    [Fact]
    public async Task GetSchoolContactsAsync_IfContactDoesNotExist_ShouldLogAndReturnNull()
    {
        var notFoundUrn = 1234;

        var result = await _sut.GetSchoolContactsAsync(notFoundUrn);

        result.Should().BeNull();
        _mockLogger.VerifyLogErrors($"Unable to find head teacher contact for school with URN {notFoundUrn}");
    }

    [Fact]
    public async Task GetSchoolSenProvisionAsync_should_return_sen_provision()
    {
        var urn = 123456;

        _mockAcademiesDbContext.GiasEstablishments.AddRange(
        [
            new GiasEstablishment
            {
                Urn = urn,
                EstablishmentName = "cool school",
                EstablishmentTypeGroupName = "Local authority maintained schools",
                EstablishmentStatusName = "Open",
                ResourcedProvisionOnRoll = "2",
                ResourcedProvisionCapacity = "3",
                SenUnitOnRoll = "22",
                SenUnitCapacity = "4",
                TypeOfResourcedProvisionName = "Resourced",
                Sen1Name = "Sen1",
                Sen2Name = "Sen2",
                Sen3Name = "Sen3",
                Sen4Name = "Sen4",
                Sen5Name = "Sen5",
                Sen6Name = "Sen6",
                Sen7Name = "Sen7",
                Sen8Name = "Sen8",
                Sen9Name = "Sen9",
                Sen10Name = "Sen10",
                Sen11Name = "Sen11",
                Sen12Name = "Sen12",
                Sen13Name = "Sen13"
            }
        ]);

        var result = await _sut.GetSchoolSenProvisionAsync(urn);

        result.Should().BeEquivalentTo(new SenProvision("2", "3", "22",
            "4", "Resourced", new List<string>
            {
                "Sen1", "Sen2", "Sen3", "Sen4", "Sen5", "Sen6", "Sen7", "Sen8", "Sen9", "Sen10", "Sen11", "Sen12",
                "Sen13"
            }));
    }

    [Fact]
    public async Task GetSchoolFederationDetailsAsync_should_return_correct_values()
    {
        var urn = 123456;
        var openedDate = "24/05/2024";
        var federationsCode = "12345";

        _mockAcademiesDbContext.GiasEstablishments.AddRange(
        [
            new GiasEstablishment
            {
                Urn = urn,
                EstablishmentStatusName = "Open",
                EstablishmentName = "cool school",
                EstablishmentTypeGroupName = "Local authority maintained schools",
                FederationsName = "Funky Federation",
                FederationsCode = federationsCode
            },
            new GiasEstablishment
            {
                Urn = urn + 1,
                EstablishmentStatusName = "Open",
                EstablishmentName = "super school",
                EstablishmentTypeGroupName = "Local authority maintained schools",
                FederationsName = "Funky Federation",
                FederationsCode = federationsCode
            },
            new GiasEstablishment
            {
                Urn = urn + 2,
                EstablishmentStatusName = "Open",
                EstablishmentName = "amazing school",
                EstablishmentTypeGroupName = "Local authority maintained schools",
                FederationsName = "Funky Federation",
                FederationsCode = federationsCode
            }
        ]);

        _mockAcademiesDbContext.GiasGroupLinks.AddRange(
        [
            new GiasGroupLink
            {
                Urn = urn.ToString(),
                GroupUid = federationsCode,
                GroupStatusCode = "OPEN",
                OpenDate = openedDate
            }
        ]);

        var result = await _sut.GetSchoolFederationDetailsAsync(urn);
        result.FederationName.Should().BeEquivalentTo("Funky Federation");
        result.FederationUid.Should().BeEquivalentTo(federationsCode);
        result.OpenedOnDate.Should().Be(new DateOnly(2024, 05, 24));
        result.Schools.Should().BeEquivalentTo(new Dictionary<string, string>
        {
            { urn.ToString(), "cool school" },
            { (urn + 1).ToString(), "super school" },
            { (urn + 2).ToString(), "amazing school" }
        });
    }

    [Fact]
    public async Task GetSchoolFederationDetailsAsync_should_return_null_values_if_no_federation()
    {
        var urn = 123456;

        _mockAcademiesDbContext.GiasEstablishments.AddRange(
        [
            new GiasEstablishment
            {
                Urn = urn,
                EstablishmentStatusName = "Open",
                EstablishmentName = "cool school",
                EstablishmentTypeGroupName = "Local authority maintained schools"
            }
        ]);

        var result = await _sut.GetSchoolFederationDetailsAsync(urn);
        result.Should().BeEquivalentTo(new FederationDetails(null, null));
    }
}
