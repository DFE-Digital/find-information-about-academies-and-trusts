import commonPage from "../../../pages/commonPage";
import pipelineAcademiesPage from "../../../pages/trusts/pipelineAcademiesPage";
import navigation from "../../../pages/navigation";

const testPreAdvisoryData = [
    {
        uid: 16002
    },
    {
        uid: 4921
    }
];

const testPostAdvisoryData = [
    {
        uid: 17584
    },
    {
        uid: 16857
    }
];

const testFreeSchoolsData = [
    {
        uid: 17538
    },
    {
        uid: 15786
    }
];

describe("Testing the Pipeline academies pages", () => {

    describe(`On the Pre advisory board page for a trust`, () => {
        testPreAdvisoryData.forEach(({ uid }) => {
            beforeEach(() => {
                cy.login();
                cy.visit(`/trusts/academies/pipeline/pre-advisory-board?uid=${uid}`);
            });

            it("Checks the browser title is correct", () => {
                commonPage
                    .checkThatBrowserTitleForTrustPageMatches('Pre advisory board - Pipeline academies - Academies - {trustName} - Find information about academies and trusts');
            });

            it("Checks the Pre advisory board Pipeline academies subpage header is present", () => {
                pipelineAcademiesPage
                    .checkPreAdvisoryPageHeaderPresent();
            });

            it("Checks the breadcrumb shows the correct page name", () => {
                navigation
                    .checkPageNameBreadcrumbPresent("Academies");
            });

            it("Checks the correct Pipeline academies Pre advisory board table headers are present", () => {
                pipelineAcademiesPage
                    .checkPreAdvisoryTableHeadersPresent();
            });

            it("Checks the Pipeline academies Pre advisory page sorting", () => {
                pipelineAcademiesPage
                    .checkPreAdvisoryTableSorting();
            });

        });
    });

    describe(`On the Post advisory board page for`, () => {
        testPostAdvisoryData.forEach(({ uid }) => {
            beforeEach(() => {
                cy.login();
                cy.visit(`/trusts/academies/pipeline/post-advisory-board?uid=${uid}`);
            });

            it("Checks the browser title is correct", () => {
                commonPage
                    .checkThatBrowserTitleForTrustPageMatches('Post advisory board - Pipeline academies - Academies - {trustName} - Find information about academies and trusts');
            });

            it("Checks the Post advisory board Pipeline academies subpage header is present", () => {
                pipelineAcademiesPage
                    .checkPostAdvisoryPageHeaderPresent();
            });

            it("Checks the breadcrumb shows the correct page name", () => {
                navigation
                    .checkPageNameBreadcrumbPresent("Academies");
            });

            it("Checks the correct Pipeline academies Post advisory board table headers are present", () => {
                pipelineAcademiesPage
                    .checkPostAdvisoryTableHeadersPresent();
            });

            it("Checks the Pipeline academies Post advisory page sorting", () => {
                pipelineAcademiesPage
                    .checkPostAdvisoryTableHeadersPresent();
            });

            it("Checks the Pipeline academies Post advisory page sorting", () => {
                pipelineAcademiesPage
                    .checkPostAdvisoryTableSorting();
            });

        });
    });

    describe(`On the Free schools page`, () => {
        testFreeSchoolsData.forEach(({ uid }) => {
            beforeEach(() => {
                cy.login();
                cy.visit(`/trusts/academies/pipeline/free-schools?uid=${uid}`);
            });

            it("Checks the browser title is correct", () => {
                commonPage
                    .checkThatBrowserTitleForTrustPageMatches('Free schools - Pipeline academies - Academies - {trustName} - Find information about academies and trusts');
            });

            it("Checks the Free schools Pipeline academies subpage header is present", () => {
                pipelineAcademiesPage
                    .checkFreeSchoolsPageHeaderPresent();
            });

            it("Checks the breadcrumb shows the correct page name", () => {
                navigation
                    .checkPageNameBreadcrumbPresent("Academies");
            });

            it("Checks the correct Pipeline academies Free schools table headers are present", () => {
                pipelineAcademiesPage
                    .checkFreeSchoolsTableHeadersPresent();
            });

            it("Checks the Pipeline academies Free schools page sorting", () => {
                pipelineAcademiesPage
                    .checkFreeSchoolsTableHeadersPresent();
            });

            it("Checks the Pipeline academies Free schools page sorting", () => {
                pipelineAcademiesPage
                    .checkFreeSchoolsTableSorting();
            });

        });
    });

    describe(`On the pages with no pipeline academy data under them`, () => {

        beforeEach(() => {
            cy.login();
        });

        it("Checks the Pipeline academies Pre advisory page when an academy doesnt exist under it to ensure the correct message is displayed", () => {
            cy.visit(`/trusts/academies/pipeline/pre-advisory-board?uid=5712`);
            pipelineAcademiesPage
                .checkPreAdvisoryNoAcademyPresent();
        });

        it("Checks the Pipeline academies Post advisory page when an academy doesnt exist under it to ensure the correct message is displayed", () => {
            cy.visit(`/trusts/academies/pipeline/post-advisory-board?uid=5712`);
            pipelineAcademiesPage
                .checkPostAdvisoryNoAcademyPresent();
        });

        it("Checks the Pipeline academies Free schools page when an academy doesnt exist under it to ensure the correct message is displayed", () => {
            cy.visit(`/trusts/academies/pipeline/free-schools?uid=5712`);
            pipelineAcademiesPage
                .checkFreeSchoolsNoAcademyPresent();
        });
    });

    describe("Testing a trust that has no ofsted data within it to ensure the issue of a 500 page appearing does not happen", () => {
        beforeEach(() => {
            cy.login();
            commonPage
                .interceptAndVerifyNo500Errors();
        });

        it(`Should have no 500 error on the Pre advisory page`, () => {
            cy.visit(`/trusts/academies/pipeline/pre-advisory-board?uid=5712`);
        });

        it(`Should have no 500 error on the Post advisory page`, () => {
            cy.visit(`/trusts/academies/pipeline/post-advisory-board?uid=5712`);
        });

        it(`Should have no 500 error on the free schools page`, () => {
            cy.visit(`/trusts/academies/pipeline/free-schools?uid=5712`);
        });
    });

});
