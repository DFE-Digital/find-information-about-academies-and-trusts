import { Locator, Page, expect } from '@playwright/test'
export class HealthPage {
  readonly expect: HealthPageAssertions
  readonly body: Locator

  constructor (readonly page: Page) {
    this.expect = new HealthPageAssertions(this)
    this.body = page.locator('body')
  }

  async goTo (): Promise<void> {
    await this.page.goto('/health')
  }
}

class HealthPageAssertions {
  constructor (readonly healthPage: HealthPage) { }

  async toBeHealthy (): Promise<void> {
    await expect(this.healthPage.body).toHaveText('Healthy')
  }
}
