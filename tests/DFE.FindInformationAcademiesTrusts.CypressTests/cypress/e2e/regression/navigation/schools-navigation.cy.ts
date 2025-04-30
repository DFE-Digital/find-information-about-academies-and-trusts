import { TestDataStore } from "../../../support/test-data-store";
import commonPage from "../../../pages/commonPage";

describe('Schools Navigation Tests', () => {
    beforeEach(() => {
        cy.login();
    });

    describe("Routing tests", () => {

        const schoolTypesNotToShow = [
            { urn: 136083, unsupportedSchoolType: "Independent schools" },
            { urn: 150163, unsupportedSchoolType: "Online provider" },
            { urn: 131832, unsupportedSchoolType: "Other types" },
            { urn: 133793, unsupportedSchoolType: "Universities" }
        ];

        schoolTypesNotToShow.forEach(({ urn, unsupportedSchoolType }) => {
            TestDataStore.GetAllSchoolSubpagesForUrn(urn).forEach(({ pageName, subpages }) => {

                describe(pageName, () => {

                    subpages.forEach(({ subpageName, url }) => {
                        it(`Should check that navigating to subpages for unsupported school type displays the 404 not found page ${pageName} > ${subpageName} > ${unsupportedSchoolType}`, () => {
                            // Go to the given subpage
                            cy.visit(url, { failOnStatusCode: false });

                            // Check that the data sources component has a subheading for each subnav
                            commonPage
                                .check404PageDisplayed();
                        });
                    });
                });
            });
        });
    });
});
