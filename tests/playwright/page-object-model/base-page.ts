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

  async reload (): Promise<void> {
    await this.page.reload()
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
    await expect(
      async () => {
        await this.basePage.reload() // Cookie settings may not apply until refresh of page due to server side order of setting/using cookie preferences cookie. Also cookies assertions are flakey
        const allCookies = await this.basePage.page.context().cookies()
        const appInsightCookies = allCookies.filter(cookie => cookie.name === 'ai_user' || cookie.name === 'ai_session')
        expect(appInsightCookies).toHaveLength(2)
      })
      .toPass({
        timeout: 10_000
      })
  }

  async notToHaveAppInsightsCookies (): Promise<void> {
    await expect(
      async () => {
        await this.basePage.reload() // Cookie settings may not apply until refresh of page due to server side order of setting/using cookie preferences cookie. Also cookies assertions are flakey
        const allCookies = await this.basePage.page.context().cookies()
        const appInsightCookies = allCookies.filter(cookie => cookie.name === 'ai_user' || cookie.name === 'ai_session')
        expect(appInsightCookies).toHaveLength(0)
      })
      .toPass({
        timeout: 10_000
      })
  }
}
