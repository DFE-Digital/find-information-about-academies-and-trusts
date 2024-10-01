import trustDetailsPage from "../../../pages/trusts/trustDetailsPage";

describe("Testing the components of the Trust details page", () => {

    describe("On a Trust Detail page with data", () => {
        beforeEach(() => {
            cy.login()
            cy.visit('/trusts/details?uid=5712')
        });

        it("The page loads with the correct headings and data in the details table", () => {
            trustDetailsPage
                .checkDetailsHeaderPresent()
                .checkTrustDetailSummaryCardPresent()
                .checkTrustDetailSummaryCardItemsPresent()
        })

        it("The page loads with the correct headings and data in the reference numbers table", () => {
            trustDetailsPage
                .checkReferenceNumbersSummaryCardPresent()
                .checkReferenceNumbersSummaryCardItemsPresent()

        })

    })
})