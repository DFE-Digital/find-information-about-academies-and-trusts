class FooterLinks {

    public privacyLinkPresent(): this {
        const privacyFooterButton = () => cy.contains('Privacy')

        privacyFooterButton().scrollIntoView().should('be.visible');

        return this;
    }

    public clickPrivacyLink(): this {
        const privacyFooterButton = () => cy.contains('Privacy')

        privacyFooterButton().scrollIntoView().click();

        return this;
    }

    public cookiesLinkPresent(): this {
        const cookiesFooterButton = () => cy.get('[data-testid="cookies-footer-link"]')

        cookiesFooterButton().scrollIntoView().should('be.visible');

        return this;
    }

    public clickCookiesLink(): this {
        const cookiesFooterButton = () => cy.get('[data-testid="cookies-footer-link"]')

        cookiesFooterButton().scrollIntoView().click();

        return this;
    }

}

const footerLinks = new FooterLinks();

export default footerLinks;
