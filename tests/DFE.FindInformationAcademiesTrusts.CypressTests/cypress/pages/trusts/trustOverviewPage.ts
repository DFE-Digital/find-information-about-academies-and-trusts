class TrustOverviewPage {

    elements = {
        overviewHeader: () => cy.get('[data-testid="page-name"]'),
        overviewSummaryCardContentBox: () => cy.get('[data-testid="trust-summary"]'),
        overviewOfstedSummaryCardContentBox: () => cy.get('[data-testid="ofsted-ratings"]'),
        topRatingItem: () => cy.get('[data-sort-value="1"]'),
        ratingHeader: () => cy.get('thead th').contains('Rating'),
        tableRowSortValues: () => cy.get('tbody.govuk-table__body tr td[data-sort-value]'),
        firstRowRatingText: () => cy.get('tbody.govuk-table__body tr:first-child td:first-child'),
    };

    public checkOverviewHeaderPresent(): this {
        this.elements.overviewHeader().should('be.visible')
        this.elements.overviewHeader().should('contain', 'Overview');
        return this;
    }

    public checkTrustOverviewSummaryCardItemsPresent(): this {
        this.elements.overviewSummaryCardContentBox().should('contain', 'Total academies')
        this.elements.overviewSummaryCardContentBox().should('contain', 'Academies in each local authority')
        this.elements.overviewSummaryCardContentBox().should('contain', 'Pupil numbers')
        this.elements.overviewSummaryCardContentBox().should('contain', 'Pupil capacity');
        return this;
    }

    public checkOverviewOfstedRatingsSummaryCardItemsPresent(): this {
        this.elements.overviewOfstedSummaryCardContentBox().should('contain', 'Outstanding')
        this.elements.overviewOfstedSummaryCardContentBox().should('contain', 'Good')
        this.elements.overviewOfstedSummaryCardContentBox().should('contain', 'Requires improvement')
        this.elements.overviewOfstedSummaryCardContentBox().should('contain', 'Inadequate')
        this.elements.overviewOfstedSummaryCardContentBox().should('contain', 'Not yet inspected');
        return this;
    }

    public checkOfstedHeaderPresent(): this {
        this.elements.overviewHeader().should('be.visible')
        this.elements.overviewHeader().should('contain', 'Ofsted ratings');
        return this;
    }

    public checkCurrentTopRatingItemIs(currentTopRating: string): this {
        this.elements.topRatingItem().should('contain', currentTopRating);
        return this;
    }

    public clickRatingHeader(): this {
        this.elements.ratingHeader().click();
        return this;
    }

    public checkRatingSortAscending(): this {
        const sortValues: number[] = [];

        this.elements.tableRowSortValues().each(($el) => {
            const value = parseInt($el.attr('data-sort-value') || '', 10);
            sortValues.push(value);
        }).then(() => {
            const isSortedAsc = sortValues.every((val, i, arr) => !i || val >= arr[i - 1]);
            expect(isSortedAsc).to.be.true;
        });

        return this;
    }

    public checkRatingSortDescending(): this {
        const sortValues: number[] = [];

        this.elements.tableRowSortValues().each(($el) => {
            const value = parseInt($el.attr('data-sort-value') || '', 10);
            sortValues.push(value);
        }).then(() => {
            const isSortedDesc = sortValues.every((val, i, arr) => !i || val <= arr[i - 1]);
            expect(isSortedDesc).to.be.true;
        });

        return this;
    }

    public checkTopResultItem(topResultText: string): this {
        this.elements.firstRowRatingText().should('have.text', topResultText);
        return this;
    }


}

const trustOverviewPage = new TrustOverviewPage();
export default trustOverviewPage;
