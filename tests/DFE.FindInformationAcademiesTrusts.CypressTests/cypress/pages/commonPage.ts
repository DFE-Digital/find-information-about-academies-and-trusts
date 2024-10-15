class CommonPage {

    elements = {
        SuccessPopup: {
            Section: () => cy.get('.govuk-notification-banner'),
            Message: () => this.elements.SuccessPopup.Section().find('.govuk-notification-banner__content')
        },

        Datasource: {
            Section: () => cy.get('.govuk-details'),
            LatestUpdatedBy: () => cy.get('.govuk-list > :nth-child(1) > :nth-child(3)')
        }
    };

    public checkSuccessPopup(expectedMessage: string): this {
        const { SuccessPopup } = this.elements;
        SuccessPopup.Section().should('be.visible');
        SuccessPopup.Message().should('contain.text', expectedMessage);
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