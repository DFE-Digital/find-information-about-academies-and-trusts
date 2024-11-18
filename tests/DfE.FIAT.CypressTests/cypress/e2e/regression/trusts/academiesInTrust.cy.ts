import academiesInTrustPage from "../../../pages/trusts/academiesInTrustPage";
import navigation from "../../../pages/navigation";

describe("Testing the components of the Academies in this trust page", () => {

    describe("Details tab", () => {
        beforeEach(() => {
            cy.login()
            cy.visit('/trusts/academies/details?uid=5712')
        });

        it("Checks the correct details page headers are present", () => {
            academiesInTrustPage
                .checkDetailsHeadersPresent()
        })

        it("Checks that the correct school types are present", () => {
            academiesInTrustPage
                .checkSchoolTypesOnDetailsTable()
        })

        it("Checks that a different and larger trusts correct school types are present", () => {
            cy.visit('/trusts/academies/details?uid=5143')
            academiesInTrustPage
                .checkSchoolTypesOnDetailsTable()
        })

        it("Checks the detail page sorting", () => {
            cy.visit('/trusts/academies/details?uid=5143')
            academiesInTrustPage
                .checkTrustDetailsSorting()
        })

        it('should match the academy count in the sidebar with the actual table row count on the Details page', () => {
            academiesInTrustPage.getAcademyCountFromSidebar().then(expectedCount => {
                academiesInTrustPage.getTableRowCountOnDetailsPage().should('eq', expectedCount);
            });
        });

        it('should match the academy count in the sidebar with the actual table row count on the Details page after visiting', () => {
            cy.visit('/trusts/academies/details?uid=5143');
            academiesInTrustPage.getAcademyCountFromSidebar().then(expectedCount => {
                academiesInTrustPage.getTableRowCountOnDetailsPage().should('eq', expectedCount);
            });
        });
    })

    describe("Ofsted ratings tab", () => {
        beforeEach(() => {
            cy.login()
            cy.visit('/trusts/academies/ofsted-ratings?uid=5712')
        });

        it("Checks the correct Ofsted page headers are present", () => {
            academiesInTrustPage
                .checkOfstedHeadersPresent()
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

        it("Checks the Ofsted page sorting", () => {
            academiesInTrustPage
                .checkOfstedSorting()
        })

    })

    describe("Pupil numbers tab", () => {
        beforeEach(() => {
            cy.login()
            cy.visit('/trusts/academies/pupil-numbers?uid=5712')
        });

        it("Checks the correct Pupil numbers page headers are present", () => {
            academiesInTrustPage
                .checkPupilNumbersHeadersPresent()
        })

        it("Checks the Ofsted page sorting", () => {
            cy.visit('/trusts/academies/pupil-numbers?uid=5143')
            academiesInTrustPage
                .checkPupilNumbersSorting()
        })


    })

    describe("Free school meals", () => {
        beforeEach(() => {
            cy.login()
            cy.visit('/trusts/academies/free-school-meals?uid=5712')
        });

        it("Checks the correct Free school meals page headers are present", () => {
            academiesInTrustPage
                .checkFreeSchoolMealsHeadersPresent()
        })

    })

    describe("Testing the academies in this trust sub navigation", () => {
        beforeEach(() => {
            cy.login();
            cy.visit('/trusts/academies/details?uid=5527');
        });

        it('Should check that the acdemiesInThisTrust Ofsted ratings navigation button takes me to the correct page', () => {
            navigation
                .clickOfstedAcadmiesTrustButton()
                .checkCurrentURLIsCorrect('/trusts/academies/ofsted-ratings?uid=5527')
                .checkAllServiceNavItemsPresent()
                .checkAllAcademiesInTrustNavItemsPresent()
            academiesInTrustPage
                .checkOfstedHeadersPresent()
        });

        it('Should check that the acdemiesInThisTrust Pupil numbers navigation button takes me to the correct page', () => {
            navigation
                .clickPupilNumbersAcadmiesTrustButton()
                .checkCurrentURLIsCorrect('/trusts/academies/pupil-numbers?uid=5527')
                .checkAllServiceNavItemsPresent()
                .checkAllAcademiesInTrustNavItemsPresent()
            academiesInTrustPage
                .checkPupilNumbersHeadersPresent()
        });

        it('Should check that the acdemiesInThisTrust Free school meals navigation button takes me to the correct page', () => {
            navigation
                .clickFreeSchoolMealsAcadmiesTrustButton()
                .checkCurrentURLIsCorrect('/trusts/academies/free-school-meals?uid=5527')
                .checkAllServiceNavItemsPresent()
                .checkAllAcademiesInTrustNavItemsPresent()
            academiesInTrustPage
                .checkFreeSchoolMealsHeadersPresent()
        });

        it('Should check that the acdemiesInThisTrust Details navigation button takes me to the correct page', () => {
            cy.visit('/trusts/academies/free-school-meals?uid=5527')
            navigation
                .clickDetailsAcadmiesTrustButton()
                .checkCurrentURLIsCorrect('/trusts/academies/details?uid=5527')
                .checkAllServiceNavItemsPresent()
                .checkAllAcademiesInTrustNavItemsPresent()
            academiesInTrustPage
                .checkDetailsHeadersPresent()
        });

        it('Should check that the academies in this trust nav items are not present when I am not in the relevant academies page', () => {
            cy.visit('/trusts/overview?uid=5527');
            navigation
                .checkAcademiesInTrustNavNotPresent()
        });
    })
})
