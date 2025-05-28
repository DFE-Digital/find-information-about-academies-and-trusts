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

    var fileName = (Cypress as any).mocha.getRunner().suite.ctx.test.file;

    if(fileName.includes('\\e2e\\data-tests\\')){

        Cypress.env('testUrl', Cypress.env("TEST_DATA_URL"));
    }else{
        Cypress.env('testUrl', Cypress.env("REGRESSION_URL"));
    }

    cy.login();
});


