import { TableUtility } from "../tableUtility";

class AcademiesInTrustPage {

    elements = {
        pageTabs: {
            academyCountLabel: () => cy.get('[data-testid="academies-nav"]'),
        },
        detailsPage: this.createDetailsPageElements(),
        pupilNumbersPage: this.createPupilNumbersPageElements(),
        freeSchoolMeals: this.createFreeSchoolMealsElements(),
    };

    private createDetailsPageElements() {
        const table = () => cy.get('[aria-describedby="details-caption"]');
        return {
            table,
            tableRows: () => table().find('tbody tr'),
            schoolName: () => table().find('[data-testid="school-name"]'),
            schoolNameHeader: () => table().find("th:contains('School name')"),
            urn: () => table().find('[data-testid="urn"]'),
            urnHeader: () => table().find("th:contains('URN')"),
            localAuthority: () => table().find('[data-testid="local-authority"]'),
            localAuthorityHeader: () => table().find("th:contains('Local authority')"),
            schoolType: () => table().find('[data-testid="type-of-establishment"]'),
            schoolTypeHeader: () => table().find("th:contains('Type')"),
            ruralOrUrban: () => table().find('[data-testid="urban-or-rural"]'),
            ruralOrUrbanHeader: () => table().find("th:contains('Rural or urban')"),
        };
    }

    private createPupilNumbersPageElements() {
        const table = () => cy.get('[aria-describedby="pupil-numbers-caption"]');
        return {
            table,
            tableRows: () => table().find('tbody tr'),
            schoolName: () => table().find('[data-testid="school-name"]'),
            schoolNameHeader: () => table().find("th:contains('School name')"),
            urn: () => table().find('[data-testid="urn"]'),
            urnHeader: () => table().find("th:contains('URN')"),
            phaseAndAge: () => table().find('[data-testid="phase-and-age-range"]'),
            phaseAndAgeHeader: () => table().find("th:contains('Phase and age range')"),
            pupilNumbers: () => table().find('[data-testid="pupil-numbers"]'),
            pupilNumbersHeader: () => table().find("th:contains('Pupil numbers')"),
            pupilCapacity: () => table().find('[data-testid="pupil-capacity"]'),
            pupilCapacityHeader: () => table().find("th:contains('Pupil capacity')"),
        };
    }

    private createFreeSchoolMealsElements() {
        const table = () => cy.get('[aria-describedby="free-school-meals-caption"]');
        return {
            table,
            tableRows: () => table().find('tbody tr'),
            schoolName: () => table().find('[data-testid="school-name"]'),
            schoolNameHeader: () => table().find("th:contains('School name')"),
            urn: () => table().find('[data-testid="urn"]'),
            urnHeader: () => table().find("th:contains('URN')"),
            pupilsEligible: () => table().find('[data-testid="pupils-eligible"]'),
            pupilsEligibleHeader: () => table().find("th:contains('Pupils eligible for free school meals')"),
            localAuthorityAverage: () => table().find('[data-testid="local-authority-average"]'),
            localAuthorityAverageHeader: () => table().find("th:contains('Local authority average')"),
            nationalAverage: () => table().find('[data-testid="national-average"]'),
            nationalAverageHeader: () => table().find("th:contains('National average')"),
        };
    }

    public getAcademyCountFromSidebar(): Cypress.Chainable<number> {
        return this.elements.pageTabs.academyCountLabel()
            .invoke('text')
            .then(text => parseInt(text.match(/\d+/)[0]));
    }

    public getTableRowCountOnDetailsPage(): Cypress.Chainable<number> {
        return this.elements.detailsPage.tableRows().its('length');
    }

    public getTableRowCountOnPupilNumbersPage(): Cypress.Chainable<number> {
        return this.elements.pupilNumbersPage.tableRows().its('length');
    }

    public getTableRowCountOnFreeSchoolMealsPage(): Cypress.Chainable<number> {
        return this.elements.freeSchoolMeals.tableRows().its('length');
    }

    public checkDetailsHeadersPresent(): this {
        const { detailsPage } = this.elements;
        detailsPage.table().should('contain', 'School name')
            .and('contain', 'URN')
            .and('contain', 'Local authority')
            .and('contain', 'Type')
            .and('contain', 'Rural or urban')
            .and('contain', 'Get information about schools');
        return this;
    }

    public checkPupilNumbersHeadersPresent(): this {
        const { pupilNumbersPage } = this.elements;
        pupilNumbersPage.table().should('contain', 'School name')
            .and('contain', 'URN')
            .and('contain', 'Phase and age range')
            .and('contain', 'Pupil numbers')
            .and('contain', 'Pupil capacity')
            .and('contain', '% full');
        return this;
    }

    public checkFreeSchoolMealsHeadersPresent(): this {
        const { freeSchoolMeals } = this.elements;
        freeSchoolMeals.table().should('contain', 'School name')
            .and('contain', 'URN')
            .and('contain', 'Pupils eligible for free school meals')
            .and('contain', 'Local authority average')
            .and('contain', 'National average');
        return this;
    }

    public checkSchoolTypesOnDetailsTable() {
        this.elements.detailsPage.schoolType().each(element => {
            expect(element.text().trim()).to.be.oneOf(["Academy sponsor led", "Academy converter", "University technical college", "Free schools"]);
        });
    }

    public checkTrustDetailsSorting() {
        const { detailsPage } = this.elements;
        TableUtility.checkStringSorting(detailsPage.schoolName, detailsPage.schoolNameHeader);
        TableUtility.checkStringSorting(detailsPage.urn, detailsPage.urnHeader);
        TableUtility.checkStringSorting(detailsPage.localAuthority, detailsPage.localAuthorityHeader);
        TableUtility.checkStringSorting(detailsPage.schoolType, detailsPage.schoolTypeHeader);
        TableUtility.checkStringSorting(detailsPage.ruralOrUrban, detailsPage.ruralOrUrbanHeader);
    }

    public checkPupilNumbersSorting() {
        const { pupilNumbersPage } = this.elements;
        TableUtility.checkStringSorting(pupilNumbersPage.schoolName, pupilNumbersPage.schoolNameHeader);
        TableUtility.checkStringSorting(pupilNumbersPage.urn, pupilNumbersPage.urnHeader);
        TableUtility.checkStringSorting(pupilNumbersPage.phaseAndAge, pupilNumbersPage.phaseAndAgeHeader);
        TableUtility.checkNumericSorting(pupilNumbersPage.pupilNumbers, pupilNumbersPage.pupilNumbersHeader);
        TableUtility.checkNumericSorting(pupilNumbersPage.pupilCapacity, pupilNumbersPage.pupilCapacityHeader);
    }

    public checkFreeSchoolMealsSorting() {
        const { freeSchoolMeals } = this.elements;
        TableUtility.checkStringSorting(freeSchoolMeals.schoolName, freeSchoolMeals.schoolNameHeader);
        TableUtility.checkStringSorting(freeSchoolMeals.urn, freeSchoolMeals.urnHeader);
        TableUtility.checkNumericSorting(freeSchoolMeals.pupilsEligible, freeSchoolMeals.pupilsEligibleHeader);
        TableUtility.checkNumericSorting(freeSchoolMeals.localAuthorityAverage, freeSchoolMeals.localAuthorityAverageHeader);
        TableUtility.checkNumericSorting(freeSchoolMeals.nationalAverage, freeSchoolMeals.nationalAverageHeader);
    }
}

const academiesInTrustPage = new AcademiesInTrustPage();
export default academiesInTrustPage;
