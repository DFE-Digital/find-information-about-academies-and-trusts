import academiesPage from "../../../pages/trusts/academiesPage";
import navigation from "../../../pages/navigation";

describe("Testing the components of the Academies page", () => {

    describe("Details tab", () => {
        beforeEach(() => {
            cy.login();
            cy.visit('/trusts/academies/details?uid=5712');
        });

        it("Checks the correct details page headers are present", () => {
            academiesPage
                .checkDetailsHeadersPresent();
        });

        it("Checks that the correct school types are present", () => {
            academiesPage
                .checkSchoolTypesOnDetailsTable();
        });

        it("Checks that a different and larger trusts correct school types are present", () => {
            cy.visit('/trusts/academies/details?uid=5143');
            academiesPage
                .checkSchoolTypesOnDetailsTable();
        });

        it("Checks the detail page sorting", () => {
            cy.visit('/trusts/academies/details?uid=5143');
            academiesPage
                .checkTrustDetailsSorting();
        });

        it('should match the academy count in the sidebar with the actual table row count on the Details page', () => {
            academiesPage.getAcademyCountFromSidebar().then(expectedCount => {
                academiesPage.getTableRowCountOnDetailsPage().should('eq', expectedCount);
            });
        });

        it('should match the academy count in the sidebar with the actual table row count on the Details page after visiting', () => {
            cy.visit('/trusts/academies/details?uid=5143');
            academiesPage.getAcademyCountFromSidebar().then(expectedCount => {
                academiesPage.getTableRowCountOnDetailsPage().should('eq', expectedCount);
            });
        });
    });

    describe("Ofsted ratings tab", () => {
        beforeEach(() => {
            cy.login();
            cy.visit('/trusts/academies/ofsted-ratings?uid=5712');
        });

        it("Checks the correct Ofsted page headers are present", () => {
            academiesPage
                .checkOfstedHeadersPresent();
        });

        it("Checks that a trusts correct Current Ofsted rating types are present", () => {
            academiesPage
                .checkCurrentOfstedTypesOnOfstedTable();
        });

        it("Checks that a trusts correct Previous Ofsted rating types are present", () => {
            academiesPage
                .checkPreviousOfstedTypesOnOfstedTable();
        });

        it("Checks that a different trusts correct Current Ofsted rating types are present", () => {
            cy.visit('/trusts/academies/ofsted-ratings?uid=5143');
            academiesPage
                .checkCurrentOfstedTypesOnOfstedTable();
        });

        it("Checks that a different trusts correct Previous Ofsted rating types are present", () => {
            cy.visit('/trusts/academies/ofsted-ratings?uid=5143');
            academiesPage
                .checkPreviousOfstedTypesOnOfstedTable();
        });

        it("Checks the Ofsted page sorting", () => {
            academiesPage
                .checkOfstedSorting();
        });

    });

    describe("Pupil numbers tab", () => {
        beforeEach(() => {
            cy.login();
            cy.visit('/trusts/academies/pupil-numbers?uid=5712');
        });

        it("Checks the correct Pupil numbers page headers are present", () => {
            academiesPage
                .checkPupilNumbersHeadersPresent();
        });

        it("Checks the Ofsted page sorting", () => {
            cy.visit('/trusts/academies/pupil-numbers?uid=5143');
            academiesPage
                .checkPupilNumbersSorting();
        });


    });

    describe("Free school meals", () => {
        beforeEach(() => {
            cy.login();
            cy.visit('/trusts/academies/free-school-meals?uid=5712');
        });

        it("Checks the correct Free school meals page headers are present", () => {
            academiesPage
                .checkFreeSchoolMealsHeadersPresent();
        });

    });

    describe("Testing the academies sub navigation", () => {
        beforeEach(() => {
            cy.login();
            cy.visit('/trusts/academies/details?uid=5527');
        });

        it('Should check that the academies Ofsted ratings navigation button takes me to the correct page', () => {
            navigation
                .clickOfstedAcadmiesTrustButton()
                .checkCurrentURLIsCorrect('/trusts/academies/ofsted-ratings?uid=5527')
                .checkAllServiceNavItemsPresent()
                .checkAllAcademiesNavItemsPresent();
            academiesPage
                .checkOfstedHeadersPresent();
        });

        it('Should check that the academies Pupil numbers navigation button takes me to the correct page', () => {
            navigation
                .clickPupilNumbersAcadmiesTrustButton()
                .checkCurrentURLIsCorrect('/trusts/academies/pupil-numbers?uid=5527')
                .checkAllServiceNavItemsPresent()
                .checkAllAcademiesNavItemsPresent();
            academiesPage
                .checkPupilNumbersHeadersPresent();
        });

        it('Should check that the academies Free school meals navigation button takes me to the correct page', () => {
            navigation
                .clickFreeSchoolMealsAcadmiesTrustButton()
                .checkCurrentURLIsCorrect('/trusts/academies/free-school-meals?uid=5527')
                .checkAllServiceNavItemsPresent()
                .checkAllAcademiesNavItemsPresent();
            academiesPage
                .checkFreeSchoolMealsHeadersPresent();
        });

        it('Should check that the academies Details navigation button takes me to the correct page', () => {
            cy.visit('/trusts/academies/free-school-meals?uid=5527');
            navigation
                .clickDetailsAcadmiesTrustButton()
                .checkCurrentURLIsCorrect('/trusts/academies/details?uid=5527')
                .checkAllServiceNavItemsPresent()
                .checkAllAcademiesNavItemsPresent();
            academiesPage
                .checkDetailsHeadersPresent();
        });

        it('Should check that the academies sub nav items are not present when I am not in the relevant academies page', () => {
            cy.visit('/trusts/overview/trust-details?uid=5527');
            navigation
                .checkAcademiesSubNavNotPresent();
        });
    });
});
