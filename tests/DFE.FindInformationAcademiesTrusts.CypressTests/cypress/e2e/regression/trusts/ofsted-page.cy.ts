import ofstedPage from "../../../pages/trusts/ofstedPage";
import navigation from "../../../pages/navigation";
import dataDownload from "../../../pages/trusts/dataDownload";
import commonPage from "../../../pages/commonPage";

describe("Testing the Ofsted page and its subpages ", () => {

    describe("Testing the Ofsted current ratings page ", () => {
        beforeEach(() => {
            cy.login();
            cy.visit('/trusts/ofsted/current-ratings?uid=5143');

            cy.task('checkForFiles', 'cypress/downloads').then((files) => {
                if (files) {
                    cy.task('clearDownloads', 'cypress/downloads');
                }
            });
        });

        it("Checks the correct Ofsted current ratings subpage header is present", () => {
            ofstedPage
                .checkOfstedCurrentRatingsPageHeaderPresent();
        });

        it("Checks the browser title is correct", () => {
            commonPage
                .checkThatBrowserTitleForTrustPageMatches('Current ratings - Ofsted - {trustName} - Find information about academies and trusts');
        });

        it("Checks the breadcrumb shows the correct page name", () => {
            navigation
                .checkPageNameBreadcrumbPresent("Ofsted");
        });

        it("Checks the correct Ofsted current ratings table headers are present", () => {
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
                .checkCurrentRatingsPersonalDevelopmentJudgementsPresent()
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
                .checkCurrentRatingsPersonalDevelopmentJudgementsPresent()
                .checkCurrentRatingsLeadershipAndManagementJudgementsPresent()
                .checkCurrentRatingsEarlyYearsProvisionJudgementsPresent()
                .checkCurrentRatingsSixthFormProvisionJudgementsPresent()
                .checkCurrentRatingsBeforeOrAfterJoiningJudgementsPresent();
        });

        it('should export academies data as an xlsx and verify it has downloaded and has content', () => {
            ofstedPage
                .clickDownloadButton();
            dataDownload
                .checkFileDownloaded()
                .checkFileHasContent()
                .deleteDownloadedFile();
        });
    });

    describe("Testing the Ofsted previous ratings page ", () => {
        beforeEach(() => {
            cy.login();
            cy.visit('/trusts/ofsted/previous-ratings?uid=5143');

            cy.task('checkForFiles', 'cypress/downloads').then((files) => {
                if (files) {
                    cy.task('clearDownloads', 'cypress/downloads');
                }
            });
        });

        it("Checks the correct Ofsted Previous ratings subpage header is present", () => {
            ofstedPage
                .checkOfstedPreviousRatingsPageHeaderPresent();
        });

        it("Checks the browser title is correct", () => {
            commonPage
                .checkThatBrowserTitleForTrustPageMatches('Previous ratings - Ofsted - {trustName} - Find information about academies and trusts');
        });

        it("Checks the breadcrumb shows the correct page name", () => {
            navigation
                .checkPageNameBreadcrumbPresent("Ofsted");
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
                .checkPreviousRatingsPersonalDevelopmentJudgementsPresent()
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
                .checkPreviousRatingsPersonalDevelopmentJudgementsPresent()
                .checkPreviousRatingsLeadershipAndManagementJudgementsPresent()
                .checkPreviousRatingsEarlyYearsProvisionJudgementsPresent()
                .checkPreviousRatingsSixthFormProvisionJudgementsPresent()
                .checkPreviousRatingsBeforeOrAfterJoiningJudgementsPresent();
        });

        it('should export academies data as an xlsx and verify it has downloaded and has content', () => {
            ofstedPage
                .clickDownloadButton();
            dataDownload
                .checkFileDownloaded()
                .checkFileHasContent()
                .deleteDownloadedFile();
        });
    });

    describe("Testing the Ofsted Safeguarding and concerns page", () => {
        beforeEach(() => {
            cy.login();
            cy.visit('/trusts/ofsted/safeguarding-and-concerns?uid=5143');

            cy.task('checkForFiles', 'cypress/downloads').then((files) => {
                if (files) {
                    cy.task('clearDownloads', 'cypress/downloads');
                }
            });
        });

        it("Checks the correct Ofsted safeguarding and concerns subpage header is present", () => {
            ofstedPage
                .checkOfstedSafeguardingConcernsPageHeaderPresent();
        });

        it("Checks the browser title is correct", () => {
            commonPage
                .checkThatBrowserTitleForTrustPageMatches('Safeguarding and concerns - Ofsted - {trustName} - Find information about academies and trusts');
        });

        it("Checks the breadcrumb shows the correct page name", () => {
            navigation
                .checkPageNameBreadcrumbPresent("Ofsted");
        });

        it("Checks the correct Ofsted safeguarding and concerns headers are present", () => {
            ofstedPage
                .checkOfstedSafeguardingConcernsTableHeadersPresent();
        });

        it("Checks the Ofsted page safeguarding and concerns sorting", () => {
            ofstedPage
                .checkOfstedSafeguardingConcernsSorting();
        });

        it("Checks that a trusts safeguarding and concerns correct judgement types are present", () => {
            ofstedPage
                .checkSafeguardingConcernsEffectiveSafeguardingJudgementsPresent()
                .checkSafeguardingConcernsCategoryOfConcernJudgementsPresent()
                .checkSafeguardingConcernsBeforeOrAfterJoiningJudgementsPresent();
        });

        it("Checks that a different trusts safeguarding and concerns correct judgement types are present", () => {
            cy.visit('/trusts/ofsted/safeguarding-and-concerns?uid=5712');
            ofstedPage
                .checkSafeguardingConcernsEffectiveSafeguardingJudgementsPresent()
                .checkSafeguardingConcernsCategoryOfConcernJudgementsPresent()
                .checkSafeguardingConcernsBeforeOrAfterJoiningJudgementsPresent();
        });

        it('should export academies data as an xlsx and verify it has downloaded and has content', () => {
            ofstedPage
                .clickDownloadButton();
            dataDownload
                .checkFileDownloaded()
                .checkFileHasContent()
                .deleteDownloadedFile();
        });
    });

    describe("Testing the Ofsted important dates page ", () => {
        beforeEach(() => {
            cy.login();
            cy.visit('/trusts/ofsted/important-dates?uid=5143');

            // Clear the downloads folder before running each test
            cy.task('checkForFiles', 'cypress/downloads').then((files) => {
                if (files) {
                    cy.task('clearDownloads', 'cypress/downloads');
                }
            });
        });

        it("Checks the correct Ofsted important dates sub page header is present", () => {
            ofstedPage
                .checkOfstedImportantDatesPageHeaderPresent();
        });

        it("Checks the browser title is correct", () => {
            commonPage
                .checkThatBrowserTitleForTrustPageMatches('Important dates - Ofsted - {trustName} - Find information about academies and trusts');
        });

        it("Checks the breadcrumb shows the correct page name", () => {
            navigation
                .checkPageNameBreadcrumbPresent("Ofsted");
        });

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

        it('should export academies data as an xlsx and verify it has downloaded and has content', () => {
            ofstedPage
                .clickDownloadButton();
            dataDownload
                .checkFileDownloaded()
                .checkFileHasContent()
                .deleteDownloadedFile();
        });
    });

    describe("Testing a trust that has no ofsted data within it to ensure the issue of a 500 page appearing does not happen", () => {
        beforeEach(() => {
            cy.login();
            commonPage.interceptAndVerifyNo500Errors();
        });

        ['/trusts/ofsted/current-ratings?uid=17728', '/trusts/ofsted/previous-ratings?uid=17728', '/trusts/ofsted/important-dates?uid=17728', '/trusts/ofsted/safeguarding-and-concerns?uid=17728'].forEach((url) => {
            it(`Should have no 500 error on ${url}`, () => {
                cy.visit(url);
            });
        });
    });
});
