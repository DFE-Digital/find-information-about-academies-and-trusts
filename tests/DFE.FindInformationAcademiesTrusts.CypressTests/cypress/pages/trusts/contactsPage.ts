import commonPage from "../commonPage";
class ContactsPage {

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

        Datasource: {
            Section: () => cy.get('.govuk-details'),
            TRMLatestUpdatedBy: () => cy.get('[data-testid="data-source-fiatdb-trust-relationship-manager"] > :nth-child(3)'),
            SFSOLatestUpdatedBy: () => cy.get('[data-testid="data-source-fiatdb-sfso-(schools-financial-support-and-oversight)-lead"] > :nth-child(3)')
        }
    };


    public editTRM(name: string, email: string): this {
        const { TrustRelationshipManager, EditContacts } = this.elements;
        TrustRelationshipManager.EditLink().click();
        EditContacts.NameInput().clear().type(name);
        EditContacts.EmailInput().clear().type(email);
        EditContacts.SaveButton().click();
        return this;
    }

    public checkTRMFieldsAndDatasource(name: string, email: string): this {
        contactsPage
            .editTRM(name, email);
        contactsPage.elements.TrustRelationshipManager
            .Name().should('contain.text', name);
        contactsPage.elements.TrustRelationshipManager
            .Email().should('contain.text', email);
        commonPage
            .checkSuccessPopup('Changes made to the Trust relationship manager name and email were updated')
            .checkErrorPopupNotPresent('Enter a DfE email address without any spaces')
            .checkErrorPopupNotPresent('Enter an email address in the correct format, like name@education.gov.uk');

        cy.contains("Source and updates").click();
        contactsPage
            .checkLatestTRMDatasourceUser('Automation User - email');
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

    public checkSFSOFieldsAndDatasource(name: string, email: string): this {
        contactsPage
            .editSFSO(name, email);
        contactsPage.elements.SchoolsFinancialSupportOversight
            .Name().should('contain.text', name);
        contactsPage.elements.SchoolsFinancialSupportOversight
            .Email().should('contain.text', email);
        commonPage
            .checkSuccessPopup('Changes made to the SFSO (Schools financial support and oversight) lead name and email were updated')
            .checkErrorPopupNotPresent('Enter a DfE email address without any spaces')
            .checkErrorPopupNotPresent('Enter an email address in the correct format, like name@education.gov.uk');

        cy.contains("Source and updates").click();
        contactsPage
            .checkLatestSFSODatasourceUser('Automation User - email');
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

    public checkLatestTRMDatasourceUser(expectedMessage: string): this {
        const { Datasource } = this.elements;
        Datasource.Section().should('be.visible');
        Datasource.TRMLatestUpdatedBy().should('contain.text', expectedMessage);
        return this;
    }

    public checkLatestSFSODatasourceUser(expectedMessage: string): this {
        const { Datasource } = this.elements;
        Datasource.Section().should('be.visible');
        Datasource.SFSOLatestUpdatedBy().should('contain.text', expectedMessage);
        return this;
    }
}

const contactsPage = new ContactsPage();
export default contactsPage;
