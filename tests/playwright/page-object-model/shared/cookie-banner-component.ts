import { Locator, Page, expect } from '@playwright/test'

export class CookieBannerComponent {
  readonly expect: CookieBannerComponentAssertions
  readonly banner: Locator
  readonly acceptCookiesLocator: Locator
  readonly rejectCookiesLocator: Locator
  readonly cookiePageLinkLocator: Locator
  readonly cookieAcceptedBanner: Locator
  readonly cookieRejectedBanner: Locator

  constructor (readonly page: Page) {
    this.expect = new CookieBannerComponentAssertions(this)
    this.banner = page.getByLabel('Cookies on Find information about academies and trusts')
    this.acceptCookiesLocator = this.banner.getByRole('button', { name: 'Accept analytics cookies' })
    this.rejectCookiesLocator = this.banner.getByRole('button', { name: 'Reject analytics cookies' })
    this.cookiePageLinkLocator = this.banner.getByRole('link', { name: 'View cookies' })
    this.cookieAcceptedBanner = this.banner.getByText('You\u2019ve accepted additional cookies')
    this.cookieRejectedBanner = this.banner.getByText('You\u2019ve rejected additional cookies')
  }

  async acceptCookies (): Promise<void> {
    await this.acceptCookiesLocator.click()
  }

  async rejectCookies (): Promise<void> {
    await this.rejectCookiesLocator.click()
  }

  async goToCookiesPage (): Promise<void> {
    await this.cookiePageLinkLocator.click()
  }
}

class CookieBannerComponentAssertions {
  constructor (readonly cookieBanner: CookieBannerComponent) {
  }

  async toAskForCookiePreferences (): Promise<void> {
    await expect(this.cookieBanner.acceptCookiesLocator).toBeVisible()
  }

  async notToAskForCookiePreferences (): Promise<void> {
    await expect(this.cookieBanner.acceptCookiesLocator).not.toBeVisible()
  }

  async toShowCookiesAcceptedMessage (): Promise<void> {
    await expect(this.cookieBanner.cookieAcceptedBanner).toBeVisible()
  }

  async toShowCookiesRejectedMessage (): Promise<void> {
    await expect(this.cookieBanner.cookieRejectedBanner).toBeVisible()
  }
}
