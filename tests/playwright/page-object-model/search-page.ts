import { Locator, Page, expect } from '@playwright/test'
import { CurrentSearch, SearchFormComponent } from './shared/search-form-component'
import { FakeTestData, FakeTrust } from '../fake-data/fake-test-data'
import { SearchTerms } from '../fake-data/search-terms'
import { PaginationComponent } from './shared/pagination-component'

export class SearchPage {
  readonly expect: SearchPageAssertions
  readonly searchForm: SearchFormComponent
  readonly pagination: PaginationComponent
  readonly _headerLocator: Locator
  readonly _searchResultsListHeaderLocator: Locator
  readonly _searchResultsSectionLocator: Locator
  readonly _searchResultsListItemLocator: Locator
  currentSearch: CurrentSearch
  testData: FakeTestData
  numberOfResultsOnOnePage = 20

  constructor (readonly page: Page, currentSearch: CurrentSearch, fakeTestData?: FakeTestData) {
    if (fakeTestData !== undefined) {
      this.testData = fakeTestData
    }

    this.expect = new SearchPageAssertions(this)
    this.searchForm = new SearchFormComponent(
      page,
      'Search',
      currentSearch
    )
    this.currentSearch = currentSearch
    this.pagination = new PaginationComponent(page)
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
    await this.page.goto(`/search?keywords=${searchTerm}`)
  }

  async goToSearchWithResults (): Promise<void> {
    await this.goToSearchFor(SearchTerms.CommonName)
  }

  async goToSearchWithOnePageOfResults (): Promise<void> {
    await this.goToSearchFor(SearchTerms.OnePage)
  }

  async goToSearchWithManyPagesOfResults (): Promise<void> {
    await this.goToSearchFor(SearchTerms.ManyPages)
  }

  async goToSearchWithNoResults (): Promise<void> {
    await this.goToSearchFor(SearchTerms.NoMatches)
  }

  getListItemLocatorAt (index: number): Locator {
    return this._searchResultsListItemLocator.nth(index)
  }

  async getExpectedResultMatching (element: Locator): Promise<FakeTrust> {
    const uid = (await element.getByTestId('uid').textContent())?.trim() ?? ''
    await expect(uid, 'Expected result to contain a UID value, but it did not').toBeTruthy()
    return this.testData.getTrustByUid(uid)
  }

  async clickOnSearchResultLink (resultNumber: number): Promise<void> {
    const itemToSelect = await this.getListItemLocatorAt(resultNumber).getByRole('link')
    this.currentSearch.selectedTrustName = await itemToSelect.innerText()

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
    const expectedNoOfResults = this.searchPage.testData.getNumberOfTrustsWithNameMatching(this.searchPage.searchForm.currentSearch.term)
    const noOfResultsOnPage = expectedNoOfResults < this.searchPage.numberOfResultsOnOnePage ? expectedNoOfResults : this.searchPage.numberOfResultsOnOnePage
    await expect(this.searchPage._searchResultsListHeaderLocator).toContainText(
      `${noOfResultsOnPage} results for`
    )
  }

  async toSeeInformationForEachResult (): Promise<void> {
    const resultsCount = await this.searchPage._searchResultsListItemLocator.count()

    for (let resultNumber = 0; resultNumber < resultsCount; resultNumber++) {
      const resultLocator = await this.searchPage.getListItemLocatorAt(resultNumber)
      const expectedResult = await this.searchPage.getExpectedResultMatching(resultLocator)

      await expect(resultLocator).toContainText(expectedResult.name)
      await expect(resultLocator).toContainText(`Address: ${expectedResult.address}`)
      await expect(resultLocator).toContainText(`Group ID/TRN: ${expectedResult.groupId}`)
    }
  }

  async toSeeNoResultsMessage (): Promise<void> {
    await expect(this.searchPage._searchResultsListHeaderLocator).toContainText(`0 ${this.searchPage._searchResultsHeadingName}`)
    await expect(this.searchPage._searchResultsSectionLocator).toContainText(
      'Check the spelling of the trust name. Make sure you include the right punctuation.'
    )
  }
}
