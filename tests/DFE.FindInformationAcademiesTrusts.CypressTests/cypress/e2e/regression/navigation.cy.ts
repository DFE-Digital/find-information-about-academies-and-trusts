import navigation from "../../pages/navigation";
import academiesPage from "../../pages/trusts/academiesPage";
import governancePage from "../../pages/trusts/governancePage";
import contactsPage from "../../pages/trusts/contactsPage";
import overviewPage from "../../pages/trusts/overviewPage";

describe('Testing Navigation', () => {

    describe("Testing the footer-links", () => {
        beforeEach(() => {
            cy.login();
        });

        it("Should check that the home page footer bar privacy link is present and functional", () => {
            navigation
                .checkPrivacyLinkPresent()
                .clickPrivacyLink();

            navigation
                .checkCurrentURLIsCorrect('privacy');

        });

        it("Should check that the home page footer bar cookies link is present and functional", () => {
            navigation
                .checkCookiesLinkPresent()
                .clickCookiesLink();

            navigation
                .checkCurrentURLIsCorrect('cookies');

        });

        it("Should check that the home page footer bar accessibility statement link is present and functional", () => {
            navigation
                .checkAccessibilityStatementLinkPresent()
                .clickAccessibilityStatementLink();

            navigation
                .checkCurrentURLIsCorrect('accessibility');
        });
    });

    describe("Testing the breadcrumb links and their relevant functionality", () => {
        beforeEach(() => {
            cy.login();
        });

        ['/search', '/accessibility', '/cookies', '/privacy', '/notfound'].forEach((url) => {
            it(`Should have Home breadcrumb only on ${url}`, () => {
                cy.visit(url, { failOnStatusCode: false });

                navigation
                    .checkCurrentURLIsCorrect(url)
                    .checkHomeBreadcrumbPresent()
                    .clickHomeBreadcrumbButton()
                    .checkBrowserPageTitleContains('Home page');
            });
        });

        ['/', '/error'].forEach((url) => {
            it(`Should have no breadcrumb on ${url}`, () => {
                cy.visit(url);

                navigation
                    .checkCurrentURLIsCorrect(url)
                    .checkAccessibilityStatementLinkPresent() // ensure page content has loaded - all pages have an a11y statement link
                    .checkBreadcrumbNotPresent();
            });
        });

        it('Should check that a trusts name breadcrumb is displayed on the trusts page', () => {
            cy.visit('/trusts/overview/trust-details?uid=5712');

            navigation
                .checkTrustNameBreadcrumbPresent('ASPIRE NORTH EAST MULTI ACADEMY TRUST')
                .clickHomeBreadcrumbButton()
                .checkBrowserPageTitleContains('Home page');
        });

        it('Should check a different trusts name breadcrumb is displayed on the trusts page', () => {
            cy.visit('/trusts/overview/trust-details?uid=5527');

            navigation
                .checkTrustNameBreadcrumbPresent('ASHTON WEST END PRIMARY ACADEMY')
                .clickHomeBreadcrumbButton()
                .checkBrowserPageTitleContains('Home page');
        });
    });

    describe("Testing the trusts service navigation", () => {
        beforeEach(() => {
            cy.login();
            cy.visit('/trusts/overview/trust-details?uid=5527');
        });

        it('Should check that the contacts navigation button takes me to the contacts in DfE page', () => {
            navigation
                .clickContactsServiceNavButton()
                .checkContactsServiceNavButtonIsHighlighed()
                .checkCurrentURLIsCorrect('/contacts/in-dfe?uid=5527')
                .checkAllServiceNavItemsPresent();
            contactsPage
                .checkChairOfTrusteesPresent()
                .checkAccountingOfficerPresent();
        });

        it('Should check that the Academies navigation button takes me to the academies details page', () => {
            navigation
                .clickAcademiesServiceNavButton()
                .checkAcademiesServiceNavButtonIsHighlighted()
                .checkCurrentURLIsCorrect('/academies/details?uid=5527')
                .checkAllServiceNavItemsPresent();
            academiesPage
                .checkDetailsHeadersPresent();
        });

        it('Should check that the Governance navigation button takes me to the governance trust leadership page', () => {
            navigation
                .clickGovernanceServiceNavButton()
                .checkGovernanceServiceNavButtonIsHighlighted()
                .checkCurrentURLIsCorrect('/governance/trust-leadership?uid=5527')
                .checkAllServiceNavItemsPresent();
            governancePage
                .checkTrusteesTableHeadersAreVisible();
        });

        it('Should check that the Overview navigation button takes me to the overview trust details page', () => {
            cy.visit('trusts/trust-leadership?uid=5527');
            navigation
                .clickOverviewServiceNavButton()
                .checkOverviewServiceNavButtonIsHighlighted()
                .checkCurrentURLIsCorrect('/overview/trust-details?uid=5527')
                .checkAllServiceNavItemsPresent();
            overviewPage
                .checkOverviewHeaderPresent();
        });

    });
});
