
export class AuthenticationInterceptor {
    register(automationUserProperties: AutomationUserProperties) {
        cy.intercept(
            {
                url: Cypress.env("URL") + "/**",
                middleware: true,
            },
            (req) => {
                // Set an auth header on every request made by the browser
                req.headers = {
                    ...req.headers,
                    'Authorization': `Bearer ${Cypress.env("AUTH_KEY")}`,
                    'x-user-context': JSON.stringify(automationUserProperties)
                };
            }
        ).as("AuthInterceptor");
    }
}

export type AutomationUserProperties = {
    name: string;
    email: string;
    roles: string[];
}
