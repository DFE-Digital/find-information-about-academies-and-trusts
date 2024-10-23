import commonPage from "../../pages/commonPage";
import cookiesPage from "../../pages/cookiesPage";
import navigation from "../../pages/navigation";

describe('Cookie page and consent tests', () => {

    beforeEach(() => {
        cy.login()
        cy.visit('/cookies')
    });

    it('should check that both mandatory and optional cookies both exist after accepting optional cookies', () => {
        cookiesPage
            .acceptCookies()
            .clickSaveChangesButton()

        commonPage
            .checkSuccessPopup('You’ve set your cookie preferences')

        cy.visit('/trusts/details?uid=5712')

        //check optional cookies exist
        cy.getCookie('ai_user').should('exist')
        cy.getCookie('ai_session').should('exist')
        cy.getCookie('ai_authUser').should('exist')
        cy.getCookie('_ga').should('exist')

        //check mandatory cookies exist after saving
        cy.getCookie('.FindInformationAcademiesTrust.CookieConsent').should('exist')
        cy.getCookie('ASLBSA').should('exist')
        cy.getCookie('ASLBSACORS').should('exist')
        cy.getCookie('.AspNetCore.Antiforgery.VyLW6ORzMgk').should('exist')

    });

    it('should check that mandatory cookies exist but otional cookies do not after rejecting optional cookies', () => {
        cookiesPage
            .rejectCookies()
            .clickSaveChangesButton()

        commonPage
            .checkSuccessPopup('You’ve set your cookie preferences')

        cy.visit('/trusts/details?uid=5712')

        //check optional cookies exist
        cy.getCookie('ai_user').should('not.exist')
        cy.getCookie('ai_session').should('not.exist')
        cy.getCookie('ai_authUser').should('not.exist')
        cy.getCookie('_ga').should('not.exist')

        //check mandatory cookies exist after saving
        cy.getCookie('.FindInformationAcademiesTrust.CookieConsent').should('exist')
        cy.getCookie('ASLBSA').should('exist')
        cy.getCookie('ASLBSACORS').should('exist')
        cy.getCookie('.AspNetCore.Antiforgery.VyLW6ORzMgk').should('exist')
    });

    it('should check that the return to previous page button actually takes me to my previous page after accept cookies', () => {
        cy.visit('/search')

        navigation
            .clickCookiesLink()
            .checkCurrentURLIsCorrect('/cookies')

        cookiesPage
            .acceptCookies()
            .clickSaveChangesButton()
            .clickReturnToPreviousPageButton()

        navigation
            .checkCurrentURLIsCorrect('/search')
    });

    it('should check that the return to previous page button actually takes me to my previous page after reject cookies', () => {
        cy.visit('/trusts/contacts?uid=5712')

        navigation
            .clickCookiesLink()
            .checkCurrentURLIsCorrect('/cookies')

        cookiesPage
            .rejectCookies()
            .clickSaveChangesButton()
            .clickReturnToPreviousPageButton()

        navigation
            .checkCurrentURLIsCorrect('/trusts/contacts?uid=5712')
    });
});
