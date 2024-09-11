class Navigation {

    elements = {
        privacyFooterButton: () => cy.contains('Privacy'),
        cookiesFooterButton: () => cy.get('[data-testid="cookies-footer-link"]'),
        accessibilityFooterButton: () => cy.contains('Accessibility'),
        homeBreadcrumbButton: () => cy.contains('Home'),
        trustBreadcrumbButton: (trustname: string) => cy.get('.govuk-breadcrumbs__list > :nth-child(2)').contains(trustname)
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
        this.elements.trustBreadcrumbButton(trustname).should('be.visible')
        return this;
    }

    public clickHomeBreadcrumbButton(): this {
        this.elements.homeBreadcrumbButton().click()

        return this;
    }

}

const navigation = new Navigation();
export default navigation;
