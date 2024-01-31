import { Logger } from "cypress/common/logger";

export class PaginationComponent {
    constructor() {

    }

    public next(): this {
        cy.getByTestId(`next-page`).click();

        return this;
    }

    public hasNoNext(): this {
        cy.getByTestId("next-page").should("not.exist");

        return this;
    }

    public previous(): this {
        cy.getByTestId(`previous-page`).click();

        return this;
    }

    public hasNoPrevious(): this {
        cy.getByTestId("previous-page").should("not.exist");

        return this;
    }

    public goToPage(pageNumber: string) {
        Logger.log(`Moving to page ${pageNumber}`);

        cy.getByTestId(`page-${pageNumber}`).click();

        return this;
    }

    public isCurrentPage(pageNumber: string): this {
        cy.getByTestId(`page-${pageNumber}`).parent().should("have.class", "govuk-pagination__item--current");

        return this;
    }
}

const paginationComponent = new PaginationComponent();

export default paginationComponent;