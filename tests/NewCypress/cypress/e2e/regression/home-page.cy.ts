import footerLinks from "../../pages/footerLinks";
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
            .autocompleteIsPresent()
            .searchButtonPresent()
            .autocompleteContainsTypedText("West")
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
            .searchButtonPresent()

    });

    it("Should check that the home page footer bar privacy link is present and functional", () => {
        footerLinks
            .privacyLinkPresent()
            .clickPrivacyLink()

        paginationPage
            .checkImAtTheCorrectUrl('privacy')

    });

    it("Should check that the home page footer bar cookies link is present and functional", () => {
        homePage
        footerLinks
            .cookiesLinkPresent()
            .clickCookiesLink()

        paginationPage
            .checkImAtTheCorrectUrl('cookies')

    });

    it("Should check that the home page footer bar accessibility statement link is present and functional", () => {
        footerLinks
            .acessibilityStatementLinkPresent()
            .clickAccessibilityStatementLink()

        paginationPage
        .checkImAtTheCorrectUrl('accessibility')
    });
})