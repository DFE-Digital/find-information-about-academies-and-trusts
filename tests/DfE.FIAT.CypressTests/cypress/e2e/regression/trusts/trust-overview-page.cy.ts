import trustOverviewPage from "../../../pages/trusts/trustOverviewPage";

describe("Testing the components of the Trust overview page", () => {

    describe("On a Trust Overview page with data", () => {
        beforeEach(() => {
            cy.login()
            cy.visit('/trusts/overview?uid=5712')
        });

        it("The page loads with the correct headings and data in the Trust summary card", () => {
            trustOverviewPage
                .checkOverviewHeaderPresent()
                .checkTrustSummaryCardPresent()
                .checkTrustSummaryCardItemsPresent()
        })

        it("The page loads with the correct headings and data in the Trust details card", () => {
            trustOverviewPage
                .checkTrustDetailCardPresent()
                .checkTrustDetailCardItemsPresent()
        })

        it("The page loads with the correct headings and data in the reference numbers card", () => {
            trustOverviewPage
                .checkReferenceNumbersCardPresent()
                .checkReferenceNumbersCardItemsPresent()

        })

    })
})
