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
        schoolURN: 107188,
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

    describe("Schools main navigation tests", () => {
        it('Should check that the schools main navigation is present and correct', () => {
            // School Overview --> School Contacts (School)
            cy.visit(`/schools/overview/details?urn=${navTestSchool.schoolURN}`);
            navigation
                .clickSchoolsContactsButton()
                .checkCurrentURLIsCorrect(`/schools/contacts/in-the-school?urn=${navTestSchool.schoolURN}`)
                .checkAllSchoolServiceNavItemsPresent();
            schoolsPage
                .checkHeadTeacherContactCardPresent();

            // School Overview --> School Contacts (Academy)
            cy.visit(`/schools/overview/details?urn=${navTestAcademy.academyURN}`);
            navigation
                .clickSchoolsContactsButton()
                .checkCurrentURLIsCorrect(`/schools/contacts/in-the-school?urn=${navTestAcademy.academyURN}`)
                .checkAllSchoolServiceNavItemsPresent();
            schoolsPage
                .checkHeadTeacherContactCardPresent();
        });

    });

    describe("Schools overview sub navigation round robin tests", () => {
        context('School overview subnav round robin tests -- (School)', () => {
            // school details --> Federation details (school)
            it('Should check that the school details navigation button takes me to the correct page for a schools type subnav', () => {
                cy.visit(`/schools/overview/details?urn=${navTestSchool.schoolURN}`);
                navigation
                    .clickSchoolsFederationButton()
                    .checkCurrentURLIsCorrect(`/schools/overview/federation?urn=${navTestSchool.schoolURN}`)
                    .checkAllSchoolServiceNavItemsPresent()
                    .checkAllSchoolsSubNavItemsPresent();
                schoolsPage
                    .checkFederationDetailsHeaderPresent();
            });

            // federation details --> SEN (school)
            it('Should check that the school details navigation button takes me to the SEN page for a schools type subnav', () => {
                cy.visit(`/schools/overview/federation?urn=${navTestSchool.schoolURN}`);
                navigation
                    .clickSchoolsSENButton()
                    .checkCurrentURLIsCorrect(`/schools/overview/sen?urn=${navTestSchool.schoolURN}`)
                    .checkAllSchoolServiceNavItemsPresent()
                    .checkAllSchoolsSubNavItemsPresent();
                schoolsPage
                    .checkSENSubpageHeaderCorrect();
            });

            // SEN --> school details (school)
            it('Should check that the school details navigation button takes me to the correct page for a schools type subnav', () => {
                cy.visit(`/schools/overview/sen?urn=${navTestSchool.schoolURN}`);
                navigation
                    .clickSchoolsDetailsButton()
                    .checkCurrentURLIsCorrect(`/schools/overview/details?urn=${navTestSchool.schoolURN}`)
                    .checkAllSchoolServiceNavItemsPresent()
                    .checkAllSchoolsSubNavItemsPresent();
                schoolsPage
                    .checkSchoolDetailsHeaderPresent();
            });
        });

        context('School overview subnav round robin tests -- (Academy)', () => {
            // school details --> SEN (academy)
            it('Should check that the school details navigation button takes me to the correct page for a schools type subnav', () => {
                cy.visit(`/schools/overview/details?urn=${navTestAcademy.academyURN}`);
                navigation
                    .clickSchoolsSENButton()
                    .checkCurrentURLIsCorrect(`/schools/overview/sen?urn=${navTestAcademy.academyURN}`)
                    .checkAllSchoolServiceNavItemsPresent()
                    .checkAllSchoolsSubNavItemsPresent();
                schoolsPage
                    .checkSENSubpageHeaderCorrect();
            });

            // SEN --> school details (academy)
            it('Should check that the school details navigation button takes me to the correct page for a schools type subnav', () => {
                cy.visit(`/schools/overview/sen?urn=${navTestAcademy.academyURN}`);
                navigation
                    .clickSchoolsDetailsButton()
                    .checkCurrentURLIsCorrect(`/schools/overview/details?urn=${navTestAcademy.academyURN}`)
                    .checkAllSchoolServiceNavItemsPresent()
                    .checkAllSchoolsSubNavItemsPresent();
                schoolsPage
                    .checkAcademyDetailsHeaderPresent();
            });
        });
    });
});
