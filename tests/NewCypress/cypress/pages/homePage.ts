class HomePage {

    public enterMainSearchText(searchText: string): this {
        const getMainSearchBox = () => cy.get('#home-search');
        getMainSearchBox().type(searchText);

        return this;
    }

    public clickMainSearchButton(): this {
        const mainSearchButton = () => cy.get('[data-testid="search"]');

        mainSearchButton().click();

        return this;
    }

    public mainSearchButtonPresent(): this {
        const mainSearchButton = () => cy.get('[data-testid="search"]');

        mainSearchButton().should('be.visible');
        mainSearchButton().should('be.enabled');
        return this;
    }


}

const homePage = new HomePage();

export default homePage;
