import { Locator, Page, expect } from '@playwright/test'

export class NavigationComponent {
  readonly expect: NavigationComponentAssertions
  readonly locator: Locator

  constructor (readonly page: Page, name: string) {
    this.expect = new NavigationComponentAssertions(this)
    this.locator = page.getByRole('navigation', { name })
  }

  async clickOn (item: string): Promise<void> {
    await this.locator.getByRole('link', { name: item }).click()
  }
}

class NavigationComponentAssertions {
  constructor (readonly trustNavigation: NavigationComponent) {}

  async toBeVisible (): Promise<void> {
    await expect(this.trustNavigation.locator).toBeVisible()
  }
}
