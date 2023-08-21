import { Locator, Page, expect } from '@playwright/test'

export class SearchPage {
  readonly expect: SearchPageAssertions
  readonly _headerLocator: Locator
  readonly _searchResultsListHeaderLocator: Locator
  readonly _searchResultsListItemLocator: Locator
  readonly _searchInputLocator: Locator
  readonly _searchButtonLocator: Locator

  constructor (readonly page: Page) {
    const searchResultsHeadingName = 'results for'

    this.expect = new SearchPageAssertions(this)
    this._headerLocator = this.page.locator('h1')
    this._searchResultsListHeaderLocator = this.page.getByRole('heading', {
      name: searchResultsHeadingName
    })
    this._searchResultsListItemLocator = this.page.getByLabel(searchResultsHeadingName).getByRole('listitem')
    this._searchInputLocator = this.page.getByLabel('Search')
    this._searchButtonLocator = this.page.getByRole('button', { name: 'Search' })
  }

  readonly expectedSearchResults = [
    { name: 'trust 1', address: '12 Paddle Road, Bushy Park, Letworth, Manchester, MX12 P34', ukprn: '123', academiesInTrustCount: 1 },
    { name: 'trust 2', address: '12 Paddle Road, Manchester, MX12 P34', ukprn: '124', academiesInTrustCount: 2 },
    { name: 'trust 3', address: 'Bushy Park, Manchester', ukprn: '125', academiesInTrustCount: 0 }
  ]

  async goTo (): Promise<void> {
    await this.page.goto('/search')
  }

  async goToSearchFor (keywords: string): Promise<void> {
    await this.page.goto(`/search/?keywords=${keywords}`)
  }

  getListItemLocatorByText (text: string): Locator {
    return this._searchResultsListItemLocator.filter({ hasText: text })
  }

  async clickOnSearchResultLinkWithText (text: string): Promise<void> {
    await this._searchResultsListItemLocator.getByRole('link', { name: text }).click()
  }

  async searchForTerm (text: string): Promise<void> {
    await this._searchInputLocator.fill(text)
    await this._searchButtonLocator.click()
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

  async toDisplayNumberOfResultsFound (): Promise<void> {
    await expect(this.searchPage._searchResultsListHeaderLocator).toContainText(`${this.searchPage.expectedSearchResults.length} results for`)
  }

  async toSeeInformationForEachResult (): Promise<void> {
    await expect(this.searchPage._searchResultsListItemLocator).toHaveCount(this.searchPage.expectedSearchResults.length)

    for (const searchResultItem of this.searchPage.expectedSearchResults) {
      const searchItemLocator = this.searchPage.getListItemLocatorByText(searchResultItem.name)
      await expect(searchItemLocator).toBeVisible()

      await expect(searchItemLocator).toContainText(`Address: ${searchResultItem.address}`)
      await expect(searchItemLocator).toContainText(`Academies in this trust: ${searchResultItem.academiesInTrustCount}`)
      await expect(searchItemLocator).toContainText(`UKPRN: ${searchResultItem.ukprn}`)
    }
  }

  async toSeeSearchInputContainingSearchTerm (text: string): Promise<void> {
    await expect(this.searchPage._searchInputLocator).toHaveValue(text)
  }
}
