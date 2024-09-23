using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts;

public class OverviewModelTests
{
    private readonly OverviewModel _sut;
    private const string TrustUid = "1234";
    private readonly MockDataSourceService _mockDataSourceService = new();
    private readonly Mock<ITrustService> _mockTrustService = new();

    private static readonly TrustOverviewServiceModel BaseTrustOverviewServiceModel =
        new("1234", 0, new Dictionary<string, int>(), 0, 0, new Dictionary<OfstedRatingScore, int>());

    public OverviewModelTests()
    {
        _mockTrustService.Setup(t => t.GetTrustSummaryAsync(TrustUid))
            .ReturnsAsync(new TrustSummaryServiceModel(TrustUid, "My Trust", "Multi-academy trust", 3));

        _sut = new OverviewModel(Mock.Of<ITrustProvider>(), _mockDataSourceService.Object,
                new MockLogger<OverviewModel>().Object,
                _mockTrustService.Object)
            { Uid = TrustUid };
    }

    [Fact]
    public void PageName_should_be_Overview()
    {
        _sut.PageName.Should().Be("Overview");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(200)]
    [InlineData(1125)]
    public async Task OnGetAsync_sets_pupil_numbers(int num)
    {
        _mockTrustService.Setup(t => t.GetTrustOverviewAsync(TrustUid))
            .ReturnsAsync(BaseTrustOverviewServiceModel with { TotalPupilNumbers = num });

        await _sut.OnGetAsync();

        _sut.TotalPupilNumbers.Should().Be(num);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(200)]
    [InlineData(1125)]
    public async Task OnGetAsync_sets_pupil_capacity(int num)
    {
        _mockTrustService.Setup(t => t.GetTrustOverviewAsync(TrustUid))
            .ReturnsAsync(BaseTrustOverviewServiceModel with { TotalCapacity = num });

        await _sut.OnGetAsync();

        _sut.TotalCapacity.Should().Be(num);
    }

    [Theory]
    [InlineData(400, 300)]
    [InlineData(0, 0)]
    public async Task OnGetAsync_sets_pupil_percentage(int totalCapacity, int totalPupilNumbers)
    {
        var serviceModel =
            BaseTrustOverviewServiceModel with
            {
                TotalCapacity = totalCapacity, TotalPupilNumbers = totalPupilNumbers
            };
        _mockTrustService.Setup(t => t.GetTrustOverviewAsync(TrustUid)).ReturnsAsync(serviceModel);

        await _sut.OnGetAsync();

        _sut.PercentageFull.Should().Be(serviceModel.PercentageFull);
    }

    [Fact]
    public async Task OnGetAsync_sets_list_of_local_authorities()
    {
        var overviewWithLocalAuthorities = BaseTrustOverviewServiceModel with
        {
            AcademiesByLocalAuthority = new Dictionary<string, int>
            {
                { "localAuth1", 6 },
                { "localAuth2", 1 }
            }
        };
        _mockTrustService.Setup(t => t.GetTrustOverviewAsync(TrustUid)).ReturnsAsync(overviewWithLocalAuthorities);

        await _sut.OnGetAsync();

        _sut.AcademiesInEachLocalAuthority
            .Should()
            .BeEquivalentTo(new (string Authority, int Total)[]
            {
                ("localAuth1", 6),
                ("localAuth2", 1)
            });
    }

    [Fact]
    public async Task OnGetAsync_sets_ofsted_ratings_list_correctly()
    {
        var overviewWithOfstedRatings = BaseTrustOverviewServiceModel with
        {
            OfstedRatings = new Dictionary<OfstedRatingScore, int>
            {
                { OfstedRatingScore.Outstanding, 3 },
                { OfstedRatingScore.Good, 1 },
                { OfstedRatingScore.Inadequate, 1 },
                { OfstedRatingScore.RequiresImprovement, 1 },
                { OfstedRatingScore.None, 1 }
            }
        };
        _mockTrustService.Setup(t => t.GetTrustOverviewAsync(TrustUid)).ReturnsAsync(overviewWithOfstedRatings);

        await _sut.OnGetAsync();

        _sut.OfstedRatings
            .Should()
            .BeEquivalentTo(new (OfstedRatingScore Rating, int Total)[]
            {
                (OfstedRatingScore.Outstanding, 3),
                (OfstedRatingScore.Good, 1),
                (OfstedRatingScore.Inadequate, 1),
                (OfstedRatingScore.RequiresImprovement, 1),
                (OfstedRatingScore.None, 1)
            });
    }

    [Theory]
    [InlineData(OfstedRatingScore.Outstanding, 3)]
    [InlineData(OfstedRatingScore.Good, 4)]
    [InlineData(OfstedRatingScore.Inadequate, 5)]
    [InlineData(OfstedRatingScore.RequiresImprovement, 6)]
    [InlineData(OfstedRatingScore.None, 7)]
    public async Task OnGetAsync_ofsted_score_count_returns_correct_number_of_ratings(OfstedRatingScore score,
        int expectedCount)
    {
        var overviewWithOfstedRatings = BaseTrustOverviewServiceModel with
        {
            OfstedRatings = new Dictionary<OfstedRatingScore, int>
            {
                { OfstedRatingScore.Outstanding, 3 },
                { OfstedRatingScore.Good, 4 },
                { OfstedRatingScore.Inadequate, 5 },
                { OfstedRatingScore.RequiresImprovement, 6 },
                { OfstedRatingScore.None, 7 }
            }
        };
        _mockTrustService.Setup(t => t.GetTrustOverviewAsync(TrustUid)).ReturnsAsync(overviewWithOfstedRatings);

        await _sut.OnGetAsync();

        _sut.GetNumberOfAcademiesWithOfstedRating(score).Should().Be(expectedCount);
    }

    [Theory]
    [InlineData(OfstedRatingScore.Outstanding)]
    [InlineData(OfstedRatingScore.Good)]
    [InlineData(OfstedRatingScore.Inadequate)]
    [InlineData(OfstedRatingScore.RequiresImprovement)]
    [InlineData(OfstedRatingScore.None)]
    public async Task OnGetAsync_ofsted_score_count_returns_zero_ratings_when_non_exist(OfstedRatingScore missingRating)
    {
        var ofstedRatings = new Dictionary<OfstedRatingScore, int>
        {
            { OfstedRatingScore.Outstanding, 3 },
            { OfstedRatingScore.Good, 4 },
            { OfstedRatingScore.Inadequate, 5 },
            { OfstedRatingScore.RequiresImprovement, 6 },
            { OfstedRatingScore.None, 7 }
        };
        ofstedRatings.Remove(missingRating);

        _mockTrustService.Setup(t => t.GetTrustOverviewAsync(TrustUid))
            .ReturnsAsync(BaseTrustOverviewServiceModel with { OfstedRatings = ofstedRatings });

        await _sut.OnGetAsync();

        _sut.GetNumberOfAcademiesWithOfstedRating(missingRating).Should().Be(0);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(90)]
    public async Task OnGetAsync_returns_correct_number_of_academies(int num)
    {
        _mockTrustService.Setup(t => t.GetTrustOverviewAsync(TrustUid))
            .ReturnsAsync(BaseTrustOverviewServiceModel with { TotalAcademies = num });

        await _sut.OnGetAsync();

        _sut.TotalAcademies.Should().Be(num);
    }

    [Fact]
    public async Task OnGetAsync_returns_NotFoundResult_if_Trust_is_not_found()
    {
        _mockTrustService.Setup(t => t.GetTrustSummaryAsync(TrustUid)).ReturnsAsync((TrustSummaryServiceModel?)null);
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
