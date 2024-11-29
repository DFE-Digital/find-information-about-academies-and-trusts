class Navigation {

    elements = {
        privacyFooterButton: () => cy.contains('Privacy'),
        cookiesFooterButton: () => cy.get('[data-testid="cookies-footer-link"]'),
        accessibilityFooterButton: () => cy.contains('Accessibility'),
        breadcrumb: () => cy.get('[aria-label="Breadcrumb"]'),
        homeBreadcrumbButton: () => this.elements.breadcrumb().contains('Home'),
        trustBreadcrumbLabel: (trustname: string) => this.elements.breadcrumb().contains(trustname),

        serviceNav: {
            academiesServiceNavButton: () => cy.get('[data-testid="academies-nav"]'),
            contactsServiceNavButton: () => cy.get('[data-testid="contacts-nav"]'),
            governanceServiceNavButton: () => cy.get('[data-testid="governance-nav"]'),
            overviewServiceNavButton: () => cy.get('[data-testid="overview-nav"]'),
        },

        acadmiesSubNav: {
            ofstedAcadmiesTrustButton: () => cy.get('[data-testid="ofsted-nav"]'),
            pupilNumbersAcadmiesTrustButton: () => cy.get('#academies-pupil-numbers-link'),
            freeSchoolMealsAcadmiesTrustButton: () => cy.get('#free-school-meals-link'),
            detailsAcadmiesTrustButton: () => cy.get('#academies-details-link'),
        },
    };

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
        this.elements.homeBreadcrumbButton().should('be.visible',);
        this.elements.homeBreadcrumbButton().should('not.be.disabled',);
        return this;
    }

    public checkTrustNameBreadcrumbPresent(trustname: string): this {
        this.elements.trustBreadcrumbLabel(trustname).should('be.visible');
        return this;
    }

    public checkBreadcrumbNotPresent(): this {
        this.elements.breadcrumb().should('not.exist');
        return this;
    }

    public clickHomeBreadcrumbButton(): this {
        this.elements.homeBreadcrumbButton().click();
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
        this.elements.acadmiesSubNav.ofstedAcadmiesTrustButton().should('be.visible');
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
