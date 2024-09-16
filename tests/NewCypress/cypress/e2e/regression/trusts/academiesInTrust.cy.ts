import academiesInTrustPage from "../../../pages/trusts/academiesInTrustPage";

describe("Testing the components of the Academies in this trust page", () => {

    describe("Details tab", () => {
        beforeEach(() => {
            cy.login()
            cy.visit('/trusts/academies/details?uid=5712')
        });

        it("Checks the correct AIT details page headers are present", () => {
            academiesInTrustPage
                .checkAITDetailsHeadersPresent()
        })

        it("Checks that the correct authority types are present", () => {
            academiesInTrustPage
                .checkAuthTypesOnAITDetailsTable()
        })

        it("Checks that a different and larger trusts correct authority types are present", () => {
            cy.visit('/trusts/academies/details?uid=5143')
            academiesInTrustPage
                .checkAuthTypesOnAITDetailsTable()
        })

    })

    describe("Ofsted ratings tab", () => {
        beforeEach(() => {
            cy.login()
            cy.visit('/trusts/academies/ofsted-ratings?uid=5712')
        });

        it("Checks the correct AIT Ofsted page headers are present", () => {
            academiesInTrustPage
                .checkAITOfstedHeadersPresent()
        })

        it("Checks that a trusts correct Current Ofsted rating types are present", () => {
            academiesInTrustPage
                .checkCurrentOfstedTypesOnOfstedTable()
        })

        it("Checks that a trusts correct Previous Ofsted rating types are present", () => {
            academiesInTrustPage
                .checkPreviousOfstedTypesOnOfstedTable()
        })

        it("Checks that a different trusts correct Current Ofsted rating types are present", () => {
            cy.visit('/trusts/academies/ofsted-ratings?uid=5143')
            academiesInTrustPage
                .checkCurrentOfstedTypesOnOfstedTable()
        })

        it("Checks that a different trusts correct Previous Ofsted rating types are present", () => {
            cy.visit('/trusts/academies/ofsted-ratings?uid=5143')
            academiesInTrustPage
                .checkPreviousOfstedTypesOnOfstedTable()
        })

    })

    describe("Pupil numbers tab", () => {
        beforeEach(() => {
            cy.login()
            cy.visit('/trusts/academies/pupil-numbers?uid=5712')
        });

        it("Checks the correct AIT Pupil numbers page headers are present", () => {
            academiesInTrustPage
                .checkAITPupilNumbersHeadersPresent()
        })

    })

    describe("Free school meals", () => {
        beforeEach(() => {
            cy.login()
            cy.visit('/trusts/academies/free-school-meals?uid=5712')
        });

        it("Checks the correct AIT Free school meals page headers are present", () => {
            academiesInTrustPage
                .checkAITFreeSchoolMealsHeadersPresent()
        })

    })
})