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

    public next(): this {
        cy.get(`next-page`).click();

        return this;
    }

    public hasNoNext(): this {
        cy.get("next-page").should("not.exist");

        return this;
    }

    public previous(): this {
        cy.get(`previous-page`).click();

        return this;
    }

    public hasNoPrevious(): this {
        cy.get("previous-page").should("not.exist");

        return this;
    }

    public goToPage(pageNumber: string) {

        cy.get(`page-${pageNumber}`).click();

        return this;
    }

    public isCurrentPage(pageNumber: string): this {
        cy.get(`page-${pageNumber}`).parent().should("have.class", "govuk-pagination__item--current");

        return this;
    }
}

const paginationPage = new PaginationPage();

export default paginationPage;
