import "cypress-localstorage-commands";
import "cypress-axe";
import { AuthenticationInterceptor } from "../auth/authenticationInterceptor";
import { Logger } from "../common/logger";

Cypress.Commands.add("getByTestId", (id) => {
	cy.get(`[data-testid="${id}"]`);
});

Cypress.Commands.add("containsByTestId", (id) => {
	cy.get(`[data-testid*="${id}"]`);
});

Cypress.Commands.add("getById", (id) => {
	cy.get(`[id="${id}"]`);
});

Cypress.Commands.add("login", (params) => {
	cy.clearCookies();
	cy.clearLocalStorage();

	// Intercept all browser requests and add our special auth header
	// Means we don't have to use azure to authenticate
	new AuthenticationInterceptor().register(params);

	cy.visit("/");
});

Cypress.Commands.add("excuteAccessibilityTests", () => {
	Logger.log("Executing the command");
	const continueOnFail = false;
	Logger.log("Inject Axe");
	cy.injectAxe();

	Logger.log("Checking accessibility");
	cy.checkA11y(
		undefined,
		{
			// These will be fixed one by one
			rules: {
				region: { enabled: false },
				"color-contrast": { enabled: false }, // decisions
			},
		},
		undefined,
		continueOnFail
	);

	Logger.log("Command finished");
});
