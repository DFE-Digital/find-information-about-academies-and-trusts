class CookiesPage {


    public navigateToCookiesPage(): this {
        cy.visit('/cookies')

        return this;
    }

    public acceptCookies(): this {
        const acceptCookiesYesButton = () => cy.get('.govuk-radios').contains('Yes');

        acceptCookiesYesButton().click();

        return this;
    }

}

const cookiesPage = new CookiesPage();

export default cookiesPage;
