import { AcademiesTableBase } from "./academiesTableBase";

class AcademiesDetailsTable extends AcademiesTableBase {
    public getRow(value: string): Cypress.Chainable<AcademiesDetailsRow> {
        this.getTableRowElement(value).as("targetedRow");

        return cy.get("@targetedRow")
            .then((el) => {
                return new AcademiesDetailsRow(el);
            });
    }
}

class AcademiesDetailsRow {
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

    public hasLocalAuthority(value: string): this {
        this.containsText("local-authority", value);

        return this;
    }

    public hasTypeOfEstablishment(value: string): this {
        this.containsText("type-of-establishment", value);

        return this;
    }

    public hasUrbanOrRural(value: string): this {
        this.containsText("urban-or-rural", value);

        return this;
    }

    private containsText(id: string, value: string) {
        cy.wrap(this.element)
            .within(() => {
                cy.getByTestId(id).should("contain.text", value);
            })

    }
}

const academiesDetailsTable = new AcademiesDetailsTable();

export default academiesDetailsTable;