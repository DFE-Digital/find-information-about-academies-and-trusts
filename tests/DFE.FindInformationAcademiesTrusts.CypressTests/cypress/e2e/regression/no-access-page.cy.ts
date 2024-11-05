import navigation from "../../pages/navigation";

describe("Testing the components of the no access page", () => {

    describe("As an unauthorised user", () => {

        beforeEach(() => {
            cy.login({
                name: "Unauthorised User - name",
                email: "Unauthorised User - email",
                roles: []
            });
        });

        ['/', '/search', '/notfound', '/trusts/details?uid=5712', '/trusts/contacts?uid=5143', '/trusts/governance?5527'].forEach((url) => {
            it(`Should redirect to no access page when accessing ${url}`, () => {
                cy.visit(url);

                navigation
                    .checkCurrentURLIsCorrect('no-access');

                //todo: this test might get upset if the application redirects to microsoft login
                // ideally we want to check that it tries to go to MS login and then redirects to no access page
                // with the correct return url!
                //This may not be possible though
            });
        });

        ['/cookies', '/accessibility', '/privacy'].forEach((url) => {
            it(`Should be able to go to ${url}`, () => {
                cy.visit(url);

                navigation
                    .checkCurrentURLIsCorrect(url);
            });
        });
    });

    describe("As an authorised user", () => {
        beforeEach(() => {
            cy.login();
        });

        ['/', '/search', '/notfound', '/trusts/details?uid=5712', '/trusts/contacts?uid=5143', '/trusts/governance?5527', '/cookies', '/accessibility', '/privacy'].forEach((url) => {
            it(`Should be able to go to ${url}`, () => {
                cy.visit(url);

                navigation
                    .checkCurrentURLIsCorrect(url);
            });
        });
    });

    //Todo: Other tests - when not authenticated - no breadcrumb or search box or feedback footer section
});
