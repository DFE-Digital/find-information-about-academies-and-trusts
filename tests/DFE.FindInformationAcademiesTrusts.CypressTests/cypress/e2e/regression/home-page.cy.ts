import paginationPage from "../../pages/paginationPage";
import homePage from "../../pages/homePage";
import searchPage from "../../pages/searchPage";

describe("Testing the components of the home page", () => {

    beforeEach(() => {
        cy.login();
    });

    it("Should check that the home pages search bar and autocomplete is present and functional", () => {
        homePage
            .enterMainSearchText("West")
        searchPage
            .checkMainAutocompleteIsPresent()
        homePage
            .checkMainSearchButtonPresent()
        searchPage
            .checkAutocompleteContainsTypedText("West")
    });

    it("Should check that the autocomplete does not return results when entry does not exist", () => {

        homePage
            .enterMainSearchText("KnowWhere")
        searchPage
            .checkMainAutocompleteIsPresent()
            .checkAutocompleteContainsTypedText("No results found")
    });

    it("Should check that search results are returned with a valid name entered when using the main search bar ", () => {
        homePage
            .enterMainSearchText("west")
            .clickMainSearchButton()

        searchPage
            .checkSearchResultsReturned('west')

        paginationPage
            .returnToHome()

        homePage
            .checkMainSearchButtonPresent()

    });
})
