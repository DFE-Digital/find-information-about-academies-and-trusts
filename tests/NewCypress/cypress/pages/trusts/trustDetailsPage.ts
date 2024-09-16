class TrustDetailsPage {

    elements = {
        detailsHeader: () => cy.get('header > .govuk-heading-l'),
        trustDetailSummaryCard: () => cy.get(':nth-child(2) > .govuk-summary-card'),
        detailsSummaryCardContentBox: () => cy.get(':nth-child(2) > .govuk-summary-card > .govuk-summary-card__content'),
        referenceNumberSummaryCard: () => cy.get(':nth-child(3) > .govuk-summary-card'),
        referenceNumberSummaryCardContentBox: () => cy.get(':nth-child(3) > .govuk-summary-card > .govuk-summary-card__content'),

    };

    public checkDetailsHeaderPresent(): this {
        this.elements.detailsHeader().should('be.visible');
        this.elements.detailsHeader().should('contain', 'Details');
        return this;
    }

    public checkTrustDetailSummaryCardPresent(): this {
        this.elements.trustDetailSummaryCard().should('be.visible');
        this.elements.trustDetailSummaryCard().should('contain', 'Trust details');
        return this;
    }

    public checkTrustDetailSummaryCardItemsPresent(): this {
        this.elements.detailsSummaryCardContentBox().should('contain', 'Address');
        this.elements.detailsSummaryCardContentBox().should('contain', 'Opened on');
        this.elements.detailsSummaryCardContentBox().should('contain', 'Region and territory');
        this.elements.detailsSummaryCardContentBox().should('contain', 'Information from other services');
        return this;
    }

    public checkReferenceNumbersSummaryCardPresent(): this {
        this.elements.referenceNumberSummaryCard().should('be.visible');
        this.elements.referenceNumberSummaryCard().should('contain', 'Reference numbers');
        return this;
    }

    public checkReferenceNumbersSummaryCardItemsPresent(): this {
        this.elements.referenceNumberSummaryCardContentBox().should('contain', 'UID');
        this.elements.referenceNumberSummaryCardContentBox().should('contain', 'Group ID');
        this.elements.referenceNumberSummaryCardContentBox().should('contain', 'UKPRN');
        this.elements.referenceNumberSummaryCardContentBox().should('contain', 'Companies House number');
        return this;
    }


}

const trustDetailsPage = new TrustDetailsPage();
export default trustDetailsPage;
