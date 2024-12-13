import contactsPage from "../../../pages/trusts/contactsPage";
import commonPage from "../../../pages/commonPage";
import navigation from "../../../pages/navigation";

function generateNameAndEmail() {
    const randomNumber = Math.floor(Math.random() * 9999);
    return {
        name: `Name${randomNumber}`,
        email: `email${randomNumber}@education.gov.uk`
    };
}

const testTrustData = [
    {
        typeOfTrust: "single academy trust with contacts",
        uid: 5527
    },
    {
        typeOfTrust: "multi academy trust with contacts",
        uid: 5712
    }
];

describe("Testing the components of the Trust contacts page", () => {
    testTrustData.forEach(({ typeOfTrust, uid }) => {
        describe(`On the contacts in DfE page for a ${typeOfTrust}`, () => {
            beforeEach(() => {
                cy.login();
                cy.visit(`/trusts/contacts/in-dfe?uid=${uid}`);
            });

            it("Checks the breadcrumb shows the correct page name", () => {
                navigation
                    .checkPageNameBreadcrumbPresent("Contacts");
            });

            it(`Can change Trust relationship manager contact details`, () => {
                const { name, email } = generateNameAndEmail();

                contactsPage
                    .editTrustRelationshipManager(name, email)
                    .checkTrustRelationshipManagerIsSuccessfullyUpdated(name, email)
                    .checkTrustRelationshipManagerDatasourceLastUpdatedByUser('Automation User - email');

                commonPage
                    .checkSuccessPopup('Changes made to the Trust relationship manager name and email were updated')
                    .checkErrorPopupNotPresent();
            });

            it(`Can change Schools financial support oversight lead contact details`, () => {
                const { name, email } = generateNameAndEmail();

                contactsPage
                    .editSfsoLead(name, email)
                    .checkSfsoLeadIsSuccessfullyUpdated(name, email)
                    .checkSfsoLeadDatasourceLastUpdatedByUser('Automation User - email');

                commonPage
                    .checkSuccessPopup('Changes made to the SFSO (Schools financial support and oversight) lead name and email were updated')
                    .checkErrorPopupNotPresent();
            });
        });

        describe(`On the contacts in the trust page for a ${typeOfTrust}`, () => {
            beforeEach(() => {
                cy.login();
                cy.visit(`/trusts/contacts/in-the-trust?uid=${uid}`);
            });

            it("Checks the breadcrumb shows the correct page name", () => {
                navigation
                    .checkPageNameBreadcrumbPresent("Contacts");
            });

            it(`Checks a trusts external contact details are present`, () => {
                contactsPage
                    .checkAccountingOfficerPresent()
                    .checkChairOfTrusteesPresent()
                    .checkChiefFinancialOfficerPresent();
            });
        });
    });

    describe('Checks the update error handling', () => {
        beforeEach(() => {
            cy.login();
            cy.visit('/trusts/contacts/in-dfe?uid=5527');
        });

        it("Checks that a full non DFE email entered returns the correct error message on a TRM ", () => {
            contactsPage
                .editTrustRelationshipManager("Name", "email@hotmail.co.uk");
            commonPage
                .checkErrorPopup('Enter a DfE email address without any spaces');
        });

        it("Checks that an incorrect email entered returns the correct error message on a TRM ", () => {
            contactsPage
                .editTrustRelationshipManager("Name", "email");
            commonPage
                .checkErrorPopup('Enter an email address in the correct format, like name@education.gov.uk');
        });

        it("Checks that illegal characters entered returns the correct error message on a TRM ", () => {
            contactsPage
                .editTrustRelationshipManager("Name", "@£$$^&");
            commonPage
                .checkErrorPopup('Enter an email address in the correct format, like name@education.gov.uk');
        });

        it("Checks that whitespace entered returns the correct error message on a TRM ", () => {
            contactsPage
                .editTrustRelationshipManager("Name", "a     b");
            commonPage
                .checkErrorPopup('Enter a DfE email address without any spaces')
                .checkErrorPopup('Enter an email address in the correct format, like name@education.gov.uk');
        });

        it("Checks that an email address without the prefix entered returns the correct error message on a TRM ", () => {
            contactsPage
                .editTrustRelationshipManager("Name", "@education.gov.uk");
            commonPage
                .checkErrorPopup('Enter an email address in the correct format, like name@education.gov.uk');
        });

        it("Checks that a full non DFE email entered returns the correct error message on a SFSO ", () => {
            contactsPage
                .editSfsoLead("Name", "email@hotmail.co.uk");
            commonPage
                .checkErrorPopup('Enter a DfE email address without any spaces');
        });

        it("Checks that an incorrect email entered returns the correct error message on a SFSO ", () => {
            contactsPage
                .editSfsoLead("Name", "email");
            commonPage
                .checkErrorPopup('Enter a DfE email address without any spaces')
                .checkErrorPopup('Enter an email address in the correct format, like name@education.gov.uk');
        });

        it("Checks that illegal characters entered returns the correct error message on a SFSO ", () => {
            contactsPage
                .editSfsoLead("Name", "@£$$^&");
            commonPage
                .checkErrorPopup('Enter a DfE email address without any spaces')
                .checkErrorPopup('Enter an email address in the correct format, like name@education.gov.uk');
        });

        it("Checks that whitespace entered returns the correct error message on a SFSO ", () => {
            contactsPage
                .editSfsoLead("Name", "a     b");
            commonPage
                .checkErrorPopup('Enter a DfE email address without any spaces')
                .checkErrorPopup('Enter an email address in the correct format, like name@education.gov.uk');
        });

        it("Checks that an email address without the prefix entered returns the correct error message on a SFSO ", () => {
            contactsPage
                .editSfsoLead("Name", "@education.gov.uk");
            commonPage
                .checkErrorPopup('Enter an email address in the correct format, like name@education.gov.uk');
        });
    });

    describe("Testing the contacts sub navigation", () => {
        beforeEach(() => {
            cy.login();
        });

        it('Should check that the contacts in dfe navigation button takes me to the correct page', () => {
            cy.visit('/trusts/contacts/in-the-trust?uid=5527');

            contactsPage
                .clickContactsInDfeSubnavButton()
                .checkContactsInDfeSubHeaderPresent();

            navigation
                .checkCurrentURLIsCorrect('/trusts/contacts/in-dfe?uid=5527');

            contactsPage
                .checkAllSubNavItemsPresent()
                .checkSfsoLeadIsPresent()
                .checkTrustRelationshipManagerIsPresent();
        });

        it('Should check that the contacts in the trust navigation button takes me to the correct page', () => {
            cy.visit('/trusts/contacts/in-dfe?uid=5527');

            contactsPage
                .clickContactsInTheTrustSubnavButton()
                .checkContactsInTheTrustSubHeaderPresent();

            navigation
                .checkCurrentURLIsCorrect('/trusts/contacts/in-the-trust?uid=5527');

            contactsPage
                .checkAllSubNavItemsPresent()
                .checkAccountingOfficerPresent()
                .checkChairOfTrusteesPresent()
                .checkChiefFinancialOfficerPresent();
        });

        it('Should check that the contacts sub nav items are not present when I am not on the contacts page', () => {
            cy.visit('/trusts/overview/trust-details?uid=5527');

            contactsPage
                .checkSubNavNotPresent();
        });

        describe("Testing a trust that has no contacts within it to ensure the issue of a 500 page appearing does not happen", () => {
            beforeEach(() => {
                cy.login();
                commonPage.interceptAndVerfiyNo500Errors();
            });

            ['/trusts/contacts/in-dfe?uid=17728', '/trusts/contacts/in-the-trust?uid=17728'].forEach((url) => {
                it(`Should have no 500 error on ${url}`, () => {
                    cy.visit(url);
                });
            });
        });
    });
});
