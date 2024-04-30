import healthPage from "cypress/pages/healthPage";

describe("Testing the health page", () => {

    beforeEach(() => {
        cy.login();
    });

    it("Can navigate to the health endpoint", () => {
        healthPage
          .isHealthy()
    });
})