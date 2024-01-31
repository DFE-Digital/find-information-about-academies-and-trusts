class TrustDetailsPage {
    public hasAddress(value): this {

        cy.getByTestId("address").should("contain.text", value);
        return this;
    }

    public hasOpenedOn(value): this {
        cy.getByTestId("opened-on").should("contain.text", value);
        return this;
    }

    public hasRegionAndTerritory(value): this {
        cy.getByTestId("region-and-territory").should("contain.text", value);
        return this;
    }

    public hasUid(value): this {
        cy.getByTestId("trust-uid").should("contain.text", value);
        return this;
    }

    public hasTrustReferenceNumber(value): this {
        cy.getByTestId("trust-reference-number").should("contain.text", value);
        return this;
    }

    public hasUkprn(value): this {
        cy.getByTestId("ukprn").should("contain.text", value);
        return this;
    }

    public hasCompaniesHouseNumber(value): this {
        cy.getByTestId("companies-house-number").should("contain.text", value);
        return this;
    }
}

const trustDetailsPage = new TrustDetailsPage();

export default trustDetailsPage;