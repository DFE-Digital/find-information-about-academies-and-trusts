import { TableUtility } from "../tableUtility";

class OfstedPage {
    elements = {
        currentRatings: {
            Section: () => cy.get('[data-testid="ofsted-current-ratings-table"]'),
            SchoolName: () => this.elements.currentRatings.Section().find('[data-testid="ofsted-current-ratings-school-name"]'),
            SchoolNameHeader: () => this.elements.currentRatings.Section().find('[data-testid="ofsted-current-ratings-school-name-header"]'),
            NoDataMessage: () => this.elements.currentRatings.Section().contains('No data available'),
            qualityOfEducationHeader: () => this.elements.currentRatings.Section().find('[data-testid="ofsted-current-ratings-quality-of-education-header"]'),
            qualityOfEducation: () => this.elements.currentRatings.Section().find('[data-testid="ofsted-current-ratings-quality-of-education"]'),
            behaviourAndAttitudesHeader: () => this.elements.currentRatings.Section().find('[data-testid="ofsted-current-ratings-behaviour-and-attitudes-header"]'),
            behaviourAndAttitudes: () => this.elements.currentRatings.Section().find('[data-testid="ofsted-current-ratings-behaviour-and-attitudes"]'),
            personalDevelopmentHeader: () => this.elements.currentRatings.Section().find('[data-testid="ofsted-current-ratings-personal-development-header"]'),
            personalDevelopment: () => this.elements.currentRatings.Section().find('[data-testid="ofsted-current-ratings-personal-development"]'),
            leadershipAndManagementHeader: () => this.elements.currentRatings.Section().find('[data-testid="ofsted-current-ratings-leadership-and-management-header"]'),
            leadershipAndManagement: () => this.elements.currentRatings.Section().find('[data-testid="ofsted-current-ratings-leadership-and-management"]'),
            earlyYearsProvisionHeader: () => this.elements.currentRatings.Section().find('[data-testid="ofsted-current-ratings-early-years-provision-header"]'),
            earlyYearsProvision: () => this.elements.currentRatings.Section().find('[data-testid="ofsted-current-ratings-early-years-provision"]'),
            sixthFormProvisionHeader: () => this.elements.currentRatings.Section().find('[data-testid="ofsted-current-ratings-sixth-form-provision-header"]'),
            sixthFormProvision: () => this.elements.currentRatings.Section().find('[data-testid="ofsted-current-ratings-sixth-form-provision"]'),
            beforeOrAfterJoiningHeader: () => this.elements.currentRatings.Section().find('[data-testid="ofsted-current-ratings-before-or-after-joining-header"]'),
            beforeOrAfterJoining: () => this.elements.currentRatings.Section().find('[data-testid="ofsted-current-ratings-before-or-after-joining "]'),
        },
        previousRatings: {
            PreviousOfstedRating: () => this.elements.currentRatings.Section().find('[data-testid="previous-ofsted-rating"]'),
            PreviousOfstedRatingHeader: () => this.elements.currentRatings.Section().find("th:contains('Previous Ofsted rating')"),
        },
        importantDates: {
            DateJoined: () => this.elements.currentRatings.Section().find('[data-testid="date-joined"]'),
            DateJoinedHeader: () => this.elements.currentRatings.Section().find("th:contains('Date joined')"),

        }
    };

    public checkOfstedCurrentRatingsTableHeadersPresent(): this {
        this.elements.currentRatings.SchoolNameHeader().should('be.visible');
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
            this.elements.currentRatings.SchoolName,
            this.elements.currentRatings.SchoolNameHeader
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

    public checkQualityOfEducationJudgementsPresent(): this {
        this.elements.currentRatings.qualityOfEducation().each((element) => {
            const text = element.text().trim();
            expect(text).to.match(/Good|Outstanding|No judgement|Outstanding|Requires improvement|Inadequate|Not yet inspected/);
        });
        return this;
    }

    public checkBehaviourAndAttitudesJudgementsPresent(): this {
        this.elements.currentRatings.behaviourAndAttitudes().each((element) => {
            const text = element.text().trim();
            expect(text).to.match(/Good|Outstanding|No judgement|Outstanding|Requires improvement|Inadequate|Not yet inspected/);
        });
        return this;
    }

    public checkPesronalDevelopmentJudgementsPresent(): this {
        this.elements.currentRatings.personalDevelopment().each((element) => {
            const text = element.text().trim();
            expect(text).to.match(/Good|Outstanding|No judgement|Outstanding|Requires improvement|Inadequate|Not yet inspected/);
        });
        return this;
    }

    public checkLeadershipAndManagementJudgementsPresent(): this {
        this.elements.currentRatings.leadershipAndManagement().each((element) => {
            const text = element.text().trim();
            expect(text).to.match(/Good|Outstanding|No judgement|Outstanding|Requires improvement|Inadequate|Not yet inspected|Does not apply/);
        });
        return this;
    }

    public checkEarlyYearsProvisionJudgementsPresent(): this {
        this.elements.currentRatings.earlyYearsProvision().each((element) => {
            const text = element.text().trim();
            expect(text).to.match(/Good|Outstanding|No judgement|Outstanding|Requires improvement|Inadequate|Not yet inspected|Does not apply/);
        });
        return this;
    }

    public checkSixthFormProvisionJudgementsPresent(): this {
        this.elements.currentRatings.sixthFormProvision().each((element) => {
            const text = element.text().trim();
            expect(text).to.match(/Good|Outstanding|No judgement|Outstanding|Requires improvement|Inadequate|Not yet inspected|Does not apply/);
        });
        return this;
    }

    public checkBeforeOrAfterJoiningJudgementsPresent(): this {
        this.elements.currentRatings.beforeOrAfterJoining().each((element) => {
            const text = element.text().trim();
            expect(text).to.match(/Before|After|Not yet inspected/);
        });
        return this;
    }

    public checkNoDataMessageIsVisible(): this {
        this.elements.currentRatings.NoDataMessage().should('be.visible');
        return this;
    }
}

const ofstedPage = new OfstedPage();
export default ofstedPage;
