import { Locator, Page, expect } from '@playwright/test'

export class NotFoundPage {
  readonly expect: NotFoundPageAssertions
  readonly _headerLocator: Locator

  constructor (readonly page: Page) {
    this.expect = new NotFoundPageAssertions(this)
    this._headerLocator = this.page.locator('h1')
  }

  async goToNonExistingUrl (): Promise<void> {
    await this.page.goto('/non-page')
  }
}

class NotFoundPageAssertions {
  constructor (readonly notFoundPage: NotFoundPage) {}

  async toBeShownNotFoundMessage (): Promise<void> {
    await expect(this.notFoundPage._headerLocator).toHaveText('Page not found')
  }
}
