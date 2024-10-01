import { SortingUtility } from "./sortingUtility";
class GovernancePage {

    elements = {
        TrustLeadership: {
            Section: () => cy.get('[data-testid="trust-leadership"]'),
            Names: () => this.elements.TrustLeadership.Section().find('[data-testid="trust-leadership-name"]'),
            NameHeader: () => this.elements.TrustLeadership.Section().find("th:contains('Name')"),
            Roles: () => this.elements.TrustLeadership.Section().find('[data-testid="trust-leadership-role"]'),
            RoleHeader: () => this.elements.TrustLeadership.Section().find("th:contains('Role')"),
            From: () => this.elements.TrustLeadership.Section().find('[data-testid="trust-leadership-from"]'),
            FromHeader: () => this.elements.TrustLeadership.Section().find("th:contains('From')"),
            To: () => this.elements.TrustLeadership.Section().find('[data-testid="trust-leadership-to"]'),
            ToHeader: () => this.elements.TrustLeadership.Section().find("th:contains('To')"),
            NoDataMessage: () => this.elements.TrustLeadership.Section().contains('No Trust Leadership')
        },
        Trustees: {
            Section: () => cy.get('[data-testid="trustees"]'),
            Names: () => this.elements.Trustees.Section().find('[data-testid="trustees-name"]'),
            NameHeader: () => this.elements.Trustees.Section().find("th:contains('Name')"),
            AppointedBys: () => this.elements.Trustees.Section().find('[data-testid="trustees-appointed"]'),
            AppointedHeader: () => this.elements.Trustees.Section().find("th:contains('Appointed by')"),
            From: () => this.elements.Trustees.Section().find('[data-testid="trustees-from"]'),
            FromHeader: () => this.elements.Trustees.Section().find("th:contains('From')"),
            To: () => this.elements.Trustees.Section().find('[data-testid="trustees-to"]'),
            ToHeader: () => this.elements.Trustees.Section().find("th:contains('To')"),
            NoDataMessage: () => this.elements.Trustees.Section().contains('No Trustees')
        },
        Members: {
            Section: () => cy.get('[data-testid="members"]'),
            Names: () => this.elements.Members.Section().find('[data-testid="members-name"]'),
            NameHeader: () => this.elements.Members.Section().find("th:contains('Name')"),
            AppointedBys: () => this.elements.Members.Section().find('[data-testid="members-appointed"]'),
            AppointedHeader: () => this.elements.Members.Section().find("th:contains('Appointed by')"),
            From: () => this.elements.Members.Section().find('[data-testid="members-from"]'),
            FromHeader: () => this.elements.Members.Section().find("th:contains('From')"),
            To: () => this.elements.Members.Section().find('[data-testid="members-to"]'),
            ToHeader: () => this.elements.Members.Section().find("th:contains('To')"),
            NoDataMessage: () => this.elements.Members.Section().contains('No Members')
        },
        HistoricMembers: {
            Section: () => cy.get('[data-testid="historic-members"]'),
            Names: () => this.elements.HistoricMembers.Section().find('[data-testid="historic-members-name"]'),
            NameHeader: () => this.elements.HistoricMembers.Section().find("th:contains('Name')"),
            Roles: () => this.elements.HistoricMembers.Section().find('[data-testid="historic-members-role"]'),
            RoleHeader: () => this.elements.HistoricMembers.Section().find("th:contains('Role')"),
            AppointedBys: () => this.elements.HistoricMembers.Section().find('[data-testid="historic-members-appointed"]'),
            AppointedHeader: () => this.elements.HistoricMembers.Section().find("th:contains('Appointed by')"),
            From: () => this.elements.HistoricMembers.Section().find('[data-testid="historic-members-from"]'),
            FromHeader: () => this.elements.HistoricMembers.Section().find("th:contains('From')"),
            To: () => this.elements.HistoricMembers.Section().find('[data-testid="historic-members-to"]'),
            ToHeader: () => this.elements.HistoricMembers.Section().find("th:contains('To')"),
            NoDataMessage: () => this.elements.HistoricMembers.Section().contains('No Historic Members')
        }
    }

    // **********
    //
    // NAVIGATION
    //
    // **********

    public navigateToFullGovernancePage(): this {
        cy.visit('/trusts/governance?uid=5712')

        return this;
    }

    public navigateToEmptyGovernancePage(): this {
        cy.visit('/trusts/governance?uid=5527')

        return this;
    }

    // *************
    //
    // HEADER CHECKS
    //
    // *************

    public checkTrustLeadershipColumnHeaders() {
        this.elements.TrustLeadership.NameHeader().should('be.visible');
        this.elements.TrustLeadership.RoleHeader().should('be.visible');
        this.elements.TrustLeadership.FromHeader().should('be.visible');
        this.elements.TrustLeadership.ToHeader().should('be.visible');
        return this;
    }

    public checkTrusteeColumnHeaders() {
        this.elements.Trustees.NameHeader().should('be.visible');
        this.elements.Trustees.AppointedHeader().should('be.visible');
        this.elements.Trustees.FromHeader().should('be.visible');
        this.elements.Trustees.ToHeader().should('be.visible');
        return this;
    }

    public checkMembersColumnHeaders() {
        this.elements.Members.NameHeader().should('be.visible');
        this.elements.Members.AppointedHeader().should('be.visible');
        this.elements.Members.FromHeader().should('be.visible');
        this.elements.Members.ToHeader().should('be.visible');
        return this;
    }

    public checkHistoricMembersColumnHeaders() {
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


    public checkTrustLeadershipSorting() {
        const { TrustLeadership } = this.elements;
        SortingUtility.checkStringSorting(TrustLeadership.Names, TrustLeadership.NameHeader);
        SortingUtility.checkStringSorting(TrustLeadership.Roles, TrustLeadership.RoleHeader);
        SortingUtility.checkStringSorting(TrustLeadership.From, TrustLeadership.FromHeader);
        SortingUtility.checkStringSorting(TrustLeadership.To, TrustLeadership.ToHeader);
    }

    public checkTrusteesSorting() {
        const { Trustees } = this.elements;
        SortingUtility.checkStringSorting(Trustees.Names, Trustees.NameHeader);
        SortingUtility.checkStringSorting(Trustees.AppointedBys, Trustees.AppointedHeader);
        SortingUtility.checkStringSorting(Trustees.From, Trustees.FromHeader);
        SortingUtility.checkStringSorting(Trustees.To, Trustees.ToHeader);
    }

    public checkMembersSorting() {
        const { Members } = this.elements;
        SortingUtility.checkStringSorting(Members.Names, Members.NameHeader);
        SortingUtility.checkStringSorting(Members.AppointedBys, Members.AppointedHeader);
        SortingUtility.checkStringSorting(Members.From, Members.FromHeader);
        SortingUtility.checkStringSorting(Members.To, Members.ToHeader);
    }

    public checkHistoricMembersSorting() {
        const { HistoricMembers } = this.elements;
        SortingUtility.checkStringSorting(HistoricMembers.Names, HistoricMembers.NameHeader);
        SortingUtility.checkStringSorting(HistoricMembers.Roles, HistoricMembers.RoleHeader);
        SortingUtility.checkStringSorting(HistoricMembers.AppointedBys, HistoricMembers.AppointedHeader);
        SortingUtility.checkStringSorting(HistoricMembers.From, HistoricMembers.FromHeader);
        SortingUtility.checkStringSorting(HistoricMembers.To, HistoricMembers.ToHeader);
    }

    // ***********
    //
    // DATE CHECKS
    //
    // ***********

    public checkDatesInTablesAreValid(): this {
        governancePage.elements.TrustLeadership.From().each(element => { governancePage.checkDateCellIsBeforeToday(element) })
        governancePage.elements.TrustLeadership.To().each(element => { governancePage.checkDateCellIsAfterToday(element) })

        governancePage.elements.Trustees.From().each(element => { governancePage.checkDateCellIsBeforeToday(element) })
        governancePage.elements.Trustees.To().each(element => { governancePage.checkDateCellIsAfterToday(element) })

        governancePage.elements.Members.From().each(element => { governancePage.checkDateCellIsBeforeToday(element) })
        governancePage.elements.Members.To().each(element => { governancePage.checkDateCellIsAfterToday(element) })

        governancePage.elements.HistoricMembers.From().each(element => { governancePage.checkDateCellIsBeforeToday(element) })
        governancePage.elements.HistoricMembers.To().each(element => { governancePage.checkDateCellIsBeforeToday(element) })
        return this;
    }

    private checkDateCellIsBeforeToday(element: JQuery<HTMLElement>) {
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

    private checkDateCellIsAfterToday(element: JQuery<HTMLElement>) {
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

    public checkNoTrustLeadershipMessageIsVisble(): this {
        this.elements.TrustLeadership.NoDataMessage().should('be.visible');
        return this;
    }

    public checkNoTrusteesMessageIsVisble(): this {
        this.elements.Trustees.NoDataMessage().should('be.visible');
        return this;
    }

    public checkNotMembersMessageIsVisble(): this {
        this.elements.Members.NoDataMessage().should('be.visible');
        return this;
    }

    public checkNoHistoricMembersMessageIsVisble(): this {
        this.elements.HistoricMembers.NoDataMessage().should('be.visible');
        return this;
    }

    public checkNoTrustLeadershipMessageIsNotVisble(): this {
        this.elements.TrustLeadership.NoDataMessage().should('not.exist');
        return this;
    }

    public checkNoTrusteesMessageIsNotVisble(): this {
        this.elements.Trustees.NoDataMessage().should('not.exist');
        return this;
    }

    public checkNoMembersMessageIsNotVisble(): this {
        this.elements.Members.NoDataMessage().should('not.exist');
        return this;
    }

    public checkNoHistoricMembersMessageIsNotVisble(): this {
        this.elements.HistoricMembers.NoDataMessage().should('not.exist');
        return this;
    }

    private checkForSortValueOrNoData(element: JQuery<HTMLElement>) {
        const sortValue = element.attr('data-sort-value');
        if (!sortValue) {
            expect(element.text().trim()).to.be.equal("No Data")
            return false;
        }
        return true;
    }

    // ***********
    //
    // ROLE CHECKS
    //
    // ***********

    public checkRolesOnTrustLeadershipTable() {
        this.elements.TrustLeadership.Roles().each(element => {
            expect(element.text().trim()).to.be.oneOf(["Chief Financial Officer", "Accounting Officer", "Chair of Trustees"])
        });
    }
}

const governancePage = new GovernancePage();

export default governancePage;
