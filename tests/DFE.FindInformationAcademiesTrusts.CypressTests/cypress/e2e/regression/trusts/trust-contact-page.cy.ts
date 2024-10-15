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

        it("Checks that an invalid email entered returns the correct error message ", () => {
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
})