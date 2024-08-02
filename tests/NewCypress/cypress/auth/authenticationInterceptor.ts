
export class AuthenticationInterceptor {

    register(params?: AuthenticationInterceptorParams) {
        cy.intercept(
            {
                url: Cypress.env("url") + "/**",
                middleware: true,
            },
            (req) => {
                // Set an auth header on every request made by the browser
                req.headers = {
                    ...req.headers,
                    'Authorization': `Bearer ${Cypress.env("authKey")}`
                };
            }
        ).as("AuthInterceptor");
    }
}

export type AuthenticationInterceptorParams = {
    role?: string;
    username?: string;
}
