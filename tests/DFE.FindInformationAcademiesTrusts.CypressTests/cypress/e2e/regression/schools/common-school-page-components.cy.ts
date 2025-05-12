import schoolsPage from "../../../pages/schools/schoolsPage";
import { TestDataStore, testSchoolData } from "../../../support/test-data-store";

describe("Testing the common components of the Schools pages", () => {

    describe("Header items", () => {
        testSchoolData.forEach(({ typeOfSchool, urn }) => {

            TestDataStore.GetAllSchoolSubpagesForUrn(urn).forEach(({ pageName, subpages }) => {

                describe(`${pageName} - ${typeOfSchool}`, () => {

                    subpages.forEach(({ subpageName, url }) => {
                        it(`Checks the school type is correct on ${pageName} > ${subpageName} for a ${typeOfSchool} on the urn ${urn}`, () => {
                            cy.visit(url);
                            schoolsPage
                                .checkCorrectSchoolTypePresent();
                        });
                    });
                });
            });
        });
    });

});
