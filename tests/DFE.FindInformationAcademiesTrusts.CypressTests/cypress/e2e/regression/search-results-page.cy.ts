import searchPage from "../../pages/searchPage";
import homePage from "../../pages/homePage";

describe("Testing the components of the search results page", () => {

    beforeEach(() => {
        cy.login();
    });

    it("Should check that the search page returns result no found when something does not exist", () => {

        homePage
            .enterMainSearchText("KnowWhere")
            .clickMainSearchButton();

        searchPage
            .checkNoSearchResultsFound();
    });

    it("Checks that the user can edit their search and search for a new trust from the search page", () => {

        homePage
            .enterMainSearchText("West")
            .clickMainSearchButton();

        searchPage
            .checkSearchResultsReturned("West");

        searchPage
            .enterSearchResultsSearchText("East")
            .clickSearchPageSearchButton();

        searchPage
            .checkSearchResultsReturned("East");
    });

    it("Validates that it returns the amount of results stated in the search text", () => {

        homePage
            .enterMainSearchText("West")
            .clickMainSearchButton();

        searchPage
            .validateSearchResultsCountWithPagination();
    });

    it("Should return the correct trust when searching by TRN", () => {

        homePage
            .enterMainSearchText("TR02343") // Enter the TRN
            .clickMainSearchButton();

        searchPage
            .checkSearchResultsReturned("UNITED LEARNING TRUST"); // Validate that the TRN appears in the results
    });

    it("Should display 'no results found' when searching with a non-existent TRN", () => {

        homePage
            .enterMainSearchText("TR99999") // Enter a TRN that doesn't exist
            .clickMainSearchButton();

        searchPage
            .checkNoSearchResultsFound(); // Validate that no results were found
    });

    it("Should allow searching by TRN from the search results page", () => {

        homePage
            .enterMainSearchText("West")
            .clickMainSearchButton();

        searchPage
            .checkSearchResultsReturned("West");

        searchPage
            .enterSearchResultsSearchText("TR02343")
            .clickSearchPageSearchButton();

        searchPage
            .checkSearchResultsReturned("UNITED LEARNING TRUST");
    });

    it("Should return the correct trust when searching with a partial TRN", () => {

        homePage
            .enterMainSearchText("TR0234")
            .clickMainSearchButton();

        searchPage
            .checkSearchResultsReturned("UNITED LEARNING TRUST"); // Validate that partial TRN returns correct results
    });

});
