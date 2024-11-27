import { SortingUtility } from "./sortingUtility";
class GovernancePage {

    elements = {
        TrustLeadership: {
            section: () => cy.get('[data-testid="trust-leadership"]'),
            names: () => this.elements.TrustLeadership.section().find('[data-testid="trust-leadership-name"]'),
            nameHeader: () => this.elements.TrustLeadership.section().find("th:contains('Name')"),
            Roles: () => this.elements.TrustLeadership.section().find('[data-testid="trust-leadership-role"]'),
            RoleHeader: () => this.elements.TrustLeadership.section().find("th:contains('Role')"),
            From: () => this.elements.TrustLeadership.section().find('[data-testid="trust-leadership-from"]'),
            FromHeader: () => this.elements.TrustLeadership.section().find("th:contains('From')"),
            To: () => this.elements.TrustLeadership.section().find('[data-testid="trust-leadership-to"]'),
            ToHeader: () => this.elements.TrustLeadership.section().find("th:contains('To')"),
            NoDataMessage: () => this.elements.TrustLeadership.section().contains('No Trust Leadership'),
        },
        Trustees: {
            section: () => cy.get('[data-testid="trustees"]'),
            Names: () => this.elements.Trustees.section().find('[data-testid="trustees-name"]'),
            NameHeader: () => this.elements.Trustees.section().find("th:contains('Name')"),
            AppointedBy: () => this.elements.Trustees.section().find('[data-testid="trustees-appointed"]'),
            AppointedHeader: () => this.elements.Trustees.section().find("th:contains('Appointed by')"),
            From: () => this.elements.Trustees.section().find('[data-testid="trustees-from"]'),
            FromHeader: () => this.elements.Trustees.section().find("th:contains('From')"),
            To: () => this.elements.Trustees.section().find('[data-testid="trustees-to"]'),
            ToHeader: () => this.elements.Trustees.section().find("th:contains('To')"),
            NoDataMessage: () => this.elements.Trustees.section().contains('No Trustees'),
        },
        Members: {
            section: () => cy.get('[data-testid="members"]'),
            names: () => this.elements.Members.section().find('[data-testid="members-name"]'),
            nameHeader: () => this.elements.Members.section().find("th:contains('Name')"),
            AppointedBy: () => this.elements.Members.section().find('[data-testid="members-appointed"]'),
            AppointedHeader: () => this.elements.Members.section().find("th:contains('Appointed by')"),
            From: () => this.elements.Members.section().find('[data-testid="members-from"]'),
            FromHeader: () => this.elements.Members.section().find("th:contains('From')"),
            To: () => this.elements.Members.section().find('[data-testid="members-to"]'),
            ToHeader: () => this.elements.Members.section().find("th:contains('To')"),
            NoDataMessage: () => this.elements.Members.section().contains('No Members'),

        },
        HistoricMembers: {
            section: () => cy.get('[data-testid="historic-members"]'),
            Names: () => this.elements.HistoricMembers.section().find('[data-testid="historic-members-name"]'),
            NameHeader: () => this.elements.HistoricMembers.section().find("th:contains('Name')"),
            Roles: () => this.elements.HistoricMembers.section().find('[data-testid="historic-members-role"]'),
            RoleHeader: () => this.elements.HistoricMembers.section().find("th:contains('Role')"),
            AppointedBy: () => this.elements.HistoricMembers.section().find('[data-testid="historic-members-appointed"]'),
            AppointedHeader: () => this.elements.HistoricMembers.section().find("th:contains('Appointed by')"),
            From: () => this.elements.HistoricMembers.section().find('[data-testid="historic-members-from"]'),
            FromHeader: () => this.elements.HistoricMembers.section().find("th:contains('From')"),
            To: () => this.elements.HistoricMembers.section().find('[data-testid="historic-members-to"]'),
            ToHeader: () => this.elements.HistoricMembers.section().find("th:contains('To')"),
            NoDataMessage: () => this.elements.HistoricMembers.section().contains('No Historic Members'),
        },
        subNav: {
            trustLeadershipSubnavButton: () => cy.get('[data-testid="governance-trust-leadership-subnav"]'),
            trusteesSubnavButton: () => cy.get('[data-testid="governance-trustees-subnav"]'),
            membersSubnavButton: () => cy.get('[data-testid="governance-members-subnav"]'),
            historicMembersSubnavButton: () => cy.get('[data-testid="governance-historic-members-subnav"]')
        },
        subHeaders: {
            subHeader: () => cy.get('[data-testid="subpage-header"]')
        },
    };

    // *************
    //
    // HEADER CHECKS
    //
    // *************

    public checkTrustLeadershipSubHeaderPresent() {
        this.elements.subHeaders.subHeader().should('be.visible');
        this.elements.subHeaders.subHeader().should('contain', 'Trust Leadership');
        return this;
    }

    public checkTrusteesSubHeaderPresent() {
        this.elements.subHeaders.subHeader().should('be.visible');
        this.elements.subHeaders.subHeader().should('contain', 'Trustees');
        return this;
    }

    public checkMembersSubHeaderPresent() {
        this.elements.subHeaders.subHeader().should('be.visible');
        this.elements.subHeaders.subHeader().should('contain', 'Members');
        return this;
    }

    public checkHistoricMembersSubHeaderPresent() {
        this.elements.subHeaders.subHeader().should('be.visible');
        this.elements.subHeaders.subHeader().should('contain', 'Historic members');
        return this;
    }

    public checkTrustLeadershipTableHeadersAreVisible() {
        this.elements.TrustLeadership.nameHeader().should('be.visible');
        this.elements.TrustLeadership.RoleHeader().should('be.visible');
        this.elements.TrustLeadership.FromHeader().should('be.visible');
        this.elements.TrustLeadership.ToHeader().should('be.visible');
        return this;
    }

    public checkTrusteesTableHeadersAreVisible() {
        this.elements.Trustees.NameHeader().should('be.visible');
        this.elements.Trustees.AppointedHeader().should('be.visible');
        this.elements.Trustees.FromHeader().should('be.visible');
        this.elements.Trustees.ToHeader().should('be.visible');
        return this;
    }

    public checkMembersTableHeadersAreVisible() {
        this.elements.Members.nameHeader().should('be.visible');
        this.elements.Members.AppointedHeader().should('be.visible');
        this.elements.Members.FromHeader().should('be.visible');
        this.elements.Members.ToHeader().should('be.visible');
        return this;
    }

    public checkHistoricMembersTableHeadersAreVisible() {
        this.elements.HistoricMembers.NameHeader().should('be.visible');
        this.elements.HistoricMembers.RoleHeader().should('be.visible');
        this.elements.HistoricMembers.AppointedHeader().should('be.visible');
        this.elements.HistoricMembers.FromHeader().should('be.visible');
        this.elements.HistoricMembers.ToHeader().should('be.visible');
        return this;
    }

    // **************
    //
    // SORTING CHECKS
    //
    // **************


    public checkTrustLeadershipColumnsSortCorrectly() {
        const { TrustLeadership } = this.elements;
        SortingUtility.checkStringSorting(TrustLeadership.names, TrustLeadership.nameHeader);
        SortingUtility.checkStringSorting(TrustLeadership.Roles, TrustLeadership.RoleHeader);
        SortingUtility.checkStringSorting(TrustLeadership.From, TrustLeadership.FromHeader);
        SortingUtility.checkStringSorting(TrustLeadership.To, TrustLeadership.ToHeader);
    }

    public checkTrusteesColumnsSortCorrectly() {
        const { Trustees } = this.elements;
        SortingUtility.checkStringSorting(Trustees.Names, Trustees.NameHeader);
        SortingUtility.checkStringSorting(Trustees.AppointedBy, Trustees.AppointedHeader);
        SortingUtility.checkStringSorting(Trustees.From, Trustees.FromHeader);
        SortingUtility.checkStringSorting(Trustees.To, Trustees.ToHeader);
    }

    public checkMembersColumnsSortCorrectly() {
        const { Members } = this.elements;
        SortingUtility.checkStringSorting(Members.names, Members.nameHeader);
        SortingUtility.checkStringSorting(Members.AppointedBy, Members.AppointedHeader);
        SortingUtility.checkStringSorting(Members.From, Members.FromHeader);
        SortingUtility.checkStringSorting(Members.To, Members.ToHeader);
    }

    public checkHistoricMembersColumnsSortCorrectly() {
        const { HistoricMembers } = this.elements;
        SortingUtility.checkStringSorting(HistoricMembers.Names, HistoricMembers.NameHeader);
        SortingUtility.checkStringSorting(HistoricMembers.Roles, HistoricMembers.RoleHeader);
        SortingUtility.checkStringSorting(HistoricMembers.AppointedBy, HistoricMembers.AppointedHeader);
        SortingUtility.checkStringSorting(HistoricMembers.From, HistoricMembers.FromHeader);
        SortingUtility.checkStringSorting(HistoricMembers.To, HistoricMembers.ToHeader);
    }

    // ***********
    //
    // DATE CHECKS
    //
    // ***********

    public checkTrustLeadershipAppointmentDatesAreCurrent(): this {
        governancePage.elements.TrustLeadership.From().each(element => { governancePage.checkDateIsBeforeTodayOrHasNoData(element); });
        governancePage.elements.TrustLeadership.To().each(element => { governancePage.checkDateIsAfterTodayOrHasNoData(element); });
        return this;
    }

    public checkTrusteesAppointmentDatesAreCurrent(): this {
        governancePage.elements.Trustees.From().each(element => { governancePage.checkDateIsBeforeTodayOrHasNoData(element); });
        governancePage.elements.Trustees.To().each(element => { governancePage.checkDateIsAfterTodayOrHasNoData(element); });
        return this;
    }

    public checkMembersAppointmentDatesAreCurrent(): this {
        governancePage.elements.Members.From().each(element => { governancePage.checkDateIsBeforeTodayOrHasNoData(element); });
        governancePage.elements.Members.To().each(element => { governancePage.checkDateIsAfterTodayOrHasNoData(element); });
        return this;
    }

    public checkHistoricMembersAppointmentDatesAreInThePast(): this {
        governancePage.elements.HistoricMembers.From().each(element => { governancePage.checkDateIsBeforeTodayOrHasNoData(element); });
        governancePage.elements.HistoricMembers.To().each(element => { governancePage.checkDateIsBeforeTodayOrHasNoData(element); });
        return this;
    }

    private checkDateIsBeforeTodayOrHasNoData(element: JQuery<HTMLElement>) {
        if (this.checkForSortValueOrNoData(element)) {
            const today = new Date(Date.now());
            today.setHours(0);
            today.setMinutes(0);
            today.setSeconds(0);
            today.setMilliseconds(0);
            const date = this.convertDataSortValueToDate(element);
            expect(date.getTime()).to.be.lessThan(today.getTime());
        }
    }

    private checkDateIsAfterTodayOrHasNoData(element: JQuery<HTMLElement>) {
        if (this.checkForSortValueOrNoData(element)) {
            const today = new Date(Date.now());
            today.setHours(0);
            today.setMinutes(0);
            today.setSeconds(0);
            today.setMilliseconds(0);
            const date = this.convertDataSortValueToDate(element);
            expect(date.getTime()).to.be.greaterThan(today.getTime());
        }
    }

    private convertDataSortValueToDate(element: JQuery<HTMLElement>) {
        const sortValue = element.attr('data-sort-value');
        if (!sortValue) {
            throw new Error("Sort value not found on element");
        }
        const year = parseInt(sortValue.substring(0, 4));
        const month = parseInt(sortValue.substring(4, 6));
        const day = parseInt(sortValue.substring(6));
        return new Date(year, month - 1, day);
    }

    // **********************
    //
    // NO DATA MESSAGE CHECKS
    //
    // **********************

    public checkNoTrustLeadershipMessageIsVisible(): this {
        this.elements.TrustLeadership.NoDataMessage().should('be.visible');
        return this;
    }

    public checkNoTrusteesMessageIsVisible(): this {
        this.elements.Trustees.NoDataMessage().should('be.visible');
        return this;
    }

    public checkNotMembersMessageIsVisible(): this {
        this.elements.Members.NoDataMessage().should('be.visible');
        return this;
    }

    public checkNoHistoricMembersMessageIsVisible(): this {
        this.elements.HistoricMembers.NoDataMessage().should('be.visible');
        return this;
    }

    public checkNoTrustLeadershipMessageIsHidden(): this {
        this.elements.TrustLeadership.NoDataMessage().should('not.exist');
        return this;
    }

    public checkNoTrusteesMessageIsHidden(): this {
        this.elements.Trustees.NoDataMessage().should('not.exist');
        return this;
    }

    public checkNoMembersMessageIsHidden(): this {
        this.elements.Members.NoDataMessage().should('not.exist');
        return this;
    }

    public checkNoHistoricMembersMessageIsHidden(): this {
        this.elements.HistoricMembers.NoDataMessage().should('not.exist');
        return this;
    }

    private checkForSortValueOrNoData(element: JQuery<HTMLElement>) {
        const sortValue = element.attr('data-sort-value');
        if (!sortValue) {
            expect(element.text().trim()).to.be.equal("No Data");
            return false;
        }
        return true;
    }

    // ***********
    //
    // ROLE CHECKS
    //
    // ***********

    public checkOnlyTrustLeadershipRolesArePresent() {
        this.elements.TrustLeadership.Roles().each(element => {
            expect(element.text().trim()).to.be.oneOf(["Chief Financial Officer", "Accounting Officer", "Chair of Trustees"]);
        });
        return this;
    }

    // ***********
    //
    // SUB NAVIGATION MENU
    //
    // ***********

    public clickTrustLeadershipSubnavButton(): this {
        this.elements.subNav.trustLeadershipSubnavButton().click();
        return this;
    }

    public clickTrusteesSubnavButton(): this {
        this.elements.subNav.trusteesSubnavButton().click();
        return this;
    }

    public clickMembersSubnavButton(): this {
        this.elements.subNav.membersSubnavButton().click();
        return this;
    }

    public clickHistoricMembersSubnavButton(): this {
        this.elements.subNav.historicMembersSubnavButton().click();
        return this;
    }

    public checkAllSubNavItemsPresent(): this {
        this.elements.subNav.trustLeadershipSubnavButton().should('be.visible');
        this.elements.subNav.trusteesSubnavButton().should('be.visible');
        this.elements.subNav.membersSubnavButton().should('be.visible');
        this.elements.subNav.historicMembersSubnavButton().should('be.visible');
        return this;
    }

    public checkSubNavNotPresent(): this {
        this.elements.subNav.trustLeadershipSubnavButton().should('not.exist');
        this.elements.subNav.trusteesSubnavButton().should('not.exist');
        this.elements.subNav.membersSubnavButton().should('not.exist');
        this.elements.subNav.historicMembersSubnavButton().should('not.exist');
        return this;
    }

    public checkTrustLeadershipSubnavButtonIsHighlighted(): this {
        this.elements.subNav.trustLeadershipSubnavButton().should('have.prop', 'aria-current', true);
        return this;
    }

    public checkTrusteesSubnavButtonIsHighlighted(): this {
        this.elements.subNav.trusteesSubnavButton().should('have.prop', 'aria-current', true);
        return this;
    }

    public checkMembersSubnavButtonIsHighlighted(): this {
        this.elements.subNav.membersSubnavButton().should('have.prop', 'aria-current', true);
        return this;
    }

    public checkHistoricMembersSubnavButtonIsHighlighted(): this {
        this.elements.subNav.historicMembersSubnavButton().should('have.prop', 'aria-current', true);
        return this;
    }

    private getCountFromSubNavButton(subNavButton: Cypress.Chainable<JQuery<HTMLElement>>): Cypress.Chainable<number> {
        return subNavButton
            .invoke('text')
            .then(text => {
                const matches = /\d+/.exec(text);
                if (!matches)
                    throw new Error("No count found in button text.");
                return parseInt(matches[0]);
            });
    }

    public checkTrustLeadershipSubnavButtonHasZeroInBrackets(): this {
        this.getCountFromSubNavButton(this.elements.subNav.trustLeadershipSubnavButton()).should('eq', 0);
        return this;
    }

    public checkTrusteesSubnavButtonHasZeroInBrackets(): this {
        this.getCountFromSubNavButton(this.elements.subNav.trusteesSubnavButton()).should('eq', 0);
        return this;
    }

    public checkMembersSubnavButtonHasZeroInBrackets(): this {
        this.getCountFromSubNavButton(this.elements.subNav.membersSubnavButton()).should('eq', 0);
        return this;
    }

    public checkHistoricMembersSubnavButtonHasZeroInBrackets(): this {
        this.getCountFromSubNavButton(this.elements.subNav.historicMembersSubnavButton()).should('eq', 0);
        return this;
    }
}

const governancePage = new GovernancePage();
export default governancePage;
