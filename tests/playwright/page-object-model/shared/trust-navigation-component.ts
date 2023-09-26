import { Locator, Page, expect } from '@playwright/test'

export class TrustNavigationComponent {
  readonly expect: TrustNavigationComponentAssertions
  readonly locator: Locator

  constructor (readonly page: Page) {
    this.expect = new TrustNavigationComponentAssertions(this)
    this.locator = page.getByRole('navigation', { name: 'Sections' })
  }

  async clickOn (item: string): Promise<void> {
    await this.locator.getByRole('link', { name: item }).click()
  }
}

class TrustNavigationComponentAssertions {
  constructor (readonly trustNavigation: TrustNavigationComponent) {}

  async toBeVisible (): Promise<void> {
    await expect(this.trustNavigation.locator).toContainText('About the trust')
  }
}
