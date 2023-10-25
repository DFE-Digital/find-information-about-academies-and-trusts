import { Locator, Page, expect } from '@playwright/test'
import { SearchFormComponent } from './shared/search-form-component'

export class SearchPage {
  readonly expect: SearchPageAssertions
  readonly searchForm: SearchFormComponent
  readonly _headerLocator: Locator
  readonly _searchResultsListHeaderLocator: Locator
  readonly _searchResultsSectionLocator: Locator
  readonly _searchResultsListItemLocator: Locator

  constructor (readonly page: Page) {
    this.expect = new SearchPageAssertions(this)
    this.searchForm = new SearchFormComponent(page, 'Search')
    this._headerLocator = this.page.locator('h1')
    this._searchResultsListHeaderLocator = this.page.getByRole('heading', {
      name: this._searchResultsHeadingName
    })
    this._searchResultsSectionLocator = this.page.getByLabel(this._searchResultsHeadingName)
    this._searchResultsListItemLocator = this._searchResultsSectionLocator.getByRole('listitem')
  }

  readonly _searchResultsHeadingName = 'results for'

  async goTo (): Promise<void> {
    await this.page.goto('/search')
  }

  async goToSearchFor (searchTerm: string): Promise<void> {
    this.searchForm.currentSearchTerm = searchTerm
    await this.page.goto(`/search/?keywords=${searchTerm}`)
  }

  getListItemLocatorByText (text: string): Locator {
    return this._searchResultsListItemLocator.filter({ hasText: text })
  }

  async clickOnSearchResultLinkWithText (text: string): Promise<void> {
    await this._searchResultsListItemLocator.getByRole('link', { name: text }).click()
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
  async toDisplayNumberOfResultsFound (): Promise<void> {
    await expect(this.searchPage._searchResultsListHeaderLocator).toContainText(
      `${this.searchPage.searchForm.expectedSearchResults[this.searchPage.searchForm.currentSearchTerm].length} results for`
    )
  }

  async toSeeInformationForEachResult (): Promise<void> {
    const searchTerm = this.searchPage.searchForm.currentSearchTerm
    await expect(this.searchPage._searchResultsListItemLocator).toHaveCount(this.searchPage.searchForm.expectedSearchResults[searchTerm].length)

    for (const searchResultItem of this.searchPage.searchForm.expectedSearchResults[searchTerm]) {
      const searchItemLocator = this.searchPage.getListItemLocatorByText(searchResultItem.name)
      await expect(searchItemLocator).toBeVisible()

      await expect(searchItemLocator).toContainText(`Address: ${searchResultItem.address}`)
      await expect(searchItemLocator).toContainText(`Academies in this trust: ${searchResultItem.academiesInTrustCount}`)
      await expect(searchItemLocator).toContainText(`UKPRN: ${searchResultItem.ukprn}`)
    }
  }

  async toSeeNoResultsMessage (): Promise<void> {
    await expect(this.searchPage._searchResultsListHeaderLocator).toContainText(`0 ${this.searchPage._searchResultsHeadingName}`)
    await expect(this.searchPage._searchResultsSectionLocator).toContainText(
      'Check the spelling of the trust name. Enter a reference number in the right format'
    )
  }
}
