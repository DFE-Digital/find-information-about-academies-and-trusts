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
            senTab: {
                senTabName: () => cy.get('[data-testid="overview-sen-subnav"]'),
                resourcedProvisionOnRollKey: () => cy.get('[data-testid="resourced-provision-on-roll-key"]'),
                resourcedProvisionCapacityKey: () => cy.get('[data-testid="resourced-provision-capacity-key"]'),
                senOnRollKey: () => cy.get('[data-testid="sen-on-roll-key"]'),
                senCapacityKey: () => cy.get('[data-testid="sen-capacity-key"]'),
                resourcedProvisionTypeKey: () => cy.get('[data-testid="resourced-provision-type-key"]'),
                senProvisionTypeKey: () => cy.get('[data-testid="sen-provision-type-key"]'),
                senProvisionType: () => cy.get('[data-testid="sen-provision-type"]'),
            },
        },
    };

    private readonly checkElementMatches = (element: JQuery<HTMLElement>, expected: RegExp) => {
        const text = element.text().trim();
        expect(text).to.match(expected);
    };

    public checkCorrectSENTypePresent(): this {
        // Valid SEN (Special Educational Needs) types:
        // - Not Applicable
        // - SpLD - Specific Learning Difficulty
        // - VI - Visual Impairment
        // - OTH - Other Difficulty/Disability
        // - HI - Hearing Impairment
        // - SLCN - Speech, language and Communication
        // - ASD - Autistic Spectrum Disorder
        // - SEMH - Social, Emotional and Mental Health
        // - MSI - Multi-Sensory Impairment
        // - PD - Physical Disability
        // - MLD - Moderate Learning Difficulty
        // - SLD - Severe Learning Difficulty
        // - PMLD - Profound and Multiple Learning Difficulty

        const validSenRegex = /^(Not Applicable|SpLD - Specific Learning Difficulty|VI|- Visual Impairment|OTH - Other Difficulty\/Disability|HI - Hearing Impairment|SLCN - Speech, language and Communication|ASD - Autistic Spectrum Disorder|SEMH - Social, Emotional and Mental Health|MSI - Multi-Sensory Impairment|PD - Physical Disability|MLD - Moderate Learning Difficulty|SLD - Severe Learning Difficulty|PMLD - Profound and Multiple Learning Difficulty)$/;

        // Find all SEN type elements on the page
        this.elements.overview.senTab.senProvisionType().each((element) => {
            // Get the raw text content
            const rawTextContent = element.text();

            // Process the text content:
            // 1. Split into separate lines
            const textLines = rawTextContent.split('\n');

            // 2. Clean up each line by removing extra spaces
            const cleanedLines = textLines.map(line => line.trim());

            // 3. Remove any empty lines and get SEN types
            const foundSenTypes = cleanedLines.filter(line => line.length > 0);

            // Check each SEN type against our valid list
            foundSenTypes.forEach(senType => {
                expect(senType).to.match(validSenRegex,
                    `Expected "${senType}" to be a valid SEN type`);
            });
        });

        return this;
    }

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

    // #region Details Tab

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

    // #endregion

    // #region SEN Tab

    public checkSENTabNameCorrect(): this {
        this.elements.overview.senTab.senTabName().should('be.visible').and('contain.text', 'SEN');
        return this;
    }

    public checkSENSubpageHeaderCorrect(): this {
        this.elements.subpageHeader().should('contain', 'SEN (special educational needs)');
        return this;
    }

    public checkSENDataItemsPresent(): this {
        this.elements.overview.senTab.resourcedProvisionOnRollKey().should('be.visible').and('contain.text', 'Resourced provision number on roll');
        this.elements.overview.senTab.resourcedProvisionCapacityKey().should('be.visible').and('contain.text', 'Resourced provision capacity');
        this.elements.overview.senTab.senOnRollKey().should('be.visible').and('contain.text', 'Special Educational Needs (SEN) unit number on roll');
        this.elements.overview.senTab.senCapacityKey().should('be.visible').and('contain.text', 'Special Educational Needs (SEN) unit number capacity');
        this.elements.overview.senTab.resourcedProvisionTypeKey().should('be.visible').and('contain.text', 'Type of resourced provision');
        this.elements.overview.senTab.senProvisionTypeKey().should('be.visible').and('contain.text', 'Type of SEN provision');
        return this;
    }



    // #endregion
}

const schoolsPage = new SchoolsPage();
export default schoolsPage;
