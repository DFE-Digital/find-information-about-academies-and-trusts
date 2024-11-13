import navigation from "../../pages/navigation";
import academiesInTrustPage from "../../pages/trusts/academiesInTrustPage";
import governancePage from "../../pages/trusts/governancePage";
import trustContactsPage from "../../pages/trusts/trustContactsPage";
import trustOverviewPage from "../../pages/trusts/trustOverviewPage";

describe('Testing Navigation', () => {

    describe("Testing the footer-links", () => {
        beforeEach(() => {
            cy.login();
        });

        it("Should check that the home page footer bar privacy link is present and functional", () => {
            navigation
                .checkPrivacyLinkPresent()
                .clickPrivacyLink()

            navigation
                .checkCurrentURLIsCorrect('privacy')

        });

        it("Should check that the home page footer bar cookies link is present and functional", () => {
            navigation
                .checkCookiesLinkPresent()
                .clickCookiesLink()

            navigation
                .checkCurrentURLIsCorrect('cookies')

        });

        it("Should check that the home page footer bar accessibility statement link is present and functional", () => {
            navigation
                .checkAccessibilityStatementLinkPresent()
                .clickAccessibilityStatementLink()

            navigation
                .checkCurrentURLIsCorrect('accessibility')
        });
    })

    describe("Testing the breadcrumb links and their relevant functionality", () => {
        beforeEach(() => {
            cy.login();
        });

        ['/search', '/accessibility', '/cookies', '/privacy', '/notfound'].forEach((url) => {
            it(`Should have Home breadcrumb only on ${url}`, () => {
                cy.visit(url, { failOnStatusCode: false })

                navigation
                    .checkCurrentURLIsCorrect(url)
                    .checkHomeBreadcrumbPresent()
                    .clickHomeBreadcrumbButton()
                    .checkBrowserPageTitleContains('Home page')
            });
        });

        ['/', '/error'].forEach((url) => {
            it(`Should have no breadcrumb on ${url}`, () => {
                cy.visit(url)

                navigation
                    .checkCurrentURLIsCorrect(url)
                    .checkAccessibilityStatementLinkPresent() // ensure page content has loaded - all pages have an a11y statement link
                    .checkBreadcrumbNotPresent()
            });
        })

        it('Should check that a trusts name breadcrumb is displayed on the trusts page', () => {
            cy.visit('/trusts/overview?uid=5712');

            navigation
                .checkTrustNameBreadcrumbPresent('ASPIRE NORTH EAST MULTI ACADEMY TRUST')
                .clickHomeBreadcrumbButton()
                .checkBrowserPageTitleContains('Home page')
        });

        it('Should check a different trusts name breadcrumb is displayed on the trusts page', () => {
            cy.visit('/trusts/overview?uid=5527');

            navigation
                .checkTrustNameBreadcrumbPresent('ASHTON WEST END PRIMARY ACADEMY')
                .clickHomeBreadcrumbButton()
                .checkBrowserPageTitleContains('Home page')
        });
    })

    describe("Testing the service navigation", () => {
        beforeEach(() => {
            cy.login();
            cy.visit('/trusts/overview?uid=5527');
        });

        it('Should check that the contacts navigation button takes me to the correct page', () => {
            navigation
                .clickContactsServiceNavButton()
                .checkCurrentURLIsCorrect('/contacts?uid=5527')
                .checkAllServiceNavItemsPresent()
            trustContactsPage
                .checkChairOfTrusteesPresent()
                .checkAccountingOfficerPresent()
        });

        it('Should check that the Academies navigation button takes me to the correct page', () => {
            navigation
                .clickAcademiesInThisTrustServiceNavButton()
                .checkCurrentURLIsCorrect('/academies/details?uid=5527')
                .checkAllServiceNavItemsPresent()
            academiesInTrustPage
                .checkDetailsHeadersPresent()
        });

        it('Should check that the Governance navigation button takes me to the correct page', () => {
            navigation
                .clickGovernanceServiceNavButton()
                .checkCurrentURLIsCorrect('/governance?uid=5527')
                .checkAllServiceNavItemsPresent()
            governancePage
                .checkTrusteeColumnHeaders()
        });

        it('Should check that the Overview navigation button takes me to the correct page', () => {
            cy.visit('trusts/governance?uid=5527');
            navigation
                .clickOverviewServiceNavButton()
                .checkCurrentURLIsCorrect('/overview?uid=5527')
                .checkAllServiceNavItemsPresent()
            trustOverviewPage
                .checkOverviewHeaderPresent()
        });

    })

    describe("Testing the academies in this trust navigation", () => {
        beforeEach(() => {
            cy.login();
            cy.visit('/trusts/academies/details?uid=5527');
        });

        it('Should check that the acdemiesInThisTrust Ofsted ratings navigation button takes me to the correct page', () => {
            navigation
                .clickOfstedAcadmiesTrustButton()
                .checkCurrentURLIsCorrect('/trusts/academies/ofsted-ratings?uid=5527')
                .checkAllServiceNavItemsPresent()
                .checkAllAcademiesInTrustNavItemsPresent()
            academiesInTrustPage
                .checkOfstedHeadersPresent()
        });

        it('Should check that the acdemiesInThisTrust Pupil numbers navigation button takes me to the correct page', () => {
            navigation
                .clickPupilNumbersAcadmiesTrustButton()
                .checkCurrentURLIsCorrect('/trusts/academies/pupil-numbers?uid=5527')
                .checkAllServiceNavItemsPresent()
                .checkAllAcademiesInTrustNavItemsPresent()
            academiesInTrustPage
                .checkPupilNumbersHeadersPresent()
        });

        it('Should check that the acdemiesInThisTrust Free school meals navigation button takes me to the correct page', () => {
            navigation
                .clickFreeSchoolMealsAcadmiesTrustButton()
                .checkCurrentURLIsCorrect('/trusts/academies/free-school-meals?uid=5527')
                .checkAllServiceNavItemsPresent()
                .checkAllAcademiesInTrustNavItemsPresent()
            academiesInTrustPage
                .checkFreeSchoolMealsHeadersPresent()
        });

        it('Should check that the acdemiesInThisTrust Details navigation button takes me to the correct page', () => {
            navigation
                .clickDetailsAcadmiesTrustButton()
                .checkCurrentURLIsCorrect('/trusts/academies/details?uid=5527')
                .checkAllServiceNavItemsPresent()
                .checkAllAcademiesInTrustNavItemsPresent()
            academiesInTrustPage
                .checkDetailsHeadersPresent()
        });

        it('Should check that the academies in this trust nav items are not present when I am not in the relevant academies page', () => {
            cy.visit('/trusts/overview?uid=5527');
            navigation
                .checkAcademiesInTrustNavNotPresent()
        });
    })
})
