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
            academiesServiceNavButton: () => cy.get('[data-testid="academies-nav"]'),
            contactsServiceNavButton: () => cy.get('[data-testid="contacts-nav"]'),
            governanceServiceNavButton: () => cy.get('[data-testid="governance-nav"]'),
            overviewServiceNavButton: () => cy.get('[data-testid="overview-nav"]'),
        },
        currentPageSubnavLinks: () => cy.get('.moj-sub-navigation__link'),
        acadmiesInTrustSubNav: {
            academiesInTrustofstedButton: () => cy.get('[data-testid="ofsted-nav"]'),
            academiesInTrustpupilNumbersButton: () => cy.get('[data-testid="in-this-trust-pupil-numbers-tab"]'),
            academiesInTrustfreeSchoolMealsButton: () => cy.get('[data-testid="in-this-trust-free-school-meals-tab"]'),
            academiesInTrustdetailsButton: () => cy.get('[data-testid="in-this-trust-details-tab"]'),
        },
        pipelineAcadmiesSubNav: {
            pipelineAcademiesPreAdvisoryButton: () => cy.get('[data-testid="pipeline-pre-advisory-board-tab"]'),
            pipelineAcademiesPostAdvisoryButton: () => cy.get('[data-testid="pipeline-post-advisory-board-tab"]'),
            pipelineAcademiesFreeSchoolMealsButton: () => cy.get('[data-testid="pipeline-free-schools-tab"]'),
        },
        breadcrumbs: {
            breadcrumbParent: () => cy.get('[aria-label="Breadcrumb"]'),
            homeBreadcrumbButton: () => this.elements.breadcrumbs.breadcrumbParent().contains('Home'),
            trustBreadcrumbLabel: (trustname: string) => this.elements.breadcrumbs.breadcrumbParent().contains(trustname),
            pageNameBreadcrumbLabel: () => this.elements.breadcrumbs.breadcrumbParent().find('[data-testid="breadcrumb-page-name"]')
        }
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

    public clickOfstedAcadmiesTrustButton(): this {
        this.elements.acadmiesInTrustSubNav.academiesInTrustofstedButton().click();
        return this;
    }

    public clickPupilNumbersAcadmiesTrustButton(): this {
        this.elements.acadmiesInTrustSubNav.academiesInTrustpupilNumbersButton().click();
        return this;
    }

    public clickFreeSchoolMealsAcadmiesTrustButton(): this {
        this.elements.acadmiesInTrustSubNav.academiesInTrustfreeSchoolMealsButton().click();
        return this;
    }

    public clickDetailsAcadmiesTrustButton(): this {
        this.elements.acadmiesInTrustSubNav.academiesInTrustdetailsButton().click();
        return this;
    }

    public checkAcademiesSubNavNotPresent(): this {
        this.elements.acadmiesInTrustSubNav.academiesInTrustdetailsButton().should('not.exist');
        this.elements.acadmiesInTrustSubNav.academiesInTrustpupilNumbersButton().should('not.exist');
        this.elements.acadmiesInTrustSubNav.academiesInTrustfreeSchoolMealsButton().should('not.exist');
        return this;
    }

    public checkAllAcademiesNavItemsPresent(): this {
        this.elements.acadmiesInTrustSubNav.academiesInTrustdetailsButton().should('be.visible');
        this.elements.acadmiesInTrustSubNav.academiesInTrustpupilNumbersButton().should('be.visible');
        this.elements.acadmiesInTrustSubNav.academiesInTrustfreeSchoolMealsButton().should('be.visible');
        return this;
    }

    public checkAllServiceNavItemsPresent(): this {
        this.elements.serviceNav.overviewServiceNavButton().should('be.visible');
        this.elements.serviceNav.contactsServiceNavButton().should('be.visible');
        this.elements.serviceNav.academiesServiceNavButton().should('be.visible');
        this.elements.serviceNav.governanceServiceNavButton().should('be.visible');
        this.elements.acadmiesInTrustSubNav.academiesInTrustofstedButton().should('be.visible');
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
        this.elements.pipelineAcadmiesSubNav.pipelineAcademiesPreAdvisoryButton().click();
        return this;
    }

    public clickPipelineAcademiesPostAdvisoryNavButton(): this {
        this.elements.pipelineAcadmiesSubNav.pipelineAcademiesPostAdvisoryButton().click();
        return this;
    }

    public clickPipelineAcademiesFreeSchoolsNavButton(): this {
        this.elements.pipelineAcadmiesSubNav.pipelineAcademiesFreeSchoolMealsButton().click();
        return this;
    }
}

const navigation = new Navigation();
export default navigation;
