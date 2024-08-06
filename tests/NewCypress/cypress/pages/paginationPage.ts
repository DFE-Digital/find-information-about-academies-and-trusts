class PaginationPage {

    public checkImAtTheCorrectUrl(urlPageName: string): this {
        cy.url().should('include', urlPageName);

        return this;
    }

    public returnToHome(): this {
        const homeButton = () => cy.get('.dfe-header__link')

        homeButton().click();

        return this;
    }

}

const paginationPage = new PaginationPage();

export default paginationPage;
