import paginationPage from "../../pages/paginationPage";
import homePage from "../../pages/homePage";
import searchPage from "../../pages/searchPage";
import navigation from "../../pages/navigation";
import commonPage from "../../pages/commonPage";

describe("Testing the components of the home page", () => {

    beforeEach(() => {
        cy.login();
        cy.visit('/');
    });

    it("Checks the browser title is correct", () => {
        commonPage
            .checkThatBrowserTitleMatches('Home page - Find information about academies and trusts');
    });

    it("Should check that the home pages search bar and autocomplete is present and functional", () => {
        homePage
            .enterMainSearchText("West");
        searchPage
            .checkMainAutocompleteIsPresent();
        homePage
            .checkMainSearchButtonPresent();
        searchPage
            .checkAutocompleteContainsTypedText("West");
    });

    it("Should check that the autocomplete does not return results when entry does not exist", () => {

        homePage
            .enterMainSearchText("KnowWhere");
        searchPage
            .checkMainAutocompleteIsPresent()
            .checkAutocompleteContainsTypedText("No results found");
    });

    it("Should check that search results are returned with a valid name entered when using the main search bar ", () => {
        homePage
            .enterMainSearchText("west")
            .clickMainSearchButton();

        searchPage
            .checkSearchResultsReturned('west');

        paginationPage
            .returnToHome();

        homePage
            .checkMainSearchButtonPresent();

    });

    it("Should check that the what you can find section is collapsed when you first land on the home screen ", () => {
        homePage
            .checkWhatYouCanFindPresent()
            .checkWhatYouCanFindListCollapsed();

    });

    it("Should check that the what you can find section is collapsed when you return to the home screen ", () => {
        homePage
            .checkWhatYouCanFindPresent()
            .checkWhatYouCanFindListCollapsed();

        cy.visit('/trusts/contacts/in-dfe?uid=5712');

        navigation
            .checkCurrentURLIsCorrect('/trusts/contacts/in-dfe?uid=5712');

        cy.visit('/');

        homePage
            .checkWhatYouCanFindPresent()
            .checkWhatYouCanFindListCollapsed();
    });

    it("Should check that the what you can find section is expanded when clicked on ", () => {
        homePage
            .checkWhatYouCanFindPresent()
            .clickWhatYouCanFindList()
            .checkWhatYouCanFindListOpen();
    });

    it("Should check that the expected contents of the what you can find section are all present", () => {
        homePage
            .checkWhatYouCanFindPresent()
            .clickWhatYouCanFindList()
            .checkWhatYouCanFindListOpen()
            .checkWhatYouCanFindListItemsPresent();
    });

});
