class PaginationPage {

    public checkCurrentURLIsCorrect(urlPageName: string): this {
        cy.url().should('include', urlPageName);
        return this;
    }

    public returnToHome(): this {
        const homeButton = () => cy.get('.dfe-header__link');
        homeButton().click();
        return this;
    }

    public getPaginationButton(index: number): Cypress.Chainable<JQuery<HTMLElement>> {
        return cy.get(`[data-testid="page-${index}"]`);
    }

    public getTotalPaginationButtons(): Cypress.Chainable<number> {
        return cy.get('[data-testid^="page-"]').its('length');
    }

    public checkSingleResultOnlyHasOnePage(pageNumber: number): this {
        const getPagination = () => cy.get('.govuk-pagination');
        getPagination().should('contain', pageNumber);

        getPagination().should('not.contain', 2 , 3);
        return this;
    }

    public checkExpectedPageNumberInPaginationBar(pageNumber: number): this {
        const getPagination = () => cy.get('.govuk-pagination');
        getPagination().should('contain', pageNumber);
        return this;
    }

    public checkResultIsNotInPaginationBar(pageNumber: number): this {
        const getPagination = () => cy.get('.govuk-pagination');
        getPagination().should('not.contain', pageNumber);
        return this;
    }

    public clickPageNumber(pageNumber: number): this {
        const getPageNumberButton = () => this.getPaginationButton(pageNumber);
        getPageNumberButton().click();
        return this;
    }

    public clickNext(): this {
        const getNextButtonElement = () => cy.contains('Next');
        getNextButtonElement().click();
        return this;
    }

    public clickPrevious(): this {
        const getPreviousButtonElement = () => cy.contains('Previous');
        getPreviousButtonElement().click();
        return this;
    }

    public checkPreviousButtonNotPresent(): this {
        const getPreviousButtonElement = () => cy.contains('Previous');
        getPreviousButtonElement().should('not.exist');
        return this;
    }

    public checkNextButtonNotPresent(): this {
        const getNextButtonElement = () => cy.contains('Next');
        getNextButtonElement().should('not.exist');
        return this;
    }

    public getResults(): Cypress.Chainable<JQuery<HTMLElement>> {
        return cy.get('.govuk-list > :nth-child(1)');
    }

}

const paginationPage = new PaginationPage();

export default paginationPage;
