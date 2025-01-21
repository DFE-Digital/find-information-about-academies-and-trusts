using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Pages;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts;

public abstract class BaseTrustPageTests<T> where T : TrustsAreaModel
{
    protected T _sut = default!;
    protected readonly Mock<ITrustService> _mockTrustService = new();
    protected readonly MockDataSourceService _mockDataSourceService = new();

    protected readonly DataSourceServiceModel _giasDataSource =
        new(Source.Gias, new DateTime(2025, 1, 1), UpdateFrequency.Daily);

    protected readonly DataSourceServiceModel _mstrDataSource =
        new(Source.Mstr, new DateTime(2025, 1, 1), UpdateFrequency.Monthly);

    protected readonly DataSourceServiceModel _misDataSource =
        new(Source.Mis, new DateTime(2025, 1, 1), UpdateFrequency.Daily);

    protected const string TrustUid = "1234";
    protected TrustSummaryServiceModel dummyTrustSummary = new(TrustUid, "My Trust", "Multi-academy trust", 3);

    protected BaseTrustPageTests()
    {
        _mockTrustService.Setup(t => t.GetTrustSummaryAsync(TrustUid)).ReturnsAsync(dummyTrustSummary);

        _mockDataSourceService.Setup(s => s.GetAsync(Source.Gias)).ReturnsAsync(_giasDataSource);
        _mockDataSourceService.Setup(s => s.GetAsync(Source.Mstr)).ReturnsAsync(_mstrDataSource);
        _mockDataSourceService.Setup(s => s.GetAsync(Source.Mis)).ReturnsAsync(_misDataSource);
    }

    [Fact]
    public void ShowHeaderSearch_should_be_true()
    {
        _sut.ShowHeaderSearch.Should().Be(true);
    }

    [Fact]
    public async Task OnGetAsync_should_fetch_a_trustsummary_by_uid()
    {
        _sut.Uid = dummyTrustSummary.Uid;

        await _sut.OnGetAsync();
        _sut.TrustSummary.Should().Be(dummyTrustSummary);
    }

    [Fact]
    public async Task OnGetAsync_should_return_not_found_result_if_trust_is_not_found()
    {
        _mockTrustService.Setup(t => t.GetTrustSummaryAsync("1111"))
            .ReturnsAsync((TrustSummaryServiceModel?)null);

        _sut.Uid = "1111";
        var result = await _sut.OnGetAsync();
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task OnGetAsync_should_return_not_found_result_if_Uid_is_not_provided()
    {
        _sut.Uid = "";
        var result = await _sut.OnGetAsync();
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public abstract Task OnGetAsync_should_set_active_NavigationLink_to_current_page();

    [Fact]
    public async Task OnGetAsync_should_populate_NavigationLinks()
    {
        _ = await _sut.OnGetAsync();

        _sut.NavigationLinks.Should()
            .SatisfyRespectively(
                l =>
                {
                    l.LinkText.Should().Be(ViewConstants.OverviewPageName);
                    l.Page.Should().Be("/Trusts/Overview/TrustDetails");
                    l.DataTestId.Should().Be("overview-nav");
                },
                l =>
                {
                    l.LinkText.Should().Be(ViewConstants.ContactsPageName);
                    l.Page.Should().Be("/Trusts/Contacts/InDfe");
                    l.DataTestId.Should().Be("contacts-nav");
                },
                l =>
                {
                    l.LinkText.Should().Be("Academies (3)");
                    l.Page.Should().Be("/Trusts/Academies/Details");
                    l.DataTestId.Should().Be("academies-nav");
                },
                l =>
                {
                    l.LinkText.Should().Be(ViewConstants.OfstedPageName);
                    l.Page.Should().Be("/Trusts/Ofsted/CurrentRatings");
                    l.DataTestId.Should().Be("ofsted-nav");
                },
                l =>
                {
                    l.LinkText.Should().Be(ViewConstants.GovernancePageName);
                    l.Page.Should().Be("/Trusts/Governance/TrustLeadership");
                    l.DataTestId.Should().Be("governance-nav");
                }
            );
    }

    [Theory]
    [InlineData("1234")]
    [InlineData("5678")]
    public async Task OnGetAsync_should_set_NavigationLinks_to_trust_uid(string trustUid)
    {
        _mockTrustService.Setup(t => t.GetTrustSummaryAsync(trustUid))
            .ReturnsAsync(dummyTrustSummary with { Uid = trustUid });
        _sut.Uid = trustUid;

        _ = await _sut.OnGetAsync();

        _sut.NavigationLinks.Should().AllSatisfy(l => l.Uid.Should().Be(trustUid));
    }

    [Fact]
    public async Task OnGetAsync_should_configure_TrustPageMetadata_TrustName()
    {
        _ = await _sut.OnGetAsync();

        _sut.TrustPageMetadata.TrustName.Should().Be("My Trust");
    }

    [Fact]
    public abstract Task OnGetAsync_should_configure_TrustPageMetadata_PageName();

    [Theory]
    [InlineData("1234")]
    [InlineData("5678")]
    public async Task OnGetAsync_should_set_SubNavigationLinks_to_trust_uid(string trustUid)
    {
        _mockTrustService.Setup(t => t.GetTrustSummaryAsync(trustUid))
            .ReturnsAsync(dummyTrustSummary with { Uid = trustUid });

        _sut.Uid = trustUid;

        _ = await _sut.OnGetAsync();

        // Sub nav links collection should either be empty or all be for current trust uid
        if (_sut.SubNavigationLinks.Length == 0)
        {
            _sut.SubNavigationLinks.Should().BeEmpty();
        }
        else
        {
            _sut.SubNavigationLinks.Should().AllSatisfy(l => l.Uid.Should().Be(trustUid));
        }
    }

    [Fact]
    public abstract Task OnGetAsync_sets_correct_data_source_list();
}
