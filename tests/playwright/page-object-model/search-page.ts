import { Locator, Page, expect } from '@playwright/test'

export class SearchPage {
  readonly expect: SearchPageAssertions
  readonly _headerLocator: Locator
  readonly _searchResultsListHeaderLocator: Locator
  readonly _searchResultsListItemLocator: Locator

  constructor (readonly page: Page) {
    this.expect = new SearchPageAssertions(this)
    this._headerLocator = this.page.locator('h1')
    this._searchResultsListHeaderLocator = this.page.getByRole('heading', {
      name: 'results for'
    })
    this._searchResultsListItemLocator = this.page.locator('main>>li')
  }

  async goTo (): Promise<void> {
    await this.page.goto('/search')
  }
}

class SearchPageAssertions {
  constructor (readonly searchPage: SearchPage) {}

  async toBeOnPageWithResultsFor (searchTerm: string): Promise<void> {
    await expect(this.searchPage._searchResultsListHeaderLocator).toContainText(`results for "${searchTerm}"`)
  }

  async toBeOnTheRightPage (): Promise<void> {
    await expect(this.searchPage._headerLocator).toHaveText('Search')
  }

  async toShowResults (): Promise<void> {
    await expect(this.searchPage._searchResultsListItemLocator).not.toHaveCount(0)
  }

  async toShowEmptyResultMessage (): Promise<void> {
    await expect(this.searchPage._searchResultsListItemLocator).toHaveCount(0)
  }
}
