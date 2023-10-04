import { Locator, Page, expect } from '@playwright/test'
import { SearchFormComponent } from './shared/search-form-component'

export class HealthPage {
  readonly expect: HealthPageAssertions
  readonly searchForm: SearchFormComponent
  readonly body: Locator
  readonly _searchButtonLocator: Locator

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

  async toHaveStateMatching (state: string): Promise<void> {
    await expect(this.healthPage.body).toHaveText(state)
  }
}
