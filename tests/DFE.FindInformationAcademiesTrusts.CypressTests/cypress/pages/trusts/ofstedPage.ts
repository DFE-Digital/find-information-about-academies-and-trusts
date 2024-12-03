import { TableUtility } from "../tableUtility";

class OfstedPage {

    elements = {
        subpageHeader: () => cy.get('[data-testid="subpage-header"]'),
        downloadButton: () => cy.get('[data-testid="download-all-ofsted-data-button"]'),
        currentRatings: {
            section: () => cy.get('[data-testid="ofsted-current-ratings-table"]'),
            schoolName: () => this.elements.currentRatings.section().find('[data-testid="ofsted-current-ratings-school-name"]'),
            schoolNameHeader: () => this.elements.currentRatings.section().find('[data-testid="ofsted-current-ratings-school-name-header"]'),
            noDataMessage: () => this.elements.currentRatings.section().contains('No data available'),
            qualityOfEducationHeader: () => this.elements.currentRatings.section().find('[data-testid="ofsted-current-ratings-quality-of-education-header"]'),
            qualityOfEducation: () => this.elements.currentRatings.section().find('[data-testid="ofsted-current-ratings-quality-of-education"]'),
            behaviourAndAttitudesHeader: () => this.elements.currentRatings.section().find('[data-testid="ofsted-current-ratings-behaviour-and-attitudes-header"]'),
            behaviourAndAttitudes: () => this.elements.currentRatings.section().find('[data-testid="ofsted-current-ratings-behaviour-and-attitudes"]'),
            personalDevelopmentHeader: () => this.elements.currentRatings.section().find('[data-testid="ofsted-current-ratings-personal-development-header"]'),
            personalDevelopment: () => this.elements.currentRatings.section().find('[data-testid="ofsted-current-ratings-personal-development"]'),
            leadershipAndManagementHeader: () => this.elements.currentRatings.section().find('[data-testid="ofsted-current-ratings-leadership-and-management-header"]'),
            leadershipAndManagement: () => this.elements.currentRatings.section().find('[data-testid="ofsted-current-ratings-leadership-and-management"]'),
            earlyYearsProvisionHeader: () => this.elements.currentRatings.section().find('[data-testid="ofsted-current-ratings-early-years-provision-header"]'),
            earlyYearsProvision: () => this.elements.currentRatings.section().find('[data-testid="ofsted-current-ratings-early-years-provision"]'),
            sixthFormProvisionHeader: () => this.elements.currentRatings.section().find('[data-testid="ofsted-current-ratings-sixth-form-provision-header"]'),
            sixthFormProvision: () => this.elements.currentRatings.section().find('[data-testid="ofsted-current-ratings-sixth-form-provision"]'),
            beforeOrAfterJoiningHeader: () => this.elements.currentRatings.section().find('[data-testid="ofsted-current-ratings-before-or-after-joining-header"]'),
            beforeOrAfterJoining: () => this.elements.currentRatings.section().find('[data-testid="ofsted-current-ratings-before-or-after-joining"]'),
        },
        previousRatings: {
            section: () => cy.get('[data-testid="ofsted-previous-ratings-table"]'),
            schoolName: () => this.elements.previousRatings.section().find('[data-testid="ofsted-previous-ratings-school-name"]'),
            schoolNameHeader: () => this.elements.previousRatings.section().find('[data-testid="ofsted-previous-ratings-school-name-header"]'),
            qualityOfEducationHeader: () => this.elements.previousRatings.section().find('[data-testid="ofsted-previous-ratings-quality-of-education-header"]'),
            qualityOfEducation: () => this.elements.previousRatings.section().find('[data-testid="ofsted-previous-ratings-quality-of-education"]'),
            behaviourAndAttitudesHeader: () => this.elements.previousRatings.section().find('[data-testid="ofsted-previous-ratings-behaviour-and-attitudes-header"]'),
            behaviourAndAttitudes: () => this.elements.previousRatings.section().find('[data-testid="ofsted-previous-ratings-behaviour-and-attitudes"]'),
            personalDevelopmentHeader: () => this.elements.previousRatings.section().find('[data-testid="ofsted-previous-ratings-personal-development-header"]'),
            personalDevelopment: () => this.elements.previousRatings.section().find('[data-testid="ofsted-previous-ratings-personal-development"]'),
            leadershipAndManagementHeader: () => this.elements.previousRatings.section().find('[data-testid="ofsted-previous-ratings-leadership-and-management-header"]'),
            leadershipAndManagement: () => this.elements.previousRatings.section().find('[data-testid="ofsted-previous-ratings-leadership-and-management"]'),
            earlyYearsProvisionHeader: () => this.elements.previousRatings.section().find('[data-testid="ofsted-previous-ratings-early-years-provision-header"]'),
            earlyYearsProvision: () => this.elements.previousRatings.section().find('[data-testid="ofsted-previous-ratings-early-years-provision"]'),
            sixthFormProvisionHeader: () => this.elements.previousRatings.section().find('[data-testid="ofsted-previous-ratings-sixth-form-provision-header"]'),
            sixthFormProvision: () => this.elements.previousRatings.section().find('[data-testid="ofsted-previous-ratings-sixth-form-provision"]'),
            beforeOrAfterJoiningHeader: () => this.elements.previousRatings.section().find('[data-testid="ofsted-previous-ratings-before-or-after-joining-header"]'),
            beforeOrAfterJoining: () => this.elements.previousRatings.section().find('[data-testid="ofsted-previous-ratings-before-or-after-joining"]'),
        },
        safeguardingAndConcerns: {
            section: () => cy.get('[data-testid="ofsted-safeguarding-and-concerns-name-table"]'),
            schoolNameHeader: () => this.elements.safeguardingAndConcerns.section().find('[data-testid="ofsted-safeguarding-and-concerns-name-header"]'),
            schoolName: () => this.elements.safeguardingAndConcerns.section().find('[data-testid="ofsted-safeguarding-and-concerns-school-name"]'),
            effectiveSafeguardingHeader: () => this.elements.safeguardingAndConcerns.section().find('[data-testid="ofsted-safeguarding-and-concerns-effective-safeguarding-header"]'),
            effectiveSafeguarding: () => this.elements.safeguardingAndConcerns.section().find('[data-testid="ofsted-safeguarding-and-concerns-effective-safeguarding"]'),
            categoryOfConcernHeader: () => this.elements.safeguardingAndConcerns.section().find('[data-testid="ofsted-safeguarding-and-concerns-category-of-concern-header"]'),
            categoryOfConcern: () => this.elements.safeguardingAndConcerns.section().find('[data-testid="ofsted-safeguarding-and-concerns-category-of-concern"]'),
            beforeOrAfterJoiningHeader: () => this.elements.safeguardingAndConcerns.section().find('[data-testid="ofsted-safeguarding-and-concerns-before-or-after-joining-header"]'),
            beforeOrAfterJoining: () => this.elements.safeguardingAndConcerns.section().find('[data-testid="ofsted-safeguarding-before-or-after-joining"]'),
        },
        importantDates: {
            section: () => cy.get('[data-testid="ofsted-important-dates-school-name-table"]'),
            schoolName: () => this.elements.importantDates.section().find('[data-testid="ofsted-important-dates-school-name"]'),
            schoolNameHeader: () => this.elements.importantDates.section().find('[data-testid="ofsted-important-dates-school-name-header"]'),
            dateJoined: () => this.elements.importantDates.section().find('[data-testid="ofsted-important-dates-date-joined"]'),
            dateJoinedHeader: () => this.elements.importantDates.section().find('[data-testid="ofsted-important-dates-date-joined-header"]'),
            dateOfCurrentInspection: () => this.elements.importantDates.section().find('[data-testid="ofsted-important-dates-date-of-current-inspection"]'),
            dateOfCurrentInspectionHeader: () => this.elements.importantDates.section().find('[data-testid="ofsted-important-dates-date-of-current-inspection-header"]'),
            dateOfPreviousInspection: () => this.elements.importantDates.section().find('[data-testid="ofsted-important-dates-date-of-previous-inspection"]'),
            dateOfPreviousInspectionHeader: () => this.elements.importantDates.section().find('[data-testid="ofsted-important-dates-date-of-previous-inspection-header"]'),
        }
    };

    private readonly checkValueIsValidOfstedRating = (element: JQuery<HTMLElement>) => {
        const text = element.text().trim();
        expect(text).to.match(/^(Good|No judgement|Outstanding|Requires improvement|Inadequate|Not yet inspected|Insufficient evidence|Does not apply)$/);
    };

    private readonly checkValueIsValidDate = (element: JQuery<HTMLElement>) => {
        const text = element.text().trim();

        // Resolves to a date ({2 digits} {month} {4 digits}) or "No data" string
        // Tech debt - We are allowing Sep and Sept due to different cultures set on remote vs local builds
        expect(text).to.match(/^\d{1,2} (Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Sept|Oct|Nov|Dec) \d{4}$|^No data$/);
    };

    private readonly checkValueIsValidBeforeOrAfterJoiningTag = (element: JQuery<HTMLElement>) => {
        const text = element.text().trim();
        expect(text).to.match(/^(Before|After|Not yet inspected)$/);
    };

    ///Current ratings///

    public checkOfstedCurrentRatingsPageHeaderPresent(): this {
        this.elements.subpageHeader().should('contain', 'Current ratings');
        return this;
    }

    public checkOfstedCurrentRatingsTableHeadersPresent(): this {
        this.elements.currentRatings.schoolNameHeader().should('be.visible');
        this.elements.currentRatings.qualityOfEducationHeader().should('be.visible');
        this.elements.currentRatings.behaviourAndAttitudesHeader().should('be.visible');
        this.elements.currentRatings.personalDevelopmentHeader().should('be.visible');
        this.elements.currentRatings.leadershipAndManagementHeader().should('be.visible');
        this.elements.currentRatings.earlyYearsProvisionHeader().should('be.visible');
        this.elements.currentRatings.sixthFormProvisionHeader().should('be.visible');
        this.elements.currentRatings.beforeOrAfterJoiningHeader().scrollIntoView();
        this.elements.currentRatings.beforeOrAfterJoiningHeader().should('be.visible');
        return this;
    }

    public checkOfstedCurrentRatingsSorting(): this {
        TableUtility.checkStringSorting(
            this.elements.currentRatings.schoolName,
            this.elements.currentRatings.schoolNameHeader
        );
        TableUtility.checkStringSorting(
            this.elements.currentRatings.qualityOfEducation,
            this.elements.currentRatings.qualityOfEducationHeader
        );
        TableUtility.checkStringSorting(
            this.elements.currentRatings.behaviourAndAttitudes,
            this.elements.currentRatings.behaviourAndAttitudesHeader
        );
        TableUtility.checkStringSorting(
            this.elements.currentRatings.personalDevelopment,
            this.elements.currentRatings.personalDevelopmentHeader
        );
        TableUtility.checkStringSorting(
            this.elements.currentRatings.leadershipAndManagement,
            this.elements.currentRatings.leadershipAndManagementHeader
        );
        TableUtility.checkStringSorting(
            this.elements.currentRatings.earlyYearsProvision,
            this.elements.currentRatings.earlyYearsProvisionHeader
        );
        TableUtility.checkStringSorting(
            this.elements.currentRatings.sixthFormProvision,
            this.elements.currentRatings.sixthFormProvisionHeader
        );
        TableUtility.checkStringSorting(
            this.elements.currentRatings.beforeOrAfterJoining,
            this.elements.currentRatings.beforeOrAfterJoiningHeader
        );
        return this;
    }

    public checkCurrentRatingsQualityOfEducationJudgementsPresent(): this {
        this.elements.currentRatings.qualityOfEducation().each(this.checkValueIsValidOfstedRating);
        return this;
    }

    public checkCurrentRatingsBehaviourAndAttitudesJudgementsPresent(): this {
        this.elements.currentRatings.behaviourAndAttitudes().each(this.checkValueIsValidOfstedRating);
        return this;
    }

    public checkCurrentRatingsPersonalDevelopmentJudgementsPresent(): this {
        this.elements.currentRatings.personalDevelopment().each(this.checkValueIsValidOfstedRating);
        return this;
    }

    public checkCurrentRatingsLeadershipAndManagementJudgementsPresent(): this {
        this.elements.currentRatings.leadershipAndManagement().each(this.checkValueIsValidOfstedRating);
        return this;
    }

    public checkCurrentRatingsEarlyYearsProvisionJudgementsPresent(): this {
        this.elements.currentRatings.earlyYearsProvision().each(this.checkValueIsValidOfstedRating);
        return this;
    }

    public checkCurrentRatingsSixthFormProvisionJudgementsPresent(): this {
        this.elements.currentRatings.sixthFormProvision().each(this.checkValueIsValidOfstedRating);
        return this;
    }

    public checkCurrentRatingsBeforeOrAfterJoiningJudgementsPresent(): this {
        this.elements.currentRatings.beforeOrAfterJoining().each(this.checkValueIsValidBeforeOrAfterJoiningTag);
        return this;
    }

    public checkNoDataMessageIsVisible(): this {
        this.elements.currentRatings.noDataMessage().should('be.visible');
        return this;
    }

    ///previous ratings///

    public checkOfstedPreviousRatingsPageHeaderPresent(): this {
        this.elements.subpageHeader().should('contain', 'Previous ratings');
        return this;
    }

    public checkOfstedPreviousRatingsTableHeadersPresent(): this {
        this.elements.previousRatings.schoolNameHeader().should('be.visible');
        this.elements.previousRatings.qualityOfEducationHeader().should('be.visible');
        this.elements.previousRatings.behaviourAndAttitudesHeader().should('be.visible');
        this.elements.previousRatings.personalDevelopmentHeader().should('be.visible');
        this.elements.previousRatings.leadershipAndManagementHeader().should('be.visible');
        this.elements.previousRatings.earlyYearsProvisionHeader().should('be.visible');
        this.elements.previousRatings.sixthFormProvisionHeader().should('be.visible');
        this.elements.previousRatings.beforeOrAfterJoiningHeader().scrollIntoView();
        this.elements.previousRatings.beforeOrAfterJoiningHeader().should('be.visible');
        return this;
    }

    public checkOfstedPreviousRatingsSorting(): this {
        TableUtility.checkStringSorting(
            this.elements.previousRatings.schoolName,
            this.elements.previousRatings.schoolNameHeader
        );
        TableUtility.checkStringSorting(
            this.elements.previousRatings.qualityOfEducation,
            this.elements.previousRatings.qualityOfEducationHeader
        );
        TableUtility.checkStringSorting(
            this.elements.previousRatings.behaviourAndAttitudes,
            this.elements.previousRatings.behaviourAndAttitudesHeader
        );
        TableUtility.checkStringSorting(
            this.elements.previousRatings.personalDevelopment,
            this.elements.previousRatings.personalDevelopmentHeader
        );
        TableUtility.checkStringSorting(
            this.elements.previousRatings.leadershipAndManagement,
            this.elements.previousRatings.leadershipAndManagementHeader
        );
        TableUtility.checkStringSorting(
            this.elements.previousRatings.earlyYearsProvision,
            this.elements.previousRatings.earlyYearsProvisionHeader
        );
        TableUtility.checkStringSorting(
            this.elements.previousRatings.sixthFormProvision,
            this.elements.previousRatings.sixthFormProvisionHeader
        );
        TableUtility.checkStringSorting(
            this.elements.previousRatings.beforeOrAfterJoining,
            this.elements.previousRatings.beforeOrAfterJoiningHeader
        );
        return this;
    }

    public checkPreviousRatingsQualityOfEducationJudgementsPresent(): this {
        this.elements.previousRatings.qualityOfEducation().each(this.checkValueIsValidOfstedRating);
        return this;
    }

    public checkPreviousRatingsBehaviourAndAttitudesJudgementsPresent(): this {
        this.elements.previousRatings.behaviourAndAttitudes().each(this.checkValueIsValidOfstedRating);
        return this;
    }

    public checkPreviousRatingsPersonalDevelopmentJudgementsPresent(): this {
        this.elements.previousRatings.personalDevelopment().each(this.checkValueIsValidOfstedRating);
        return this;
    }

    public checkPreviousRatingsLeadershipAndManagementJudgementsPresent(): this {
        this.elements.previousRatings.leadershipAndManagement().each(this.checkValueIsValidOfstedRating);
        return this;
    }

    public checkPreviousRatingsEarlyYearsProvisionJudgementsPresent(): this {
        this.elements.previousRatings.earlyYearsProvision().each(this.checkValueIsValidOfstedRating);
        return this;
    }

    public checkPreviousRatingsSixthFormProvisionJudgementsPresent(): this {
        this.elements.previousRatings.sixthFormProvision().each(this.checkValueIsValidOfstedRating);
        return this;
    }

    public checkPreviousRatingsBeforeOrAfterJoiningJudgementsPresent(): this {
        this.elements.previousRatings.beforeOrAfterJoining().each(this.checkValueIsValidBeforeOrAfterJoiningTag);
        return this;
    }

    //Safeguarding and Concerns///

    public checkOfstedSafeguardingConcernsPageHeaderPresent(): this {
        this.elements.subpageHeader().should('contain', 'Safeguarding and concerns');
        return this;
    }

    public checkOfstedSafeguardingConcernsTableHeadersPresent(): this {
        this.elements.safeguardingAndConcerns.schoolNameHeader().should('be.visible');
        this.elements.safeguardingAndConcerns.effectiveSafeguardingHeader().should('be.visible');
        this.elements.safeguardingAndConcerns.categoryOfConcern().should('be.visible');
        this.elements.safeguardingAndConcerns.beforeOrAfterJoining().should('be.visible');
        return this;
    }

    public checkOfstedSafeguardingConcernsSorting(): this {
        TableUtility.checkStringSorting(
            this.elements.safeguardingAndConcerns.schoolName,
            this.elements.safeguardingAndConcerns.schoolNameHeader
        );
        TableUtility.checkStringSorting(
            this.elements.safeguardingAndConcerns.effectiveSafeguarding,
            this.elements.safeguardingAndConcerns.effectiveSafeguardingHeader
        );
        TableUtility.checkStringSorting(
            this.elements.safeguardingAndConcerns.categoryOfConcern,
            this.elements.safeguardingAndConcerns.categoryOfConcernHeader
        );
        TableUtility.checkStringSorting(
            this.elements.safeguardingAndConcerns.beforeOrAfterJoining,
            this.elements.safeguardingAndConcerns.beforeOrAfterJoiningHeader
        );
        return this;
    }

    public checkSafeguardingConcernsEffectiveSafeguardingJudgementsPresent(): this {
        this.elements.safeguardingAndConcerns.effectiveSafeguarding().each((element) => {
            const text = element.text().trim();
            expect(text).to.match(/^(Yes|No|Not recorded|Not yet inspected)$/);
        });
        return this;
    }

    public checkSafeguardingConcernsCategoryOfConcernJudgementsPresent(): this {
        this.elements.safeguardingAndConcerns.categoryOfConcern().each((element) => {
            const text = element.text().trim();
            expect(text).to.match(/^(None|Special measures|Serious weakness|Notice to improve|Not yet inspected|Does not apply)$/);
        });
        return this;
    }

    public checkSafeguardingConcernsBeforeOrAfterJoiningJudgementsPresent(): this {
        this.elements.safeguardingAndConcerns.beforeOrAfterJoining().each(this.checkValueIsValidBeforeOrAfterJoiningTag);
        return this;
    }

    // Important Dates

    public checkOfstedImportantDatesPageHeaderPresent(): this {
        this.elements.subpageHeader().should('contain', 'Important dates');
        return this;
    }

    public checkOfstedImportantDatesTableHeadersPresent(): this {
        this.elements.importantDates.schoolNameHeader().should('be.visible');
        this.elements.importantDates.dateJoinedHeader().should('be.visible');
        this.elements.importantDates.dateOfCurrentInspectionHeader().should('be.visible');
        this.elements.importantDates.dateOfPreviousInspectionHeader().should('be.visible');
        return this;
    }

    public checkOfstedImportantDatesSorting(): this {
        TableUtility.checkStringSorting(
            this.elements.importantDates.schoolName,
            this.elements.importantDates.schoolNameHeader
        );
        TableUtility.checkStringSorting(
            this.elements.importantDates.dateJoined,
            this.elements.importantDates.dateJoinedHeader
        );
        TableUtility.checkStringSorting(
            this.elements.importantDates.dateOfCurrentInspection,
            this.elements.importantDates.dateOfCurrentInspectionHeader
        );
        TableUtility.checkStringSorting(
            this.elements.importantDates.dateOfPreviousInspection,
            this.elements.importantDates.dateOfPreviousInspectionHeader
        );
        return this;
    }


    public checkDateJoinedPresent(): this {
        this.elements.importantDates.dateJoined().each(this.checkValueIsValidDate);
        return this;
    }

    public checkDateOfCurrentInspectionPresent(): this {
        this.elements.importantDates.dateOfCurrentInspection().each(this.checkValueIsValidDate);
        return this;
    }


    public checkDateOfPreviousInspectionPresent(): this {
        this.elements.importantDates.dateOfPreviousInspection().each(this.checkValueIsValidDate);
        return this;
    }

    public clickDownloadButton(): this {
        this.elements.downloadButton().click();
        return this;
    }
}

const ofstedPage = new OfstedPage();
export default ofstedPage;
