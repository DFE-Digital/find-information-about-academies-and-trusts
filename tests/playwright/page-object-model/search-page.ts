import { Locator, Page, expect } from '@playwright/test'
import { CurrentSearch, SearchFormComponent } from './shared/search-form-component'
import { FakeTestData } from '../fake-data/fake-test-data'

export class SearchPage {
  readonly expect: SearchPageAssertions
  readonly searchForm: SearchFormComponent
  readonly _headerLocator: Locator
  readonly _searchResultsListHeaderLocator: Locator
  readonly _searchResultsSectionLocator: Locator
  readonly _searchResultsListItemLocator: Locator
  currentSearch: CurrentSearch
  testData: FakeTestData
  numberOfResultsOnOnePage = 20

  constructor (readonly page: Page, currentSearch: CurrentSearch, fakeTestData: FakeTestData) {
    this.testData = fakeTestData
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

  async toShowEmptyResultMessage (): Promise<void> {
    await expect(this.searchPage._searchResultsListItemLocator).toHaveCount(0)
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

    for (let result = 0; result < resultsCount; result++) {
      const element = await this.searchPage._searchResultsListItemLocator.nth(result)
      const uid = (await element.getByTestId('uid').textContent())?.trim() ?? ''
      expect(uid, 'Expected result to contain a UID value, but it did not').toBeTruthy()
      const expectedResult = this.searchPage.testData.getTrustByUid(uid)
      expect(expectedResult, `Expected to find trust in the test data containing UID: ${uid}, but it was not found`).toBeTruthy()
      if (expectedResult !== undefined) {
        await expect(element).toContainText(expectedResult.name)
        await expect(element).toContainText(`Address: ${expectedResult.address}`)
        await expect(element).toContainText(`Group ID/TRN: ${expectedResult.groupId}`)
      }
    }
  }

  async toSeeNoResultsMessage (): Promise<void> {
    await expect(this.searchPage._searchResultsListHeaderLocator).toContainText(`0 ${this.searchPage._searchResultsHeadingName}`)
    await expect(this.searchPage._searchResultsSectionLocator).toContainText(
      'Check the spelling of the trust name. Enter a reference number in the right format'
    )
  }
}
