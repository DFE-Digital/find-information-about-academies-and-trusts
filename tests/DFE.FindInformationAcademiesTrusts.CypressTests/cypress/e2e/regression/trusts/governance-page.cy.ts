import navigation from "../../../pages/navigation";
import governancePage from "../../../pages/trusts/governancePage";

describe("Testing the components of the Governance page", () => {

    describe("On a Governance page with data", () => {
        const trustWithFullGovernanceData = 5712;

        beforeEach(() => {
            cy.login();
        });

        it("The trust leadership page loads with the correct headings and data", () => {
            cy.visit(`/trusts/governance/trust-leadership?uid=${trustWithFullGovernanceData}`);

            // Trust leadership table is visible and working
            governancePage
                .checkTrustLeadershipSubHeaderPresent()
                .checkTrustLeadershipTableHeadersAreVisible()
                .checkTrustLeadershipAppointmentDatesAreCurrent()
                .checkOnlyTrustLeadershipRolesArePresent();

            // "No Trust Leadership" message is hidden
            governancePage
                .checkNoTrustLeadershipMessageIsHidden();
        });

        it("The trustees page loads with the correct headings and data", () => {
            cy.visit(`/trusts/governance/trustees?uid=${trustWithFullGovernanceData}`);

            // Trustee table is visible and working
            governancePage
                .checkTrusteesSubHeaderPresent()
                .checkTrusteesTableHeadersAreVisible()
                .checkTrusteesAppointmentDatesAreCurrent();

            // "No Trustees" message is hidden
            governancePage
                .checkNoTrusteesMessageIsHidden();
        });

        it("The members page loads with the correct headings and data", () => {
            cy.visit(`/trusts/governance/members?uid=${trustWithFullGovernanceData}`);

            // Members table is visible and working
            governancePage
                .checkMembersSubHeaderPresent()
                .checkMembersTableHeadersAreVisible()
                .checkMembersAppointmentDatesAreCurrent();

            // "No Members" message is hidden
            governancePage
                .checkNoMembersMessageIsHidden();
        });

        it("The historic members page loads with the correct headings and data", () => {
            cy.visit(`/trusts/governance/historic-members?uid=${trustWithFullGovernanceData}`);

            // Historic members table is visible and working
            governancePage
                .checkHistoricMembersSubHeaderPresent()
                .checkHistoricMembersTableHeadersAreVisible()
                .checkHistoricMembersAppointmentDatesAreInThePast();

            // "No Historic Members" message is hidden
            governancePage
                .checkNoHistoricMembersMessageIsHidden();
        });

        it("Table sorting is working", () => {
            cy.visit(`/trusts/governance/trust-leadership?uid=${trustWithFullGovernanceData}`);
            governancePage.checkTrustLeadershipColumnsSortCorrectly();

            cy.visit(`/trusts/governance/trustees?uid=${trustWithFullGovernanceData}`);
            governancePage.checkTrusteesColumnsSortCorrectly();

            cy.visit(`/trusts/governance/members?uid=${trustWithFullGovernanceData}`);
            governancePage.checkMembersColumnsSortCorrectly();

            cy.visit(`/trusts/governance/historic-members?uid=${trustWithFullGovernanceData}`);
            governancePage.checkHistoricMembersColumnsSortCorrectly();
        });
    });

    describe("On a Governance page without data", () => {
        const trustWithNoGovernanceData = 5712;

        beforeEach(() => {
            cy.login();
        });

        it("The tables should be replaced with no data messages", () => {
            cy.visit(`/trusts/governance/trust-leadership?uid=${trustWithNoGovernanceData}`);
            governancePage.checkNoTrustLeadershipMessageIsVisble();

            cy.visit(`/trusts/governance/trustees?uid=${trustWithNoGovernanceData}`);
            governancePage.checkNoTrusteesMessageIsVisble();

            cy.visit(`/trusts/governance/members?uid=${trustWithNoGovernanceData}`);
            governancePage.checkNotMembersMessageIsVisble();
        });

        //Skipping below test until no data governance page issue sorted (Bug raised 179544)
        it.skip("The historic members table should be replaced with no data message", () => {
            cy.visit(`/trusts/governance/historic-members?uid=${trustWithNoGovernanceData}`);
            governancePage.checkNoHistoricMembersMessageIsVisble();
        });
    });

    describe("Testing the governance sub navigation", () => {
        beforeEach(() => {
            cy.login();
        });

        it('Should check that the trust leadership navigation button takes me to the correct page', () => {
            cy.visit('/trusts/governance/historic-members?uid=5527');

            governancePage
                .clickTrustLeadershipSubnavButton();

            navigation
                .checkCurrentURLIsCorrect('/trusts/governance/trust-leadership?uid=5527');

            governancePage
                .checkAllSubNavItemsPresent();

            // Todo: Check page sub-heading "Trust leadership" is visible
        });

        it('Should check that the trustees navigation button takes me to the correct page', () => {
            cy.visit('/trusts/governance/trust-leadership?uid=5527');

            governancePage
                .clickTrusteesSubnavButton();

            navigation
                .checkCurrentURLIsCorrect('/trusts/governance/trustees?uid=5527');

            governancePage
                .checkAllSubNavItemsPresent();

            // Todo: Check page sub-heading "Trustees" is visible
        });

        it('Should check that the members navigation button takes me to the correct page', () => {
            cy.visit('/trusts/governance/trustees?uid=5527');

            governancePage
                .clickMembersSubnavButton();

            navigation
                .checkCurrentURLIsCorrect('/trusts/governance/members?uid=5527');

            governancePage
                .checkAllSubNavItemsPresent();

            // Todo: Check page sub-heading "Members" is visible
        });

        it('Should check that the historic members navigation button takes me to the correct page', () => {
            cy.visit('/trusts/governance/members?uid=5527');

            governancePage
                .clickHistoricMembersSubnavButton();

            navigation
                .checkCurrentURLIsCorrect('/trusts/governance/historic-members?uid=5527');

            governancePage
                .checkAllSubNavItemsPresent();

            // Todo: Check page sub-heading "Historic members" is visible
        });


        it('Should check that the governance sub nav items are not present when I am not on the governance page', () => {
            cy.visit('/trusts/overview/trust-details?uid=5527');
            governancePage
                .checkSubNavNotPresent();
        });
    });
});
