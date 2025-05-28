import commonPage from "../../pages/commonPage";

describe("Testing the components of the home page", () => {

    beforeEach(() => {
        cy.visit('/');
    });

    it("Checks the browser title is correct", () => {
        commonPage
            .checkThatBrowserTitleMatches('Home page - Find information about academies and trusts');
    });
});
