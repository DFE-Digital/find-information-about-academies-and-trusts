import commonPage from '../../../../pages/commonPage';
import schoolsPage from '../../../../pages/schools/schoolsPage';

describe('School Federation Details Tests', () => {
    const schoolWithFederationDetails = 107188;
    const schoolWithoutFederationDetails = 100000;
    const academy = 142768;

    context('School with federation details', () => {
        beforeEach(() => {
            // Replace with a URN of a school that has federation details
            cy.visit(`/schools/overview/federation?urn=${schoolWithFederationDetails}`);
        });

        it('should display federation details correctly', () => {
            schoolsPage
                .checkFederationDetailsHeaderPresent()
                .checkFederationDetailsPresent()
                .checkFederationSchoolsListPresent();
        });
    });

    context('School without federation details', () => {
        beforeEach(() => {
            cy.visit(`/schools/overview/federation?urn=${schoolWithoutFederationDetails}`);
        });

        it('should display not available for all federation fields', () => {
            schoolsPage
                .checkFederationDetailsHeaderPresent()
                .checkFederationDetailsNotAvailable();
        });
    });

    context('Academy should not have federation page', () => {

        it('should not show federation tab for academies', () => {
            cy.visit(`/schools/overview/details?urn=${academy}`);
            schoolsPage
                .checkFederationTabNotPresent();
        });

        it('should not allow direct access to federation page for academies', () => {
            /**
             * Note: This test is currently failing due to a known bug.
             * The bug is related to the federation page being incorrectly accessible for academies.
             * Once the bug is fixed, this test should pass as expected.
             */
            commonPage.interceptAndVerifyResponseHas404Status(`/schools/overview/federation?urn=${academy}`);

            cy.visit(`/schools/overview/federation?urn=${academy}`, { failOnStatusCode: false });

            cy.wait('@checkTheResponseIs404');

            commonPage
                .checkPageNotFoundDisplayed();
        });
    });
});
