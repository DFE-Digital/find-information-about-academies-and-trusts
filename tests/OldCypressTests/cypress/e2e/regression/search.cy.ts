import { Logger } from "cypress/common/logger";
import paginationComponent from "cypress/pages/paginationComponent";
import searchTrustPage from "cypress/pages/searchTrustPage";
import trustPage from "cypress/pages/trustPage";

describe("Testing the search trust functionality", () => {
    beforeEach(() => {
        cy.login();
    });

    it("Should return no results found when searching for a trust that doesn't exist", () => {
        searchTrustPage
            .enterSearchText("The missing trust")
            .getOption("No results found");

        searchTrustPage.search();

        searchTrustPage
            .hasNumberOfResults(`0 results for "the missing trust"`);

        cy.excuteAccessibilityTests();
    });

    it("Should return results when there is only one page of results", () => {

        Logger.log("Ensure that the search shows the results that match the search string");

        searchTrustPage
            .enterSearchText("St Mary")
            .getOption("ST MARY'S ACADEMY TRUST")
            .then((option) => {
                option
                    .hasName("ST MARY'S ACADEMY TRUST")
                    .hasHint("150 Madison Ville, Lempi Junction, Darrellport, HR52 1BO");
            });

        searchTrustPage
            .getOption("ST MARY'S CATHOLIC HIGH SCHOOL ACADEMY TRUST")
            .then((option) => {
                option
                    .hasName("ST MARY'S CATHOLIC HIGH SCHOOL ACADEMY TRUST")
                    .hasHint("2616 Dameon Avenue, Tillman Union, YQ9 6WJ");                
            });

        searchTrustPage
            .getOption("ST MARY'S CATHOLIC PRIMARY SCHOOL")
            .then((option) => {
                option
                    .hasName("ST MARY'S CATHOLIC PRIMARY SCHOOL")
                    .hasHint("82929 Reynold Key, DV28 0EM");
            });

        searchTrustPage.search();

        searchTrustPage.hasNumberOfResults(`10 results for "st mary"`);

        cy.excuteAccessibilityTests();

        Logger.log("Checking a selection of the results");

        searchTrustPage
            .getSearchResult("ST MARY'S ACADEMY TRUST")
            .then((result) => {
                result
                    .hasName("ST MARY'S ACADEMY TRUST")
                    .hasAddress("150 Madison Ville, Lempi Junction, Darrellport, HR52 1BO")
                    .hasTrn("TR1471")
                    .hasUid("1283")
            });
        searchTrustPage
            .getSearchResult("ST MARY'S CATHOLIC PRIMARY SCHOOL")
            .then((result) => {
                result
                    .hasName("ST MARY'S CATHOLIC PRIMARY SCHOOL")
                    .hasAddress("82929 Reynold Key, DV28 0EM")
                    .hasTrn("TR3814")
                    .hasUid("1286")
                    .select();
            });

        trustPage
            .hasName("ST MARY'S CATHOLIC PRIMARY SCHOOL")
            .hasType("Single-academy trust");
    });

    it("should pagination the results when there is a large number of matches", () => {
        searchTrustPage
            .enterSearchText("trust")
            .search();

        searchTrustPage.hasNumberOfResults(`48 results for "trust"`);

        let trustsPageOne: Array<string> = [];
        let trustsPageTwo: Array<string> = [];

        searchTrustPage.getAllTrustResults()
            .then((trusts) => {
                trustsPageOne = trusts;

                expect(trustsPageOne.length).to.equal(20);

                paginationComponent.hasNoPrevious();

                paginationComponent.isCurrentPage("1");
                paginationComponent.goToPage("2");
                return searchTrustPage.getAllTrustResults();
            })
            .then((trusts) => {
                trustsPageTwo = trusts;

                expect(trustsPageTwo.length).to.equal(20);

                paginationComponent.isCurrentPage("2");

                paginationComponent.previous();

                return searchTrustPage.getAllTrustResults();
            })
            .then((trusts) => {

                paginationComponent.isCurrentPage("1");

                expect(trusts).to.deep.equal(trustsPageOne);

                paginationComponent.next();

                return searchTrustPage.getAllTrustResults();
            })
            .then((trusts) => {
                paginationComponent.isCurrentPage("2");

                expect(trustsPageTwo).to.deep.equal(trusts);

                paginationComponent.goToPage("3");

                return searchTrustPage.getAllTrustResults();
            })           

    });
});