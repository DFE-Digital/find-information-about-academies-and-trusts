import searchTrustPage from "cypress/pages/searchTrustPage";
import trustPage from "cypress/pages/trustPage";

describe("Testing that the service has been deployed correctly", () => {

    beforeEach(() => {
        cy.login();
    });

    it("Should be deployed and we should be able to search for trusts", () => {
        searchTrustPage
            .enterSearchText("west")
            .search()
            .hasResults()
            .selectFirstResult()
            .then((result) => {
                const trustName = result.getName();

                result.select();

                trustPage.hasName(trustName);
            });
    });
})