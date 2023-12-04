import { Locator, Page, expect } from '@playwright/test'

export class CookieBannerComponent {
  readonly expect: CookieBannerNavigationComponentAssertions
  readonly banner: Locator
  readonly acceptCookiesLocator: Locator
  readonly rejectCookiesLocator: Locator
  readonly cookiePageLinkLocator: Locator
  readonly cookieAcceptedBanner: Locator
  readonly cookieRejectedBanner: Locator

  constructor (readonly page: Page) {
    this.expect = new CookieBannerNavigationComponentAssertions(this)
    this.banner = page.getByLabel('Cookies on Find information about academies and trusts')
    this.acceptCookiesLocator = this.banner.getByRole('button', { name: 'Accept analytics cookies' })
    this.rejectCookiesLocator = this.banner.getByRole('button', { name: 'Reject analytics cookies' })
    this.cookiePageLinkLocator = this.banner.getByRole('link', { name: 'View cookies' })
    this.cookieAcceptedBanner = this.banner.getByText('You\u2019ve accepted additional cookies')
    this.cookieRejectedBanner = this.banner.getByText('You\u2019ve rejected additional cookies')
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
    await expect(this.cookieBannerNavigation.acceptCookiesLocator).not.toBeVisible()
  }

  async isAccepted (): Promise<void> {
    await expect(this.cookieBannerNavigation.cookieAcceptedBanner).toBeVisible()
  }

  async isRejected (): Promise<void> {
    await expect(this.cookieBannerNavigation.cookieRejectedBanner).toBeVisible()
  }
}
