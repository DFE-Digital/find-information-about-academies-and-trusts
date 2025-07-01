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

describe('Testing the components of the Academy "in DfE" contacts page', () => {
    beforeEach(() => {
        cy.visit(getInDfeContactsUrl(TEST_ACADEMY_URN));
    });

    it('Checks the subpage header is correct', () => {
        schoolsPage
            .checkInDfeContactsSubpageHeaderIsCorrect();
    });

    it('Checks the trust relationship manager contact card is present', () => {
        schoolsPage
            .checkTrustRelationshipManagerContactCardPresent();
    });

    it('Checks the trust relationship manager contact title is displayed', () => {
        schoolsPage
            .checkTrustRelationshipManagerContactTitlePresent();
    });

    it('Checks the trust relationship manager contact name is displayed', () => {
        schoolsPage
            .checkTrustRelationshipManagerContactNamePresent();
    });

    it('Checks the trust relationship manager contact email is displayed', () => {
        schoolsPage
            .checkTrustRelationshipManagerContactEmailPresent();
    });

    it('Checks the SFSO lead contact card is present', () => {
        schoolsPage
            .checkSfsoLeadContactCardPresent();
    });

    it('Checks the SFSO lead contact title is displayed', () => {
        schoolsPage
            .checkSfsoLeadContactTitlePresent();
    });

    it('Checks the SFSO lead contact name is displayed', () => {
        schoolsPage
            .checkSfsoLeadContactNamePresent();
    });

    it('Checks the SFSO lead contact email is displayed', () => {
        schoolsPage
            .checkSfsoLeadContactEmailPresent();
    });

    it('Checks no internal use warning is present', () => {
        schoolsPage
            .checkNoInternalUseWarningPresent();
    });

    it('Checks no Change/Edit links are present for academy contacts (editing should be done at trust level only)', () => {
        schoolsPage
            .checkNoChangeLinksPresent();
    });
});
