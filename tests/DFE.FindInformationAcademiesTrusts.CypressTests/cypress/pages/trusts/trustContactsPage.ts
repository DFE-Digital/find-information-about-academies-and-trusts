class TrustContactsPage {

    elements = {
        TrustRelationshipManager: {
            Section: () => cy.get('[data-testid="trust-relationship-manager"]'),
            Name: () => this.elements.TrustRelationshipManager.Section().find('[data-testid="contact-name"]'),
            Email: () => this.elements.TrustRelationshipManager.Section().find('[data-testid="contact-email"]'),
            EditLink: () => this.elements.TrustRelationshipManager.Section().find('[class="govuk-summary-card__actions"] > a')
        },
        EditContacts: {
            NameInput: () => cy.get('[name="Name"]'),
            EmailInput: () => cy.get('[name="Email"]'),
            SaveButton: () => cy.contains('Save and continue')
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


}

const trustContactsPage = new TrustContactsPage();
export default trustContactsPage;