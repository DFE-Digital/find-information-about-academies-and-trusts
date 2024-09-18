import { SortingUtility } from "./sortingUtility";

class AcademiesInTrustPage {

    elements = {
        DetailsPage: {
            table: () => cy.get('table'),
            schoolName: () => this.elements.DetailsPage.table().find('[data-testid="school-name"]'),
            schoolNameHeader: () => this.elements.DetailsPage.table().find("th:contains('School name')"),

            localAuthority: () => this.elements.DetailsPage.table().find('[data-testid="local-authority"]'),
            localAuthorityHeader: () => this.elements.DetailsPage.table().find("th:contains('Local authority')"),

            schoolType: () => this.elements.DetailsPage.table().find('[data-testid="type-of-establishment"]'),
            schoolTypeHeader: () => this.elements.DetailsPage.table().find("th:contains('Type')"),

            ruralOrUrban: () => this.elements.DetailsPage.table().find('[data-testid="urban-or-rural"]'),
            ruralOrUrbanHeader: () => this.elements.DetailsPage.table().find("th:contains('Rural or urban')"),
        },

        OfstedPage: {
            table: () => cy.get('table'),

            schoolName: () => this.elements.OfstedPage.table().find('[data-testid="school-name"]'),
            schoolNameHeader: () => this.elements.OfstedPage.table().find("th:contains('School name')"),

            dateJoined: () => this.elements.OfstedPage.table().find('[data-testid="date-joined"]'),
            dateJoinedHeader: () => this.elements.OfstedPage.table().find("th:contains('Date joined')"),

            previousOfstedRating: () => cy.get('[data-testid="previous-ofsted-rating"]'),
            previousOfstedRatingHeader: () => this.elements.OfstedPage.table().find("th:contains('Previous Ofsted rating')"),

            currentOfstedRating: () => cy.get('[data-testid="current-ofsted-rating"]'),
            currentOfstedRatingHeader: () => this.elements.OfstedPage.table().find("th:contains('Current Ofsted rating')"),
        },

        PupilNumbersPage: {
            table: () => cy.get('table'),

            schoolName: () => this.elements.PupilNumbersPage.table().find('[data-testid="school-name"]'),
            schoolNameHeader: () => this.elements.PupilNumbersPage.table().find("th:contains('School name')"),

            phaseAndAge: () => this.elements.PupilNumbersPage.table().find('[data-testid="phase-and-age-range"]'),
            phaseAndAgeHeader: () => this.elements.PupilNumbersPage.table().find("th:contains('Phase and age range')"),

            pupilNumbers: () => this.elements.PupilNumbersPage.table().find('[data-testid="pupil-numbers"]'),
            pupilNumbersHeader: () => this.elements.PupilNumbersPage.table().find("th:contains('Pupil numbers')"),

            pupilCapacity: () => this.elements.PupilNumbersPage.table().find('[data-testid="pupil-capacity"]'),
            pupilCapacityHeader: () => this.elements.PupilNumbersPage.table().find("th:contains('Pupil capacity')"),
        },

        FreeSchoolMeals: {
            table: () => cy.get('table'),
        },

    };

    public checkDetailsHeadersPresent(): this {
        this.elements.DetailsPage.table().should('contain', 'School name');
        this.elements.DetailsPage.table().should('contain', 'Local authority');
        this.elements.DetailsPage.table().should('contain', 'Type');
        this.elements.DetailsPage.table().should('contain', 'Rural or urban');
        this.elements.DetailsPage.table().should('contain', 'Get information about schools');
        return this;
    }

    public checkOfstedHeadersPresent(): this {
        this.elements.OfstedPage.table().should('contain', 'School name');
        this.elements.OfstedPage.table().should('contain', 'Date joined');
        this.elements.OfstedPage.table().should('contain', 'Previous Ofsted rating');
        this.elements.OfstedPage.table().should('contain', 'Current Ofsted rating');
        return this;
    }

    public checkPupilNumbersHeadersPresent(): this {
        this.elements.OfstedPage.table().should('contain', 'School name');
        this.elements.OfstedPage.table().should('contain', 'Phase and age range');
        this.elements.OfstedPage.table().should('contain', 'Pupil numbers');
        this.elements.OfstedPage.table().should('contain', 'Pupil capacity');
        this.elements.OfstedPage.table().should('contain', '% full');
        return this;
    }

    public checkFreeSchoolMealsHeadersPresent(): this {
        this.elements.OfstedPage.table().should('contain', 'School name');
        this.elements.OfstedPage.table().should('contain', 'Pupils eligible for free school meals');
        this.elements.OfstedPage.table().should('contain', 'Local authority average');
        this.elements.OfstedPage.table().should('contain', 'National average');
        return this;
    }

    public checkSchoolTypesOnDetailsTable() {
        this.elements.DetailsPage.schoolType().each(element => {
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

    public checkTrustDetailsSorting() {
        const { DetailsPage } = this.elements;
        SortingUtility.checkStringSorting(DetailsPage.schoolName, DetailsPage.schoolNameHeader);
        SortingUtility.checkStringSorting(DetailsPage.localAuthority, DetailsPage.localAuthorityHeader);
        SortingUtility.checkStringSorting(DetailsPage.schoolType, DetailsPage.schoolTypeHeader);
        SortingUtility.checkStringSorting(DetailsPage.ruralOrUrban, DetailsPage.ruralOrUrbanHeader);
    }

    public checkOfstedSorting() {
        const { OfstedPage } = this.elements;
        SortingUtility.checkStringSorting(OfstedPage.schoolName, OfstedPage.schoolNameHeader);
        SortingUtility.checkStringSorting(OfstedPage.dateJoined, OfstedPage.dateJoinedHeader);
        SortingUtility.checkStringSorting(OfstedPage.previousOfstedRating, OfstedPage.previousOfstedRatingHeader);
        SortingUtility.checkStringSorting(OfstedPage.currentOfstedRating, OfstedPage.currentOfstedRatingHeader);
    }

    public checkPupilNumbersSorting() {
        const { PupilNumbersPage: PupilNumbersPage } = this.elements;
        SortingUtility.checkStringSorting(PupilNumbersPage.schoolName, PupilNumbersPage.schoolNameHeader);
        SortingUtility.checkStringSorting(PupilNumbersPage.phaseAndAge, PupilNumbersPage.phaseAndAgeHeader);
        SortingUtility.checkNumericSorting(PupilNumbersPage.pupilNumbers, PupilNumbersPage.pupilNumbersHeader);
        SortingUtility.checkNumericSorting(PupilNumbersPage.pupilCapacity, PupilNumbersPage.pupilCapacityHeader);

    }

}

const academiesInTrustPage = new AcademiesInTrustPage();
export default academiesInTrustPage;
