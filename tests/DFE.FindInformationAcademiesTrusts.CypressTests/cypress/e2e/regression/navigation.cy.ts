import navigation from "../../pages/navigation";
import academiesInTrustPage from "../../pages/trusts/academiesInTrustPage";
import governancePage from "../../pages/trusts/governancePage";
import contactsPage from "../../pages/trusts/contactsPage";
import overviewPage from "../../pages/trusts/overviewPage";
import financialDocumentsPage from "../../pages/trusts/financialDocumentsPage";
import ofstedPage from "../../pages/trusts/ofstedPage";

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
                .checkTrustNameBreadcrumbPresent('Aspire North East Multi Academy Trust')
                .clickHomeBreadcrumbButton()
                .checkBrowserPageTitleContains('Home page');
        });

        it('Should check a different trusts name breadcrumb is displayed on the trusts page', () => {
            cy.visit('/trusts/overview/trust-details?uid=5527');

            navigation
                .checkTrustNameBreadcrumbPresent('Ashton West End Primary Academy')
                .clickHomeBreadcrumbButton()
                .checkBrowserPageTitleContains('Home page');
        });
    });

    describe("Testing the trusts service navigation", () => {
        // Test each link from a different page in round robin
        // Governance -> Overview -> Contacts -> Academies -> Ofsted -> Financial documents -> Governance

        beforeEach(() => {
            cy.login();
        });

        it('Should check that the Overview navigation button takes me to the overview trust details page', () => {
            // Governance -> Overview
            cy.visit('/trusts/governance/trust-leadership?uid=5527');

            navigation
                .clickOverviewServiceNavButton()
                .checkOverviewServiceNavButtonIsHighlighted()
                .checkCurrentURLIsCorrect('/trusts/overview/trust-details?uid=5527')
                .checkAllServiceNavItemsPresent();
            overviewPage
                .checkTrustDetailsSubHeaderPresent();
        });

        it('Should check that the contacts navigation button takes me to the contacts in DfE page', () => {
            // Overview -> Contacts
            cy.visit('/trusts/overview/trust-details?uid=5527');

            navigation
                .clickContactsServiceNavButton()
                .checkContactsServiceNavButtonIsHighlighed()
                .checkCurrentURLIsCorrect('/trusts/contacts/in-dfe?uid=5527')
                .checkAllServiceNavItemsPresent();
            contactsPage
                .checkContactsInDfeSubHeaderPresent();
        });

        it('Should check that the Academies navigation button takes me to the academies details page', () => {
            // Contacts -> Academies
            cy.visit('/trusts/contacts/in-dfe?uid=5527');

            navigation
                .clickAcademiesServiceNavButton()
                .checkAcademiesServiceNavButtonIsHighlighted()
                .checkCurrentURLIsCorrect('/trusts/academies/in-trust/details?uid=5527')
                .checkAllServiceNavItemsPresent();
            academiesInTrustPage
                .checkDetailsHeadersPresent();
        });

        it('Should check that the Ofsted navigation button takes me to the Ofsted single headline grades page', () => {
            //  Academies -> Ofsted
            cy.visit('/trusts/academies/in-trust/details?uid=5527');

            navigation
                .clickOfstedServiceNavButton()
                .checkOfstedServiceNavButtonIsHighlighted()
                .checkCurrentURLIsCorrect('/trusts/ofsted/single-headline-grades?uid=5527')
                .checkAllServiceNavItemsPresent();
            ofstedPage
                .checkOfstedSHGPageHeaderPresent();
        });

        it('Should check that the Finance documents navigation button takes me to the Financial documents financial statements page', () => {

            //  Ofsted -> Financial documents 
            cy.visit('/trusts/ofsted/single-headline-grades?uid=5527');

            navigation
                .clickFinancialDocumentsServiceNavButton()
                .checkFinancialDocumentsServiceNavButtonIsHighlighted()
                .checkCurrentURLIsCorrect('/trusts/financial-documents/financial-statements?uid=5527')
                .checkAllServiceNavItemsPresent();
            financialDocumentsPage
                .checkFinancialStatementsPageHeaderPresent();
        });

        it('Should check that the Governance navigation button takes me to the governance trust leadership page', () => {
            //  Financial documents -> Governance
            cy.visit('/trusts/financial-documents/financial-statements?uid=5527');

            navigation
                .clickGovernanceServiceNavButton()
                .checkGovernanceServiceNavButtonIsHighlighted()
                .checkCurrentURLIsCorrect('/trusts/governance/trust-leadership?uid=5527')
                .checkAllServiceNavItemsPresent();
            governancePage
                .checkTrustLeadershipSubHeaderPresent();
        });
    });

    describe("Should check that the pipeline academies navigation works", () => {
        beforeEach(() => {
            cy.login();
        });

        it('Navigates from In this trust to pipeline academies', () => {
            // Academies in Trust -> Pipeline Academies
            cy.visit('/trusts/academies/in-trust/details?uid=16002');

            navigation
                .clickPipelineAcademiesNavButton()
                .checkCurrentURLIsCorrect('/trusts/academies/pipeline/pre-advisory-board?uid=16002');
        });

        it('Navigates from In this trust to pipeline academies', () => {
            // Pipeline Academies -> Academies in Trust
            cy.visit('/trusts/academies/pipeline/pre-advisory-board?uid=16002');

            navigation
                .clickAcademiesInThisTrustNavButton()
                .checkCurrentURLIsCorrect('/trusts/academies/in-trust/details?uid=16002');
        });

        it('Navigates from In this trust to pipeline academies', () => {
            // Pipeline Academies page nav buttons
            cy.visit('/trusts/academies/pipeline/pre-advisory-board?uid=16002');

            navigation
                .clickPipelineAcademiesPostAdvisoryNavButton()
                .checkCurrentURLIsCorrect('/trusts/academies/pipeline/post-advisory-board?uid=16002')
                .clickPipelineAcademiesFreeSchoolsNavButton()
                .checkCurrentURLIsCorrect('/trusts/academies/pipeline/free-schools?uid=16002')
                .clickPipelineAcademiesPreAdvisoryNavButton()
                .checkCurrentURLIsCorrect('/trusts/academies/pipeline/pre-advisory-board?uid=16002');
        });
    });

    describe("Should check that the finance documents navigation works", () => {
        beforeEach(() => {
            cy.login();
        });

        it('Navigates from financial statements to management letters', () => {
            // Financial statements -> management letters
            cy.visit('/trusts/financial-documents/financial-statements?uid=5527');

            navigation
                .clickFinancialDocsManagementLettersButton()
                .checkCurrentURLIsCorrect('/trusts/financial-documents/management-letters?uid=5527');
        });

        it('Navigates from management letters to internal scrutiny reports', () => {
            //Management letters -> Internal scrutiny reports
            cy.visit('/trusts/financial-documents/management-letters?uid=5527');

            navigation
                .clickFinancialDocsInternalScrutinyReportsButton()
                .checkCurrentURLIsCorrect('/trusts/financial-documents/internal-scrutiny-reports?uid=5527');
        });

        it('Navigates from internal scrutiny reports to self assessment checklist', () => {
            // Internal scrutiny reports -> Self assessment checklist
            cy.visit('/trusts/financial-documents/internal-scrutiny-reports?uid=5527');

            navigation
                .clickFinancialDocsSelfAssessmentButton()
                .checkCurrentURLIsCorrect('/trusts/financial-documents/self-assessment-checklists?uid=5527');
        });

        it('Navigates from self assessment checklist to financial statements', () => {
            // Self-assessment checklist -> Finance statements
            cy.visit('/trusts/financial-documents/self-assessment-checklists?uid=5527');

            navigation
                .clickFinancialDocsFinancialStatementsButton()
                .checkCurrentURLIsCorrect('/trusts/financial-documents/financial-statements?uid=5527');
        });
    });
});
