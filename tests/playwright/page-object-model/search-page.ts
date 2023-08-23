import { Locator, Page, expect } from '@playwright/test'

interface expectedResult {
  name: string
  address: string
  ukprn: string
  academiesInTrustCount: number
}

export class SearchPage {
  readonly expect: SearchPageAssertions
  readonly _headerLocator: Locator
  readonly _searchResultsListHeaderLocator: Locator
  readonly _searchResultsSectionLocator: Locator
  readonly _searchResultsListItemLocator: Locator
  readonly _searchInputLocator: Locator
  readonly _searchButtonLocator: Locator
  _currentSearchTerm = ''

  constructor (readonly page: Page) {
    this.expect = new SearchPageAssertions(this)
    this._headerLocator = this.page.locator('h1')
    this._searchResultsListHeaderLocator = this.page.getByRole('heading', {
      name: this._searchResultsHeadingName
    })
    this._searchResultsSectionLocator = this.page.getByLabel(this._searchResultsHeadingName)
    this._searchResultsListItemLocator = this._searchResultsSectionLocator.getByRole('listitem')
    this._searchInputLocator = this.page.getByLabel('Search')
    this._searchButtonLocator = this.page.getByRole('button', { name: 'Search' })
  }

  readonly _searchResultsHeadingName = 'results for'

  readonly expectedSearchResults: { [key: string]: expectedResult[] } = {
    trust: [
      { name: 'trust 1', address: '12 Paddle Road, Bushy Park, Letworth, Manchester, MX12 P34', ukprn: '123', academiesInTrustCount: 1 },
      { name: 'trust 2', address: '12 Paddle Road, Manchester, MX12 P34', ukprn: '124', academiesInTrustCount: 2 },
      { name: 'trust 3', address: 'Bushy Park, Manchester', ukprn: '125', academiesInTrustCount: 0 }
    ],
    education: [
      { name: 'Abbey Education', address: '13 Paddle Road, Bushy Park, Letworth, Liverpool, MX12 P34', ukprn: '175', academiesInTrustCount: 1 }
    ],
    non: []
  }

  async goTo (): Promise<void> {
    await this.page.goto('/search')
  }

  async goToSearchFor (searchTerm: string): Promise<void> {
    this._currentSearchTerm = searchTerm
    await this.page.goto(`/search/?keywords=${searchTerm}`)
  }

  getListItemLocatorByText (text: string): Locator {
    return this._searchResultsListItemLocator.filter({ hasText: text })
  }

  async clickOnSearchResultLinkWithText (text: string): Promise<void> {
    await this._searchResultsListItemLocator.getByRole('link', { name: text }).click()
  }

  async searchFor (searchTerm: string): Promise<void> {
    this._currentSearchTerm = searchTerm
    await this._searchInputLocator.fill(searchTerm)
    await this._searchButtonLocator.click()
  }
}

class SearchPageAssertions {
  constructor (readonly searchPage: SearchPage) {}

  async toBeOnPageWithResultsFor (searchTerm: string): Promise<void> {
    await expect(this.searchPage._searchResultsListHeaderLocator).toContainText(
      `${this.searchPage._searchResultsHeadingName} "${searchTerm}"`
    )
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
    await expect(this.searchPage._searchResultsListHeaderLocator).toContainText(`${this.searchPage.expectedSearchResults[this.searchPage._currentSearchTerm].length} results for`)
  }

  async toSeeInformationForEachResult (): Promise<void> {
    const searchTerm = this.searchPage._currentSearchTerm
    await expect(this.searchPage._searchResultsListItemLocator).toHaveCount(this.searchPage.expectedSearchResults[searchTerm].length)

    for (const searchResultItem of this.searchPage.expectedSearchResults[searchTerm]) {
      const searchItemLocator = this.searchPage.getListItemLocatorByText(searchResultItem.name)
      await expect(searchItemLocator).toBeVisible()

      await expect(searchItemLocator).toContainText(`Address: ${searchResultItem.address}`)
      await expect(searchItemLocator).toContainText(`Academies in this trust: ${searchResultItem.academiesInTrustCount}`)
      await expect(searchItemLocator).toContainText(`UKPRN: ${searchResultItem.ukprn}`)
    }
  }

  async toSeeSearchInputContainingSearchTerm (): Promise<void> {
    await expect(this.searchPage._searchInputLocator).toHaveValue(this.searchPage._currentSearchTerm)
  }

  async toSeeSearchInputContainingNoSearchTerm (): Promise<void> {
    await expect(this.searchPage._searchInputLocator).toHaveValue('')
  }

  async toSeeNoResultsMessage (): Promise<void> {
    await expect(this.searchPage._searchResultsListHeaderLocator).toContainText(`0 ${this.searchPage._searchResultsHeadingName}`)
    await expect(this.searchPage._searchResultsSectionLocator).toContainText(
      'Check the spelling of the trust name. Enter a reference number in the right format'
    )
  }
}
