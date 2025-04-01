class Navigation {

    elements = {
        privacyFooterButton: () => cy.contains('Privacy'),
        cookiesFooterButton: () => cy.get('[data-testid="cookies-footer-link"]'),
        accessibilityFooterButton: () => cy.contains('Accessibility'),
        academyTypeNav: {
            inThisTrustButton: () => cy.get('[data-testid="academies-in-this-trust-subnav"]'),
            pipelineAcademiesButton: () => cy.get('[data-testid="academies-pipeline-academies-subnav"]'),
        },
        serviceNav: {
            overviewServiceNavButton: () => cy.get('[data-testid="overview-nav"]'),
            contactsServiceNavButton: () => cy.get('[data-testid="contacts-nav"]'),
            academiesServiceNavButton: () => cy.get('[data-testid="academies-nav"]'),
            ofstedServiceNavButton: () => cy.get('[data-testid="ofsted-nav"]'),
            financialDocumentsServiceNavButton: () => cy.get('[data-testid="financial-documents-nav"]'),
            governanceServiceNavButton: () => cy.get('[data-testid="governance-nav"]'),
        },
        currentPageSubnavLinks: () => cy.get('.moj-sub-navigation__link'),
        acadmiesInTrustSubNav: {
            academiesInTrustPupilNumbersButton: () => cy.get('[data-testid="in-this-trust-pupil-numbers-tab"]'),
            academiesInTrustFreeSchoolMealsButton: () => cy.get('[data-testid="in-this-trust-free-school-meals-tab"]'),
            academiesInTrustDetailsButton: () => cy.get('[data-testid="in-this-trust-details-tab"]'),
        },
        pipelineAcademiesSubNav: {
            pipelineAcademiesPreAdvisoryButton: () => cy.get('[data-testid="pipeline-pre-advisory-board-tab"]'),
            pipelineAcademiesPostAdvisoryButton: () => cy.get('[data-testid="pipeline-post-advisory-board-tab"]'),
            pipelineAcademiesFreeSchoolMealsButton: () => cy.get('[data-testid="pipeline-free-schools-tab"]'),
        },
        breadcrumbs: {
            breadcrumbParent: () => cy.get('[aria-label="Breadcrumb"]'),
            homeBreadcrumbButton: () => this.elements.breadcrumbs.breadcrumbParent().contains('Home'),
            trustBreadcrumbLabel: (trustname: string) => this.elements.breadcrumbs.breadcrumbParent().contains(trustname),
            pageNameBreadcrumbLabel: () => this.elements.breadcrumbs.breadcrumbParent().find('[data-testid="breadcrumb-page-name"]')
        },
        financialDocumentsSubNav: {
            financialStatementsNavButton: () => cy.get('[data-testid="financial-documents-financial-statements-subnav"]'),
            managementLettersButton: () => cy.get('[data-testid="financial-documents-management-letters-subnav"]'),
            internalScrutinyReportsButton: () => cy.get('[data-testid="financial-documents-internal-scrutiny-reports-subnav"]'),
            selfAssessmentChecklistsButton: () => cy.get('[data-testid="financial-documents-self-assessment-checklists-subnav"]'),
        },
    };

    public checkSubpageNavMatches(expectedSubpages: { subpageName: string, url: string; }[]): this {
        //Get the actual subpage nav items currently on the screen
        this.elements.currentPageSubnavLinks().should(($subpageNavElements) => {
            //Get the name and url out of the subpageNavElement jquery objects
            const actualSubpages = $subpageNavElements.map((_, subpageNavElement) => ({
                subpageName: Cypress.$(subpageNavElement).contents().last().text().replace(/\(\d+\)/, '').trim(), // Get the visible subpage name (not the hidden a11y name) without any bracketed numbers
                url: Cypress.$(subpageNavElement).attr('href')
            })).get();

            //Check that the actual subpages currently on the screen are the ones we are expecting to see
            expect(actualSubpages).to.deep.equal(expectedSubpages);
        });
        return this;
    }

    public checkBrowserPageTitleContains(pageTitle: string): this {
        cy.title().should('contain', pageTitle);
        return this;
    }

    public checkCurrentURLIsCorrect(urlPageName: string): this {
        cy.url().should('include', urlPageName);
        return this;
    }

    public checkPrivacyLinkPresent(): this {
        this.elements.privacyFooterButton().scrollIntoView().should('be.visible');
        return this;
    }

    public clickPrivacyLink(): this {
        this.elements.privacyFooterButton().scrollIntoView().click();
        return this;
    }

    public checkCookiesLinkPresent(): this {
        this.elements.cookiesFooterButton().scrollIntoView().should('be.visible');
        return this;
    }

    public clickCookiesLink(): this {
        this.elements.cookiesFooterButton().scrollIntoView().click();
        return this;
    }

    public checkAccessibilityStatementLinkPresent(): this {
        this.elements.accessibilityFooterButton().scrollIntoView().should('be.visible');
        return this;
    }

    public clickAccessibilityStatementLink(): this {
        this.elements.accessibilityFooterButton().scrollIntoView().click();
        return this;
    }

    public checkHomeBreadcrumbPresent(): this {
        this.elements.breadcrumbs.homeBreadcrumbButton().should('be.visible',);
        this.elements.breadcrumbs.homeBreadcrumbButton().should('not.be.disabled',);
        return this;
    }

    public checkTrustNameBreadcrumbPresent(trustname: string): this {
        this.elements.breadcrumbs.trustBreadcrumbLabel(trustname).should('be.visible');
        return this;
    }

    public checkPageNameBreadcrumbPresent(pageName: string): this {
        this.elements.breadcrumbs.pageNameBreadcrumbLabel().should('contain.text', pageName);
        return this;
    }

    public checkBreadcrumbNotPresent(): this {
        this.elements.breadcrumbs.breadcrumbParent().should('not.exist');
        return this;
    }

    public clickHomeBreadcrumbButton(): this {
        this.elements.breadcrumbs.homeBreadcrumbButton().click();
        return this;
    }

    public clickAcademiesServiceNavButton(): this {
        this.elements.serviceNav.academiesServiceNavButton().click();
        return this;
    }

    public checkAcademiesServiceNavButtonIsHighlighted(): this {
        this.elements.serviceNav.academiesServiceNavButton().should('have.class', 'govuk-service-navigation__item--active');
        return this;
    }

    public clickContactsServiceNavButton(): this {
        this.elements.serviceNav.contactsServiceNavButton().click();
        return this;
    }

    public checkContactsServiceNavButtonIsHighlighed(): this {
        this.elements.serviceNav.contactsServiceNavButton().should('have.class', 'govuk-service-navigation__item--active');
        return this;
    }

    public clickOfstedServiceNavButton(): this {
        this.elements.serviceNav.ofstedServiceNavButton().click();
        return this;
    }

    public checkOfstedServiceNavButtonIsHighlighted(): this {
        this.elements.serviceNav.ofstedServiceNavButton().should('have.class', 'govuk-service-navigation__item--active');
        return this;
    }

    public clickFinancialDocumentsServiceNavButton(): this {
        this.elements.serviceNav.financialDocumentsServiceNavButton().click();
        return this;
    }

    public checkFinancialDocumentsServiceNavButtonIsHighlighted(): this {
        this.elements.serviceNav.financialDocumentsServiceNavButton().should('have.class', 'govuk-service-navigation__item--active');
        return this;
    }

    public clickGovernanceServiceNavButton(): this {
        this.elements.serviceNav.governanceServiceNavButton().click();
        return this;
    }

    public checkGovernanceServiceNavButtonIsHighlighted(): this {
        this.elements.serviceNav.governanceServiceNavButton().should('have.class', 'govuk-service-navigation__item--active');
        return this;
    }

    public clickOverviewServiceNavButton(): this {
        this.elements.serviceNav.overviewServiceNavButton().click();
        return this;
    }

    public checkOverviewServiceNavButtonIsHighlighted(): this {
        this.elements.serviceNav.overviewServiceNavButton().should('have.class', 'govuk-service-navigation__item--active');
        return this;
    }

    public clickPupilNumbersAcadmiesTrustButton(): this {
        this.elements.acadmiesInTrustSubNav.academiesInTrustPupilNumbersButton().click();
        return this;
    }

    public clickFreeSchoolMealsAcadmiesTrustButton(): this {
        this.elements.acadmiesInTrustSubNav.academiesInTrustFreeSchoolMealsButton().click();
        return this;
    }

    public clickDetailsAcadmiesTrustButton(): this {
        this.elements.acadmiesInTrustSubNav.academiesInTrustDetailsButton().click();
        return this;
    }

    public checkAcademiesSubNavNotPresent(): this {
        this.elements.acadmiesInTrustSubNav.academiesInTrustDetailsButton().should('not.exist');
        this.elements.acadmiesInTrustSubNav.academiesInTrustPupilNumbersButton().should('not.exist');
        this.elements.acadmiesInTrustSubNav.academiesInTrustFreeSchoolMealsButton().should('not.exist');
        return this;
    }

    public checkAllAcademiesNavItemsPresent(): this {
        this.elements.acadmiesInTrustSubNav.academiesInTrustDetailsButton().should('be.visible');
        this.elements.acadmiesInTrustSubNav.academiesInTrustPupilNumbersButton().should('be.visible');
        this.elements.acadmiesInTrustSubNav.academiesInTrustFreeSchoolMealsButton().should('be.visible');
        return this;
    }

    public checkAllServiceNavItemsPresent(): this {
        this.elements.serviceNav.overviewServiceNavButton().should('be.visible');
        this.elements.serviceNav.contactsServiceNavButton().should('be.visible');
        this.elements.serviceNav.academiesServiceNavButton().should('be.visible');
        this.elements.serviceNav.ofstedServiceNavButton().should('be.visible');
        this.elements.serviceNav.financialDocumentsServiceNavButton().should('be.visible');
        this.elements.serviceNav.governanceServiceNavButton().should('be.visible');
        return this;
    }

    public clickAcademiesInThisTrustNavButton(): this {
        this.elements.academyTypeNav.inThisTrustButton().click();
        return this;
    }

    public clickPipelineAcademiesNavButton(): this {
        this.elements.academyTypeNav.pipelineAcademiesButton().click();
        return this;
    }

    public clickPipelineAcademiesPreAdvisoryNavButton(): this {
        this.elements.pipelineAcademiesSubNav.pipelineAcademiesPreAdvisoryButton().click();
        return this;
    }

    public clickPipelineAcademiesPostAdvisoryNavButton(): this {
        this.elements.pipelineAcademiesSubNav.pipelineAcademiesPostAdvisoryButton().click();
        return this;
    }

    public clickPipelineAcademiesFreeSchoolsNavButton(): this {
        this.elements.pipelineAcademiesSubNav.pipelineAcademiesFreeSchoolMealsButton().click();
        return this;
    }

    public clickFinancialDocsFinancialStatementsButton(): this {
        this.elements.financialDocumentsSubNav.financialStatementsNavButton().click();
        return this;
    }

    public clickFinancialDocsManagementLettersButton(): this {
        this.elements.financialDocumentsSubNav.managementLettersButton().click();
        return this;
    }

    public clickFinancialDocsInternalScrutinyReportsButton(): this {
        this.elements.financialDocumentsSubNav.internalScrutinyReportsButton().click();
        return this;
    }

    public clickFinancialDocsSelfAssessmentButton(): this {
        this.elements.financialDocumentsSubNav.selfAssessmentChecklistsButton().click();
        return this;
    }
}

const navigation = new Navigation();
export default navigation;
