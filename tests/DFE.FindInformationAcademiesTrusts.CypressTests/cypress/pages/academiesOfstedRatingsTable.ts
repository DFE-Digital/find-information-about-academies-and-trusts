import { AcademiesTableBase } from "./academiesTableBase";

class AcademiesOfstedRatingsTable extends AcademiesTableBase {
    public getRow(value: string): Cypress.Chainable<AcademiesOfsetRatingsRow> {
        this.getTableRowElement(value).as("targetedRow");

        return cy.get("@targetedRow")
            .then((el) => {
                return new AcademiesOfsetRatingsRow(el);
            });
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

        this.hasOfstedRating("previous-ofsted-rating", rating, date, beforeOrAfterJoining);

        return this;
    }

    public hasCurrentOfstedRating(rating: string, date: string, beforeOrAfterJoining): this {

        this.hasOfstedRating("current-ofsted-rating", rating, date, beforeOrAfterJoining);

        return this;
    }

    private hasOfstedRating(id: string, rating: string, date: string, beforeOrAfterJoining: string): void {
        if (rating === "Not yet inspected") {
            this.containsText(id, "Not yet inspected");

            return;
        }

        this.containsText(id, rating);
        this.containsText(id, date);
        this.containsText(id, beforeOrAfterJoining);
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