class AcademiesFreeSchoolMealsTable {
    public getRow(value: string): Cypress.Chainable<AcademiesFreeSchoolMealsRow> {
        cy.getByTestId("academy-row")
            .contains("th", value)
            .parent("tr")
            .as("targetedRow");

        return cy.get("@targetedRow")
            .then((el) => {
                return new AcademiesFreeSchoolMealsRow(el);
            });
    }

    public hasNoRows(): this {
        cy.getByTestId("academy-row")
            .should("not.exist");

        return this;
    }
}

class AcademiesFreeSchoolMealsRow {
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

    public hasPupilsEligible(value: string): this {
        this.containsText("pupils-eligible", value);

        return this;
    }

    public hasLocalAuthorityAverage(value: string): this {
        this.containsText("local-authority-average", value);

        return this;

    }

    public hasNationalAverage(value: string): this {
        this.containsText("national-average", value);

        return this;
    }

    private containsText(id: string, value: string) {
        cy.wrap(this.element)
            .within(() => {
                cy.getByTestId(id).should("contain.text", value);
            })

    }
}

const academiesFreeSchoolMealsTable = new AcademiesFreeSchoolMealsTable();

export default academiesFreeSchoolMealsTable;