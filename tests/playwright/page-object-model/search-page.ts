import { Locator, Page, expect } from '@playwright/test'
import { CurrentSearch, SearchFormComponent } from './shared/search-form-component'

export class SearchPage {
  readonly expect: SearchPageAssertions
  readonly searchForm: SearchFormComponent
  readonly _headerLocator: Locator
  readonly _searchResultsListHeaderLocator: Locator
  readonly _searchResultsSectionLocator: Locator
  readonly _searchResultsListItemLocator: Locator
  currentSearch: CurrentSearch

  constructor (readonly page: Page, currentSearch: CurrentSearch) {
    this.expect = new SearchPageAssertions(this)
    this.searchForm = new SearchFormComponent(
      page,
      'Search',
      currentSearch
    )
    this.currentSearch = currentSearch
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
    this.currentSearch.term = searchTerm
    await this.page.goto(`/search/?keywords=${searchTerm}`)
  }

  async goToPageWithResults (): Promise<void> {
    await this.goToSearchFor('mary')
  }

  async goToPageWithNoResults (): Promise<void> {
    await this.goToSearchFor('non')
  }

  getListItemLocatorByText (text: string): Locator {
    return this._searchResultsListItemLocator.filter({ hasText: text })
  }

  async clickOnSearchResultLink (resultNumber: number): Promise<void> {
    const itemToSelect = await this._searchResultsListItemLocator.getByRole('link').nth(resultNumber - 1)
    const itemText = (await itemToSelect.innerText()).split('\n')
    this.currentSearch.selectedTrust = {
      name: itemText[0],
      address: itemText[1]
    }

    await itemToSelect.click()
  }
}

class SearchPageAssertions {
  constructor (readonly searchPage: SearchPage) {}

  async toBeOnPageWithMatchingResults (): Promise<void> {
    await expect(this.searchPage._searchResultsListHeaderLocator).toContainText(
      `${this.searchPage._searchResultsHeadingName} "${this.searchPage.currentSearch.term}"`
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
      `${this.searchPage.searchForm.expectedSearchResults[this.searchPage.searchForm.currentSearch.term].length} results for`
    )
  }

  async toSeeInformationForEachResult (): Promise<void> {
    const searchTerm = this.searchPage.searchForm.currentSearch.term
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
