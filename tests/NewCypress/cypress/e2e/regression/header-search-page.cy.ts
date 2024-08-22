import searchPage from "../../pages/searchPage";
import headerPage from "../../pages/headerPage";

describe("Testing the components of the header search", () => {

    beforeEach(() => {
        cy.login()
        cy.visit('/trusts/details?uid=5527')
    });

    it("Should check that the header search bar and autocomplete is present and functional", () => {
        headerPage
            .enterHeaderSearchText("West")
            .checkHeaderAutocompleteIsPresent()
            .checkHeaderSearchButtonPresent()
            .checkAutocompleteContainsTypedText("West")
    })

    it("Should check that the autocomplete does not return results when entry does not exist", () => {

        headerPage
            .enterHeaderSearchText("KnowWhere")
            .checkHeaderAutocompleteIsPresent()
            .checkAutocompleteContainsTypedText("No results found")
    });;

    it("Should check that search results are returned with a valid name entered when using the header search bar ", () => {
        headerPage
            .enterHeaderSearchText("west")
            .clickHeaderSearchButton()

        searchPage
            .checkSearchResultsReturned('west')

    });

})
