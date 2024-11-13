import trustOverviewPage from "../../../pages/trusts/trustOverviewPage";

describe("Testing the components of the Trust overview page", () => {

    describe("On a Trust Overview page with data", () => {
        beforeEach(() => {
            cy.login()
            cy.visit('/trusts/overview?uid=5712')
        });

        it("The page loads with the correct headings and data in the Overview summary and Ofsted rating table", () => {
            trustOverviewPage
                .checkOverviewHeaderPresent()
                .checkTrustOverviewSummaryCardItemsPresent()
        })
        
    })

    describe("On a Trust Detail page with data", () => {
        beforeEach(() => {
            cy.login()
            cy.visit('/trusts/overview?uid=5712')
        });

        it("The page loads with the correct headings and data in the details table", () => {
            trustOverviewPage
                .checkTrustDetailSummaryCardPresent()
                .checkTrustDetailSummaryCardItemsPresent()
        })

        it("The page loads with the correct headings and data in the reference numbers table", () => {
            trustOverviewPage
                .checkReferenceNumbersSummaryCardPresent()
                .checkReferenceNumbersSummaryCardItemsPresent()

        })

    })
})