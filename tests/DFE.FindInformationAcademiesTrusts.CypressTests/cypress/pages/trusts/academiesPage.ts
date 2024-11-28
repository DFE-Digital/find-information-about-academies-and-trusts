import { TableUtility } from "../tableUtility";

class AcademiesPage {
    // Elements for each page section
    elements = {
        PageTabs: {
            academyCountLabel: () => cy.get('[data-testid="academies-nav"]'),
        },
        DetailsPage: this.createDetailsPageElements(),
        OfstedPage: this.createOfstedPageElements(),
        PupilNumbersPage: this.createPupilNumbersPageElements(),
        FreeSchoolMeals: this.createFreeSchoolMealsElements(),
    };

    private createDetailsPageElements() {
        const table = () => cy.get('[aria-describedby="academies-details-link"]');
        return {
            table,
            tableRows: () => table().find('tbody tr'),
            schoolName: () => table().find('[data-testid="school-name"]'),
            schoolNameHeader: () => table().find("th:contains('School name')"),
            localAuthority: () => table().find('[data-testid="local-authority"]'),
            localAuthorityHeader: () => table().find("th:contains('Local authority')"),
            schoolType: () => table().find('[data-testid="type-of-establishment"]'),
            schoolTypeHeader: () => table().find("th:contains('Type')"),
            ruralOrUrban: () => table().find('[data-testid="urban-or-rural"]'),
            ruralOrUrbanHeader: () => table().find("th:contains('Rural or urban')"),
        };
    }

    private createOfstedPageElements() {
        const table = () => cy.get('[aria-describedby="ofsted-ratings-link"]');
        return {
            table,
            schoolName: () => table().find('[data-testid="school-name"]'),
            schoolNameHeader: () => table().find("th:contains('School name')"),
            dateJoined: () => table().find('[data-testid="date-joined"]'),
            dateJoinedHeader: () => table().find("th:contains('Date joined')"),
            previousOfstedRating: () => cy.get('[data-testid="previous-ofsted-rating"]'),
            previousOfstedRatingHeader: () => table().find("th:contains('Previous Ofsted rating')"),
            currentOfstedRating: () => cy.get('[data-testid="current-ofsted-rating"]'),
            currentOfstedRatingHeader: () => table().find("th:contains('Current Ofsted rating')"),
        };
    }

    private createPupilNumbersPageElements() {
        const table = () => cy.get('[aria-describedby="academies-pupil-numbers-link"]');
        return {
            table,
            schoolName: () => table().find('[data-testid="school-name"]'),
            schoolNameHeader: () => table().find("th:contains('School name')"),
            phaseAndAge: () => table().find('[data-testid="phase-and-age-range"]'),
            phaseAndAgeHeader: () => table().find("th:contains('Phase and age range')"),
            pupilNumbers: () => table().find('[data-testid="pupil-numbers"]'),
            pupilNumbersHeader: () => table().find("th:contains('Pupil numbers')"),
            pupilCapacity: () => table().find('[data-testid="pupil-capacity"]'),
            pupilCapacityHeader: () => table().find("th:contains('Pupil capacity')"),
        };
    }

    private createFreeSchoolMealsElements() {
        const table = () => cy.get('[aria-describedby="free-school-meals-link"]');
        return { table };
    }

    public getAcademyCountFromSidebar(): Cypress.Chainable<number> {
        return this.elements.PageTabs.academyCountLabel()
            .invoke('text')
            .then(text => parseInt(text.match(/\d+/)[0]));
    }

    public getTableRowCountOnDetailsPage(): Cypress.Chainable<number> {
        return this.elements.DetailsPage.tableRows().its('length');
    }

    public checkDetailsHeadersPresent(): this {
        const { DetailsPage } = this.elements;
        DetailsPage.table().should('contain', 'School name')
            .and('contain', 'Local authority')
            .and('contain', 'Type')
            .and('contain', 'Rural or urban')
            .and('contain', 'Get information about schools');
        return this;
    }

    public checkOfstedHeadersPresent(): this {
        const { OfstedPage } = this.elements;
        OfstedPage.table().should('contain', 'School name')
            .and('contain', 'Date joined')
            .and('contain', 'Previous Ofsted rating')
            .and('contain', 'Current Ofsted rating');
        return this;
    }

    public checkPupilNumbersHeadersPresent(): this {
        const { PupilNumbersPage } = this.elements;
        PupilNumbersPage.table().should('contain', 'School name')
            .and('contain', 'Phase and age range')
            .and('contain', 'Pupil numbers')
            .and('contain', 'Pupil capacity')
            .and('contain', '% full');
        return this;
    }

    public checkFreeSchoolMealsHeadersPresent(): this {
        const { FreeSchoolMeals } = this.elements;
        FreeSchoolMeals.table().should('contain', 'School name')
            .and('contain', 'Pupils eligible for free school meals')
            .and('contain', 'Local authority average')
            .and('contain', 'National average');
        return this;
    }

    public checkSchoolTypesOnDetailsTable() {
        this.elements.DetailsPage.schoolType().each(element => {
            expect(element.text().trim()).to.be.oneOf(["Academy sponsor led", "Academy converter", "University technical college", "Free schools"]);
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
        TableUtility.checkStringSorting(DetailsPage.schoolName, DetailsPage.schoolNameHeader);
        TableUtility.checkStringSorting(DetailsPage.localAuthority, DetailsPage.localAuthorityHeader);
        TableUtility.checkStringSorting(DetailsPage.schoolType, DetailsPage.schoolTypeHeader);
        TableUtility.checkStringSorting(DetailsPage.ruralOrUrban, DetailsPage.ruralOrUrbanHeader);
    }

    public checkOfstedSorting() {
        const { OfstedPage } = this.elements;
        TableUtility.checkStringSorting(OfstedPage.schoolName, OfstedPage.schoolNameHeader);
        TableUtility.checkStringSorting(OfstedPage.dateJoined, OfstedPage.dateJoinedHeader);
        TableUtility.checkStringSorting(OfstedPage.previousOfstedRating, OfstedPage.previousOfstedRatingHeader);
        TableUtility.checkStringSorting(OfstedPage.currentOfstedRating, OfstedPage.currentOfstedRatingHeader);
    }

    public checkPupilNumbersSorting() {
        const { PupilNumbersPage } = this.elements;
        TableUtility.checkStringSorting(PupilNumbersPage.schoolName, PupilNumbersPage.schoolNameHeader);
        TableUtility.checkStringSorting(PupilNumbersPage.phaseAndAge, PupilNumbersPage.phaseAndAgeHeader);
        TableUtility.checkNumericSorting(PupilNumbersPage.pupilNumbers, PupilNumbersPage.pupilNumbersHeader);
        TableUtility.checkNumericSorting(PupilNumbersPage.pupilCapacity, PupilNumbersPage.pupilCapacityHeader);
    }
}

const academiesPage = new AcademiesPage();
export default academiesPage;
