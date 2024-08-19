import footerLinks from "../../pages/footerLinks";
import paginationPage from "../../pages/paginationPage";

describe("Testing the components of the home page", () => {

    beforeEach(() => {
        cy.login();
    });

    it("Should check that the home page footer bar privacy link is present and functional", () => {
        footerLinks
            .checkPrivacyLinkPresent()
            .clickPrivacyLink()

        paginationPage
            .checkCurrentURLIsCorrect('privacy')

    });

    it("Should check that the home page footer bar cookies link is present and functional", () => {
        footerLinks
            .checkCookiesLinkPresent()
            .clickCookiesLink()

        paginationPage
            .checkCurrentURLIsCorrect('cookies')

    });

    it("Should check that the home page footer bar accessibility statement link is present and functional", () => {
        footerLinks
            .checkAcessibilityStatementLinkPresent()
            .clickAccessibilityStatementLink()

        paginationPage
            .checkCurrentURLIsCorrect('accessibility')
    });
})
