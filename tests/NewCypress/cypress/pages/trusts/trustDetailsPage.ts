class TrustDetailsPage {

    elements = {
        detailsHeader: () => cy.get('[data-testid="page-name"]'),
        trustDetailsSummaryCard: () => cy.get('[data-testid="trust-details-summary-card"]'),
        referenceNumberSummaryCard: () => cy.get('[data-testid="reference-numbers-summary-card"]'),
    };

    public checkDetailsHeaderPresent(): this {
        this.elements.detailsHeader().should('be.visible');
        this.elements.detailsHeader().should('contain', 'Details');
        return this;
    }

    public checkTrustDetailSummaryCardPresent(): this {
        this.elements.trustDetailsSummaryCard().should('be.visible');
        this.elements.trustDetailsSummaryCard().should('contain', 'Trust details');
        return this;
    }

    public checkTrustDetailSummaryCardItemsPresent(): this {
        this.elements.trustDetailsSummaryCard().should('contain', 'Address');
        this.elements.trustDetailsSummaryCard().should('contain', 'Opened on');
        this.elements.trustDetailsSummaryCard().should('contain', 'Region and territory');
        this.elements.trustDetailsSummaryCard().should('contain', 'Information from other services');
        return this;
    }

    public checkReferenceNumbersSummaryCardPresent(): this {
        this.elements.referenceNumberSummaryCard().should('be.visible');
        this.elements.referenceNumberSummaryCard().should('contain', 'Reference numbers');
        return this;
    }

    public checkReferenceNumbersSummaryCardItemsPresent(): this {
        this.elements.referenceNumberSummaryCard().should('contain', 'UID');
        this.elements.referenceNumberSummaryCard().should('contain', 'Group ID');
        this.elements.referenceNumberSummaryCard().should('contain', 'UKPRN');
        this.elements.referenceNumberSummaryCard().should('contain', 'Companies House number');
        return this;
    }


}

const trustDetailsPage = new TrustDetailsPage();
export default trustDetailsPage;
