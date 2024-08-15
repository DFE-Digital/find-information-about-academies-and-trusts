using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Data.UnitTests.Mocks;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts;

public class OverviewModelTests
{
    private readonly Mock<ITrustProvider> _mockTrustProvider = new();
    private readonly OverviewModel _sut;
    private const string TrustUid = "1234";
    private readonly MockDataSourceService _mockDataSourceService = new();
    private readonly Mock<ITrustService> _mockTrustRepository = new();

    public OverviewModelTests()
    {
        var dummyTrust = DummyTrustFactory.GetDummyTrust(TrustUid);
        MockLogger<OverviewModel> logger = new();

        _mockTrustProvider.Setup(tp => tp.GetTrustByUidAsync(TrustUid)).ReturnsAsync(dummyTrust);
        _mockTrustRepository.Setup(t => t.GetTrustSummaryAsync(dummyTrust.Uid))
            .ReturnsAsync(new TrustSummaryServiceModel(dummyTrust.Uid, dummyTrust.Name, dummyTrust.Type,
                dummyTrust.Academies.Length));
        _sut = new OverviewModel(_mockTrustProvider.Object, _mockDataSourceService.Object, logger.Object,
                _mockTrustRepository.Object)
            { Uid = TrustUid };
    }

    [Fact]
    public void PageName_should_be_Overview()
    {
        _sut.PageName.Should().Be("Overview");
    }

    [Fact]
    public async Task OnGetAsync_sets_pupil_numbers_on_multi_academy_trust()
    {
        SetupTrustWithMultipleAcademies();
        await _sut.OnGetAsync();
        _sut.TotalPupilNumbersInTrust.Should().Be(1125);
    }

    [Fact]
    public async Task OnGetAsync_sets_pupil_numbers_on_single_academy_trust()
    {
        SetupTrustWithSingleAcademy();
        await _sut.OnGetAsync();
        _sut.TotalPupilNumbersInTrust.Should().Be(200);
    }

    [Fact]
    public async Task OnGetAsync_sets_pupil_numbers_on_trust_with_no_academies()
    {
        SetupTrustWithNoAcademies();
        await _sut.OnGetAsync();
        _sut.TotalPupilNumbersInTrust.Should().Be(0);
    }

    [Fact]
    public async Task OnGetAsync_sets_pupil_capacity_on_multi_academy_trust()
    {
        SetupTrustWithMultipleAcademies();
        await _sut.OnGetAsync();
        _sut.TotalPupilCapacityInTrust.Should().Be(1950);
    }

    [Fact]
    public async Task OnGetAsync_sets_pupil_capacity_on_single_academy_trust()
    {
        SetupTrustWithSingleAcademy();
        await _sut.OnGetAsync();
        _sut.TotalPupilCapacityInTrust.Should().Be(250);
    }

    [Fact]
    public async Task OnGetAsync_sets_pupil_capacity_on_trust_with_no_academies()
    {
        SetupTrustWithNoAcademies();
        await _sut.OnGetAsync();
        _sut.TotalPupilCapacityInTrust.Should().Be(0);
    }

    [Fact]
    public async Task OnGetAsync_sets_pupil_percentage_on_multi_academy_trust()
    {
        SetupTrustWithMultipleAcademies();
        await _sut.OnGetAsync();
        _sut.TotalPercentageCapacityInTrust.Should().Be(58);
    }

    [Fact]
    public async Task OnGetAsync_sets_pupil_percentage_on_single_academy_trust()
    {
        SetupTrustWithSingleAcademy();
        await _sut.OnGetAsync();
        _sut.TotalPercentageCapacityInTrust.Should().Be(80);
    }


    [Fact]
    public async Task OnGetAsync_sets_pupil_percentage_on_trust_with_no_academies()
    {
        SetupTrustWithNoAcademies();
        await _sut.OnGetAsync();
        _sut.TotalPercentageCapacityInTrust.Should().BeNull();
    }

    [Fact]
    public async Task OnGetAsync_sets_list_of_local_authorities()
    {
        SetupTrustWithMultipleAcademies();
        var expectedLocalAuthorityCount = new (string? Authority, int Total)[]
        {
            ("localAuth1", 6),
            ("localAuth2", 1)
        };

        await _sut.OnGetAsync();
        var result = _sut.AcademiesInEachLocalAuthority;
        result.Should().BeEquivalentTo(expectedLocalAuthorityCount);
    }

    [Fact]
    public async Task OnGetAsync_sets_ofsted_ratings_list_correctly()
    {
        SetupTrustWithMultipleAcademies();
        var expectedOfstedRatingCount = new (OfstedRatingScore Rating, int Total)[]
        {
            (OfstedRatingScore.Outstanding, 3),
            (OfstedRatingScore.Good, 1),
            (OfstedRatingScore.Inadequate, 1),
            (OfstedRatingScore.RequiresImprovement, 1),
            (OfstedRatingScore.None, 1)
        };

        await _sut.OnGetAsync();
        var result = _sut.OfstedRatings;
        result.Should().BeEquivalentTo(expectedOfstedRatingCount);
    }

    [Fact]
    public async Task OnGetAsync_ofsted_score_count_returns_correct_number_of_ratings()
    {
        SetupTrustWithMultipleAcademies();
        await _sut.OnGetAsync();
        var result = _sut.GetNumberOfAcademiesWithOfstedRating(OfstedRatingScore.Outstanding);
        result.Should().Be(3);
    }

    [Fact]
    public async Task OnGetAsync_ofsted_score_count_returns_zero_ratings_when_non_exist()
    {
        SetupTrustWithSingleAcademy();
        await _sut.OnGetAsync();
        var result = _sut.GetNumberOfAcademiesWithOfstedRating(OfstedRatingScore.Good);
        result.Should().Be(0);
    }

    [Fact]
    public async Task OnGetAsync_returns_correct_number_of_academies_for_single_academy_trust()
    {
        SetupTrustWithSingleAcademy();
        await _sut.OnGetAsync();
        var result = _sut.NumberOfAcademiesInTrust;
        result.Should().Be(1);
    }

    [Fact]
    public async Task OnGetAsync_returns_correct_number_of_academies_for_multi_academy_trust()
    {
        SetupTrustWithMultipleAcademies();
        await _sut.OnGetAsync();
        var result = _sut.NumberOfAcademiesInTrust;
        result.Should().Be(7);
    }

    [Fact]
    public async Task OnGetAsync_returns_correct_number_of_academies_for_trust_with_no_academies()
    {
        SetupTrustWithNoAcademies();
        await _sut.OnGetAsync();
        var result = _sut.NumberOfAcademiesInTrust;
        result.Should().Be(0);
    }

    private static Academy DummyAcademy(int urn, string localAuthority, int numberOfPupils,
        int schoolCapacity,
        OfstedRatingScore currentOfstedRatingScore)
    {
        return DummyAcademyFactory.GetDummyAcademy(
            urn,
            localAuthority: localAuthority,
            numberOfPupils: numberOfPupils,
            schoolCapacity: schoolCapacity,
            currentOfstedRatingScore: currentOfstedRatingScore);
    }

    private void SetupTrustWithMultipleAcademies()
    {
        _mockTrustProvider.Setup(tp => tp.GetTrustByUidAsync(TrustUid))
            .ReturnsAsync(DummyTrustFactory.GetDummyTrust(TrustUid, academies: new[]
            {
                DummyAcademy(1, "localAuth1", 50, 150, OfstedRatingScore.Outstanding),
                DummyAcademy(2, "localAuth1", 100, 200, OfstedRatingScore.Good),
                DummyAcademy(3, "localAuth1", 150, 250, OfstedRatingScore.RequiresImprovement),
                DummyAcademy(4, "localAuth1", 200, 300, OfstedRatingScore.Inadequate),
                DummyAcademy(5, "localAuth1", 0, 500, OfstedRatingScore.None),
                DummyAcademy(6, "localAuth1", 600, 500, OfstedRatingScore.Outstanding),
                DummyAcademy(7, "localAuth2", 25, 50, OfstedRatingScore.Outstanding)
            }));
    }

    private void SetupTrustWithSingleAcademy()
    {
        _mockTrustProvider.Setup(tp => tp.GetTrustByUidAsync(TrustUid))
            .ReturnsAsync(DummyTrustFactory.GetDummyTrust(TrustUid, academies: new[]
            {
                DummyAcademy(42, "localAuth1", 200, 250, OfstedRatingScore.Outstanding)
            }));
    }

    private void SetupTrustWithNoAcademies()
    {
        _mockTrustProvider.Setup(tp => tp.GetTrustByUidAsync(TrustUid))
            .ReturnsAsync(DummyTrustFactory.GetDummyTrust(TrustUid, academies: null));
    }

    [Fact]
    public async Task OnGetAsync_returns_NotFoundResult_if_Trust_is_not_found()
    {
        _mockTrustRepository.Setup(t => t.GetTrustSummaryAsync(TrustUid)).ReturnsAsync((TrustSummaryServiceModel?)null);
        var result = await _sut.OnGetAsync();
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task OnGetAsync_sets_correct_data_source_list()
    {
        _ = await _sut.OnGetAsync();
        _mockDataSourceService.Verify(e => e.GetAsync(Source.Gias), Times.Once);
        _sut.DataSources.Should().ContainSingle();
        _sut.DataSources[0].Fields.Should().Contain(new List<string> { "Trust summary", "Ofsted ratings" });
    }
}
