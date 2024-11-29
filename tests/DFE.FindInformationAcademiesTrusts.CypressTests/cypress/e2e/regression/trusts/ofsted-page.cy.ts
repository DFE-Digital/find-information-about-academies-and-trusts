import ofstedPage from "../../../pages/trusts/ofstedPage";

describe("Testing the Ofsted page and its subpages ", () => {

    describe("Testing the Ofsted current ratings page ", () => {
        beforeEach(() => {
            cy.login();
            cy.visit('/trusts/ofsted/current-ratings?uid=5143');
        });

        it("Checks the correct Ofsted current ratings headers are present", () => {
            ofstedPage
                .checkOfstedCurrentRatingsTableHeadersPresent();
        });

        it("Checks the Ofsted page sorting", () => {
            ofstedPage
                .checkOfstedCurrentRatingsSorting();
        });

        it("Checks that a trusts correct judgement types are present", () => {
            ofstedPage
                .checkQualityOfEducationJudgementsPresent()
                .checkBehaviourAndAttitudesJudgementsPresent()
                .checkPesronalDevelopmentJudgementsPresent()
                .checkLeadershipAndManagementJudgementsPresent()
                .checkEarlyYearsProvisionJudgementsPresent()
                .checkSixthFormProvisionJudgementsPresent()
                .checkBeforeOrAfterJoiningJudgementsPresent();
        });

        it("Checks that a different trusts correct judgement types are present", () => {
            cy.visit('/trusts/ofsted/current-ratings?uid=5712');
            ofstedPage
                .checkQualityOfEducationJudgementsPresent()
                .checkBehaviourAndAttitudesJudgementsPresent()
                .checkPesronalDevelopmentJudgementsPresent()
                .checkLeadershipAndManagementJudgementsPresent()
                .checkEarlyYearsProvisionJudgementsPresent()
                .checkSixthFormProvisionJudgementsPresent()
                .checkBeforeOrAfterJoiningJudgementsPresent();
        });

    });
});
