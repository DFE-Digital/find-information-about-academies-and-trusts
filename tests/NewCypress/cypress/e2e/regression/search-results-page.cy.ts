import paginationPage from "../../pages/paginationPage";
import searchPage from "../../pages/searchPage";
import homePage from "../../pages/homePage";

describe("Testing the components of the search results page", () => {

    beforeEach(() => {
        cy.login();
    });

    it("Should check that the search page returns result no found when something does not exist", () => {

        homePage
            .enterMainSearchText("KnowWhere")
            .clickMainSearchButton()

        searchPage
            .checkNoSearchResultsFound()
    });


    it("Checks that the user can edit their search and search for a new trust from the search page", () => {

        homePage
            .enterMainSearchText("West")
            .clickMainSearchButton()

        searchPage
            .checkSearchResultsReturned("West")

        searchPage
            .enterSearchResultsSearchText("East")
            .clicSearchPageSearchButton()

        searchPage
            .checkSearchResultsReturned("East")
    });

    it.only("Validates that it reutrns the amount of results stated in the search text", () => {

        homePage
            .enterMainSearchText("West")
            .clickMainSearchButton()

        searchPage
            .validateSearchResultsCountWithPagination()
    });


})
