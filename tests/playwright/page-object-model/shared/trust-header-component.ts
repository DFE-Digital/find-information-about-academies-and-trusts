import { Locator, Page } from '@playwright/test'

export class TrustHeaderComponent {
  readonly locator: Locator

  constructor (readonly page: Page) {
    this.locator = page.getByTestId('app-trust-header')
  }
}
