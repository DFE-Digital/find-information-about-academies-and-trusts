import trustContactsPage from "../../../pages/trusts/trustContactsPage";
import commonPage from "../../../pages/commonPage";

function generateNameAndEmail() {
    const randomNumber = Math.floor(Math.random() * 9999);
    return {
        name: `Name${randomNumber}`,
        email: `email${randomNumber}@education.gov.uk`
    }
}

describe("Testing the components of the Trust contacts page", () => {

    describe("On a Trust contacts page with data", () => {
        beforeEach(() => {
            cy.login()
            cy.visit('/trusts/contacts?uid=5712')
        });

        it("Can change Trust relationship manager contact details", () => {
            const { name, email } = generateNameAndEmail();

            trustContactsPage
                .checkTRMFieldsAndDatasource(name, email)

        })

        it("Can change Schools financial support oversight lead contact details", () => {
            const { name, email } = generateNameAndEmail();

            trustContactsPage
                .checkSFSOFieldsAndDatasource(name, email)
        })

        it("Checks a trusts external contact details", () => {
            trustContactsPage
                .checkAccountingOfficerPresent()
                .checkChairOfTrusteesPresent()
                .checkChiefFinancialOfficerPresent()
        })

    })

    describe("On a different Trusts contacts page with data", () => {
        beforeEach(() => {
            cy.login()
            cy.visit('/trusts/contacts?uid=5527')
        });

        it("Can change Trust relationship manager contact details", () => {
            const { name, email } = generateNameAndEmail();

            trustContactsPage
                .checkTRMFieldsAndDatasource(name, email)
        })

        it("Can change Schools financial support oversight lead contact details", () => {
            const { name, email } = generateNameAndEmail();

            trustContactsPage
                .checkSFSOFieldsAndDatasource(name, email)

        })

        it("Checks a different trusts external contact details", () => {
            trustContactsPage
                .checkAccountingOfficerPresent()
                .checkChairOfTrusteesPresent()
                .checkChiefFinancialOfficerPresent()
        })

    })

    describe('Checks the update error handling', () => {
        beforeEach(() => {
            cy.login()
            cy.visit('/trusts/contacts?uid=5527')
        });

        it("Checks that a full non DFE email entered returns the correct error message on a TRM ", () => {
            trustContactsPage
                .editTRM("Name", "email@hotmail.co.uk");
            commonPage
                .checkErrorPopup('Enter a DfE email address without any spaces')
        })

        it("Checks that an incorrect email entered returns the correct error message on a TRM ", () => {
            trustContactsPage
                .editTRM("Name", "email");
            commonPage
                .checkErrorPopup('Enter an email address in the correct format, like name@education.gov.uk');
        })

        it("Checks that illegal characters entered returns the correct error message on a TRM ", () => {
            trustContactsPage
                .editTRM("Name", "@£$$^&");
            commonPage
                .checkErrorPopup('Enter an email address in the correct format, like name@education.gov.uk');
        })

        it("Checks that whitespace entered returns the correct error message on a TRM ", () => {
            trustContactsPage
                .editTRM("Name", "a     b");
            commonPage
                .checkErrorPopup('Enter a DfE email address without any spaces')
                .checkErrorPopup('Enter an email address in the correct format, like name@education.gov.uk');
        })

        it("Checks that a full non DFE email entered returns the correct error message on a SFSO ", () => {
            trustContactsPage
                .editSFSO("Name", "email@hotmail.co.uk");

            commonPage
                .checkErrorPopup('Enter a DfE email address without any spaces')
        })

        it("Checks that an incorrect email entered returns the correct error message on a SFSO ", () => {
            trustContactsPage
                .editSFSO("Name", "email");
            commonPage
                .checkErrorPopup('Enter a DfE email address without any spaces')
                .checkErrorPopup('Enter an email address in the correct format, like name@education.gov.uk');
        })

        it("Checks that illegal characters entered returns the correct error message on a SFSO ", () => {
            trustContactsPage
                .editSFSO("Name", "@£$$^&");
            commonPage
                .checkErrorPopup('Enter a DfE email address without any spaces')
                .checkErrorPopup('Enter an email address in the correct format, like name@education.gov.uk');
        })

        it("Checks that whitespace entered returns the correct error message on a SFSO ", () => {
            trustContactsPage
                .editSFSO("Name", "a     b");
            commonPage
                .checkErrorPopup('Enter a DfE email address without any spaces')
                .checkErrorPopup('Enter an email address in the correct format, like name@education.gov.uk');
        })

    })
})
