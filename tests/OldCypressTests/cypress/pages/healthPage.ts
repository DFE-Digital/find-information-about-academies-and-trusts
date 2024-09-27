class HealthPage {

  isHealthy(): this {

    cy.request('/health').its('body').should('include', 'Healthy')

    return this
  }
}

const healthPage = new HealthPage();

export default healthPage;