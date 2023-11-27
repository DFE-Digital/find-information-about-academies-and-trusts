import { Locator, Page, expect } from '@playwright/test'

export class CookieBannerComponent {
  readonly expect: CookieBannerNavigationComponentAssertions
  readonly acceptCookiesLocator: Locator
  readonly rejectCookiesLocator: Locator
  readonly cookiePageLinkLocator: Locator

  constructor (readonly page: Page) {
    this.expect = new CookieBannerNavigationComponentAssertions(this)
    this.acceptCookiesLocator = page.getByRole('button', { name: 'Accept analytics cookies' })
    this.rejectCookiesLocator = page.getByRole('button', { name: 'Reject analytics cookies' })
    this.cookiePageLinkLocator = page.getByRole('link', { name: 'set your cookie preferences' })
  }

  async clickAcceptCookies (): Promise<void> {
    await this.acceptCookiesLocator.click()
  }

  async clickRejectCookies (): Promise<void> {
    await this.rejectCookiesLocator.click()
  }

  async clickCookiesPage (): Promise<void> {
    await this.cookiePageLinkLocator.click()
  }
}

class CookieBannerNavigationComponentAssertions {
  constructor (readonly cookieBannerNavigation: CookieBannerComponent) {
  }

  async isVisible (): Promise<void> {
    await expect(this.cookieBannerNavigation.acceptCookiesLocator).toBeVisible()
  }

  async isNotVisible (): Promise<void> {
    await expect(this.cookieBannerNavigation.acceptCookiesLocator).toHaveCount(0)
  }
}