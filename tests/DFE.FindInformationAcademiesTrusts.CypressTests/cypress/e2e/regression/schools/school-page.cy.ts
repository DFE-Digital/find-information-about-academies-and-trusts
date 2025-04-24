import schoolsPage from "../../../pages/schools/schoolsPage";
import { testSchoolData } from "../../../support/test-data-store";

describe("Testing the components of the Schools page", () => {

    describe("Header items", () => {
        testSchoolData.forEach(({ typeOfSchool, urn }) => {
            beforeEach(() => {
                cy.login();
            });

            [`/schools/overview/details?urn=${urn}`].forEach((url) => {
                it(`Checks the school type is correct on a ${typeOfSchool} on the urn ${urn}`, () => {
                    cy.visit(url);
                    schoolsPage
                        .checkCorrectSchoolTypePresent();
                });
            });

            it(`Checks the page name is correct on a ${typeOfSchool} on the urn ${urn}`, () => {
                cy.visit(`/schools/overview/details?uid=${urn}`);
                schoolsPage
                    .checkOverviewPageNamePresent();
            });
        });
    });
});
