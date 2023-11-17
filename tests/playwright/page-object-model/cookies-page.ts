import { Locator, Page, expect } from '@playwright/test'
export class CookiesPage {
  readonly expect: CookiesPageAssertions
  readonly body: Locator
  readonly acceptRadioButtonLocator: Locator
  readonly rejectRadioButtonLocator: Locator
  readonly saveChangesButtonLocator: Locator
  readonly returnToPageLocator: Locator

  constructor (readonly page: Page) {
    this.expect = new CookiesPageAssertions(this)
    this.body = page.locator('body')
    this.acceptRadioButtonLocator = page.getByLabel('Yes')
    this.rejectRadioButtonLocator = page.getByLabel('No')
    this.saveChangesButtonLocator = page.getByRole('button', { name: 'Save changes' })
    this.returnToPageLocator = page.locator('[data-test="success-banner-return-link"]')
  }

  async goTo (): Promise<void> {
    await this.page.goto('/privacy')
  }

  async acceptCookies (): Promise<void> {
    await this.acceptRadioButtonLocator.check()
    await this.saveChangesButtonLocator.click()
  }

  async rejectCookies (): Promise<void> {
    await this.rejectRadioButtonLocator.check()
    await this.saveChangesButtonLocator.click()
  }

  async clickBackToHomePage (): Promise<void> {
    await this.returnToPageLocator.click()
  }
}
class CookiesPageAssertions {
  constructor (readonly cookiesPage: CookiesPage) { }
  async toBeOnTheRightPage (): Promise<void> {
    await expect(this.cookiesPage.body).toContainText('Essential cookies')
  }

  async returnToLinkPageToBeVisble (): Promise<void> {
    await expect(this.cookiesPage.returnToPageLocator).toBeVisible()
  }

  async returnToLinkPageToNotBeVisble (): Promise<void> {
    await expect(this.cookiesPage.returnToPageLocator).toHaveCount(0)
  }

  async acceptCookiesRadioButtonIsChecked (): Promise<void> {
    await expect(this.cookiesPage.acceptRadioButtonLocator).toBeChecked()
  }

  async rejectCookiesRadioButtonIsChecked (): Promise<void> {
    await expect(this.cookiesPage.rejectRadioButtonLocator).toBeChecked()
  }
}
