import { Logger } from "cypress/common/logger";
import cookiesBanner from "cypress/pages/cookiesBanner";
import cookiesPage from "cypress/pages/cookiesPage";
import footerLinks from "cypress/pages/footerLinks";

describe("Testing cookies on the site", () => {

    beforeEach(() => {
        cy.login();
    });

    it("Should accept the cookies on the banner then decline them afterwards", () => {
        cy.visit("/privacy");

        cookiesBanner
            .accept()
            .notVisible();

        Logger.log("Upon accepting the banner it should stay on the same page");
        cy.url().should("include", "/privacy");

        footerLinks.viewCookies();

        cookiesPage
            .hasConsent("accept")

        hasCookieValue("True");

        cookiesPage
            .withConsent("reject")
            .save();

        cookiesPage.returnToPreviousPage();

        hasCookieValue("False");

        cy.url().should("include", "/privacy");

        footerLinks.viewCookies();

        cookiesPage.hasConsent("reject");
    });

    it("Should reject the cookies on the banner then accept them afterwards", () => {
        cookiesBanner.viewCookies();

        cookiesBanner
            .reject()
            .notVisible();

        footerLinks.viewCookies();

        cookiesPage
            .hasConsent("reject")

        hasCookieValue("False");

        cookiesPage
            .withConsent("accept")
            .save();

        cookiesPage.returnToPreviousPage();

        hasCookieValue("True");

        footerLinks.viewCookies();

        cookiesPage.hasConsent("accept");
    });

    function hasCookieValue(cookieValue: string) {
        Logger.log(`Should set the consent cookie to ${cookieValue}`);

        cy.getCookie(".FindInformationAcademiesTrust.CookieConsent")
            .then(cookie => {
                expect(cookie?.value).to.equal(cookieValue);
            });
    }
});