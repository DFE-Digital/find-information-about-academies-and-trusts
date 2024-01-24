export class AcademiesTableBase {

    public getTableRowElement(value: string): Cypress.Chainable<JQuery<HTMLTableRowElement>> {
        return cy.getByTestId("academy-row")
            .contains("th", value)
            .parent("tr");
    }

    public hasNoRows(): this {
        cy.getByTestId("academy-row")
            .should("not.exist");

        return this;

    }

}