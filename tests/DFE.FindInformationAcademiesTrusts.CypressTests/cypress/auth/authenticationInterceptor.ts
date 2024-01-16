import { EnvUrl, EnvAuthKey } from "../constants/cypressConstants";

export class AuthenticationInterceptor {

    register(params?: AuthenticationInterceptorParams) {
        cy.intercept(
            {
                url: Cypress.env(EnvUrl) + "/**",
                middleware: true,
            },
            (req) =>
            {
                // Set an auth header on every request made by the browser
                req.headers = {
                    ...req.headers,
                    'Authorization': `Bearer ${Cypress.env(EnvAuthKey)}`
                };
            }
        ).as("AuthInterceptor");
    }
}

export type AuthenticationInterceptorParams = {
    role?: string;
    username?: string;
}