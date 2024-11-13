class TrustOverviewPage {

    elements = {
        overviewHeader: () => cy.get('[data-testid="page-name"]'),
        trustSummaryCard: () => cy.get('[data-testid="trust-summary"]'),
        overviewOfstedSummaryCardContentBox: () => cy.get('[data-testid="ofsted-ratings"]'),
        tableRowSortValues: () => cy.get('tbody.govuk-table__body tr td[data-sort-value]'),
        firstRowRatingText: () => cy.get('tbody.govuk-table__body tr:first-child td:first-child'),
        detailsHeader: () => cy.get('[data-testid="page-name"]'),
        trustDetailsCard: () => cy.get('[data-testid="trust-details-summary-card"]'),
        referenceNumberCard: () => cy.get('[data-testid="reference-numbers-summary-card"]'),
    };

    public checkOverviewHeaderPresent(): this {
        this.elements.overviewHeader().should('be.visible')
        this.elements.overviewHeader().should('contain', 'Overview');
        return this;
    }

    public checkTrustSummaryCardItemsPresent(): this {
        this.elements.trustSummaryCard().should('contain', 'Total academies')
        this.elements.trustSummaryCard().should('contain', 'Academies in each local authority')
        this.elements.trustSummaryCard().should('contain', 'Pupil numbers')
        this.elements.trustSummaryCard().should('contain', 'Pupil capacity');
        return this;
    }


    public checkTrustSummaryCardPresent(): this {
        this.elements.trustSummaryCard().should('be.visible');
        this.elements.trustSummaryCard().should('contain', 'Trust summary');
        return this;
    }

    public checkTrustDetailCardPresent(): this {
        this.elements.trustDetailsCard().should('be.visible');
        this.elements.trustDetailsCard().should('contain', 'Trust details');
        return this;
    }

    public checkTrustDetailCardItemsPresent(): this {
        this.elements.trustDetailsCard().should('contain', 'Address');
        this.elements.trustDetailsCard().should('contain', 'Opened on');
        this.elements.trustDetailsCard().should('contain', 'Region and territory');
        this.elements.trustDetailsCard().should('contain', 'Information from other services');
        return this;
    }

    public checkReferenceNumbersCardPresent(): this {
        this.elements.referenceNumberCard().should('be.visible');
        this.elements.referenceNumberCard().should('contain', 'Reference numbers');
        return this;
    }

    public checkReferenceNumbersCardItemsPresent(): this {
        this.elements.referenceNumberCard().should('contain', 'UID');
        this.elements.referenceNumberCard().should('contain', 'Group ID');
        this.elements.referenceNumberCard().should('contain', 'UKPRN');
        this.elements.referenceNumberCard().should('contain', 'Companies House number');
        return this;
    }

}

const trustOverviewPage = new TrustOverviewPage();
export default trustOverviewPage;
