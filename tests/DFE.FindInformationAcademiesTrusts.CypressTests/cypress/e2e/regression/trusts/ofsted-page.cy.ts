import ofstedPage from "../../../pages/trusts/ofstedPage";

describe("Testing the Ofsted page and its subpages ", () => {
    beforeEach(() => {
        cy.login();
        cy.visit('/trusts/ofsted/current-ratings?uid=5712');
    });

    it("Checks the correct Ofsted page headers are present", () => {
        ofstedPage
            .checkOfstedHeadersPresent();
    });

    it("Checks that a trusts correct Current Ofsted rating types are present", () => {
        ofstedPage
            .checkCurrentOfstedTypesOnOfstedTable();
    });

    it("Checks that a trusts correct Previous Ofsted rating types are present", () => {
        ofstedPage
            .checkPreviousOfstedTypesOnOfstedTable();
    });

    it("Checks that a different trusts correct Current Ofsted rating types are present", () => {
        cy.visit('/trusts/academies/ofsted-ratings?uid=5143');
        ofstedPage
            .checkCurrentOfstedTypesOnOfstedTable();
    });

    it("Checks that a different trusts correct Previous Ofsted rating types are present", () => {
        cy.visit('/trusts/academies/ofsted-ratings?uid=5143');
        ofstedPage
            .checkPreviousOfstedTypesOnOfstedTable();
    });

    it("Checks the Ofsted page sorting", () => {
        ofstedPage
            .checkOfstedSorting();
    });

});
