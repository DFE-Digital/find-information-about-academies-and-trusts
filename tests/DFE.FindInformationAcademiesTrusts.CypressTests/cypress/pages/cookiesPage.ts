import { Logger } from "../common/logger";

export class CookiesPage {
    public withConsent(consent: string): this {
        cy.getById(`cookies-${consent.toLowerCase()}`).check();

        return this;
    }

    public hasConsent(consent): this {
        cy.getById(`cookies-${consent.toLowerCase()}`).should("be.checked");

        return this;
    }

    public returnToPreviousPage(): this {
        cy.getByTestId("return-to-previous-page").click();

        return this;
    }

    public save(): this {
        cy.getByTestId("save-cookie-preferences-button").click();

        return this;
    }
}

const cookiesPage = new CookiesPage();

export default cookiesPage;