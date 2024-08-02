class GeneralAndNavPage {

    public returnToHome(): this {
        const homeButton = () => cy.get('.dfe-header__link')

        homeButton().click();

        return this;
    }

    public homePagePrivacyLinkPresent(): this {
        const privacyFooterButton = () => cy.contains('Privacy')

        privacyFooterButton().scrollIntoView().click();

        return this;
    }


}

const generalAndNavPage = new GeneralAndNavPage();

export default generalAndNavPage;
