import commonPage from "../../../pages/commonPage";

const testTrustData = [
    {
        typeOfTrust: "single academy trust",
        uid: 5527
    },
    {
        typeOfTrust: "multi academy trust",
        uid: 5712
    }
];

describe.skip("Testing the Pipeline academies pages", () => {
    testTrustData.forEach(({ typeOfTrust, uid }) => {

        describe(`On the Pre advisory board page for a ${typeOfTrust}`, () => {
            beforeEach(() => {
                cy.login();
                cy.visit(`/trusts/contacts/editsfsolead?uid=${uid}`);
            });

            it("Checks the browser title is correct", () => {
                pipelineAcademiesPage;
            });
        });

        describe(`On the Post advisory board page for a ${typeOfTrust}`, () => {
            beforeEach(() => {
                cy.login();
                cy.visit(`/trusts/contacts/editsfsolead?uid=${uid}`);
            });

            it("Checks the browser title is correct", () => {
                pipelineAcademiesPage;
            });
        });

        describe(`On the Free schools page for a ${typeOfTrust}`, () => {
            beforeEach(() => {
                cy.login();
                cy.visit(`/trusts/contacts/editsfsolead?uid=${uid}`);
            });

            it("Checks the browser title is correct", () => {
                pipelineAcademiesPage;
            });
        });
    });
});
