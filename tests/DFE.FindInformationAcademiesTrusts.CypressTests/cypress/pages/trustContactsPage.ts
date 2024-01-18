class TrustContacts {
    public hasTrustRelationshipManager(name: string, email: string): this {
        this.assertContact("trust-relationship-manager", name, email);
        return this;
    }

    public hasSfsoLead(name: string, email: string): this {
        this.assertContact("sfso-lead", name, email);

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