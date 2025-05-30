import { TestDataStore } from "../../../support/test-data-store";
import commonPage from "../../../pages/commonPage";
import schoolsPage from "../../../pages/schools/schoolsPage";
import navigation from "../../../pages/navigation";
import overviewPage from "../../../pages/trusts/overviewPage";

describe('Schools Navigation Tests', () => {
    const navTestAcademy = {
        academyURN: 140214,
        trustAcademyName: "ABBEY ACADEMIES TRUST",
    };

    const navTestSchool = {
        schoolURN: 123452,
    };

    describe("Routing tests", () => {

        const schoolTypesNotToShow = [
            { urn: 136083, unsupportedSchoolType: "Independent schools" },
            { urn: 150163, unsupportedSchoolType: "Online provider" },
            { urn: 131832, unsupportedSchoolType: "Other types" },
            { urn: 133793, unsupportedSchoolType: "Universities" },
            { urn: 137210, unsupportedSchoolType: "Closed academy" },
            { urn: 142109, unsupportedSchoolType: "Closed school" },
        ];

        schoolTypesNotToShow.forEach(({ urn, unsupportedSchoolType }) => {
            TestDataStore.GetAllSchoolSubpagesForUrn(urn).forEach(({ pageName, subpages }) => {

                describe(pageName, () => {

                    subpages.forEach(({ subpageName, url }) => {
                        it(`Should check that navigating to subpages for unsupported and closed school types display the 404 not found page ${pageName} > ${subpageName} > ${unsupportedSchoolType}`, () => {
                            // Set up an interceptor to check that the page response is a 404
                            commonPage.interceptAndVerifyResponseHas404Status(url);

                            // Go to the given subpage
                            cy.visit(url, { failOnStatusCode: false });

                            // Check that the 404 response interceptor was called
                            cy.wait('@checkTheResponseIs404');

                            // Check that the data sources component has a subheading for each subnav
                            commonPage
                                .checkPageNotFoundDisplayed();
                        });
                    });
                });
            });
        });

        it('Should check that an academy has the link to the trust in the header and it takes me to the correct trust', () => {
            cy.visit(`/schools/overview/details?urn=${navTestAcademy.academyURN}`);
            schoolsPage
                .checkAcademyLinkPresentAndCorrect(`${navTestAcademy.trustAcademyName}`)
                .clickAcademyTrustLink();
            navigation
                .checkCurrentURLIsCorrect('/trusts/overview/trust-details?uid=2044');
            overviewPage
                .checkTrustDetailsSubHeaderPresent();

        });

        it('Should check that an school does not have the link to the trust in the header', () => {
            cy.visit(`/schools/overview/details?urn=${navTestSchool.schoolURN}`);
            schoolsPage
                .checkAcademyLinkNotPresentForSchool();
        });
    });

    describe("Schools overview sub navigation round robin tests", () => {
        // school details --> SEN
        it('Should check that the school details navigation button takes me to the correct page for a schools type subnav', () => {
            cy.visit(`/schools/overview/details?urn=${navTestSchool.schoolURN}`);
            navigation
                .clickSchoolsSENButton()
                .checkCurrentURLIsCorrect('/schools/overview/sen?urn=140214')
                .checkAllSchoolServiceNavItemsPresent()
                .checkAllSchoolsSubNavItemsPresent();
            schoolsPage
                .checkSchoolDetailsHeaderPresent();
        });
    });

    describe("Schools overview sub navigation round robin tests", () => {
        // school details --> SEN
        it('Should check that the school details navigation button takes me to the correct page for a schools type subnav', () => {
            cy.visit(`/schools/overview/details?urn=${navTestSchool.schoolURN}`);
            navigation
                .clickSchoolsSENButton()
                .checkCurrentURLIsCorrect('/schools/overview/sen?urn=140214')
                .checkAllSchoolServiceNavItemsPresent()
                .checkAllSchoolsSubNavItemsPresent();
            schoolsPage
                .checkSchoolDetailsHeaderPresent();
        });

        // SEN --> school details
        it('Should check that the school details navigation button takes me to the correct page for a schools type subnav', () => {
            cy.visit(`/schools/overview/sen?urn=${navTestSchool.schoolURN}`);
            navigation
                .clickSchoolsDetailsButton()
                .checkCurrentURLIsCorrect('/schools/overview/details?urn=140214')
                .checkAllSchoolServiceNavItemsPresent()
                .checkAllSchoolsSubNavItemsPresent();
            schoolsPage
                .checkSENSubpageHeaderCorrect();
        });
    });
});
