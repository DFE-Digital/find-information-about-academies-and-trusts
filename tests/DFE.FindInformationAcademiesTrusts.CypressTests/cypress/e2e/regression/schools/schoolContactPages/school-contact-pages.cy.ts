import schoolsPage from '../../../../pages/schools/schoolsPage';
import { testSchoolData } from '../../../../support/test-data-store';

// Helper to build the URL for the school contacts page
const getInTheSchoolContactsUrl = (urn: number) => `/schools/contacts/in-the-school?urn=${urn}`;

// Helper to build the URL for the DfE contacts page
const getInDfeContactsUrl = (urn: number) => `/schools/contacts/in-dfe?urn=${urn}`;

describe('Testing the components of the School contacts page', () => {
    (testSchoolData as { schoolName: string; urn: number; }[]).forEach(({ schoolName, urn }) => {
        describe(`On the contacts in school page for ${schoolName}`, () => {
            beforeEach(() => {
                cy.visit(getInTheSchoolContactsUrl(urn));
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

describe('Testing the components of the School DfE contacts page', () => {
    (testSchoolData as { schoolName: string; urn: number; }[]).forEach(({ schoolName, urn }) => {
        describe(`On the 'inDfE contacts page for ${schoolName}`, () => {
            beforeEach(() => {
                cy.visit(getInDfeContactsUrl(urn));
            });

            it('Checks the subpage header is correct', () => {
                schoolsPage
                    .checkInDfeContactsSubpageHeaderIsCorrect();
            });

            it('Checks the regions group LA lead contact card is present', () => {
                schoolsPage
                    .checkRegionsGroupLaLeadContactCardPresent();
            });

            it('Checks the regions group LA lead contact title is displayed', () => {
                schoolsPage
                    .checkRegionsGroupLaLeadContactTitlePresent();
            });

            it('Checks the regions group LA lead contact name is displayed', () => {
                schoolsPage
                    .checkRegionsGroupLaLeadContactNamePresent();
            });

            it('Checks the regions group LA lead contact email is displayed', () => {
                schoolsPage
                    .checkRegionsGroupLaLeadContactEmailPresent();
            });

            it('Checks no internal use warning is present (DfE contacts are not internal use only)', () => {
                schoolsPage
                    .checkNoInternalUseWarningPresent();
            });

        });
    });
});
