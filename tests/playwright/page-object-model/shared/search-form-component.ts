import { Locator, Page, expect } from '@playwright/test'

interface expectedResult {
  name: string
  address: string
  ukprn: string
  academiesInTrustCount: number
}

export class SearchFormComponent {
  readonly expect: SearchFormComponentAssertions
  readonly searchFormLocator: Locator
  readonly searchInputLocator: Locator
  readonly searchButtonLocator: Locator

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

  currentSearchTerm: string

  constructor (readonly page: Page, label: string) {
    this.expect = new SearchFormComponentAssertions(this)
    this.searchFormLocator = page.getByTestId('app-search-form')
    this.searchInputLocator = this.searchFormLocator.getByLabel(label)
    this.searchButtonLocator = this.searchFormLocator.getByRole('button', { name: 'Search' })
  }

  getAutocompleteOptionWithText (trustName: string): Locator {
    return this.searchFormLocator.getByRole('option', { name: trustName })
  }

  async typeSearchTerm (searchTerm: string): Promise<void> {
    this.currentSearchTerm = searchTerm
    await this.searchInputLocator.fill(searchTerm)
  }

  async searchFor (searchTerm: string): Promise<void> {
    await this.typeSearchTerm(searchTerm)
    await this.submitSearch()
  }

  async submitSearch (): Promise<void> {
    await this.searchButtonLocator.click()
  }

  async chooseItemFromAutocompleteWithText (trustName: string): Promise<void> {
    await this.getAutocompleteOptionWithText(trustName).click()
  }
}

class SearchFormComponentAssertions {
  constructor (readonly searchForm: SearchFormComponent) {}

  async inputToContainSearchTerm (): Promise<void> {
    await expect(this.searchForm.searchInputLocator).toHaveValue(this.searchForm.currentSearchTerm)
  }

  async inputToContainNoSearchTerm (): Promise<void> {
    await expect(this.searchForm.searchInputLocator).toHaveValue('')
  }

  async toshowNoResultsFoundInAutocomplete (): Promise<void> {
    await expect(this.searchForm.searchFormLocator.getByRole('listitem').first()).toHaveText('No results found')
  }

  async toShowAllResultsInAutocomplete (): Promise<void> {
    const listItems = await this.searchForm.searchFormLocator.getByRole('option')
    await expect(listItems).toHaveCount(this.searchForm.expectedSearchResults[this.searchForm.currentSearchTerm].length)

    for (const searchResultItem of this.searchForm.expectedSearchResults[this.searchForm.currentSearchTerm]) {
      const searchItemLocator = this.searchForm.getAutocompleteOptionWithText(searchResultItem.name)
      await expect(searchItemLocator).toBeVisible()

      await expect(searchItemLocator).toContainText(searchResultItem.address)
    }
  }
}
