import navigation from "../../../pages/navigation";
import { testSchoolData, testTrustData } from "../../../support/test-data-store";

describe('Testing Navigation', () => {

    describe("Testing the breadcrumb-links", () => {
        beforeEach(() => {
            cy.login();
        });

        describe("Testing the general page breadcrumb links and their relevant functionality", () => {
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
        });

        describe("Testing the breadcrumb links on the trust academy details page", () => {
            beforeEach(() => {
                cy.login();
            });

            describe("Testing the breadcrumb links on the trust page", () => {
                testTrustData.forEach(({ uid, trustName }) => {
                    it('Should check that a trusts name breadcrumb is displayed on the trusts page', () => {
                        cy.visit(`/trusts/overview/trust-details?uid=${uid}`);
                        navigation
                            .checkTrustNameBreadcrumbPresent(`${trustName}`)
                            .clickHomeBreadcrumbButton()
                            .checkBrowserPageTitleContains('Home page');
                    });
                });
            });

            describe("Testing the breadcrumb links on the pipeline academies pages", () => {
                [`/trusts/academies/pipeline/pre-advisory-board?uid=16002`, `/trusts/academies/pipeline/post-advisory-board?uid=17584`, `/trusts/academies/pipeline/free-schools?uid=17584`].forEach((url) => {
                    it("Checks the breadcrumb shows the correct page name", () => {
                        cy.visit(url);
                        navigation
                            .checkPageNameBreadcrumbPresent("Academies");
                        //revist adding this later need to rethink how the foreach handles differing uid pulls for the differnt pages .checkTrustNameBreadcrumbPresent('Ashton West End Primary Academy');
                    });
                });
            });
        });
    });

    describe("Testing the breadcrumb links on the schools overview pages", () => {
        testSchoolData.forEach(({ urn }) => {
            beforeEach(() => {
                cy.login();
            });

            [`/schools/overview/details?urn=${urn}`].forEach((url) => {
                it("Checks the breadcrumb shows the correct page name", () => {
                    cy.visit(url);
                    navigation
                        .checkPageNameBreadcrumbPresent("Overview");
                });
            });
        });
    });
});
