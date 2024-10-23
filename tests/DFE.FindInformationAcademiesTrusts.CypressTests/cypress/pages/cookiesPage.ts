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
        this.elements.buttons.saveChangesButton().click();
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

}

const cookiesPage = new CookiesPage();
export default cookiesPage;
