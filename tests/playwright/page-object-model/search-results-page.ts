import { Locator, Page, expect } from '@playwright/test'

export class SearchResultsPage {
  readonly expect: SearchResultsPageAssertions
  readonly _headerLocator: Locator
  constructor (readonly page: Page) {
    this.expect = new SearchResultsPageAssertions(this)
    this._headerLocator = this.page.locator('h1')
  }
}

class SearchResultsPageAssertions {
  constructor (readonly searchResultsPage: SearchResultsPage) {}

  async toBeOnPageWithResultsFor (searchTerm: string): Promise<void> {
    await expect(this.searchResultsPage._headerLocator).toHaveText(`Search results for "${searchTerm}"`)
  }
}
