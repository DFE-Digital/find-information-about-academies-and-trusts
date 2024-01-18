class AcademiesPupilNumbersTable {
    public getRow(value: string): Cypress.Chainable<AcademiesPupilNumbersRow> {
        cy.getByTestId("academy-row")
            .contains("th", value)
            .parent("tr")
            .as("targetedRow");

        return cy.get("@targetedRow")
            .then((el) => {
                return new AcademiesPupilNumbersRow(el);
            });
    }
}

class AcademiesPupilNumbersRow {
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

    public hasPhase(value: string): this {
        this.containsText("phase-and-age-range", value);

        return this;
    }

    public hasAgeRange(value: string): this {
        this.containsText("phase-and-age-range", value);

        return this;
    }

    public hasPupilNumbers(value: string): this {
        this.containsText("pupil-numbers", value);

        return this;
    }

    public hasPupilCapacity(value: string): this {
        this.containsText("pupil-capacity", value);

        return this;
    }

    public hasPercentageFull(value: string): this {
        this.containsText("percentage-full", value);

        return this;
    }

    private containsText(id: string, value: string) {
        cy.wrap(this.element)
            .within(() => {
                cy.getByTestId(id).should("contain.text", value);
            })

    }
}

const academiesPupilNumbersTable = new AcademiesPupilNumbersTable();

export default academiesPupilNumbersTable;