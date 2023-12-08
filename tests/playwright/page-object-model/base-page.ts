import { Locator, Page, expect } from '@playwright/test'
import { FooterNavigationComponent } from '../page-object-model/shared/footer-navigation-component'
import { CookieBannerComponent } from '../page-object-model/shared/cookie-banner-component'

export class BasePage {
  readonly footerNavigation: FooterNavigationComponent
  readonly cookieBanner: CookieBannerComponent
  readonly expect: BasePageAssertions
  readonly pageHeadingLocator: Locator

  constructor (readonly page: Page, readonly pageUrl: string, readonly browserPageTitle: string, readonly pageHeading: string) {
    this.expect = new BasePageAssertions(this)
    this.footerNavigation = new FooterNavigationComponent(page)
    this.cookieBanner = new CookieBannerComponent(page)

    this.pageHeadingLocator = page.locator('h1')
  }

  async goTo (): Promise<void> {
    await this.page.goto(this.pageUrl)
  }
}

export class BasePageAssertions {
  constructor (readonly basePage: BasePage) { }

  async toBeOnTheRightPage (): Promise<void> {
    await expect(this.basePage.page).toHaveTitle(this.basePage.browserPageTitle)
    await expect(this.basePage.pageHeadingLocator).toHaveText(this.basePage.pageHeading)
  }

  async notToBeOnThePage (): Promise<void> {
    await expect(this.basePage.page).not.toHaveTitle(this.basePage.browserPageTitle)
  }

  async toHaveAppInsightCookies (): Promise<void> {
    await expect(async () => expect((await this.basePage.page.context().cookies()).filter(cookie => cookie.name === 'ai_user' || cookie.name === 'ai_session')).toHaveLength(2)).toPass({
      timeout: 10_000
    })
  }

  async notToHaveAppInsightsCookies (): Promise<void> {
    await expect(async () => expect((await this.basePage.page.context().cookies()).filter(cookie => cookie.name === 'ai_user' || cookie.name === 'ai_session')).toHaveLength(0)).toPass({
      timeout: 10_000
    })
  }
}
