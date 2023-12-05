import { Locator, Page, expect } from '@playwright/test'

export class FooterNavigationComponent {
  readonly expect: FooterNavigationComponentAssertions
  readonly locator: Locator
  readonly privacyPolicyLinkLocator: Locator
  readonly cookiesPageLinkLocator: Locator

  constructor (readonly page: Page) {
    this.expect = new FooterNavigationComponentAssertions(this)
    this.locator = page.locator('footer')
    this.privacyPolicyLinkLocator = this.locator.getByRole('link', { name: 'Privacy' })
    this.cookiesPageLinkLocator = this.locator.getByRole('link', { name: 'Cookies' })
  }

  async goToPrivacyPolicy (): Promise<void> {
    await this.privacyPolicyLinkLocator.click()
  }

  async goToCookies (): Promise<void> {
    await this.cookiesPageLinkLocator.click()
  }
}
class FooterNavigationComponentAssertions {
  constructor (readonly footerNavigation: FooterNavigationComponent) {
  }

  async toBeVisible (): Promise<void> {
    await expect(this.footerNavigation.locator).toContainText('All content is available under the Open Government Licence v3.0, except where otherwise stated')
  }
}
