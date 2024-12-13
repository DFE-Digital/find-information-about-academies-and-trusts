import academiesPage from "../../../pages/trusts/academiesPage";
import navigation from "../../../pages/navigation";
import commonPage from "../../../pages/commonPage";

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


    describe("Pupil numbers tab", () => {
        beforeEach(() => {
            cy.login();
            cy.visit('/trusts/academies/pupil-numbers?uid=5712');
        });

        it("Checks the correct Pupil numbers page headers are present", () => {
            academiesPage
                .checkPupilNumbersHeadersPresent();
        });

        it("Checks the Pupil numbers page sorting", () => {
            academiesPage
                .checkPupilNumbersSorting();
        });

        it('should match the academy count in the sidebar with the actual table row count on the Pupil numbers page', () => {
            academiesPage.getAcademyCountFromSidebar().then(expectedCount => {
                academiesPage.getTableRowCountOnPupilNumbersPage().should('eq', expectedCount);
            });
        });

        it('should match the academy count in the sidebar with the actual table row count on the Pupil numbers page after visiting', () => {
            cy.visit('/trusts/academies/pupil-numbers?uid=5143');
            academiesPage.getAcademyCountFromSidebar().then(expectedCount => {
                academiesPage.getTableRowCountOnPupilNumbersPage().should('eq', expectedCount);
            });
        });

        it("Checks the Pupil numbers page sorting on a larger trust", () => {
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

        it("Checks the Free school meals page sorting", () => {
            academiesPage
                .checkFreeSchoolMealsSorting();
        });

        it('should match the academy count in the sidebar with the actual table row count on the Free school meals page', () => {
            academiesPage.getAcademyCountFromSidebar().then(expectedCount => {
                academiesPage.getTableRowCountOnFreeSchoolMealsPage().should('eq', expectedCount);
            });
        });

        it('should match the academy count in the sidebar with the actual table row count on the Free school meals page after visiting', () => {
            cy.visit('/trusts/academies/free-school-meals?uid=5143');
            academiesPage.getAcademyCountFromSidebar().then(expectedCount => {
                academiesPage.getTableRowCountOnFreeSchoolMealsPage().should('eq', expectedCount);
            });
        });

        it("Checks the Free school meals page sorting on a larger trust", () => {
            cy.visit('/trusts/academies/free-school-meals?uid=5143');
            academiesPage
                .checkFreeSchoolMealsSorting();
        });

    });

    describe("Testing the academies sub navigation", () => {
        beforeEach(() => {
            cy.login();
            cy.visit('/trusts/academies/details?uid=5527');
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

    describe("Testing a trust that has no academies within it to ensure the issue of a 500 page appearing does not happen", () => {
        beforeEach(() => {
            cy.login();
            commonPage.interceptAndVerfiyNo500Errors();
        });

        ['/trusts/academies/details?uid=17728', '/trusts/academies/pupil-numbers?uid=17728', '/trusts/academies/free-school-meals?uid=17728'].forEach((url) => {
            it(`Should have no 500 error on ${url}`, () => {
                cy.visit(url);
            });
        });
    });
});
