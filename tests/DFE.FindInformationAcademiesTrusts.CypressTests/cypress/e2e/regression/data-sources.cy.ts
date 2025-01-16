import commonPage from "../../pages/commonPage";

describe("Testing the data sources component", () => {

    beforeEach(() => {
        cy.login();
    });

    ['/', '/search', '/accessibility', '/cookies', '/privacy', '/error'].forEach((url) => {
        it(`Should not have a data sources component on ${url}`, () => {
            cy.visit(url); // don't turn off fail on status code because we want the test to fail if visit returns 404 as that means our test urls are incorrect

            commonPage
                .checkPageContentHasLoaded()
                .checkDoesNotHaveDataSourcesComponent();
        });
    });

    it(`Should not have a data sources component on /notfound`, () => {
        cy.visit('/notfound', { failOnStatusCode: false });

        commonPage
            .checkPageContentHasLoaded()
            .checkDoesNotHaveDataSourcesComponent();
    });
});
