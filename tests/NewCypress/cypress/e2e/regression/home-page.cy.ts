import generalAndNavPage from "../../pages/generalAndNav";
import homePage from "../../pages/homePage";
import searchPage from "../../pages/searchPage";

describe("Testing the components of the home page", () => {

    beforeEach(() => {
        cy.login();
    });

    it("Should check that the home pages search bar is present and functional", () => {
        homePage
            .enterMainSearchText("west")
            .searchButtonPresent()
    });

    it.only("Should check that the home page footer bar privacy link is present and functional", () => {
        generalAndNavPage
            .homePagePrivacyLinkPresent()

    });

    it("Should check that the home page footer bar cookies link is present and functional", () => {
        homePage

    });

    it("Should check that the home page footer bar accessibility statement link is present and functional", () => {
        homePage

    });
})