import trustContactsPage from "../../../pages/trusts/trustContactsPage";
import commonPage from "../../../pages/commonPage";

describe("Testing the components of the Trust contacts page", () => {

    describe("On a Trust contacts page with data", () => {
        beforeEach(() => {
            cy.login()
            cy.visit('/trusts/contacts?uid=5712')
        });

        it("Can change Trust relationship manager contact details", () => {
            const randomNumber = Math.floor(Math.random() * 9999);
            const name = `Name${randomNumber}`
            const email = `emai${randomNumber}l@education.gov.uk`

            trustContactsPage
                .editTRM(name, email);
            trustContactsPage.elements.TrustRelationshipManager
                .Name().should('contain.text', name);
            trustContactsPage.elements.TrustRelationshipManager
                .Email().should('contain.text', email);
            commonPage
                .checkSuccessPopup('Changes made to the Trust relationship manager name and email were updated');

            cy.contains("Source and updates").click();
            commonPage
                .checkLatestDatasourceUser('Automation User - email')
        })

        it("Can change Schools financial support oversight lead contact details", () => {
            const randomNumber = Math.floor(Math.random() * 9999);
            const name = `Name${randomNumber}`
            const email = `emai${randomNumber}l@education.gov.uk`

            trustContactsPage
                .editSFSO(name, email);
            trustContactsPage.elements.SchoolsFinancialSupportOversight
                .Name().should('contain.text', name);
            trustContactsPage.elements.SchoolsFinancialSupportOversight
                .Email().should('contain.text', email);
            commonPage
                .checkSuccessPopup('Changes made to the SFSO (Schools financial support and oversight) lead name and email were updated');

            cy.contains("Source and updates").click();
            commonPage
                .checkLatestDatasourceUser('Automation User - email')
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
            const randomNumber = Math.floor(Math.random() * 9999);
            const name = `Name${randomNumber}`
            const email = `emai${randomNumber}l@education.gov.uk`
            trustContactsPage
                .editTRM(name, email);
            trustContactsPage.elements.TrustRelationshipManager
                .Name().should('contain.text', name);
            trustContactsPage.elements.TrustRelationshipManager
                .Email().should('contain.text', email);
            commonPage
                .checkSuccessPopup('Changes made to the Trust relationship manager name and email were updated');

            cy.contains("Source and updates").click();
            commonPage
                .checkLatestDatasourceUser('Automation User - email')
        })

        it("Can change Schools financial support oversight lead contact details", () => {
            const randomNumber = Math.floor(Math.random() * 9999);
            const name = `Name${randomNumber}`
            const email = `emai${randomNumber}l@education.gov.uk`
            trustContactsPage.editSFSO(name, email);
            trustContactsPage.elements.SchoolsFinancialSupportOversight
                .Name().should('contain.text', name);
            trustContactsPage.elements.SchoolsFinancialSupportOversight
                .Email().should('contain.text', email);
            commonPage
                .checkSuccessPopup('Changes made to the SFSO (Schools financial support and oversight) lead name and email were updated');

            cy.contains("Source and updates").click();
            commonPage
                .checkLatestDatasourceUser('Automation User - email')
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
            const randomNumber = Math.floor(Math.random() * 9999);
            const name = `Name${randomNumber}`
            const email = `email${randomNumber}@hotmail.co.uk`

            trustContactsPage
                .editTRM(name, email);
            commonPage
                .checkErrorPopup('Enter a DfE email address without any spaces')
            // Below line to be added in when current bug is fixed as this should be displaying but is not
            // .checkErrorPopup('Enter an email address in the correct format, like name@education.gov.uk');
        })

        it("Checks that a full non DFE email entered returns the correct error message on a SFSO ", () => {
            const randomNumber = Math.floor(Math.random() * 9999);
            const name = `Name${randomNumber}`
            const email = `email${randomNumber}@hotmail.co.uk`

            trustContactsPage
                .editSFSO(name, email);
            commonPage
                .checkErrorPopup('Enter a DfE email address without any spaces')
            // Below line to be added in when current bug is fixed as this should be displaying but is not
            // .checkErrorPopup('Enter an email address in the correct format, like name@education.gov.uk');
        })

        it("Checks that a partial email entered returns the correct error message on a TRM ", () => {
            const randomNumber = Math.floor(Math.random() * 9999);
            const name = `Name${randomNumber}`
            const email = `email${randomNumber}`

            trustContactsPage
                .editTRM(name, email);
            commonPage
                .checkErrorPopup('Enter a DfE email address without any spaces')
                .checkErrorPopup('Enter an email address in the correct format, like name@education.gov.uk');
        })

        it("Checks that a partial email entered returns the correct error message on a SFSO ", () => {
            const randomNumber = Math.floor(Math.random() * 9999);
            const name = `Name${randomNumber}`
            const email = `email${randomNumber}`

            trustContactsPage
                .editSFSO(name, email);
            commonPage
                .checkErrorPopup('Enter a DfE email address without any spaces')
                .checkErrorPopup('Enter an email address in the correct format, like name@education.gov.uk');
        })
    })
})