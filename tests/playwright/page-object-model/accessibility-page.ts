import { Locator, Page, expect } from '@playwright/test'
export class AccessibilityPage {
  readonly expect: AccessibilityPageAssertions
  readonly body: Locator

  constructor (readonly page: Page) {
    this.expect = new AccessibilityPageAssertions(this)
    this.body = page.locator('body')
  }

  async goTo (): Promise<void> {
    await this.page.goto('/accessibility')
  }
}

class AccessibilityPageAssertions {
  constructor (readonly privacyPage: AccessibilityPage) { }
  async toBeOnTheRightPage (): Promise<void> {
    await expect(this.privacyPage.body).toContainText('This accessibility statement applies to content published on Find information about academies and trusts.')
  }
}
