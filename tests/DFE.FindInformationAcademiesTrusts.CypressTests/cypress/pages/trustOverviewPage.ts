class TrustOverviewPage {
    public hasTotalAcademies(value: string): this {
        cy.getByTestId("total-academies").should("contain.text", value);

        return this;
    }

    public hasAcademiesInEachAuthority(value: string): this {
        cy.getByTestId("academies-in-each-authority").should("contain.text", value);

        return this;
    }

    public hasNumberOfPupils(value: string): this {
        cy.getByTestId("number-of-pupils").should("contain.text", value);

        return this;

    }

    public hasPupilCapacity(total: string, percentage: string): this {
        cy.getByTestId("pupil-capacity").should("contain.text", total);
        cy.getByTestId("pupil-capacity").should("contain.text", percentage);

        return this;

    }

    public hasOfstedRatingOutstanding(value: string): this {
        cy.getByTestId("ofsted-rating-outstanding").should("contain.text", value);

        return this;
    }

    public hasOfstedRatingsGood(value: string): this {
        cy.getByTestId("ofsted-rating-good").should("contain.text", value);

        return this;
    }


    public hasOfstedRatingRequiresImprovement(value: string): this {
        cy.getByTestId("ofsted-rating-requires-improvement").should("contain.text", value);

        return this;
    }

    public hasOfstedRatingInadequate(value: string): this {
        cy.getByTestId("ofsted-rating-inadequate").should("contain.text", value);

        return this;
    }

    public hasOfstedRatingNotInspectedYet(value: string): this {
        cy.getByTestId("ofsted-rating-not-inspected-yet").should("contain.text", value);

        return this;
    }
}

const trustOverviewPage = new TrustOverviewPage();

export default trustOverviewPage;