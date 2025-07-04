import trustContactsPage from "../../../pages/trusts/trustContactsPage";
import commonPage from "../../../pages/commonPage";
import navigation from "../../../pages/navigation";
import { testTrustData } from "../../../support/test-data-store";

function generateNameAndEmail() {
    const randomNumber = Math.floor(Math.random() * 9999);
    return {
        name: `Name${randomNumber}`,
        email: `email${randomNumber}@education.gov.uk`
    };
}

// Helper function to check browser title for different contact pages
function checkContactPageTitle(pageType: string) {
    const titleMappings: Record<string, string> = {
        'in-dfe': 'In DfE - Contacts - {trustName} - Find information about schools and trusts',
        'in-the-trust': 'In this trust - Contacts - {trustName} - Find information about schools and trusts',
        'edit-trm': 'Edit Trust relationship manager details - Contacts - {trustName} - Find information about schools and trusts',
        'edit-sfso': 'Edit SFSO (Schools financial support and oversight) lead details - Contacts - {trustName} - Find information about schools and trusts'
    };

    commonPage
        .checkThatBrowserTitleForTrustPageMatches(titleMappings[pageType]);
}

// Email validation test cases
const emailValidationTestCases = [
    {
        description: "full non DFE email",
        email: "email@hotmail.co.uk",
        expectedErrors: ['Enter a DfE email address without any spaces']
    },
    {
        description: "incorrect email format",
        email: "email",
        trmExpectedErrors: ['Enter an email address in the correct format, like name@education.gov.uk'],
        sfsoExpectedErrors: ['Enter a DfE email address without any spaces', 'Enter an email address in the correct format, like name@education.gov.uk']
    },
    {
        description: "illegal characters",
        email: "@£$$^&",
        trmExpectedErrors: ['Enter an email address in the correct format, like name@education.gov.uk'],
        sfsoExpectedErrors: ['Enter a DfE email address without any spaces', 'Enter an email address in the correct format, like name@education.gov.uk']
    },
    {
        description: "whitespace",
        email: "a     b",
        expectedErrors: ['Enter a DfE email address without any spaces', 'Enter an email address in the correct format, like name@education.gov.uk']
    },
    {
        description: "email without prefix",
        email: "@education.gov.uk",
        expectedErrors: ['Enter an email address in the correct format, like name@education.gov.uk']
    }
];

describe("Testing the components of the Trust contacts page", () => {
    testTrustData.forEach(({ typeOfTrust, uid }) => {
        describe(`On the contacts in DfE page for a ${typeOfTrust}`, () => {
            beforeEach(() => {
                cy.visit(`/trusts/contacts/in-dfe?uid=${uid}`);
            });

            it("Checks the browser title is correct", () => {
                checkContactPageTitle('in-dfe');
            });

            it("Checks the breadcrumb shows the correct page name", () => {
                navigation
                    .checkPageNameBreadcrumbPresent("Contacts");
            });

            it(`Can change Trust relationship manager contact details`, () => {
                const { name, email } = generateNameAndEmail();

                trustContactsPage
                    .editTrustRelationshipManager(name, email)
                    .checkTrustRelationshipManagerIsSuccessfullyUpdated(name, email)
                    .checkTrustRelationshipManagerDatasourceLastUpdatedByUser('Automation User - email');

                commonPage
                    .checkSuccessPopup('Changes made to the Trust relationship manager name and email were updated')
                    .checkErrorPopupNotPresent();
            });

            it(`Can change Schools financial support oversight lead contact details`, () => {
                const { name, email } = generateNameAndEmail();

                trustContactsPage
                    .editSfsoLead(name, email)
                    .checkSfsoLeadIsSuccessfullyUpdated(name, email)
                    .checkSfsoLeadDatasourceLastUpdatedByUser('Automation User - email');

                commonPage
                    .checkSuccessPopup('Changes made to the SFSO (Schools financial support and oversight) lead name and email were updated')
                    .checkErrorPopupNotPresent();
            });

            it(`Checks that when cancelling the edit of a TRM contact that I am taken back to the previous page and that entered data is not saved`, () => {
                trustContactsPage
                    .editTrustRelationshipManagerWithoutSaving("Should Notbe Seen", "exittest@education.gov.uk")
                    .clickContactUpdateCancelButton()
                    .checkTrustRelationshipManagerIsNotUpdated("Should Notbe Seen", "exittest@education.gov.uk");

                navigation
                    .checkCurrentURLIsCorrect(`/trusts/contacts/in-dfe?uid=${uid}`);
            });

            it(`Checks that when cancelling the edit of a SFSO contact that I am taken back to the previous page and that entered data is not saved`, () => {
                trustContactsPage
                    .editSfsoLeadWithoutSaving("Should Notbe Seen", "exittest@education.gov.uk")
                    .clickContactUpdateCancelButton()
                    .checkSfsoLeadIsNotUpdated("Should Notbe Seen", "exittest@education.gov.uk");

                navigation
                    .checkCurrentURLIsCorrect(`/trusts/contacts/in-dfe?uid=${uid}`);
            });
        });

        describe(`On the edit Trust relationship manager contact details page for a ${typeOfTrust}`, () => {
            beforeEach(() => {
                cy.visit(`/trusts/contacts/edittrustrelationshipmanager?uid=${uid}`);
            });

            it("Checks the browser title is correct", () => {
                checkContactPageTitle('edit-trm');
            });
        });

        describe(`On the edit SFSO lead contact details page for a ${typeOfTrust}`, () => {
            beforeEach(() => {
                cy.visit(`/trusts/contacts/editsfsolead?uid=${uid}`);
            });

            it("Checks the browser title is correct", () => {
                checkContactPageTitle('edit-sfso');
            });
        });

        describe(`On the contacts in the trust page for a ${typeOfTrust}`, () => {
            beforeEach(() => {
                cy.visit(`/trusts/contacts/in-the-trust?uid=${uid}`);
            });

            it("Checks the browser title is correct", () => {
                checkContactPageTitle('in-the-trust');
            });

            it("Checks the breadcrumb shows the correct page name", () => {
                navigation
                    .checkPageNameBreadcrumbPresent("Contacts");
            });

            it(`Checks a trusts external contact details are present`, () => {
                trustContactsPage
                    .checkAccountingOfficerPresent()
                    .checkChairOfTrusteesPresent()
                    .checkChiefFinancialOfficerPresent();
            });
        });
    });

    describe('Checks the update error handling', () => {
        beforeEach(() => {
            cy.visit('/trusts/contacts/in-dfe?uid=5527');
        });

        describe('TRM email validation errors', () => {
            emailValidationTestCases.forEach(({ description, email, expectedErrors, trmExpectedErrors }) => {
                const errors = trmExpectedErrors ?? expectedErrors;
                it(`Checks that ${description} entered returns the correct error message on a TRM`, () => {
                    trustContactsPage.editTrustRelationshipManager("Name", email);
                    errors.forEach(error => {
                        commonPage.checkErrorPopup(error);
                    });
                });
            });
        });

        describe('SFSO email validation errors', () => {
            emailValidationTestCases.forEach(({ description, email, expectedErrors, sfsoExpectedErrors }) => {
                const errors = sfsoExpectedErrors ?? expectedErrors;
                it(`Checks that ${description} entered returns the correct error message on a SFSO`, () => {
                    trustContactsPage.editSfsoLead("Name", email);
                    errors.forEach(error => {
                        commonPage.checkErrorPopup(error);
                    });
                });
            });
        });
    });

    describe("Testing the contacts sub navigation", () => {
        const testUid = '5527';

        it('Should check that the contacts in dfe navigation button takes me to the correct page', () => {
            cy.visit(`/trusts/contacts/in-the-trust?uid=${testUid}`);

            trustContactsPage
                .clickContactsInDfeSubnavButton()
                .checkContactsInDfeSubHeaderPresent();

            navigation.checkCurrentURLIsCorrect(`/trusts/contacts/in-dfe?uid=${testUid}`);

            trustContactsPage
                .checkAllSubNavItemsPresent()
                .checkSfsoLeadIsPresent()
                .checkTrustRelationshipManagerIsPresent();
        });

        it('Should check that the contacts in this trust navigation button takes me to the correct page', () => {
            cy.visit(`/trusts/contacts/in-dfe?uid=${testUid}`);

            trustContactsPage
                .clickContactsInTheTrustSubnavButton()
                .checkContactsInTheTrustSubHeaderPresent();

            navigation.checkCurrentURLIsCorrect(`/trusts/contacts/in-the-trust?uid=${testUid}`);

            trustContactsPage
                .checkAllSubNavItemsPresent()
                .checkAccountingOfficerPresent()
                .checkChairOfTrusteesPresent()
                .checkChiefFinancialOfficerPresent();
        });

        it('Should check that the contacts sub nav items are not present when I am not on the contacts page', () => {
            cy.visit(`/trusts/overview/trust-details?uid=${testUid}`);

            trustContactsPage.checkSubNavNotPresent();
        });

        describe("Testing a trust that has no contacts within it to ensure the issue of a 500 page appearing does not happen", () => {
            const noContactsTrustUid = '17728';

            beforeEach(() => {
                commonPage.interceptAndVerifyNo500Errors();
            });

            [
                `/trusts/contacts/in-dfe?uid=${noContactsTrustUid}`,
                `/trusts/contacts/in-the-trust?uid=${noContactsTrustUid}`
            ].forEach((url) => {
                it(`Should have no 500 error on ${url}`, () => {
                    cy.visit(url);
                });
            });
        });
    });
});
