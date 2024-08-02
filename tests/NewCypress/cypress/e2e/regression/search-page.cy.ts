import generalAndNavPage from "../../pages/generalAndNav";
import homePage from "../../pages/homePage";
import searchPage from "../../pages/searchPage";

describe("Testing the components of the home page", () => {

    beforeEach(() => {
        cy.login();
    });

    it("Should check that search results are returned with a valid name entered ", () => {
        homePage
            .enterMainSearchText("west")
            .clickMainSearchButton()

        searchPage
            .checkSearchResultsReturned('west')

        generalAndNavPage  
            .returnToHome()

    });


})
