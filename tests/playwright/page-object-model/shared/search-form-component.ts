import { Locator, Page, expect } from '@playwright/test'

export class SearchFormComponent {
  readonly expect: SearchFormComponentAssertions
  readonly searchFormLocator: Locator
  readonly searchInputLocator: Locator
  readonly searchButtonLocator: Locator

  currentSearchTerm: string

  constructor (readonly page: Page, label: string) {
    this.expect = new SearchFormComponentAssertions(this)
    this.searchFormLocator = page.getByTestId('app-search-form')
    this.searchInputLocator = this.searchFormLocator.getByLabel(label)
    this.searchButtonLocator = this.searchFormLocator.getByRole('button', { name: 'Search' })
  }

  getAutocompleteLocator (): Locator {
    return this.searchFormLocator.locator('#search-autocomplete-container')
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
    await expect(this.searchForm.getAutocompleteLocator().getByRole('listitem').first()).toHaveText('No results found')
  }

  async toShowAllOfTheResultsInAutocomplete (): Promise<void> {
    
  }
}
