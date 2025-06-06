import schoolsPage from '../../../../pages/schools/schoolsPage';
import { testSchoolData } from '../../../../support/test-data-store';

// Helper to build the URL for the school contacts page
const getSchoolContactsUrl = (urn: number) => `/schools/contacts/in-the-school?urn=${urn}`;

describe('Testing the components of the School contacts page', () => {
    (testSchoolData as { schoolName: string; urn: number; }[]).forEach(({ schoolName, urn }) => {
        describe(`On the contacts in school page for ${schoolName}`, () => {
            beforeEach(() => {
                cy.visit(getSchoolContactsUrl(urn));
            });

            it('Checks the subpage header is correct', () => {
                // The subpage header should contain either 'In this school' or 'In this academy'
                schoolsPage
                    .checkSubpageHeaderIsCorrect();
            });

            it('Checks the head teacher contact card is present', () => {
                schoolsPage
                    .checkHeadTeacherContactCardPresent();
            });

            it('Checks the head teacher name is displayed', () => {
                schoolsPage
                    .checkHeadTeacherContactNamePresent();
            });

            it('Checks the head teacher email is displayed', () => {
                schoolsPage
                    .checkHeadTeacherContactEmailPresent();
            });

            it('Checks the internal use warning is present', () => {
                schoolsPage
                    .checkInternalUseWarningPresent();
            });

        });
    });
});
