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
    protected T Sut = default!;
    protected readonly Mock<ITrustService> MockTrustService = new();
    protected readonly MockDataSourceService MockDataSourceService = new();

    protected readonly DataSourceServiceModel GiasDataSource =
        new(Source.Gias, new DateTime(2025, 1, 1), UpdateFrequency.Daily);

    protected readonly DataSourceServiceModel MstrDataSource =
        new(Source.Mstr, new DateTime(2025, 1, 1), UpdateFrequency.Monthly);

    protected readonly DataSourceServiceModel MisDataSource =
        new(Source.Mis, new DateTime(2025, 1, 1), UpdateFrequency.Daily);

    protected readonly DataSourceServiceModel EesDataSource = new(Source.ExploreEducationStatistics,
        new DateTime(2025, 1, 1), UpdateFrequency.Annually);

    protected const string TrustUid = "1234";
    protected readonly TrustSummaryServiceModel DummyTrustSummary = new(TrustUid, "My Trust", "Multi-academy trust", 3);

    protected BaseTrustPageTests()
    {
        MockTrustService.Setup(t => t.GetTrustSummaryAsync(TrustUid)).ReturnsAsync(DummyTrustSummary);

        MockDataSourceService.Setup(s => s.GetAsync(Source.Gias)).ReturnsAsync(GiasDataSource);
        MockDataSourceService.Setup(s => s.GetAsync(Source.Mstr)).ReturnsAsync(MstrDataSource);
        MockDataSourceService.Setup(s => s.GetAsync(Source.Mis)).ReturnsAsync(MisDataSource);
        MockDataSourceService.Setup(s => s.GetAsync(Source.ExploreEducationStatistics)).ReturnsAsync(EesDataSource);
    }

    [Fact]
    public void ShowHeaderSearch_should_be_true()
    {
        Sut.ShowHeaderSearch.Should().Be(true);
    }

    [Fact]
    public async Task OnGetAsync_should_fetch_a_trustsummary_by_uid()
    {
        Sut.Uid = DummyTrustSummary.Uid;

        await Sut.OnGetAsync();
        Sut.TrustSummary.Should().Be(DummyTrustSummary);
    }

    [Fact]
    public async Task OnGetAsync_should_return_not_found_result_if_trust_is_not_found()
    {
        MockTrustService.Setup(t => t.GetTrustSummaryAsync("1111"))
            .ReturnsAsync((TrustSummaryServiceModel?)null);

        Sut.Uid = "1111";
        var result = await Sut.OnGetAsync();
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task OnGetAsync_should_return_not_found_result_if_Uid_is_not_provided()
    {
        Sut.Uid = "";
        var result = await Sut.OnGetAsync();
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public abstract Task OnGetAsync_should_set_active_NavigationLink_to_current_page();

    [Fact]
    public async Task OnGetAsync_should_populate_NavigationLinks()
    {
        _ = await Sut.OnGetAsync();

        Sut.NavigationLinks.Should()
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
        MockTrustService.Setup(t => t.GetTrustSummaryAsync(trustUid))
            .ReturnsAsync(DummyTrustSummary with { Uid = trustUid });
        Sut.Uid = trustUid;

        _ = await Sut.OnGetAsync();

        Sut.NavigationLinks.Should().AllSatisfy(l => l.Uid.Should().Be(trustUid));
    }

    [Fact]
    public async Task OnGetAsync_should_configure_TrustPageMetadata_TrustName()
    {
        _ = await Sut.OnGetAsync();

        Sut.TrustPageMetadata.TrustName.Should().Be("My Trust");
    }

    [Fact]
    public abstract Task OnGetAsync_should_configure_TrustPageMetadata_PageName();

    [Theory]
    [InlineData("1234")]
    [InlineData("5678")]
    public async Task OnGetAsync_should_set_SubNavigationLinks_to_trust_uid(string trustUid)
    {
        MockTrustService.Setup(t => t.GetTrustSummaryAsync(trustUid))
            .ReturnsAsync(DummyTrustSummary with { Uid = trustUid });

        Sut.Uid = trustUid;

        _ = await Sut.OnGetAsync();

        // Sub nav links collection should either be empty or all be for current trust uid
        if (Sut.SubNavigationLinks.Length == 0)
        {
            Sut.SubNavigationLinks.Should().BeEmpty();
        }
        else
        {
            Sut.SubNavigationLinks.Should().AllSatisfy(l => l.Uid.Should().Be(trustUid));
        }
    }

    [Fact]
    public abstract Task OnGetAsync_sets_correct_data_source_list();
}
