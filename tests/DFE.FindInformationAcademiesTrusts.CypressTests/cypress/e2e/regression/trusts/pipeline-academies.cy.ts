import commonPage from "../../../pages/commonPage";
import pipelineAcademiesPage from "../../../pages/trusts/pipelineAcademiesPage";

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

describe("Testing the Pipeline academies pages", () => {
    testTrustData.forEach(({ typeOfTrust, uid }) => {

        describe(`On the Pre advisory board page for a ${typeOfTrust}`, () => {
            beforeEach(() => {
                cy.login();
                cy.visit(`/trusts/academies/pipeline/pre-advisory-board?uid=${uid}`);
            });

            it("Checks the browser title is correct", () => {
                commonPage
                    .checkThatBrowserTitleForTrustPageMatches('Pre advisory board - Pipeline academies - Academies - {trustName} - Find information about academies and trusts');
            });

            it("Checks the correct Pipeline academies  subpage header is present", () => {
                pipelineAcademiesPage
                    .checkOfstedSHGPageHeaderPresent();
            });
        });

        describe(`On the Post advisory board page for a ${typeOfTrust}`, () => {
            beforeEach(() => {
                cy.login();
                cy.visit(`/trusts/academies/pipeline/post-advisory-board?uid=${uid}`);
            });

            it("Checks the browser title is correct", () => {
                commonPage
                    .checkThatBrowserTitleForTrustPageMatches('Post advisory board - Pipeline academies - Academies - {trustName} - Find information about academies and trusts');
            });
        });

        describe(`On the Free schools page for a ${typeOfTrust}`, () => {
            beforeEach(() => {
                cy.login();
                cy.visit(`/trusts/academies/pipeline/free-schools?uid=${uid}`);
            });

            it("Checks the browser title is correct", () => {
                commonPage
                    .checkThatBrowserTitleForTrustPageMatches('Free schools - Pipeline academies - Academies - {trustName} - Find information about academies and trusts');
            });
        });
    });
});
