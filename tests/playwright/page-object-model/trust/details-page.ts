import { Locator, Page, expect } from '@playwright/test'

export class DetailsPage {
  readonly expect: DetailsPageAssertions
  readonly _trustHeadingLocator: Locator

  constructor (readonly page: Page) {
    this.expect = new DetailsPageAssertions(this)
    this._trustHeadingLocator = this.page.locator('h1')
  }
}

class DetailsPageAssertions {
  constructor (readonly detailsPage: DetailsPage) {}

  async toBeOnTheRightPageFor (trust: string): Promise<void> {
    await await expect(this.detailsPage._trustHeadingLocator).toHaveText(trust)
  }
}
