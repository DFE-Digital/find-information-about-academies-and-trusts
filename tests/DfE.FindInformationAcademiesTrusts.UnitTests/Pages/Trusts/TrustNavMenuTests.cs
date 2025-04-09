using DfE.FindInformationAcademiesTrusts.Pages.Trusts;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Academies.InTrust;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Academies.Pipeline;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Contacts;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts.FinancialDocuments;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Governance;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Ofsted;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Overview;
using DfE.FindInformationAcademiesTrusts.Services.Trust;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts;

public class TrustNavMenuTests
{
    [Theory]
    [InlineData("1234")]
    [InlineData("5678")]
    public void GetServiceNavLinks_should_set_route_data_to_uid(string expectedUid)
    {
        var activePage = GetMockPage(typeof(TrustDetailsModel), expectedUid);

        var results = TrustNavMenu.GetServiceNavLinks(activePage);

        results.Should().AllSatisfy(link =>
        {
            var route = link.AspAllRouteData.Should().ContainSingle().Subject;
            route.Key.Should().Be("uid");
            route.Value.Should().Be(expectedUid);
        });
    }

    [Fact]
    public void GetServiceNavLinks_should_set_hidden_text_to_trust()
    {
        var activePage = GetMockPage(typeof(InTrustModel));

        var results = TrustNavMenu.GetServiceNavLinks(activePage);

        results.Should().AllSatisfy(link => { link.VisuallyHiddenLinkText.Should().Be("Trust"); });
    }

    [Fact]
    public void GetServiceNavLinks_should_return_expected_links()
    {
        var activePage = GetMockPage(typeof(TrustDetailsModel));

        var results = TrustNavMenu.GetServiceNavLinks(activePage);

        results.Should().SatisfyRespectively(
            l =>
            {
                l.LinkDisplayText.Should().Be("Overview");
                l.AspPage.Should().Be("/Trusts/Overview/TrustDetails");
                l.TestId.Should().Be("overview-nav");
            },
            l =>
            {
                l.LinkDisplayText.Should().Be("Contacts");
                l.AspPage.Should().Be("/Trusts/Contacts/InDfe");
                l.TestId.Should().Be("contacts-nav");
            },
            l =>
            {
                l.LinkDisplayText.Should().Be("Academies (3)");
                l.AspPage.Should().Be("/Trusts/Academies/InTrust/Details");
                l.TestId.Should().Be("academies-nav");
            },
            l =>
            {
                l.LinkDisplayText.Should().Be("Ofsted");
                l.AspPage.Should().Be("/Trusts/Ofsted/SingleHeadlineGrades");
                l.TestId.Should().Be("ofsted-nav");
            },
            l =>
            {
                l.LinkDisplayText.Should().Be("Financial documents");
                l.AspPage.Should().Be("/Trusts/FinancialDocuments/FinancialStatements");
                l.TestId.Should().Be("financial-documents-nav");
            },
            l =>
            {
                l.LinkDisplayText.Should().Be("Governance");
                l.AspPage.Should().Be("/Trusts/Governance/TrustLeadership");
                l.TestId.Should().Be("governance-nav");
            }
        );
    }

    [Theory]
    [InlineData(1)]
    [InlineData(25)]
    public void GetServiceNavLinks_should_show_number_of_academies_in_display_text(int numberOfAcademies)
    {
        var activePage = GetMockPage(typeof(TrustDetailsModel), "1234", numberOfAcademies);

        var results = TrustNavMenu.GetServiceNavLinks(activePage);

        results.Should().ContainSingle(l => l.TestId == "academies-nav")
            .Which.LinkDisplayText.Should().Be($"Academies ({numberOfAcademies})");
    }

    [Theory]
    [MemberData(nameof(PageTypeToServiceNavAspLinks))]
    public void GetServiceNavLinks_should_set_active_page_link(Type activePageType, string expectedActivePageLink)
    {
        var activePage = GetMockPage(activePageType);

        var results = TrustNavMenu.GetServiceNavLinks(activePage);

        results.Should().ContainSingle(l => l.LinkIsActive).Which.AspPage.Should().Be(expectedActivePageLink);
    }

    public static TheoryData<Type, string> PageTypeToServiceNavAspLinks =>
        new()
        {
            //Overview
            { typeof(TrustDetailsModel), "/Trusts/Overview/TrustDetails" },
            { typeof(TrustSummaryModel), "/Trusts/Overview/TrustDetails" },
            { typeof(ReferenceNumbersModel), "/Trusts/Overview/TrustDetails" },
            //Contacts
            { typeof(InDfeModel), "/Trusts/Contacts/InDfe" },
            { typeof(InTrustModel), "/Trusts/Contacts/InDfe" },
            //Academies
            { typeof(AcademiesInTrustDetailsModel), "/Trusts/Academies/InTrust/Details" },
            { typeof(PupilNumbersModel), "/Trusts/Academies/InTrust/Details" },
            { typeof(FreeSchoolMealsModel), "/Trusts/Academies/InTrust/Details" },
            { typeof(PreAdvisoryBoardModel), "/Trusts/Academies/InTrust/Details" },
            { typeof(PostAdvisoryBoardModel), "/Trusts/Academies/InTrust/Details" },
            { typeof(FreeSchoolsModel), "/Trusts/Academies/InTrust/Details" },
            //Ofsted
            { typeof(SingleHeadlineGradesModel), "/Trusts/Ofsted/SingleHeadlineGrades" },
            { typeof(CurrentRatingsModel), "/Trusts/Ofsted/SingleHeadlineGrades" },
            { typeof(PreviousRatingsModel), "/Trusts/Ofsted/SingleHeadlineGrades" },
            { typeof(SafeguardingAndConcernsModel), "/Trusts/Ofsted/SingleHeadlineGrades" },
            //Financial documents
            { typeof(FinancialStatementsModel), "/Trusts/FinancialDocuments/FinancialStatements" },
            { typeof(ManagementLettersModel), "/Trusts/FinancialDocuments/FinancialStatements" },
            { typeof(InternalScrutinyReportsModel), "/Trusts/FinancialDocuments/FinancialStatements" },
            { typeof(SelfAssessmentChecklistsModel), "/Trusts/FinancialDocuments/FinancialStatements" },
            //Governance
            { typeof(TrustLeadershipModel), "/Trusts/Governance/TrustLeadership" },
            { typeof(TrusteesModel), "/Trusts/Governance/TrustLeadership" },
            { typeof(MembersModel), "/Trusts/Governance/TrustLeadership" },
            { typeof(HistoricMembersModel), "/Trusts/Governance/TrustLeadership" }
        };

    private static TrustsAreaModel GetMockPage(Type pageType, string uid = "1234", int numberOfAcademies = 3)
    {
        var parameters = pageType.GetConstructors()[0].GetParameters();
        var arguments = parameters.Select(p => p.ParameterType.Name switch
        {
            _ => Substitute.For([p.ParameterType], [])
        }).ToArray();

        var mockPage = Activator.CreateInstance(pageType, arguments) as TrustsAreaModel ??
                       throw new ArgumentException("Couldn't create mock for given page type", nameof(pageType));

        mockPage.Uid = uid;
        mockPage.TrustSummary = new TrustSummaryServiceModel(uid, "MY TRUST", "Multi-academy trust", numberOfAcademies);

        return mockPage;
    }
}
