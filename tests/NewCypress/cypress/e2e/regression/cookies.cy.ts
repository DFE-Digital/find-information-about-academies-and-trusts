import cookiesPage from "../../pages/cookiesPage";
describe("Testing the Cookie page and its options", () => {

    beforeEach(() => {
        cy.login();
    });

    it("Should test that the user can accept cookies at the cookies page", () => {
        cookiesPage
            .navigateToCookiesPage();
            .acceptCookies();
    });

})
