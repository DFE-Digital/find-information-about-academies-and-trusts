import schoolsPage from "../../../../pages/schools/schoolsPage";

describe("Testing the Schools Overview SEN pages", () => {

    const senSchoolData = [
        {
            typeOfSchool: "school with SEN provision",
            urn: 122957
        },
        {
            typeOfSchool: "academy with SEN provision",
            urn: 143934
        }
    ];

    describe("SEN page tab", () => {
        senSchoolData.forEach(({ typeOfSchool, urn }) => {
            it(`Checks the page name is correct for a ${typeOfSchool} on the urn ${urn}`, () => {
                cy.visit(`/schools/overview/sen?urn=${urn}`);
                schoolsPage
                    .checkOverviewPageNamePresent();
            });

            it(`Checks the tab name is present and correct for a school and academy`, () => {
                cy.visit(`/schools/overview/sen?urn=${urn}`);
                schoolsPage
                    .checkSENTabNameCorrect();
            });

            it(`Checks the tab name is present and correct for a school and academy`, () => {
                cy.visit(`/schools/overview/sen?urn=${urn}`);
                schoolsPage
                    .checkSENSubpageHeaderCorrect();
            });

            it(`Checks the subpage name is present and correct for a school and academy`, () => {
                cy.visit(`/schools/overview/sen?urn=${urn}`);
                schoolsPage
                    .checkSENSubpageHeaderCorrect();
            });

            it(`Checks the details page detail data components are present for a school and academy`, () => {
                cy.visit(`/schools/overview/sen?urn=${urn}`);
                schoolsPage
                    .checkSENDataItemsPresent();
            });

            it('Check SEN list only contains valid SEN provision types', () => {
                cy.visit(`/schools/overview/sen?urn=${urn}`);
                schoolsPage
                    .checkCorrectSENTypePresent();
            });
        });
    });
});
