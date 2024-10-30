import { AuthenticationInterceptor, AutomationUserProperties } from "../auth/authenticationInterceptor";

Cypress.Commands.add("login", (automationUserProperties?: AutomationUserProperties) => {

	if (!automationUserProperties)
		automationUserProperties = {
			name: "Automation User - name",
			email: "Automation User - email",
			roles: ["User.Role.Authorised"]
		};

	// Intercept all browser requests and add our special auth header
	new AuthenticationInterceptor().register(automationUserProperties);

	cy.visit("/");
});
