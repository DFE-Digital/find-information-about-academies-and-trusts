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

    public checkErrorPopupNotPresent(): this {
        const { ErrorPopup } = this.elements;
        ErrorPopup.Section().should('not.exist');
        return this;
    }
}

const commonPage = new CommonPage();
export default commonPage;
