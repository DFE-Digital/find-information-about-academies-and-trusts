import schoolsPage from "../../../pages/schools/schoolsPage";
import { TestDataStore } from "../../../support/test-data-store";

describe("Testing the common components of the Schools pages", () => {
    const commonTestSchools = [
        {
            schoolName: "Abbey Green Nursery School",
            typeOfSchool: "Local authority nursery school",
            urn: 107188,
            getSubpages: (urn: number) => TestDataStore.GetAllSchoolSubpagesForUrn(urn)
        },
        {
            schoolName: "Abbey Grange Church of England Academy",
            typeOfSchool: "Academy converter",
            urn: 137083,
            getSubpages: (urn: number) => TestDataStore.GetAllAcademySubpagesForUrn(urn)
        }
    ];

    describe("Header items", () => {
        commonTestSchools.forEach(({ schoolName, typeOfSchool, urn, getSubpages }) => {

            getSubpages(urn).forEach(({ pageName, subpages }) => {

                describe(`${pageName} - ${typeOfSchool}`, () => {

                    subpages.forEach(({ subpageName, url }) => {
                        it(`Checks the school type is correct on ${pageName} > ${subpageName} for ${schoolName} (${typeOfSchool}) on urn ${urn}`, () => {
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
