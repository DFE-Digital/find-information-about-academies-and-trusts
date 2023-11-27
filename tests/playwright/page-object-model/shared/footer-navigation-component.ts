import { Locator, Page, expect } from '@playwright/test'

export class FooterNavigationComponent {
  readonly expect: FooterNavigationComponentAssertions
  readonly locator: Locator
  readonly privacyPolicyLinkLocator: Locator
  readonly cookiesPageLinkLocator: Locator
  constructor (readonly page: Page) {
    this.expect = new FooterNavigationComponentAssertions(this)
    this.privacyPolicyLinkLocator = page.getByRole('link', { name: 'Privacy' })
    this.cookiesPageLinkLocator = page.getByRole('link', { name: 'Cookies' })
  }

  async clickPrivacyPolicy (): Promise<void> {
    await this.privacyPolicyLinkLocator.click()
  }

  async clickCookies (): Promise<void> {
    await this.cookiesPageLinkLocator.click()
  }
}
class FooterNavigationComponentAssertions {
  constructor (readonly footerNavigation: FooterNavigationComponent) {
  }

  async isVisible (): Promise<void> {
    await expect(this.footerNavigation.locator).toContainText('All content is available under the Open Government Licence v3.0, except where otherwise stated')
  }
}
