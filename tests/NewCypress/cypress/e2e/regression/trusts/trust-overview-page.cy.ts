import trustOverviewPage from "../../../pages/trusts/trustOverviewPage";

describe("Testing the components of the Trust overview page", () => {

    describe("On a Trust Overview page with data", () => {
        beforeEach(() => {
            cy.login()
            cy.visit('/trusts/overview?uid=5712')
        });

        it("The page loads with the correct headings and data in the Overview trust summary table", () => {
            trustOverviewPage
                .checkOverviewHeaderPresent()
                .checkTrustOverviewSummaryCardItemsPresent()
                .checkOverviewOfstedRatingsSummaryCardItemsPresent()
        })

        it("The page loads with the correct headings and data in the Overview Ofsted rating table", () => {
            trustOverviewPage
                .checkOverviewHeaderPresent()
                .checkTrustOverviewSummaryCardItemsPresent()
                .checkOverviewOfstedRatingsSummaryCardItemsPresent()
        })
        
        it("Checks Ofsted rating table 'Rating' sort functionality", () => {
            trustOverviewPage
                .checkRatingSortAscending()
                .checkTopResultItem('Outstanding')
                .clickRatingHeader()
                .checkRatingSortDescending()
                .checkTopResultItem('Not yet inspected')
        })      

    })
})