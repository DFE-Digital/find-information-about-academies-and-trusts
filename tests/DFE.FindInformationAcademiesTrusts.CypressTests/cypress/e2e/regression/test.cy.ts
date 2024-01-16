describe("Starting a basic test", () =>
{
    beforeEach(() =>
    {
        cy.login();
    });

    it("Should be able to get to a homepage", () =>
    {
        cy.visit("/");
    });
});