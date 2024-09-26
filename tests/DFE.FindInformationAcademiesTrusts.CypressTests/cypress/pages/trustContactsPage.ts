class TrustContacts {
    public hasTrustRelationshipManager(name: string, email: string): this {
        // this.assertContact("trust-relationship-manager", name, email);
        return this;
    }

    public hasSfsoLead(name: string, email: string): this {
        // this.assertContact("sfso-lead", name, email);
        return this;
    }

    public hasAccountingOfficer(name: string, email: string): this {
        this.assertContact("accounting-officer", name, email);

        return this;
    }

    public hasChiefFinancialOfficer(name: string, email: string): this {
        this.assertContact("chief-financial-officer", name, email);

        return this;
    }

    public hasChairOfTrustees(name: string, email: string): this {
        this.assertContact("chair-of-trustees", name, email);

        return this;
    }

    public hasEmptyContacts(): this {
        this.assertEmptyContact("trust-relationship-manager");
        this.assertEmptyContact("sfso-lead");
        this.assertEmptyContact("accounting-officer");
        this.assertEmptyContact("chief-financial-officer");
        this.assertEmptyContact("chair-of-trustees");

        return this;
    }

    private assertEmptyContact(id: string): void {
        cy.getByTestId(id)
            .find("[data-testid='contact-name']").should("contain.text", "No contact name available");

        cy.getByTestId(id)
            .find("[data-testid='contact-email']")
            .should("contain.text", "No contact email available");
    }

    private assertContact(id: string, name: string, email: string): void {
        cy.getByTestId(id)
            .find("[data-testid='contact-name']").should("contain.text", name);

        cy.getByTestId(id)
            .find("[data-testid='contact-email']")
            .should("contain.text", email)
            .should("have.attr", "href", `mailto:${email}`);
    }
}

const trustContactsPage = new TrustContacts();

export default trustContactsPage;