import commonPage from "../../pages/commonPage";
import navigation from "../../pages/navigation";
import { TestDataStore } from "../../support/test-data-store";

describe("Testing the data sources component", () => {

    describe("Content pages", () => {
        ['/', '/search', '/accessibility', '/cookies', '/privacy', '/error'].forEach((url) => {
            it(`Should not have a data sources component on ${url}`, () => {
                cy.visit(url); // don't turn off fail on status code because we want the test to fail if visit returns 404 as that means our test urls are incorrect

                commonPage
                    .checkPageContentHasLoaded() // We need to make sure the page content has finished loading before we check that something is *not* on the page
                    .checkDoesNotHaveDataSourcesComponent();
            });
        });

        it(`Should not have a data sources component on /notfound`, () => {
            cy.visit('/notfound', { failOnStatusCode: false });

            commonPage
                .checkPageContentHasLoaded() // We need to make sure the page content has finished loading before we check that something is *not* on the page
                .checkDoesNotHaveDataSourcesComponent();
        });
    });

    describe("Trust pages", () => {
        TestDataStore.GetAllTrustSubpagesForUid(5712).forEach(({ pageName, subpages }) => {
            const subpageNames = subpages.map(s => s.subpageName);

            describe(pageName, () => {

                subpages.forEach(({ subpageName, url }) => {
                    it(`Should have a data sources component on ${pageName} > ${subpageName}`, () => {
                        // Go to the given subpage
                        cy.visit(url);

                        // Check that the given subpage list is up to date (so we don't miss any if new pages are added)
                        navigation.checkSubpageNavMatches(subpages);

                        // Check that the data sources component has a subheading for each subnav
                        commonPage
                            .checkHasDataSourcesComponent()
                            .checkDataSourcesComponentHasSubpageHeadings(subpageNames);
                    });
                });
            });
        });
    });
    describe("School pages", () => {
        TestDataStore.GetAllSchoolSubpagesForUrn(123452).forEach(({ pageName, subpages }) => {
            const subpageNames = subpages.map(s => s.subpageName);

            describe(pageName, () => {

                subpages.forEach(({ subpageName, url }) => {
                    it(`Should have a data sources component on ${pageName} > ${subpageName}`, () => {
                        // Go to the given subpage
                        cy.visit(url);

                        // Check that the given subpage list is up to date (so we don't miss any if new pages are added)
                        navigation.checkSubpageNavMatches(subpages);

                        // Check that the data sources component has a subheading for each subnav
                        commonPage
                            .checkHasDataSourcesComponent()
                            .checkDataSourcesComponentHasSubpageHeadings(subpageNames);
                    });
                });
            });
        });
    });
});
