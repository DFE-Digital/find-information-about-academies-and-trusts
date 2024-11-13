class Navigation {

    elements = {
        privacyFooterButton: () => cy.contains('Privacy'),
        cookiesFooterButton: () => cy.get('[data-testid="cookies-footer-link"]'),
        accessibilityFooterButton: () => cy.contains('Accessibility'),
        breadcrumb: () => cy.get('[aria-label="Breadcrumb"]'),
        homeBreadcrumbButton: () => this.elements.breadcrumb().contains('Home'),
        trustBreadcrumbLabel: (trustname: string) => this.elements.breadcrumb().contains(trustname),

        ServiceNav: {
            academiesInThisTrustServiceNavButton: () => cy.get('[data-testid="academies-nav"]'),
            ContactsServiceNavButton: () => cy.get('[data-testid="contacts-nav"]'),
            GovernanceServiceNavButton: () => cy.get('[data-testid="governance-nav"]'),
            OverviewServiceNavButton: () => cy.get('[data-testid="overview-nav"]'),
        },

        AcadmiesInThisTrustNav: {
            OfstedAcadmiesTrustButton: () => cy.get('#ofsted-ratings-link'),
            PupilNumbersAcadmiesTrustButton: () => cy.get('#academies-pupil-numbers-link'),
            FreeSchoolMealsAcadmiesTrustButton: () => cy.get('#free-school-meals-link'),
            DetailsAcadmiesTrustButton: () => cy.get('#academies-details-link'),
        },
    };

    public checkBrowserPageTitleContains(pageTitle: string): this {
        cy.title().should('contain', pageTitle)
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
        this.elements.homeBreadcrumbButton().should('be.visible',)
        this.elements.homeBreadcrumbButton().should('not.be.disabled',);
        return this;
    }

    public checkTrustNameBreadcrumbPresent(trustname: string): this {
        this.elements.trustBreadcrumbLabel(trustname).should('be.visible')
        return this;
    }

    public checkBreadcrumbNotPresent(): this {
        this.elements.breadcrumb().should('not.exist')
        return this;
    }

    public clickHomeBreadcrumbButton(): this {
        this.elements.homeBreadcrumbButton().click()
        return this;
    }

    public clickAcademiesInThisTrustServiceNavButton(): this {
        this.elements.ServiceNav.academiesInThisTrustServiceNavButton().click()
        return this;
    }

    public clickContactsServiceNavButton(): this {
        this.elements.ServiceNav.ContactsServiceNavButton().click()
        return this;
    }

    public clickGovernanceServiceNavButton(): this {
        this.elements.ServiceNav.GovernanceServiceNavButton().click()
        return this;
    }

    public clickOverviewServiceNavButton(): this {
        this.elements.ServiceNav.OverviewServiceNavButton().click()
        return this;
    }

    public clickOfstedAcadmiesTrustButton(): this {
        this.elements.AcadmiesInThisTrustNav.OfstedAcadmiesTrustButton().click()
        return this;
    }

    public clickPupilNumbersAcadmiesTrustButton(): this {
        this.elements.AcadmiesInThisTrustNav.PupilNumbersAcadmiesTrustButton().click()
        return this;
    }

    public clickFreeSchoolMealsAcadmiesTrustButton(): this {
        this.elements.AcadmiesInThisTrustNav.FreeSchoolMealsAcadmiesTrustButton().click()
        return this;
    }

    public clickDetailsAcadmiesTrustButton(): this {
        this.elements.AcadmiesInThisTrustNav.DetailsAcadmiesTrustButton().click()
        return this;
    }

    public checkAcademiesInTrustNavNotPresent(): this {
        this.elements.AcadmiesInThisTrustNav.DetailsAcadmiesTrustButton().should('not.exist')
        this.elements.AcadmiesInThisTrustNav.OfstedAcadmiesTrustButton().should('not.exist')
        this.elements.AcadmiesInThisTrustNav.PupilNumbersAcadmiesTrustButton().should('not.exist')
        this.elements.AcadmiesInThisTrustNav.FreeSchoolMealsAcadmiesTrustButton().should('not.exist')
        return this;
    }

    public checkAllAcademiesInTrustNavItemsPresent(): this {
        this.elements.AcadmiesInThisTrustNav.DetailsAcadmiesTrustButton().should('be.visible')
        this.elements.AcadmiesInThisTrustNav.OfstedAcadmiesTrustButton().should('be.visible')
        this.elements.AcadmiesInThisTrustNav.PupilNumbersAcadmiesTrustButton().should('be.visible')
        this.elements.AcadmiesInThisTrustNav.FreeSchoolMealsAcadmiesTrustButton().should('be.visible')
        return this;
    }

    public checkAllServiceNavItemsPresent(): this {
        this.elements.ServiceNav.OverviewServiceNavButton().should('be.visible')
        this.elements.ServiceNav.ContactsServiceNavButton().should('be.visible')
        this.elements.ServiceNav.academiesInThisTrustServiceNavButton().should('be.visible')
        this.elements.ServiceNav.GovernanceServiceNavButton().should('be.visible')
        return this;
    }
}

const navigation = new Navigation();
export default navigation;
