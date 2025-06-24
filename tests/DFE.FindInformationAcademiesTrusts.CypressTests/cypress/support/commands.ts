import { AuthenticationInterceptor } from "../auth/authenticationInterceptor";
import cookiesPage from "../pages/cookiesPage";

Cypress.Commands.add("login", (params) => {

	// Intercept all browser requests and add our special auth header
	// Means we don't have to use azure to authenticate
	new AuthenticationInterceptor().register(params);

	//Reject cookies (and save session state) to
	// a) test that the login works
	// b) stop the cookies banner appearing on every page
	cy.session("rejectedCookies", () => {
		cy.visit('/cookies');
		cookiesPage
			.rejectCookies()
			.clickSaveChangesButton();
		cookiesPage.checkCookiesRejectIsInteracted();
	},
		{
			cacheAcrossSpecs: true,
		}
	);
});

Cypress.Commands.add("expandDetailsElement", { prevSubject: 'element', }, (subject) => {

	// Open the details element (disable logging so we can add a nice log below)
	cy.wrap(subject, { log: false })
		.find('summary', { log: false })
		.click({ log: false });

	//Add a nice log
	Cypress.log({
		name: 'expandDetailsElement',
		displayName: 'open',
		message: `${subject.selector}`,
		consoleProps: () => {			// print to dev tools console on click
			return {
				AppliedTo: subject
			};
		},
	});
});
