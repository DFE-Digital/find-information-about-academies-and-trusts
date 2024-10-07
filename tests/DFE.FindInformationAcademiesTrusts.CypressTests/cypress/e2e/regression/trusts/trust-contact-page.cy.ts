import trustContactsPage from "../../../pages/trusts/trustContactsPage";

describe("Testing the components of the Trust contacts page", () => {

    describe("On a Trust Overview page with data", () => {
        beforeEach(() => {
            cy.login()
            cy.visit('/trusts/contacts?uid=5712')
        });

        it("Can change contact", () => {
            const randomNumber = Math.floor(Math.random() * 9999);
            const name = `Name${randomNumber}`
            const email = `emai${randomNumber}l@education.gov.uk`
            trustContactsPage.editTRM(name, email);
            trustContactsPage.elements.TrustRelationshipManager.Name().should('contain.text', name);
            trustContactsPage.elements.TrustRelationshipManager.Email().should('contain.text', email);
            cy.contains("Source and updates").click();

        })

    })
})