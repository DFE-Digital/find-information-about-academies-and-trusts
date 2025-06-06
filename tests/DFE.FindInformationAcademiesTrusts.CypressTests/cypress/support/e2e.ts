/* eslint-disable @typescript-eslint/no-namespace */ //required to enable custom Cypress commands typescript support
import { AuthenticationInterceptorParams } from '../auth/authenticationInterceptor';
import './commands';

declare global {
    namespace Cypress {
        interface Chainable {
            login(params?: AuthenticationInterceptorParams): Chainable<Element>;

            /**
             * Expands a details element
             * 
             * As of Jan 2025, Cypress is unable to expand a HTML details element using a click. This is a workaround.
             */
            expandDetailsElement(): Cypress.Chainable<JQuery<HTMLElement>>;
        }
    }
}

//Always log in before every test
beforeEach(() => {
    cy.login();
});


