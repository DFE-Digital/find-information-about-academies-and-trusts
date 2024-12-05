class CommonPage {

    elements = {
        successPopup: {
            section: () => cy.get('.govuk-notification-banner'),
            message: () => this.elements.successPopup.section().find('.govuk-notification-banner__content')
        },

        errorPopup: {
            section: () => cy.get('.govuk-error-summary'),
            message: () => this.elements.errorPopup.section().find('[data-testid="error-summary"]')
        },

    };

    public checkSuccessPopup(expectedMessage: string): this {
        const { successPopup } = this.elements;
        successPopup.section().should('be.visible');
        successPopup.message().should('contain.text', expectedMessage);
        return this;
    }

    public checkErrorPopup(expectedMessage: string): this {
        const { errorPopup } = this.elements;
        errorPopup.section().should('be.visible');
        errorPopup.message().should('contain.text', expectedMessage);
        return this;
    }

    public checkErrorPopupNotPresent(): this {
        const { errorPopup } = this.elements;
        errorPopup.section().should('not.exist');
        return this;
    }

    public checkPageLoad(): void {
        cy.window().then((win) => {
            expect(win.document.readyState).to.eq('complete');
        });
    }

    public checkNo500Errors(): void {
        cy.intercept('**', (req) => {
            req.on('response', (res) => {
                expect(res.statusCode).to.not.eq(500);
            });
        }).as('allRequests');
    }

}

const commonPage = new CommonPage();
export default commonPage;
