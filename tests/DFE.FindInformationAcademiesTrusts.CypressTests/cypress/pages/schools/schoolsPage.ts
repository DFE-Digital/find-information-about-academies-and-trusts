
class SchoolsPage {

    elements = {
        pageName: () => cy.get('[data-testid="page-name"]'),
        subpageHeader: () => cy.get('[data-testid="subpage-header"]'),
        schoolType: () => cy.get('[data-testid="school-type"]'),
        trustLink: () => cy.get('[data-testid="header-trust-link"]'),
        nav: {
            overviewNav: () => cy.get('[data-testid="overview-nav"]'),
        },
        overview: {
            detailsTabHeader: () => cy.get('[data-testid="overview-details-subnav"]'),
            detailsTab: {
                addressHeader: () => cy.get('[data-testid="details-address-header"]'),
                dateJoinedTrustHeader: () => cy.get('[data-testid="details-date-joined-trust-header"]'),
                academyTrustHeader: () => cy.get('[data-testid="details-trust-header"]'),
                regionAndTerritoryHeader: () => cy.get('[data-testid="details-region-and-territory-header"]'),
                localAuthorityHeader: () => cy.get('[data-testid="details-local-authority-header"]'),
                phaseAndAgeRangeHeader: () => cy.get('[data-testid="details-phase-and-age-range-header"]'),
                hasNurseryClassesHeader: () => cy.get('[data-testid="details-has-nursery-classes-header"]'),
                informationForOtherServicesHeader: () => cy.get('[data-testid="details-information-from-other-services-header"]'),
                giasLink: () => cy.get('[data-testid="details-gias-link"]'),
                financialBenchmarkingLink: () => cy.get('[data-testid="details-financial-benchmarking-link"]'),
                findSchoolPerformanceDataLink: () => cy.get('[data-testid="details-find-school-performance-link"]'),
            },
        },
    };

    private readonly checkElementMatches = (element: JQuery<HTMLElement>, expected: RegExp) => {
        const text = element.text().trim();
        expect(text).to.match(expected);
    };

    public checkValueIsValidSchoolType = (element: JQuery<HTMLElement>) =>
        this.checkElementMatches(element, /^(Community school|Academy converter)$/);

    public checkCorrectSchoolTypePresent(): this {
        this.elements.schoolType().each(this.checkValueIsValidSchoolType);
        return this;
    }

    public checkOverviewPageNamePresent(): this {
        this.elements.pageName().should('contain', 'Overview');
        return this;
    }

    public checkAcademyLinkPresentAndCorrect(trustAcademyName: string): this {
        this.elements.trustLink().should('be.visible');
        this.elements.trustLink().should('contain.text', trustAcademyName);
        return this;
    }

    public checkAcademyLinkNotPresentForSchool(): this {
        this.elements.trustLink().should('not.exist');
        return this;
    }

    public clickAcademyTrustLink(): this {
        this.elements.trustLink().click();
        return this;
    }

    public checkSchoolDetailsHeaderPresent(): this {
        this.elements.subpageHeader().should('contain', 'School details');
        return this;
    }

    public checkAcademyDetailsHeaderPresent(): this {
        this.elements.subpageHeader().should('contain', 'Academy details');
        return this;
    }

    public checkSchoolDetailsTabCorrect(): this {
        this.elements.overview.detailsTabHeader().should('contain', 'School details');
        return this;
    }

    public checkAcademyDetailsTabCorrect(): this {
        this.elements.overview.detailsTabHeader().should('contain', 'Academy details');
        return this;
    }

    public checkDetailsSchoolDataItemsPresent(): this {
        this.elements.overview.detailsTab.addressHeader().should('be.visible').and('contain.text', 'Address');
        this.elements.overview.detailsTab.regionAndTerritoryHeader().should('be.visible').and('contain.text', 'Region and territory');
        this.elements.overview.detailsTab.localAuthorityHeader().should('be.visible').and('contain.text', 'Local authority');
        this.elements.overview.detailsTab.phaseAndAgeRangeHeader().should('be.visible').and('contain.text', 'Phase and age range');
        this.elements.overview.detailsTab.hasNurseryClassesHeader().should('be.visible').and('contain.text', 'Has nursery classes');
        return this;
    }

    public checkDetailsAcademyDataItemsPresent(): this {
        this.elements.overview.detailsTab.addressHeader().should('be.visible').and('contain.text', 'Address');
        this.elements.overview.detailsTab.dateJoinedTrustHeader().should('be.visible').and('contain.text', 'Date joined the trust');
        this.elements.overview.detailsTab.academyTrustHeader().should('be.visible').and('contain.text', 'Trust');
        this.elements.overview.detailsTab.regionAndTerritoryHeader().should('be.visible').and('contain.text', 'Region and territory');
        this.elements.overview.detailsTab.localAuthorityHeader().should('be.visible').and('contain.text', 'Local authority');
        this.elements.overview.detailsTab.phaseAndAgeRangeHeader().should('be.visible').and('contain.text', 'Phase and age range');
        this.elements.overview.detailsTab.hasNurseryClassesHeader().should('be.visible').and('contain.text', 'Has nursery classes');
        return this;
    }

    public checkDetailsAcademyDataItemsNotPresent(): this {
        this.elements.overview.detailsTab.dateJoinedTrustHeader().should('not.exist');
        this.elements.overview.detailsTab.academyTrustHeader().should('not.exist');
        return this;
    }

    public checkDetailsOtherServicesItemsPresent(): this {
        this.elements.overview.detailsTab.giasLink().should('be.visible').and('contain.text', 'Get information about schools');
        this.elements.overview.detailsTab.financialBenchmarkingLink().should('be.visible').and('contain.text', 'Financial benchmarking');
        this.elements.overview.detailsTab.findSchoolPerformanceDataLink().should('be.visible').and('contain.text', 'Find school college and performance data');
        return this;
    }

}

const schoolsPage = new SchoolsPage();
export default schoolsPage;
