class Navigation {

    elements = {
        privacyFooterButton: () => cy.contains('Privacy'),
        cookiesFooterButton: () => cy.get('[data-testid="cookies-footer-link"]'),
        accessibilityFooterButton: () => cy.contains('Accessibility'),


        serviceNav: {
            academiesServiceNavButton: () => cy.get('[data-testid="academies-nav"]'),
            contactsServiceNavButton: () => cy.get('[data-testid="contacts-nav"]'),
            governanceServiceNavButton: () => cy.get('[data-testid="governance-nav"]'),
            overviewServiceNavButton: () => cy.get('[data-testid="overview-nav"]'),
        },

        currentPageSubnavLinks: () => cy.get('.moj-sub-navigation__link'),

        acadmiesSubNav: {
            ofstedAcadmiesTrustButton: () => cy.get('[data-testid="ofsted-nav"]'),
            pupilNumbersAcadmiesTrustButton: () => cy.get('#academies-pupil-numbers-link'),
            freeSchoolMealsAcadmiesTrustButton: () => cy.get('#free-school-meals-link'),
            detailsAcadmiesTrustButton: () => cy.get('#academies-details-link'),
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
        this.elements.acadmiesSubNav.ofstedAcadmiesTrustButton().click();
        return this;
    }

    public clickPupilNumbersAcadmiesTrustButton(): this {
        this.elements.acadmiesSubNav.pupilNumbersAcadmiesTrustButton().click();
        return this;
    }

    public clickFreeSchoolMealsAcadmiesTrustButton(): this {
        this.elements.acadmiesSubNav.freeSchoolMealsAcadmiesTrustButton().click();
        return this;
    }

    public clickDetailsAcadmiesTrustButton(): this {
        this.elements.acadmiesSubNav.detailsAcadmiesTrustButton().click();
        return this;
    }

    public checkAcademiesSubNavNotPresent(): this {
        this.elements.acadmiesSubNav.detailsAcadmiesTrustButton().should('not.exist');
        this.elements.acadmiesSubNav.pupilNumbersAcadmiesTrustButton().should('not.exist');
        this.elements.acadmiesSubNav.freeSchoolMealsAcadmiesTrustButton().should('not.exist');
        return this;
    }

    public checkAllAcademiesNavItemsPresent(): this {
        this.elements.acadmiesSubNav.detailsAcadmiesTrustButton().should('be.visible');
        this.elements.acadmiesSubNav.pupilNumbersAcadmiesTrustButton().should('be.visible');
        this.elements.acadmiesSubNav.freeSchoolMealsAcadmiesTrustButton().should('be.visible');
        return this;
    }

    public checkAllServiceNavItemsPresent(): this {
        this.elements.serviceNav.overviewServiceNavButton().should('be.visible');
        this.elements.serviceNav.contactsServiceNavButton().should('be.visible');
        this.elements.serviceNav.academiesServiceNavButton().should('be.visible');
        this.elements.serviceNav.governanceServiceNavButton().should('be.visible');
        this.elements.acadmiesSubNav.ofstedAcadmiesTrustButton().should('be.visible');
        return this;
    }
}

const navigation = new Navigation();
export default navigation;
