import searchTrustPage from "cypress/pages/searchTrustPage";

describe("Checking the details of a trust that does not have any information", () => {
    beforeEach(() => {
        cy.login();
    });

    it("Should be able to find the trust and verify the empty information", () => {
        searchTrustPage
            .enterSearchText("The empty trust")
            .withOption("The empty trust")
            .then((option) => {
                option
                    .hasName("The empty trust")
                    .select();
            });

        searchTrustPage.search();
    });
});