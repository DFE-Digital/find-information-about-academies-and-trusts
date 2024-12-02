import ofstedPage from "../../../pages/trusts/ofstedPage";

describe("Testing the Ofsted page and its subpages ", () => {

    describe("Testing the Ofsted current ratings page ", () => {
        beforeEach(() => {
            cy.login();
            cy.visit('/trusts/ofsted/current-ratings?uid=5143');
        });

        it("Checks the correct Ofsted current ratings subpage header is present", () => {
            ofstedPage
                .checkOfstedCurrentRatingsPageHeaderPresent();
        });

        it("Checks the correct Ofsted current ratings headers are present", () => {
            ofstedPage
                .checkOfstedCurrentRatingsTableHeadersPresent();
        });

        it("Checks the Ofsted current ratings page sorting", () => {
            ofstedPage
                .checkOfstedCurrentRatingsSorting();
        });

        it("Checks that a trusts current ratings correct judgement types are present", () => {
            ofstedPage
                .checkCurrentRatingsQualityOfEducationJudgementsPresent()
                .checkCurrentRatingsBehaviourAndAttitudesJudgementsPresent()
                .checkCurrentRatingsPesronalDevelopmentJudgementsPresent()
                .checkCurrentRatingsLeadershipAndManagementJudgementsPresent()
                .checkCurrentRatingsEarlyYearsProvisionJudgementsPresent()
                .checkCurrentRatingsSixthFormProvisionJudgementsPresent()
                .checkCurrentRatingsBeforeOrAfterJoiningJudgementsPresent();
        });

        it("Checks that a different trusts current ratings correct judgement types are present", () => {
            cy.visit('/trusts/ofsted/current-ratings?uid=5712');
            ofstedPage
                .checkCurrentRatingsQualityOfEducationJudgementsPresent()
                .checkCurrentRatingsBehaviourAndAttitudesJudgementsPresent()
                .checkCurrentRatingsPesronalDevelopmentJudgementsPresent()
                .checkCurrentRatingsLeadershipAndManagementJudgementsPresent()
                .checkCurrentRatingsEarlyYearsProvisionJudgementsPresent()
                .checkCurrentRatingsSixthFormProvisionJudgementsPresent()
                .checkCurrentRatingsBeforeOrAfterJoiningJudgementsPresent();
        });

    });

    describe("Testing the Ofsted previous ratings page ", () => {
        beforeEach(() => {
            cy.login();
            cy.visit('/trusts/ofsted/previous-ratings?uid=5143');
        });

        it("Checks the correct Ofsted Previous ratings subpage header is present", () => {
            ofstedPage
                .checkOfstedPreviousRatingsPageHeaderPresent();
        });

        it("Checks the correct Ofsted previous ratings headers are present", () => {
            ofstedPage
                .checkOfstedPreviousRatingsTableHeadersPresent();
        });

        it("Checks the Ofsted page previous ratings sorting", () => {
            ofstedPage
                .checkOfstedPreviousRatingsSorting();
        });

        it("Checks that a trusts previous ratings correct judgement types are present", () => {
            ofstedPage
                .checkPreviousRatingsQualityOfEducationJudgementsPresent()
                .checkPreviousRatingsBehaviourAndAttitudesJudgementsPresent()
                .checkPreviousRatingsPesronalDevelopmentJudgementsPresent()
                .checkPreviousRatingsLeadershipAndManagementJudgementsPresent()
                .checkPreviousRatingsEarlyYearsProvisionJudgementsPresent()
                .checkPreviousRatingsSixthFormProvisionJudgementsPresent()
                .checkPreviousRatingsBeforeOrAfterJoiningJudgementsPresent();
        });

        it("Checks that a different trusts previous ratings correct judgement types are present", () => {
            cy.visit('/trusts/ofsted/previous-ratings?uid=5712');
            ofstedPage
                .checkPreviousRatingsQualityOfEducationJudgementsPresent()
                .checkPreviousRatingsBehaviourAndAttitudesJudgementsPresent()
                .checkPreviousRatingsPesronalDevelopmentJudgementsPresent()
                .checkPreviousRatingsLeadershipAndManagementJudgementsPresent()
                .checkPreviousRatingsEarlyYearsProvisionJudgementsPresent()
                .checkPreviousRatingsSixthFormProvisionJudgementsPresent()
                .checkPreviousRatingsBeforeOrAfterJoiningJudgementsPresent();
        });

    });

    describe("Testing the Ofsted important dates page ", () => {
        beforeEach(() => {
            cy.login();
            cy.visit('/trusts/ofsted/important-dates?uid=5143');
        });


        it("Checks the correct Ofsted important dates sub page header is present", () => {
            ofstedPage
                .checkOfstedImportantDatesPageHeaderPresent();
        })

        it("Checks the correct Ofsted important dates table headers are present", () => {
            ofstedPage
                .checkOfstedImportantDatesTableHeadersPresent();
        });

        it("Checks that a trusts important dates fields are present ", () => {
            ofstedPage
                .checkDateJoinedPresent()
                .checkDateOfCurrentInspectionPresent()
                .checkDateOfPreviousInspectionPresent();
        });

        it("Checks that a trusts important dates sorting is working", () => {
            ofstedPage
                .checkOfstedImportantDatesSorting();
        });

        it("Checks that a different trusts important dates fields are present", () => {
            cy.visit('/trusts/ofsted/important-dates?uid=5712');
            ofstedPage
                .checkDateJoinedPresent()
                .checkDateOfCurrentInspectionPresent()
                .checkDateOfPreviousInspectionPresent();
        });

    });
});
