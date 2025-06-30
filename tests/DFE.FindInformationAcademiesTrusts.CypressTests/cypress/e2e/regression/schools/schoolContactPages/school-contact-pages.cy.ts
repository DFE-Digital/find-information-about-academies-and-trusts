import schoolsPage from '../../../../pages/schools/schoolsPage';
import { testSchoolData } from '../../../../support/test-data-store';

// Test data constants
const TEST_SCHOOL_URN = 123452; // The Meadows Primary School
const TEST_ACADEMY_URN = 137083; // Abbey Grange Church of England Academy

// Helper to build the URL for the school contacts page
const getInTheSchoolContactsUrl = (urn: number) => `/schools/contacts/in-the-school?urn=${urn}`;

// Helper to build the URL for the "in DfE" contacts page
const getInDfeContactsUrl = (urn: number) => `/schools/contacts/in-dfe?urn=${urn}`;

describe('Testing the components of the School "in this school/academy" contacts page', () => {
    (testSchoolData as { schoolName: string; urn: number; schoolOrAcademy: string; }[]).forEach(({ schoolName, urn, schoolOrAcademy }) => {
        describe(`On the "in this ${schoolOrAcademy}" contacts page for ${schoolName}`, () => {
            beforeEach(() => {
                cy.visit(getInTheSchoolContactsUrl(urn));
            });

            it('Checks the subpage header is correct', () => {
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

describe('Testing the components of the School "in DfE" contacts page', () => {
    beforeEach(() => {
        cy.visit(getInDfeContactsUrl(TEST_SCHOOL_URN));
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

describe('Testing the components of the Academy "in DfE" contacts page (placeholder until in dfe academies are implemented)', () => {
    beforeEach(() => {
        cy.visit(getInDfeContactsUrl(TEST_ACADEMY_URN));
    });

    it('Checks the subpage header is correct', () => {
        schoolsPage
            .checkInDfeContactsSubpageHeaderIsCorrect();
    });

    it('Checks the regions group LA lead contact card is present (placeholder behavior)', () => {
        schoolsPage
            .checkRegionsGroupLaLeadContactCardPresent();
    });

    it('Checks no internal use warning is present', () => {
        schoolsPage
            .checkNoInternalUseWarningPresent();
    });

    // TODO: When academy implementation is complete in future feature, add tests for:
    // - SFSO (Schools financial support and oversight) lead contact card
    // - SFSO contact title, name, and email verification
    // - Update to handle two contact cards instead of one
    // - update  placeholder tests 
});
