class TrustContactsPage {

    elements = {
        TrustRelationshipManager: {
            Section: () => cy.get('[data-testid="trust-relationship-manager"]'),
            Name: () => this.elements.TrustRelationshipManager.Section().find('[data-testid="contact-name"]'),
            Email: () => this.elements.TrustRelationshipManager.Section().find('[data-testid="contact-email"]'),
            EditLink: () => this.elements.TrustRelationshipManager.Section().find('[class="govuk-summary-card__actions"] > a')
        },

        SchoolsFinancialSupportOversight: {
            Section: () => cy.get('[data-testid="sfso-lead"]'),
            Name: () => this.elements.SchoolsFinancialSupportOversight.Section().find('[data-testid="contact-name"]'),
            Email: () => this.elements.SchoolsFinancialSupportOversight.Section().find('[data-testid="contact-email"]'),
            EditLink: () => this.elements.SchoolsFinancialSupportOversight.Section().find('[class="govuk-summary-card__actions"] > a')
        },
        
        EditContacts: {
            NameInput: () => cy.get('[name="Name"]'),
            EmailInput: () => cy.get('[name="Email"]'),
            SaveButton: () => cy.contains('Save and continue')
        },

        AccountingOfficer: {
            Section: () => cy.get('[data-testid="accounting-officer"]'),
            Title: () => cy.get('[data-testid="accounting-officer"] > .govuk-summary-card__title-wrapper > .govuk-summary-card__title'),
            Name: () => this.elements.AccountingOfficer.Section().find('[data-testid="contact-name"]'),
            Email: () => this.elements.AccountingOfficer.Section().find('[data-testid="contact-email"]'),
        },

        ChairOfTrustees: {
            Section: () => cy.get('[data-testid="chair-of-trustees"]'),
            Title: () => cy.get('[data-testid="chair-of-trustees"] > .govuk-summary-card__title-wrapper > .govuk-summary-card__title'),
            Name: () => this.elements.ChairOfTrustees.Section().find('[data-testid="contact-name"]'),
            Email: () => this.elements.ChairOfTrustees.Section().find('[data-testid="contact-email"]'),
        },

        ChiefFinancialOfficer: {
            Section: () => cy.get('[data-testid="chief-financial-officer"]'),
            Title: () => cy.get('[data-testid="chief-financial-officer"] > .govuk-summary-card__title-wrapper > .govuk-summary-card__title'),
            Name: () => this.elements.ChiefFinancialOfficer.Section().find('[data-testid="contact-name"]'),
            Email: () => this.elements.ChiefFinancialOfficer.Section().find('[data-testid="contact-email"]'),
        },
    };


    public editTRM(name: string, email: string): this {
        const { TrustRelationshipManager, EditContacts } = this.elements;
        TrustRelationshipManager.EditLink().click();
        EditContacts.NameInput().clear().type(name);
        EditContacts.EmailInput().clear().type(email);
        EditContacts.SaveButton().click();
        return this;
    }

    public editSFSO(name: string, email: string): this {
        const { SchoolsFinancialSupportOversight, EditContacts } = this.elements;
        SchoolsFinancialSupportOversight.EditLink().click();
        EditContacts.NameInput().clear().type(name);
        EditContacts.EmailInput().clear().type(email);
        EditContacts.SaveButton().click();
        return this;
    }

    public checkAccountingOfficerPresent(): this {
        this.elements.AccountingOfficer.Title().should('contain', 'Accounting officer');
        this.elements.AccountingOfficer.Name().should('be.visible');
        this.elements.AccountingOfficer.Email().should('be.visible');
        return this;
    }

    public checkChairOfTrusteesPresent(): this {
        this.elements.ChairOfTrustees.Title().should('contain', 'Chair of trustees');
        this.elements.ChairOfTrustees.Name().should('be.visible');
        this.elements.ChairOfTrustees.Email().should('be.visible');
        return this;
    }

    public checkChiefFinancialOfficerPresent(): this {
        this.elements.ChiefFinancialOfficer.Title().should('contain', 'Chief financial officer');
        this.elements.ChiefFinancialOfficer.Name().should('be.visible');
        this.elements.ChiefFinancialOfficer.Email().should('be.visible');
        return this;
    }



}

const trustContactsPage = new TrustContactsPage();
export default trustContactsPage;