import { AuthenticationInterceptor } from "../auth/authenticationInterceptor";

Cypress.Commands.add("login", (params) => {
	cy.clearCookies();
	cy.clearLocalStorage();

	// Intercept all browser requests and add our special auth header
	// Means we don't have to use azure to authenticate
	new AuthenticationInterceptor().register(params);

	cy.visit("/");
}); 
