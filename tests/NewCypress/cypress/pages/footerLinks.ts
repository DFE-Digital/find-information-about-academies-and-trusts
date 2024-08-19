class FooterLinks {

    public checkPrivacyLinkPresent(): this {
        const privacyFooterButton = () => cy.contains('Privacy')

        privacyFooterButton().scrollIntoView().should('be.visible');

        return this;
    }

    public clickPrivacyLink(): this {
        const privacyFooterButton = () => cy.contains('Privacy')

        privacyFooterButton().scrollIntoView().click();

        return this;
    }

    public checkCookiesLinkPresent(): this {
        const cookiesFooterButton = () => cy.get('[data-testid="cookies-footer-link"]')

        cookiesFooterButton().scrollIntoView().should('be.visible');

        return this;
    }

    public clickCookiesLink(): this {
        const cookiesFooterButton = () => cy.get('[data-testid="cookies-footer-link"]')

        cookiesFooterButton().scrollIntoView().click();

        return this;
    }

    public checkAcessibilityStatementLinkPresent(): this {
        const acessibilityFooterButton = () => cy.contains('Accessibility')

        acessibilityFooterButton().scrollIntoView().should('be.visible');

        return this;
    }

    public clickAccessibilityStatementLink(): this {
        const acessibilityFooterButton = () => cy.contains('Accessibility')

        acessibilityFooterButton().scrollIntoView().click();

        return this;
    }

}

const footerLinks = new FooterLinks();

export default footerLinks;
