import contactsPage from "../../../pages/trusts/contactsPage";
import commonPage from "../../../pages/commonPage";

function generateNameAndEmail() {
    const randomNumber = Math.floor(Math.random() * 9999);
    return {
        name: `Name${randomNumber}`,
        email: `email${randomNumber}@education.gov.uk`
    };
}

describe("Testing the components of the Trust contacts page", () => {

    describe("On a Trust contacts page with data", () => {
        beforeEach(() => {
            cy.login();
        });

        ['/trusts/contacts?uid=5712', '/trusts/contacts?uid=5527'].forEach((url) => {
            it(`Can change Trust relationship manager contact details ${url}`, () => {
                const { name, email } = generateNameAndEmail();
                cy.visit(url, { failOnStatusCode: false });
                contactsPage
                    .checkTRMFieldsAndDatasource(name, email);
            });

            it(`Can change Schools financial support oversight lead contact details ${url}`, () => {
                const { name, email } = generateNameAndEmail();
                cy.visit(url, { failOnStatusCode: false });
                contactsPage
                    .checkSFSOFieldsAndDatasource(name, email);
            });

            it(`Checks a trusts external contact details ${url}`, () => {
                cy.visit(url, { failOnStatusCode: false });
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
            cy.visit('/trusts/contacts?uid=5527');
        });

        it("Checks that a full non DFE email entered returns the correct error message on a TRM ", () => {
            contactsPage
                .editTRM("Name", "email@hotmail.co.uk");
            commonPage
                .checkErrorPopup('Enter a DfE email address without any spaces');
        });

        it("Checks that an incorrect email entered returns the correct error message on a TRM ", () => {
            contactsPage
                .editTRM("Name", "email");
            commonPage
                .checkErrorPopup('Enter an email address in the correct format, like name@education.gov.uk');
        });

        it("Checks that illegal characters entered returns the correct error message on a TRM ", () => {
            contactsPage
                .editTRM("Name", "@£$$^&");
            commonPage
                .checkErrorPopup('Enter an email address in the correct format, like name@education.gov.uk');
        });

        it("Checks that whitespace entered returns the correct error message on a TRM ", () => {
            contactsPage
                .editTRM("Name", "a     b");
            commonPage
                .checkErrorPopup('Enter a DfE email address without any spaces')
                .checkErrorPopup('Enter an email address in the correct format, like name@education.gov.uk');
        });

        it("Checks that an email address without the prefix entered returns the correct error message on a TRM ", () => {
            contactsPage
                .editTRM("Name", "@education.gov.uk");
            commonPage
                .checkErrorPopup('Enter an email address in the correct format, like name@education.gov.uk');
        });

        it("Checks that a full non DFE email entered returns the correct error message on a SFSO ", () => {
            contactsPage
                .editSFSO("Name", "email@hotmail.co.uk");

            commonPage
                .checkErrorPopup('Enter a DfE email address without any spaces');
        });

        it("Checks that an incorrect email entered returns the correct error message on a SFSO ", () => {
            contactsPage
                .editSFSO("Name", "email");
            commonPage
                .checkErrorPopup('Enter a DfE email address without any spaces')
                .checkErrorPopup('Enter an email address in the correct format, like name@education.gov.uk');
        });

        it("Checks that illegal characters entered returns the correct error message on a SFSO ", () => {
            contactsPage
                .editSFSO("Name", "@£$$^&");
            commonPage
                .checkErrorPopup('Enter a DfE email address without any spaces')
                .checkErrorPopup('Enter an email address in the correct format, like name@education.gov.uk');
        });

        it("Checks that whitespace entered returns the correct error message on a SFSO ", () => {
            contactsPage
                .editSFSO("Name", "a     b");
            commonPage
                .checkErrorPopup('Enter a DfE email address without any spaces')
                .checkErrorPopup('Enter an email address in the correct format, like name@education.gov.uk');
        });

        it("Checks that an email address without the prefix entered returns the correct error message on a SFSO ", () => {
            contactsPage
                .editSFSO("Name", "@education.gov.uk");
            commonPage
                .checkErrorPopup('Enter an email address in the correct format, like name@education.gov.uk');
        });
    });
});
