class AcademiesOfstedRatingsTable {
    public getRow(value: string): Cypress.Chainable<AcademiesOfsetRatingsRow> {
        cy.getByTestId("academy-row")
            .contains("th", value)
            .parent("tr")
            .as("targetedRow");

        return cy.get("@targetedRow")
            .then((el) => {
                return new AcademiesOfsetRatingsRow(el);
            });
    }

    public hasNoRows(): this {
        cy.getByTestId("academy-row")
            .should("not.exist");

        return this;

    }
}

class AcademiesOfsetRatingsRow {
    constructor(private element: JQuery<Element>) {

    }

    public hasUrn(value: string): this {
        this.containsText("urn", value);

        return this;
    }

    public hasName(value: string): this {
        this.containsText("school-name", value);

        return this;
    }

    public hasDateJoined(value: string): this {

        this.containsText("date-joined", value);

        return this;
    }

    public hasPreviousOfstedRating(rating: string, date: string, beforeOrAfterJoining: string): this {

        this.containsText("previous-ofsted-rating", rating);
        this.containsText("previous-ofsted-rating", date);
        this.containsText("previous-ofsted-rating", beforeOrAfterJoining);

        return this;
    }

    public hasCurrentOfstedRating(value: string, date: string, beforeOrAfterJoining): this {

        this.containsText("current-ofsted-rating", value);
        this.containsText("current-ofsted-rating", date);
        this.containsText("previous-ofsted-rating", beforeOrAfterJoining);

        return this;
    }

    private containsText(id: string, value: string) {
        cy.wrap(this.element)
            .within(() => {
                cy.getByTestId(id).should("contain.text", value);
            })

    }
}

const academiesOfstedRatingsTable = new AcademiesOfstedRatingsTable();

export default academiesOfstedRatingsTable;