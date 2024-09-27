class TrustPage {

    public hasName(value: string): this {
        cy.getByTestId("trust-name-heading").should("contain.text", value);

        return this;
    }

    public hasType(value: string): this {
        cy.getByTestId("trust-type").should("contain.text", value);

        return this;
    }

    public viewContacts(): this {
        cy.getByTestId("contacts-nav").click();

        return this;
    }

    public viewOverview(): this {
        cy.getByTestId("overview-nav").click();

        return this;
    }

    public viewAcademies(): this {
        cy.getByTestId("academies-nav").click();

        return this;
    }

    public viewAcademyOfstedRatings(): this {
        cy.getById("ofsted-ratings-link").click();

        return this;
    }

    public viewAcademyPupilNumbers(): this {
        cy.getById("academies-pupil-numbers-link").click();

        return this;
    }

    public viewFreeSchoolMeals(): this {
        cy.getById("free-school-meals-link").click();

        return this;

    }
}

const trustPage = new TrustPage();

export default trustPage;