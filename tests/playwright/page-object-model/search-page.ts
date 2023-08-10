import { Locator, Page, expect } from '@playwright/test'

export class SearchPage {
  readonly expect: SearchPageAssertions
  readonly _headerLocator: Locator
  readonly _searchResultsListItemLocator: Locator

  constructor (readonly page: Page) {
    this.expect = new SearchPageAssertions(this)
    this._headerLocator = this.page.locator('h1')
    this._searchResultsListItemLocator = this.page.locator('main>>li')
  }

  async goTo (): Promise<void> {
    await this.page.goto('/search')
  }
}

class SearchPageAssertions {
  constructor (readonly searchPage: SearchPage) {}

  async toBeOnPageWithResultsFor (searchTerm: string): Promise<void> {
    await expect(this.searchPage._headerLocator).toHaveText(`Search results for "${searchTerm}"`)
  }

  async toShowResults (): Promise<void> {
    await expect(this.searchPage._searchResultsListItemLocator).not.toHaveCount(0)
  }

  async toShowEmptyResultMessage (): Promise<void> {
    await expect(this.searchPage._searchResultsListItemLocator).toHaveCount(0)
  }
}
