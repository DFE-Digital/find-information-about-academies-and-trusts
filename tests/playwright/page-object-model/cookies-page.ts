import { Locator, Page, expect } from '@playwright/test'
import { BasePage, BasePageAssertions } from './base-page'

export class CookiesPage extends BasePage {
  readonly expect: CookiesPageAssertions
  readonly acceptRadioButtonLocator: Locator
  readonly rejectRadioButtonLocator: Locator
  readonly saveChangesButtonLocator: Locator
  readonly returnToPageLocator: Locator

  constructor (page: Page) {
    super(page, '/cookies', 'Cookies - Find information about academies and trusts', 'Cookie preferences')
    this.expect = new CookiesPageAssertions(this)

    this.acceptRadioButtonLocator = page.getByLabel('Yes')
    this.rejectRadioButtonLocator = page.getByLabel('No')
    this.saveChangesButtonLocator = page.getByRole('button', { name: 'Save changes' })
    this.returnToPageLocator = page.getByRole('link', { name: 'Go back to the page you were looking at' })
  }

  async acceptCookies (): Promise<void> {
    await this.acceptRadioButtonLocator.check()
    await this.saveChangesButtonLocator.click()
  }

  async rejectCookies (): Promise<void> {
    await this.rejectRadioButtonLocator.check()
    await this.saveChangesButtonLocator.click()
  }

  async returnToPreviousPageViaLink (): Promise<void> {
    await this.returnToPageLocator.click()
  }
}
class CookiesPageAssertions extends BasePageAssertions {
  constructor (readonly cookiesPage: CookiesPage) {
    super(cookiesPage)
  }

  async returnToLinkPageToBeVisible (): Promise<void> {
    await expect(this.cookiesPage.returnToPageLocator).toBeVisible()
  }

  async returnToLinkPageToNotBeVisible (): Promise<void> {
    await expect(this.cookiesPage.returnToPageLocator).toHaveCount(0)
  }

  async acceptCookiesRadioButtonToBeChecked (): Promise<void> {
    await expect(this.cookiesPage.acceptRadioButtonLocator).toBeChecked()
  }

  async rejectCookiesRadioButtonToBeChecked (): Promise<void> {
    await expect(this.cookiesPage.rejectRadioButtonLocator).toBeChecked()
  }

  async acceptCookiesRadioButtonNotToBeChecked (): Promise<void> {
    await expect(this.cookiesPage.acceptRadioButtonLocator).not.toBeChecked()
  }

  async rejectCookiesRadioButtonNotToBeChecked (): Promise<void> {
    await expect(this.cookiesPage.rejectRadioButtonLocator).not.toBeChecked()
  }
}
