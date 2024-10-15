class CommonPage {

    elements = {
        SuccessPopup: {
            Section: () => cy.get('.govuk-notification-banner'),
            Message: () => this.elements.SuccessPopup.Section().find('.govuk-notification-banner__content')
        },

        ErrorPopup: {
            Section: () => cy.get('.govuk-error-summary'),
            Message: () => this.elements.ErrorPopup.Section().find('[data-testid="error-summary"]')
        },

        Datasource: {
            Section: () => cy.get('.govuk-details'),
            LatestUpdatedBy: () => cy.get('[data-testid="data-source-fiatdb-trust-relationship-manager"] > :nth-child(3)')
        }
    };

    public checkSuccessPopup(expectedMessage: string): this {
        const { SuccessPopup } = this.elements;
        SuccessPopup.Section().should('be.visible');
        SuccessPopup.Message().should('contain.text', expectedMessage);
        return this;
    }

    public checkErrorPopup(expectedMessage: string): this {
        const { ErrorPopup } = this.elements;
        ErrorPopup.Section().should('be.visible');
        ErrorPopup.Message().should('contain.text', expectedMessage);
        return this;
    }

    public checkLatestDatasourceUser(expectedMessage: string): this {
        const { Datasource } = this.elements;
        Datasource.Section().should('be.visible');
        Datasource.LatestUpdatedBy().should('contain.text', expectedMessage);
        return this;
    }
}

const commonPage = new CommonPage();
export default commonPage;