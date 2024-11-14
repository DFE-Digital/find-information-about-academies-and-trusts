using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.FiatDb.Repositories;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.Academy;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.Trust;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Services;

public class TrustServiceGovernanceTurnoverTests
{
    private readonly TrustService _sut;
    private readonly Mock<IAcademyRepository> _mockAcademyRepository = new();
    private readonly Mock<ITrustRepository> _mockTrustRepository = new();
    private readonly Mock<IContactRepository> _mockContactRepository = new();
    private readonly Mock<IDateTimeProvider> _mockDateTimeProvider = new();
    private readonly MockMemoryCache _mockMemoryCache = new();

    public TrustServiceGovernanceTurnoverTests()
    {
        _sut = new TrustService(_mockAcademyRepository.Object,
                                _mockTrustRepository.Object,
                                _mockContactRepository.Object,
                                _mockMemoryCache.Object,
                                _mockDateTimeProvider.Object);
    }

    private Governor CreateGovernor(
        string role,
        DateTime? dateOfAppointment,
        DateTime? dateOfTermEnd)
    {
        return new Governor(
            GID: Guid.NewGuid().ToString(),
            UID: Guid.NewGuid().ToString(),
            FullName: "Test Governor",
            Role: role,
            AppointingBody: "Test Body",
            DateOfAppointment: dateOfAppointment,
            DateOfTermEnd: dateOfTermEnd,
            Email: "test@example.com");
    }

    [Fact]
    public void CalculateTurnoverRate_WhenTotalCurrentGovernorsIsZero_ReturnsZero()
    {
        var trustGovernance = new TrustGovernance(
            CurrentTrustLeadership: Array.Empty<Governor>(),
            CurrentMembers: Array.Empty<Governor>(),
            CurrentTrustees: Array.Empty<Governor>(),
            HistoricMembers: Array.Empty<Governor>()
        );

        var today = new DateTime(2023, 10, 1);
        _mockDateTimeProvider.Setup(d => d.Today).Returns(today);

        var result = _sut.CalculateTurnoverRate(trustGovernance);

        result.Should().Be(0m);
    }

    [Fact]
    public void CalculateTurnoverRate_NoAppointmentsOrResignations_ReturnsZero()
    {
        var today = new DateTime(2023, 10, 1);
        _mockDateTimeProvider.Setup(d => d.Today).Returns(today);

        var currentTrustees = new List<Governor>
            {
                CreateGovernor("Trustee", today.AddYears(-2), null)
            };

        var trustGovernance = new TrustGovernance(
            CurrentTrustLeadership: Array.Empty<Governor>(),
            CurrentMembers: Array.Empty<Governor>(),
            CurrentTrustees: currentTrustees.ToArray(),
            HistoricMembers: Array.Empty<Governor>()
        );

        var result = _sut.CalculateTurnoverRate(trustGovernance);

        result.Should().Be(0m);
    }

    [Fact]
    public void CalculateTurnoverRate_WithAppointments_ReturnsCorrectRate()
    {
        var today = new DateTime(2023, 10, 1);
        _mockDateTimeProvider.Setup(d => d.Today).Returns(today);

        var currentTrustees = new List<Governor>
            {
                CreateGovernor("Trustee", today.AddMonths(-6), null),
                CreateGovernor("Trustee", today.AddMonths(-13), null)
            };

        var trustGovernance = new TrustGovernance(
            CurrentTrustLeadership: Array.Empty<Governor>(),
            CurrentMembers: Array.Empty<Governor>(),
            CurrentTrustees: currentTrustees.ToArray(),
            HistoricMembers: Array.Empty<Governor>()
        );

        var result = _sut.CalculateTurnoverRate(trustGovernance);

        result.Should().Be(50.0m);
    }

    [Fact]
    public void CalculateTurnoverRate_WithResignations_ReturnsCorrectRate()
    {
        var today = new DateTime(2023, 10, 1);
        _mockDateTimeProvider.Setup(d => d.Today).Returns(today);

        var historicMembers = new List<Governor>
            {
                CreateGovernor("Trustee", today.AddYears(-2), today.AddMonths(-6))
            };

        var currentTrustees = new List<Governor>
            {
                CreateGovernor("Trustee", today.AddYears(-3), null)
            };

        var trustGovernance = new TrustGovernance(
            CurrentTrustLeadership: Array.Empty<Governor>(),
            CurrentMembers: Array.Empty<Governor>(),
            CurrentTrustees: currentTrustees.ToArray(),
            HistoricMembers: historicMembers.ToArray()
        );

        var result = _sut.CalculateTurnoverRate(trustGovernance);

        result.Should().Be(100.0m);
    }

    [Fact]
    public void CalculateTurnoverRate_WithAppointmentsAndResignations_ReturnsCorrectRate()
    {
        var today = new DateTime(2023, 10, 1);
        _mockDateTimeProvider.Setup(d => d.Today).Returns(today);

        var currentTrustees = new List<Governor>
            {
                CreateGovernor("Trustee", today.AddMonths(-6), null),
                CreateGovernor("Trustee", today.AddYears(-2), null)
            };

        var historicMembers = new List<Governor>
            {
                CreateGovernor("Trustee", today.AddYears(-3), today.AddMonths(-4))
            };

        var trustGovernance = new TrustGovernance(
            CurrentTrustLeadership: Array.Empty<Governor>(),
            CurrentMembers: Array.Empty<Governor>(),
            CurrentTrustees: currentTrustees.ToArray(),
            HistoricMembers: historicMembers.ToArray()
        );

        var result = _sut.CalculateTurnoverRate(trustGovernance);

        result.Should().Be(100.0m);
    }

    [Fact]
    public void CalculateTurnoverRate_ExcludesLeadershipRoles()
    {
        var today = new DateTime(2023, 10, 1);
        _mockDateTimeProvider.Setup(d => d.Today).Returns(today);

        var leadershipGovernor = CreateGovernor("Chair of Trustees", today.AddMonths(-6), null);
        var trusteeGovernor = CreateGovernor("Trustee", today.AddMonths(-6), null);

        var trustGovernance = new TrustGovernance(
            CurrentTrustLeadership: new[] { leadershipGovernor },
            CurrentMembers: Array.Empty<Governor>(),
            CurrentTrustees: new[] { leadershipGovernor, trusteeGovernor },
            HistoricMembers: Array.Empty<Governor>()
        );

        var result = _sut.CalculateTurnoverRate(trustGovernance);

        result.Should().Be(50.0m);
    }

    [Fact]
    public void CalculateTurnoverRate_AppointmentOnPast12MonthsStartDate_Included()
    {
        var today = new DateTime(2023, 10, 1);
        var past12MonthsStart = today.AddYears(-1);
        _mockDateTimeProvider.Setup(d => d.Today).Returns(today);

        var currentTrustees = new List<Governor>
            {
                CreateGovernor("Trustee", past12MonthsStart, null)
            };

        var trustGovernance = new TrustGovernance(
            CurrentTrustLeadership: Array.Empty<Governor>(),
            CurrentMembers: Array.Empty<Governor>(),
            CurrentTrustees: currentTrustees.ToArray(),
            HistoricMembers: Array.Empty<Governor>()
        );

        var result = _sut.CalculateTurnoverRate(trustGovernance);

        result.Should().Be(100.0m);
    }

    [Fact]
    public void CalculateTurnoverRate_AppointmentOnTodayDate_Included()
    {
        var today = new DateTime(2023, 10, 1);
        _mockDateTimeProvider.Setup(d => d.Today).Returns(today);

        var currentTrustees = new List<Governor>
            {
                CreateGovernor("Trustee", today, null)
            };

        var trustGovernance = new TrustGovernance(
            CurrentTrustLeadership: Array.Empty<Governor>(),
            CurrentMembers: Array.Empty<Governor>(),
            CurrentTrustees: currentTrustees.ToArray(),
            HistoricMembers: Array.Empty<Governor>()
        );

        var result = _sut.CalculateTurnoverRate(trustGovernance);

        result.Should().Be(100.0m);
    }

    [Fact]
    public void CalculateTurnoverRate_AppointmentBeforePast12MonthsStartDate_Excluded()
    {
        var today = new DateTime(2023, 10, 1);
        var past12MonthsStart = today.AddYears(-1);
        _mockDateTimeProvider.Setup(d => d.Today).Returns(today);

        var currentTrustees = new List<Governor>
            {
                CreateGovernor("Trustee", past12MonthsStart.AddDays(-1), null)
            };

        var trustGovernance = new TrustGovernance(
            CurrentTrustLeadership: Array.Empty<Governor>(),
            CurrentMembers: Array.Empty<Governor>(),
            CurrentTrustees: currentTrustees.ToArray(),
            HistoricMembers: Array.Empty<Governor>()
        );

        var result = _sut.CalculateTurnoverRate(trustGovernance);

        result.Should().Be(0.0m);
    }

    [Fact]
    public void CalculateTurnoverRate_RoundsToOneDecimalPlace()
    {
        var today = new DateTime(2023, 10, 1);
        _mockDateTimeProvider.Setup(d => d.Today).Returns(today);

        var currentTrustees = new List<Governor>
            {
                CreateGovernor("Trustee", today.AddMonths(-1), null),
                CreateGovernor("Trustee", today.AddMonths(-2), null),
                CreateGovernor("Trustee", today.AddMonths(-13), null),
                CreateGovernor("Trustee", today.AddMonths(-14), null),
                CreateGovernor("Trustee", today.AddMonths(-15), null),
            };

        var trustGovernance = new TrustGovernance(
            CurrentTrustLeadership: Array.Empty<Governor>(),
            CurrentMembers: Array.Empty<Governor>(),
            CurrentTrustees: currentTrustees.ToArray(),
            HistoricMembers: Array.Empty<Governor>()
        );

        var result = _sut.CalculateTurnoverRate(trustGovernance);

        result.Should().Be(40.0m);
    }
}