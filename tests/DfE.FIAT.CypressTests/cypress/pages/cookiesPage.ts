export class CookiesPage {

    elements = {
        buttons: {
            accept: () => cy.get('#cookies-accept'),
            reject: () => cy.get('#cookies-reject'),
            saveChangesButton: () => cy.get('[data-testid="save-cookie-preferences-button"]'),
            returnToPreviousPageButton: () => cy.get('[data-testid="return-to-previous-page"]'),
        },
    };

    public clickSaveChangesButton(): this {
        this.elements.buttons.saveChangesButton().click();
        return this;
    }

    public acceptCookies(): this {
        this.elements.buttons.accept().click();
        return this;
    }

    public rejectCookies(): this {
        this.elements.buttons.reject().click();
        return this;
    }

    public clickReturnToPreviousPageButton(): this {
        this.elements.buttons.returnToPreviousPageButton().click();
        return this;
    }

    public checkCookiesAcceptIsUninteracted(): this {
        this.elements.buttons.accept().should('not.be.checked')
        return this;
    }

    public checkCookiesAcceptIsInteracted(): this {
        this.elements.buttons.accept().should('be.checked')
        return this;
    }

    public checkCookiesRejectIsUninteracted(): this {
        this.elements.buttons.reject().should('not.be.checked')
        return this;
    }

    public checkCookiesRejectIsInteracted(): this {
        this.elements.buttons.reject().should('be.checked')
        return this;
    }

}

const cookiesPage = new CookiesPage();
export default cookiesPage;
