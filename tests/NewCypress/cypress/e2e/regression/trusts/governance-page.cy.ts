import governancePage from "../../../pages/trusts/governancePage";

describe("Testing the components of the Governance page", () => {

    describe("On a Governance page with data", () => {
        beforeEach(() => {
            cy.login()
            governancePage
                .navigateToFullGovernancePage()
        });

        it("The page loads with the correct headings and data", () => {
            // No missing tables messages
            governancePage.checkNoTrustLeadershipMessageIsNotVisble()
                .checkNoTrusteesMessageIsNotVisble()
                .checkNoMembersMessageIsNotVisble()
                .checkNoHistoricMembersMessageIsNotVisble();

            // Column headers are visible
            governancePage.checkTrustLeadershipColumnHeaders()
                .checkTrusteeColumnHeaders()
                .checkMembersColumnHeaders()
                .checkHistoricMembersColumnHeaders();

            // Check table sorting is working
            governancePage.checkTrustLeadershipSorting();
            governancePage.checkTrusteesSorting();
            governancePage.checkMembersSorting();
            governancePage.checkHistoricMembersSorting();
        })

    })

    describe("On a Governance page without data", () => {
        beforeEach(() => {
            cy.login()
            governancePage.navigateToEmptyGovernancePage();
        });

        it("The tables should be replaced with messages", () => {
            governancePage.checkNoTrustLeadershipMessageIsVisble();
            governancePage.checkNoTrusteesMessageIsVisble();
            governancePage.checkNotMembersMessageIsVisble();
            governancePage.checkNoHistoricMembersMessageIsVisble();
        })
    })
})
