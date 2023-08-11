import { Locator, Page, expect } from '@playwright/test'

export class SearchPage {
  readonly expect: SearchPageAssertions
  readonly _headerLocator: Locator
  readonly _searchResultsListHeaderLocator: Locator
  readonly _searchResultsListItemLocator: Locator

  constructor (readonly page: Page) {
    const searchResultsHeadingName = 'results for'

    this.expect = new SearchPageAssertions(this)
    this._headerLocator = this.page.locator('h1')
    this._searchResultsListHeaderLocator = this.page.getByRole('heading', {
      name: searchResultsHeadingName
    })
    this._searchResultsListItemLocator = this.page.getByLabel(searchResultsHeadingName).getByRole('listitem')
  }

  async goTo (): Promise<void> {
    await this.page.goto('/search')
  }

  async goToSearchFor (keywords: string): Promise<void> {
    await this.page.goto(`/search/?keywords=${keywords}`)
  }

  getListItemByText (text: string): Locator {
    return this._searchResultsListItemLocator.filter({ hasText: text })
  }

  async getAcademiesInTrustsTextBy (trustName: string, count: number): Promise<Locator> {
    return this._searchResultsListItemLocator.filter({ hasText: trustName }).getByText(`Academies in this trust: ${count}`)
  }

  async clickOnSearchResultLinkWithText (text: string): Promise<void> {
    await this._searchResultsListItemLocator.getByRole('link', { name: text }).click()
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

  async toShowResultsWithCount (count: number): Promise<void> {
    await expect(this.searchPage._searchResultsListItemLocator).toHaveCount(count)
  }

  async toShowEmptyResultMessage (): Promise<void> {
    await expect(this.searchPage._searchResultsListItemLocator).toHaveCount(0)
  }

  async toShowResultWithText (trustName: string, text: string): Promise<void> {
    await expect(this.searchPage.getListItemByText(trustName)).toContainText(text)
  }

  async toShowResultWithAcademiesinTrustCount (trustName: string, count: number): Promise<void> {
    await expect(this.searchPage.getAcademiesInTrustsTextBy(trustName, count)).toBeTruthy()
  }
}
