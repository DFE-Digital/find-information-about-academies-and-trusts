class AcademiesInTrustPage {

    elements = {
        DetailsPage: {
            table: () => cy.get('.govuk-main-wrapper > .dfe-width-container > .govuk-grid-row > .govuk-grid-column-three-quarters'),
            authTypes: () => cy.get('[data-testid="type-of-establishment"]'),
        },
        OfstedPage: {
            table: () => cy.get('.govuk-main-wrapper > .dfe-width-container > .govuk-grid-row > .govuk-grid-column-three-quarters'),
            previousOfstedRating: () => cy.get('[data-testid="previous-ofsted-rating"]'),
            currentOfstedRating: () => cy.get('[data-testid="current-ofsted-rating"]'),
        },
        PupilNumbers: {
            table: () => cy.get('.govuk-main-wrapper > .dfe-width-container > .govuk-grid-row > .govuk-grid-column-three-quarters'),
        },
        FreeSchoolMeals: {
            table: () => cy.get('.govuk-main-wrapper > .dfe-width-container > .govuk-grid-row > .govuk-grid-column-three-quarters'),
        },

    };

    public checkAITDetailsHeadersPresent(): this {
        this.elements.DetailsPage.table().should('contain', 'School name');
        this.elements.DetailsPage.table().should('contain', 'Local authority');
        this.elements.DetailsPage.table().should('contain', 'Type');
        this.elements.DetailsPage.table().should('contain', 'Rural or urban');
        this.elements.DetailsPage.table().should('contain', 'Get information about schools');
        return this;
    }

    public checkAITOfstedHeadersPresent(): this {
        this.elements.OfstedPage.table().should('contain', 'School name');
        this.elements.OfstedPage.table().should('contain', 'Date joined');
        this.elements.OfstedPage.table().should('contain', 'Previous Ofsted rating');
        this.elements.OfstedPage.table().should('contain', 'Current Ofsted rating');
        return this;
    }

    public checkAITPupilNumbersHeadersPresent(): this {
        this.elements.OfstedPage.table().should('contain', 'School name');
        this.elements.OfstedPage.table().should('contain', 'Phase and age range');
        this.elements.OfstedPage.table().should('contain', 'Pupil numbers');
        this.elements.OfstedPage.table().should('contain', 'Pupil capacity');
        this.elements.OfstedPage.table().should('contain', '% full');
        return this;
    }

    public checkAITFreeSchoolMealsHeadersPresent(): this {
        this.elements.OfstedPage.table().should('contain', 'School name');
        this.elements.OfstedPage.table().should('contain', 'Pupils eligible for free school meals');
        this.elements.OfstedPage.table().should('contain', 'Local authority average');
        this.elements.OfstedPage.table().should('contain', 'National average');
        return this;
    }

    public checkAuthTypesOnAITDetailsTable() {
        this.elements.DetailsPage.authTypes().each(element => {
            expect(element.text().trim()).to.be.oneOf(["Academy sponsor led", "Academy converter", "University technical college", "Free schools"])
        });
    }

    public checkPreviousOfstedTypesOnOfstedTable(): this {
        this.elements.OfstedPage.previousOfstedRating().should(($elements) => {
            $elements.each((index, element) => {
                const text = Cypress.$(element).text().trim();
                expect(text).to.match(/Good|Inadequate|Not yet inspected|Outstanding|Requires improvement/);
            });
        });
        return this;
    }

    public checkCurrentOfstedTypesOnOfstedTable(): this {
        this.elements.OfstedPage.currentOfstedRating().should(($elements) => {
            $elements.each((index, element) => {
                const text = Cypress.$(element).text().trim();
                expect(text).to.match(/Good|Inadequate|Not yet inspected|Outstanding|Requires improvement/);
            });
        });
        return this;
    }

}

const academiesInTrustPage = new AcademiesInTrustPage();
export default academiesInTrustPage;
