import homePage from "../../pages/homePage";
import navigation from "../../pages/navigation";
import paginationPage from "../../pages/paginationPage";

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

        ['/search', '/accessibility', '/cookies', '/privacy'].forEach((url) => {
            it(`Should have Home breadcrumb only on ${url}` + url, () => {
                cy.visit(url)

                navigation
                    .checkCurrentURLIsCorrect(url)
                    .checkHomeBreadcrumbPresent()
                    .clickHomeBreadcrumbButton()
                    .checkBrowserPageTitleContains('Home page')

            });

        })
        
        it('Should check that a trusts name breadcrumb is displayed on the trusts page', () => {
            cy.visit('/trusts/details?uid=5712');

            navigation
                .checkTrustNameBreadcrumbPresent('ASPIRE NORTH EAST MULTI ACADEMY TRUST')
                .clickHomeBreadcrumbButton()
                .checkBrowserPageTitleContains('Home page')

        });

        it('Should check a different trusts name breadcrumb is displayed on the trusts page', () => {
            cy.visit('/trusts/details?uid=5527');

            navigation
                .checkTrustNameBreadcrumbPresent('ASHTON WEST END PRIMARY ACADEMY')
                .clickHomeBreadcrumbButton()
                .checkBrowserPageTitleContains('Home page')

        });



    })
})