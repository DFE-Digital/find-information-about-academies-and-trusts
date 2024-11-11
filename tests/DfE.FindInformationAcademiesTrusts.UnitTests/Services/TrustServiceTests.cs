using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Data.FiatDb.Repositories;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.Academy;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.Contacts;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.Trust;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Services;

public class TrustServiceTests
{
    private readonly TrustService _sut;
    private readonly Mock<IAcademyRepository> _mockAcademyRepository = new();
    private readonly Mock<ITrustRepository> _mockTrustRepository = new();
    private readonly Mock<IContactRepository> _mockContactRepository = new();
    private readonly Mock<IDateTimeProvider> _mockDateTimeProvider = new();
    private readonly MockMemoryCache _mockMemoryCache = new();

    public TrustServiceTests()
    {
        _sut = new TrustService(_mockAcademyRepository.Object,
                                _mockTrustRepository.Object,
                                _mockContactRepository.Object,
                                _mockMemoryCache.Object,
                                _mockDateTimeProvider.Object);
    }

    [Fact]
    public async Task GetTrustSummaryAsync_cached_should_return_cached_result()
    {
        var uid = "1234";
        var key = $"{nameof(TrustService)}:{uid}";
        var cachedResult = new TrustSummaryServiceModel(uid, "My Trust", "Multi-academy trust", 3);
        _mockMemoryCache.AddMockCacheEntry(key, cachedResult);

        var result = await _sut.GetTrustSummaryAsync(uid);
        result.Should().Be(cachedResult);

        _mockTrustRepository.Verify(t => t.GetTrustSummaryAsync(uid), Times.Never);
        _mockAcademyRepository.Verify(a => a.GetNumberOfAcademiesInTrustAsync(uid), Times.Never);
    }

    [Fact]
    public async Task GetTrustSummaryAsync_should_return_null_if_no_giasGroup_found()
    {
        _mockTrustRepository.Setup(t => t.GetTrustSummaryAsync("this uid doesn't exist"))
            .ReturnsAsync((TrustSummary?)null);

        var result = await _sut.GetTrustSummaryAsync("this uid doesn't exist");
        result.Should().BeNull();
    }

    [Theory]
    [InlineData("2806", "My Trust", "Multi-academy trust", 3)]
    [InlineData("9008", "Another Trust", "Single-academy trust", 1)]
    [InlineData("9008", "Trust with no academies", "Multi-academy trust", 0)]
    public async Task GetTrustSummaryAsync_should_return_trustSummary_if_found(string uid, string name, string type,
        int numAcademies)
    {
        _mockTrustRepository.Setup(t => t.GetTrustSummaryAsync(uid)).ReturnsAsync(new TrustSummary(name, type));
        _mockAcademyRepository.Setup(a => a.GetNumberOfAcademiesInTrustAsync(uid))
            .ReturnsAsync(numAcademies);

        var result = await _sut.GetTrustSummaryAsync(uid);
        result.Should().BeEquivalentTo(new TrustSummaryServiceModel(uid, name, type, numAcademies));
    }

    [Theory]
    [InlineData("2806", "My Trust", "Multi-academy trust", 3)]
    [InlineData("9008", "Another Trust", "Single-academy trust", 1)]
    [InlineData("9008", "Trust with no academies", "Multi-academy trust", 0)]
    public async Task GetTrustSummaryAsync_uncached_should_cache_result(string uid, string name, string type,
        int numAcademies)
    {
        var key = $"{nameof(TrustService)}:{uid}";

        _mockTrustRepository.Setup(t => t.GetTrustSummaryAsync(uid))
            .ReturnsAsync(new TrustSummary(name, type));
        _mockAcademyRepository.Setup(a => a.GetNumberOfAcademiesInTrustAsync(uid))
            .ReturnsAsync(numAcademies);

        await _sut.GetTrustSummaryAsync(uid);

        _mockMemoryCache.Verify(m => m.CreateEntry(key), Times.Once);

        var cachedEntry = _mockMemoryCache.MockCacheEntries[key];

        cachedEntry.Value.Should().BeEquivalentTo(new TrustSummaryServiceModel(uid, name, type, numAcademies));
        cachedEntry.SlidingExpiration.Should().Be(TimeSpan.FromMinutes(10));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("123456")]
    [InlineData("567890")]
    public async Task GetTrustDetailsAsync_should_get_singleAcademyTrustAcademyUrn_from_academy_repository(
        string? singleAcademyTrustAcademyUrn)
    {
        _mockAcademyRepository.Setup(a => a.GetSingleAcademyTrustAcademyUrnAsync("2806"))
            .ReturnsAsync(singleAcademyTrustAcademyUrn);
        _mockTrustRepository.Setup(t => t.GetTrustDetailsAsync("2806"))
            .ReturnsAsync(new TrustDetails("2806",
                "TR0012",
                "10012345",
                "123456",
                "Multi-academy trust",
                "123 Fairyland Drive, Gotham, GT12 1AB",
                "Oxfordshire",
                new DateTime(2007, 6, 28)));

        var result = await _sut.GetTrustDetailsAsync("2806");

        result.SingleAcademyTrustAcademyUrn.Should().Be(singleAcademyTrustAcademyUrn);
    }

    [Fact]
    public async Task GetTrustDetailsAsync_should_set_properties_from_TrustRepo()
    {
        _mockTrustRepository.Setup(t => t.GetTrustDetailsAsync("2806"))
            .ReturnsAsync(new TrustDetails("2806",
                "TR0012",
                "10012345",
                "123456",
                "Multi-academy trust",
                "123 Fairyland Drive, Gotham, GT12 1AB",
                "Oxfordshire",
                new DateTime(2007, 6, 28)));

        var result = await _sut.GetTrustDetailsAsync("2806");

        result.Should().BeEquivalentTo(new TrustDetailsServiceModel("2806",
            "TR0012",
            "10012345",
            "123456",
            "Multi-academy trust",
            "123 Fairyland Drive, Gotham, GT12 1AB",
            "Oxfordshire",
            null,
            new DateTime(2007, 6, 28)
        ));
    }

    [Fact]
    public async Task GetTrustContactsAsync_should_get_governanceResults_for_single_trust()
    {
        var person = new Person("First Middle Last", "firstlast@email.com");
        var contacts = new TrustContacts(person, person, person);
        _mockTrustRepository.Setup(t => t.GetTrustContactsAsync("1234", null))
            .ReturnsAsync(contacts);
        var internalContact =
            new InternalContact("First Middle Last", "firstlast@email.com", DateTime.Today, "Test@email.com");
        var internalContacts = new InternalContacts(internalContact, internalContact);
        _mockContactRepository.Setup(t => t.GetInternalContactsAsync("1234"))
            .ReturnsAsync(internalContacts);

        var result = await _sut.GetTrustContactsAsync("1234");

        result.Should().BeEquivalentTo(contacts);
    }

    [Fact]
    public async Task GetTrustOverviewAsync_returns_correct_overview_for_trust_with_academies()
    {
        // Arrange
        var uid = "1234";
        var academiesOverview = new AcademyOverview[]
        {
            new("1001", "LocalAuthorityA", 500, 600, OfstedRatingScore.Good),
            new("1002", "LocalAuthorityB", 400, 500, OfstedRatingScore.Outstanding),
            new("1003", "LocalAuthorityA", 300, 400, OfstedRatingScore.RequiresImprovement)
        };

        _mockAcademyRepository.Setup(a => a.GetAcademiesInTrustOverviewAsync(uid))
            .ReturnsAsync(academiesOverview);

        // Act
        var result = await _sut.GetTrustOverviewAsync(uid);

        // Assert
        result.Uid.Should().Be(uid);
        result.TotalAcademies.Should().Be(3);
        result.AcademiesByLocalAuthority.Should().BeEquivalentTo(new Dictionary<string, int>
        {
            { "LocalAuthorityA", 2 },
            { "LocalAuthorityB", 1 }
        });
        result.TotalPupilNumbers.Should().Be(500 + 400 + 300);
        result.TotalCapacity.Should().Be(600 + 500 + 400);
        result.OfstedRatings.Should().BeEquivalentTo(new Dictionary<OfstedRatingScore, int>
        {
            { OfstedRatingScore.Good, 1 },
            { OfstedRatingScore.Outstanding, 1 },
            { OfstedRatingScore.RequiresImprovement, 1 }
        });
    }

    [Fact]
    public async Task GetTrustOverviewAsync_returns_zero_values_for_trust_with_no_academies()
    {
        // Arrange
        var uid = "1234";
        var academiesOverview = Array.Empty<AcademyOverview>();

        _mockAcademyRepository.Setup(a => a.GetAcademiesInTrustOverviewAsync(uid))
            .ReturnsAsync(academiesOverview);

        // Act
        var result = await _sut.GetTrustOverviewAsync(uid);

        // Assert
        result.Uid.Should().Be(uid);
        result.TotalAcademies.Should().Be(0);
        result.AcademiesByLocalAuthority.Should().BeEmpty();
        result.TotalPupilNumbers.Should().Be(0);
        result.TotalCapacity.Should().Be(0);
        result.OfstedRatings.Should().BeEmpty();
    }

    [Theory]
    [InlineData(true, true)]
    [InlineData(true, false)]
    [InlineData(false, true)]
    [InlineData(false, false)]
    public async Task UpdateContactsAsync_returns_the_correct_values_changed(bool emailUpdated, bool nameUpdated)
    {
        var expected = new TrustContactUpdated(emailUpdated, nameUpdated);
        _mockContactRepository.Setup(t =>
                t.UpdateInternalContactsAsync(1234, "Name", "Email", ContactRole.SfsoLead))
            .ReturnsAsync(expected);
        var result = await _sut.UpdateContactAsync(1234, "Name", "Email", ContactRole.SfsoLead);

        result.Should().BeEquivalentTo(expected);
    }
    [Fact]
    public async Task GetTrustGovernanceAsync_NoGovernors_ReturnsZeroTurnoverRate()
    {
        // Arrange
        var uid = "1234";
        var urn = "5678";
        var fixedToday = new DateTime(2023, 10, 1);
        _mockDateTimeProvider.Setup(d => d.Today).Returns(fixedToday);

        _mockAcademyRepository.Setup(a => a.GetSingleAcademyTrustAcademyUrnAsync(uid))
            .ReturnsAsync(urn);

        var trustGovernance = new TrustGovernance(
            TrustLeadership: Array.Empty<Governor>(),
            Members: Array.Empty<Governor>(),
            Trustees: Array.Empty<Governor>(),
            HistoricMembers: Array.Empty<Governor>());

        _mockTrustRepository.Setup(t => t.GetTrustGovernanceAsync(uid, urn))
            .ReturnsAsync(trustGovernance);

        // Act
        var result = await _sut.GetTrustGovernanceAsync(uid);

        // Assert
        result.TurnoverRate.Should().Be(0.0m);
    }

    [Fact]
    public async Task GetTrustGovernanceAsync_NoEventsInPast12Months_ReturnsZeroTurnoverRate()
    {
        // Arrange
        var uid = "1234";
        var urn = "5678";
        var fixedToday = new DateTime(2023, 10, 1);
        _mockDateTimeProvider.Setup(d => d.Today).Returns(fixedToday);

        _mockAcademyRepository.Setup(a => a.GetSingleAcademyTrustAcademyUrnAsync(uid))
            .ReturnsAsync(urn);

        var trustees = new[]
        {
                new Governor(
                    GID: "1",
                    UID: "UID1",
                    FullName: "Trustee One",
                    Role: "Trustee",
                    AppointingBody: "Body",
                    DateOfAppointment: fixedToday.AddYears(-2),
                    DateOfTermEnd: null,
                    Email: null)
            };

        var members = new[]
        {
                new Governor(
                    GID: "2",
                    UID: "UID2",
                    FullName: "Member One",
                    Role: "Member",
                    AppointingBody: "Body",
                    DateOfAppointment: fixedToday.AddYears(-3),
                    DateOfTermEnd: null,
                    Email: null)
            };

        var trustGovernance = new TrustGovernance(
            TrustLeadership: Array.Empty<Governor>(),
            Members: members,
            Trustees: trustees,
            HistoricMembers: Array.Empty<Governor>());

        _mockTrustRepository.Setup(t => t.GetTrustGovernanceAsync(uid, urn))
            .ReturnsAsync(trustGovernance);

        // Act
        var result = await _sut.GetTrustGovernanceAsync(uid);

        // Assert
        result.TurnoverRate.Should().Be(0.0m);
    }

    [Fact]
    public async Task GetTrustGovernanceAsync_AppointmentsInPast12Months_CalculatesCorrectTurnoverRate()
    {
        // Arrange
        var uid = "1234";
        var urn = "5678";
        var fixedToday = new DateTime(2023, 10, 1);
        _mockDateTimeProvider.Setup(d => d.Today).Returns(fixedToday);

        _mockAcademyRepository.Setup(a => a.GetSingleAcademyTrustAcademyUrnAsync(uid))
            .ReturnsAsync(urn);

        var trustee = new Governor(
            GID: "1",
            UID: "UID1",
            FullName: "Trustee One",
            Role: "Trustee",
            AppointingBody: "Body",
            DateOfAppointment: fixedToday.AddMonths(-6),
            DateOfTermEnd: null,
            Email: null);

        var trustGovernance = new TrustGovernance(
            TrustLeadership: Array.Empty<Governor>(),
            Members: Array.Empty<Governor>(),
            Trustees: new[] { trustee },
            HistoricMembers: Array.Empty<Governor>());

        _mockTrustRepository.Setup(t => t.GetTrustGovernanceAsync(uid, urn))
            .ReturnsAsync(trustGovernance);

        // Act
        var result = await _sut.GetTrustGovernanceAsync(uid);

        // Expected turnover rate: (1 appointment) / (1 current governor) * 100% = 100%
        // Assert
        result.TurnoverRate.Should().Be(100.0m);
    }

    [Fact]
    public async Task GetTrustGovernanceAsync_AppointmentsAndResignationsInPast12Months_CalculatesCorrectTurnoverRate()
    {
        // Arrange
        var uid = "1234";
        var urn = "5678";
        var fixedToday = new DateTime(2023, 10, 1);
        _mockDateTimeProvider.Setup(d => d.Today).Returns(fixedToday);

        _mockAcademyRepository.Setup(a => a.GetSingleAcademyTrustAcademyUrnAsync(uid))
            .ReturnsAsync(urn);

        var newTrustee = new Governor(
            GID: "1",
            UID: "UID1",
            FullName: "New Trustee",
            Role: "Trustee",
            AppointingBody: "Body",
            DateOfAppointment: fixedToday.AddMonths(-6),
            DateOfTermEnd: null,
            Email: null);

        var resignedTrustee = new Governor(
            GID: "2",
            UID: "UID2",
            FullName: "Resigned Trustee",
            Role: "Trustee",
            AppointingBody: "Body",
            DateOfAppointment: fixedToday.AddYears(-2),
            DateOfTermEnd: fixedToday.AddMonths(-2),
            Email: null);

        var trustGovernance = new TrustGovernance(
            TrustLeadership: Array.Empty<Governor>(),
            Members: Array.Empty<Governor>(),
            Trustees: new[] { newTrustee },
            HistoricMembers: new[] { resignedTrustee });

        _mockTrustRepository.Setup(t => t.GetTrustGovernanceAsync(uid, urn))
            .ReturnsAsync(trustGovernance);

        // Act
        var result = await _sut.GetTrustGovernanceAsync(uid);

        // Total current governors: 1
        // Total events: 1 appointment + 1 resignation = 2
        // Expected turnover rate: (2 / 1) * 100% = 200%
        // Assert
        result.TurnoverRate.Should().Be(200.0m);
    }

    [Fact]
    public async Task GetTrustGovernanceAsync_ExcludesLeadershipRoles()
    {
        // Arrange
        var uid = "1234";
        var urn = "5678";
        var fixedToday = new DateTime(2023, 10, 1);
        _mockDateTimeProvider.Setup(d => d.Today).Returns(fixedToday);

        _mockAcademyRepository.Setup(a => a.GetSingleAcademyTrustAcademyUrnAsync(uid))
            .ReturnsAsync(urn);

        var chair = new Governor(
            GID: "1",
            UID: "UID1",
            FullName: "Chair of Trustees",
            Role: "Chair of Trustees",
            AppointingBody: "Body",
            DateOfAppointment: fixedToday.AddYears(-2),
            DateOfTermEnd: null,
            Email: null);

        var trustee = new Governor(
            GID: "2",
            UID: "UID2",
            FullName: "Trustee One",
            Role: "Trustee",
            AppointingBody: "Body",
            DateOfAppointment: fixedToday.AddMonths(-8),
            DateOfTermEnd: null,
            Email: null);

        var trustGovernance = new TrustGovernance(
            TrustLeadership: new[] { chair },
            Members: Array.Empty<Governor>(),
            Trustees: new[] { trustee },
            HistoricMembers: Array.Empty<Governor>());

        _mockTrustRepository.Setup(t => t.GetTrustGovernanceAsync(uid, urn))
            .ReturnsAsync(trustGovernance);

        // Act
        var result = await _sut.GetTrustGovernanceAsync(uid);

        // Total current governors: 1 (excludes chair)
        // Total events: 1 appointment (trustee)
        // Expected turnover rate: (1 / 1) * 100% = 100%
        // Assert
        result.TurnoverRate.Should().Be(100.0m);
    }

    [Fact]
    public async Task GetTrustGovernanceAsync_NullDates_AreHandledGracefully()
    {
        // Arrange
        var uid = "1234";
        var urn = "5678";
        var fixedToday = new DateTime(2023, 10, 1);
        _mockDateTimeProvider.Setup(d => d.Today).Returns(fixedToday);

        _mockAcademyRepository.Setup(a => a.GetSingleAcademyTrustAcademyUrnAsync(uid))
            .ReturnsAsync(urn);

        var trustee = new Governor(
            GID: "1",
            UID: "UID1",
            FullName: "Trustee One",
            Role: "Trustee",
            AppointingBody: "Body",
            DateOfAppointment: null,
            DateOfTermEnd: null,
            Email: null);

        var trustGovernance = new TrustGovernance(
            TrustLeadership: Array.Empty<Governor>(),
            Members: Array.Empty<Governor>(),
            Trustees: new[] { trustee },
            HistoricMembers: Array.Empty<Governor>());

        _mockTrustRepository.Setup(t => t.GetTrustGovernanceAsync(uid, urn))
            .ReturnsAsync(trustGovernance);

        // Act
        var result = await _sut.GetTrustGovernanceAsync(uid);

        // Expected turnover rate: 0% (since no events)
        // Assert
        result.TurnoverRate.Should().Be(0.0m);
    }
}
