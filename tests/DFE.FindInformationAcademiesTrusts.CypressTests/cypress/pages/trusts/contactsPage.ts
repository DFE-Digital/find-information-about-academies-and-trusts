class ContactsPage {

    elements = {
        TrustRelationshipManager: {
            Section: () => cy.get('[data-testid="trust-relationship-manager"]'),
            Title: () => this.elements.TrustRelationshipManager.Section().find('.govuk-summary-card__title'),
            Name: () => this.elements.TrustRelationshipManager.Section().find('[data-testid="contact-name"]'),
            Email: () => this.elements.TrustRelationshipManager.Section().find('[data-testid="contact-email"]'),
            EditLink: () => this.elements.TrustRelationshipManager.Section().find('[class="govuk-summary-card__actions"] > a')
        },

        SchoolsFinancialSupportOversight: {
            Section: () => cy.get('[data-testid="sfso-lead"]'),
            Title: () => this.elements.SchoolsFinancialSupportOversight.Section().find('.govuk-summary-card__title'),
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

        Datasource: {
            Section: () => cy.get('[data-testid="data-source-and-updates"]'),
            TRMLatestUpdatedBy: () => cy.get('[data-testid="data-source-fiatdb-trust-relationship-manager"] > :nth-child(3)'),
            SFSOLatestUpdatedBy: () => cy.get('[data-testid="data-source-fiatdb-sfso-(schools-financial-support-and-oversight)-lead"] > :nth-child(3)')
        },

        subNav: {
            contactsInDfeSubnavButton: () => cy.get('[data-testid="contacts-in-dfe-subnav"]'),
            contactsInTheTrustSubnavButton: () => cy.get('[data-testid="contacts-in-the-trust-subnav"]'),
        }
    };

    public editTrustRelationshipManager(name: string, email: string): this {
        const { TrustRelationshipManager, EditContacts } = this.elements;
        TrustRelationshipManager.EditLink().click();
        EditContacts.NameInput().clear().type(name);
        EditContacts.EmailInput().clear().type(email);
        EditContacts.SaveButton().click();
        return this;
    }

    public checkTrustRelationshipManagerIsSuccessfullyUpdated(name: string, email: string): this {
        this.elements.TrustRelationshipManager.Name().should('contain.text', name);
        this.elements.TrustRelationshipManager.Email().should('contain.text', email);
        return this;
    }

    public editSfsoLead(name: string, email: string): this {
        const { SchoolsFinancialSupportOversight, EditContacts } = this.elements;
        SchoolsFinancialSupportOversight.EditLink().click();
        EditContacts.NameInput().clear().type(name);
        EditContacts.EmailInput().clear().type(email);
        EditContacts.SaveButton().click();
        return this;
    }

    public checkSfsoLeadIsSuccessfullyUpdated(name: string, email: string): this {
        this.elements.SchoolsFinancialSupportOversight.Name().should('contain.text', name);
        this.elements.SchoolsFinancialSupportOversight.Email().should('contain.text', email);
        return this;
    }

    public checkTrustRelationshipManagerIsPresent(): this {
        this.elements.TrustRelationshipManager.Title().should('contain', 'Trust relationship manager');
        this.elements.TrustRelationshipManager.Name().should('be.visible');
        this.elements.TrustRelationshipManager.Email().should('be.visible');
        return this;
    }

    public checkSfsoLeadIsPresent(): this {
        this.elements.SchoolsFinancialSupportOversight.Title().should('contain', 'SFSO (Schools financial support and oversight) lead');
        this.elements.SchoolsFinancialSupportOversight.Name().should('be.visible');
        this.elements.SchoolsFinancialSupportOversight.Email().should('be.visible');
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

    public checkTrustRelationshipManagerDatasourceLastUpdatedByUser(expectedUser: string): this {
        const { Datasource } = this.elements;
        Datasource.Section().click();
        Datasource.Section().should('be.visible');
        Datasource.TRMLatestUpdatedBy().should('contain.text', expectedUser);
        return this;
    }

    public checkSfsoLeadDatasourceLastUpdatedByUser(expectedUser: string): this {
        const { Datasource } = this.elements;
        Datasource.Section().click();
        Datasource.Section().should('be.visible');
        Datasource.SFSOLatestUpdatedBy().should('contain.text', expectedUser);
        return this;
    }

    public clickContactsInDfeSubnavButton(): this {
        this.elements.subNav.contactsInDfeSubnavButton().click();
        return this;
    }

    public clickContactsInTheTrustSubnavButton(): this {
        this.elements.subNav.contactsInTheTrustSubnavButton().click();
        return this;
    }

    public checkAllSubNavItemsPresent(): this {
        this.elements.subNav.contactsInDfeSubnavButton().should('be.visible');
        this.elements.subNav.contactsInTheTrustSubnavButton().should('be.visible');
        return this;
    }

    public checkSubNavNotPresent(): this {
        this.elements.subNav.contactsInDfeSubnavButton().should('not.exist');
        this.elements.subNav.contactsInTheTrustSubnavButton().should('not.exist');
        return this;
    }

    public checkContactsInDfeSubnavButtonIsHighlighted(): this {
        this.elements.subNav.contactsInDfeSubnavButton().should('have.prop', 'aria-current', true);
        return this;
    }

    public checkContactsInTheTrustSubnavButtonIsHighlighted(): this {
        this.elements.subNav.contactsInTheTrustSubnavButton().should('have.prop', 'aria-current', true);
        return this;
    }
}

const contactsPage = new ContactsPage();
export default contactsPage;
