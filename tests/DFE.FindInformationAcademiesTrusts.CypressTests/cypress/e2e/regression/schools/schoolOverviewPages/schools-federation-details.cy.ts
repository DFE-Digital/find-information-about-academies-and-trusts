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

    const noFederationUrns = [
        { urn: schoolWithoutFederationDetails, type: 'school with no federation' },
        { urn: academy, type: 'academy' }
    ];

    noFederationUrns.forEach(({ urn, type }) => {
        context(`${type} should not have federation details`, () => {
            it('should not show federation tab in overview nav', () => {
                cy.visit(`/schools/overview/details?urn=${urn}`);
                schoolsPage
                    .checkFederationTabNotPresent();
            });

            it('should not allow direct access to federation page (404)', () => {
                commonPage
                    .interceptAndVerifyResponseHas404Status(`/schools/overview/federation?urn=${urn}`);

                cy.visit(`/schools/overview/federation?urn=${urn}`, { failOnStatusCode: false });
                cy.wait('@checkTheResponseIs404');

                commonPage
                    .checkPageNotFoundDisplayed();
            });
        });
    });
});
